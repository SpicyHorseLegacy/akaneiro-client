using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Reflection;

    public class SocketReadStateObject
    {
        private Socket      _workSocket;
        
        public byte[]       _buffer;
        public const int    BUFFER_SIZE = 8 * 1024;

        public SocketReadStateObject(Socket _socket)
        {
            _buffer         = new byte[BUFFER_SIZE];
            _workSocket     = _socket;
        }

        public Socket WorkSocket
        {
            get { return _workSocket; }
        }
    }

    public class SocketSendStateObject
    {
        private Socket      _workSocket;

        public byte[]       _buffer;
        public int          _iLeft;

        public SocketSendStateObject(Socket _socket, byte[] buffer)
        {
            _buffer     = buffer;
            _iLeft      = _buffer.Length;
            _workSocket = _socket;
        }

        public Socket WorkSocket
        {
            get { return _workSocket; }
        }
    }

    public enum ESessionError
    {
        UNKNOWN     = -1,
        FAILED      = 0,
        SUCCESS     = 1,
        ERR_CONNECT,
        ERR_RECEIVE_BEGIN_EXCEPTION,
        ERR_RECEIVE_END_EXCEPTION,
        ERR_RECEIVE_OVERBUFFER,
        ERR_SEND_BEGIN_EXCEPTION,
        ERR_SEND_END_EXCEPTION,
        ERR_END_OVERBUFFER,
    }

    public class TcpSessionBase
    {        
        
        protected bool              _isConnected        = false;
        protected Socket            _socket;


        protected ReaderWriterLock  _receiveLock        = new ReaderWriterLock();
        protected AutoResetEvent    _sendDataReady      = new AutoResetEvent(false); //Send事件的信号量
        protected Queue             _sendDataQueue      = new Queue();


        protected ReaderWriterLock  _sendLock           = new ReaderWriterLock();
        protected static int        _msgBufferLength    = 512 * 1024;        
        protected CircleBuffer      _msgBuffer          = new CircleBuffer(_msgBufferLength);


        protected ManualResetEvent  _stopEvent          = new ManualResetEvent(false);

        protected string            _errMessage;
        protected int               _sessoinID;
        static private int          Id;
        protected long              _startTime;
        ESessionError               _errCode;
                
        public TcpSessionBase()
        {

            Id++;               // 累加计算当前进程中socket的ID
            _sessoinID  = Id;    // 保存本地的socket id
            _startTime  = DateTime.Now.Ticks;
            _errCode    = ESessionError.UNKNOWN;
        }

        ~TcpSessionBase()
        {
            Close();            
        }

        // Properties
        public int          SessionID
        {
            get { return _sessoinID; }
        }

        public string       ErrorMessage
        {
            get
            {
                return _errMessage;
            }
        }

        public bool         IsConnected
        {
            get { return (_socket == null ? false : _socket.Connected); }
        }

        // Operations
        public bool         Connect(string ip, int serviceport)
        {
            if (_isConnected)
            {
                _errMessage = "error; socket has already connected!";
                return false; //need not do anything once connected
            }

            // resolve...
		//IPHostEntry hostEntry = Dns.GetHostEntry(ip);
            //if (hostEntry == null)
            //    return false;

            // create an end-point for the first address...
		//IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip) hostEntry.AddressList[0], serviceport);
		//GUILogManager.LogErr("IP: "+ip+" Port: "+serviceport);
		IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), serviceport);

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);

            //byte[] inOptionValues = new byte[4 * 3];
            //BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            //BitConverter.GetBytes((uint)1000).CopyTo(inOptionValues, 4);
            //BitConverter.GetBytes((uint)1000).CopyTo(inOptionValues, 4 * 2);
            //_socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);

            try
            {
				_socket.BeginConnect(endPoint, new AsyncCallback(AyncConnectCallback), _socket);
                //_socket.BeginConnect(ip, serviceport, new AsyncCallback(AyncConnectCallback), _socket);
            }
            catch(Exception ex)
            {
                _errCode    = ESessionError.ERR_CONNECT;
                _errMessage = ex.Message;                    
                return false;
            }

            return true;
        }

        public void         Close()
        {
            if (IsConnected)
            {
                _isConnected = false;

                // 通知线程停止
                _stopEvent.Set();

                // 关闭Socket
                if (_socket != null)
                {
                    _socket.Close();
                }

            }
            OnClose(_sessoinID, _errCode);
        }

		public void SendMessageToGame(byte[] bytesSend)
        {
            _sendLock.AcquireWriterLock(-1);
            try
            {
                _sendDataQueue.Enqueue(bytesSend);
            }
            catch (Exception ex)
            {
                _errCode = ESessionError.ERR_END_OVERBUFFER;
                _errMessage = ex.Message;
                Close();
                return;
            }
            finally
            {
                _sendLock.ReleaseWriterLock();
            }

            // 通知发送数据
            _sendDataReady.Set();
        }


        // interfaces
        public virtual void OnConnect(ESessionError errCode)
        {
        }

        public virtual bool OnReceive(ESessionError errCode)
        {
            return true;
        }

        public virtual void OnClose(int sessionID, ESessionError errCode)
        {
        }

        // asynchronous callbacks
        private void        AyncConnectCallback(IAsyncResult ar)
        {
            Socket sock = (Socket)ar.AsyncState;
        
            _isConnected = sock.Connected;            

            try
            {
                sock.EndConnect(ar);            
            }
            catch(Exception ex)
            {                
                OnConnect(ESessionError.ERR_CONNECT);
                _errMessage = ex.Message;                
                return;
            }

            if (sock.Connected == true)
            {
                OnConnect(ESessionError.SUCCESS);
            }
            else
            {
                OnConnect(ESessionError.FAILED);
            }

            if (_isConnected == true )
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReceiveThreadEntryPoint));
                ThreadPool.QueueUserWorkItem(new WaitCallback(SendThreadEntryPoint));
            }
        }

        private void        AsynchReadCallback(System.IAsyncResult ar)
        {
            SocketReadStateObject so = (SocketReadStateObject)ar.AsyncState;
            Socket sock = so.WorkSocket;
            if (sock == null || !sock.Connected)
            {
                return;
            }

            try
            {
                int read = sock.EndReceive(ar);
                if (read > 0)
                {
                    _receiveLock.AcquireWriterLock(-1);
                    try
                    {
                        if (!_msgBuffer.AppendData(so._buffer, read))
                        {
                            //内存不够了。
                            _errCode = ESessionError.ERR_RECEIVE_OVERBUFFER;
                            Close();
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        //出现问题了，关闭socket吧。                       
                        _errCode = ESessionError.ERR_RECEIVE_END_EXCEPTION;
                        _errMessage = ex.Message;
                        Close();
                    }
                    finally
                    {
                        _receiveLock.ReleaseWriterLock();
                    }

                    while (OnReceive(ESessionError.SUCCESS) == true);

                    // 开始接收更多数据
                    sock.BeginReceive(so._buffer, 0, SocketReadStateObject.BUFFER_SIZE, 0, new AsyncCallback(AsynchReadCallback), so);
                }
                else
                {
                    Console.WriteLine("Socket shutdown!");

                    Close();
                }

            }
            catch (System.Exception e)
            {
                _errCode = ESessionError.ERR_RECEIVE_END_EXCEPTION;
                _errMessage = e.Message;
                Close();
            }
        }

        private void        AsynchSendCallback(System.IAsyncResult ar)
        {
            SocketSendStateObject so = (SocketSendStateObject)ar.AsyncState;
            Socket s = so.WorkSocket;

            if (s == null || !s.Connected)
            {
                return;
            }

            try
            {
                int sz = s.EndSend(ar);
                if (sz < so._iLeft)
                {
                    // Handle in-completed sent

                    so._iLeft -= sz;

                    s.BeginSend(so._buffer, so._buffer.Length - so._iLeft, so._iLeft, 0, new AsyncCallback(AsynchSendCallback), so);
                }
            }
            catch (Exception ex)
            {
                _errCode = ESessionError.ERR_SEND_END_EXCEPTION;
                _errMessage = ex.Message;
                Close();
            }
        }

        // Work threads
        private void        ReceiveThreadEntryPoint(object state)
        {
            // loop...
            while (true)
            {
                WaitHandle[] handles = new WaitHandle[1];
                handles[0] = _stopEvent;

                if (IsConnected)
                {
                    try
                    {
                        // 开始接收数据
                        //System.IAsyncResult iar;
                        SocketReadStateObject so2 = new SocketReadStateObject(_socket);
                        _socket.BeginReceive(so2._buffer, 0, SocketReadStateObject.BUFFER_SIZE, 0, new AsyncCallback(AsynchReadCallback), so2);

                        if (WaitHandle.WaitAny(handles) == 0)
                        {
                            break;
                        }
                    }
                    catch(Exception ex)
                    {
                        _errCode = ESessionError.ERR_RECEIVE_BEGIN_EXCEPTION;
                        _errMessage = ex.Message;                        
                        Close();
                    }
                }
           
            }
        }

        private void        SendThreadEntryPoint(object state)
        {
            try
            {
                Queue workQueue = new Queue();

                // loop...
                while (true)
                {
                    WaitHandle[] handles = new WaitHandle[2];
                    handles[0] = _stopEvent;
                    handles[1] = _sendDataReady;

                    if (WaitHandle.WaitAny(handles) == 0)
                    {
                        break;
                    }
                    else if (IsConnected)
                    {
                        _sendLock.AcquireWriterLock(-1);
                        try
                        {
                            workQueue.Clear();
                            foreach (byte[] message in _sendDataQueue)
                            {
                                SocketSendStateObject so = new SocketSendStateObject(_socket, message);
                                workQueue.Enqueue(so);
                            }
                            _sendDataQueue.Clear();
                        }
                        catch
                        {
                            _errCode = ESessionError.ERR_SEND_BEGIN_EXCEPTION;
                            Close();
                        }
                        finally
                        {
                            _sendLock.ReleaseWriterLock();
                        }

                        // loop the outbound messages...
                        foreach (SocketSendStateObject so in workQueue)
                        {
                            // send it...
                            _socket.BeginSend(so._buffer, 0, so._buffer.Length, 0, new AsyncCallback(AsynchSendCallback), so);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errCode = ESessionError.ERR_SEND_BEGIN_EXCEPTION;
                _errMessage = ex.Message;
                Close();
            }
        }

    }
