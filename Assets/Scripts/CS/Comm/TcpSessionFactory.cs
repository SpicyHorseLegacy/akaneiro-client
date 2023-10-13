using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Reflection;


    enum EPacketType
    {
        ePacketGameLogic = 0x0,
        ePacketHeartBeatPing,
        ePacketHeartBeatPong,
        ePacketPeerDecryptKey,
        ePacketPeerDecryptKeyGot,
        ePacketTypeMax
    };

    public enum EEventType
    {
        TEVENT_CONNECT,				// 链接消息
        TEVENT_RECEIVE,				// 接收到消息
        TEVENT_CLOSE,				// 关闭Socket
    };

    public class SSessionEvent
    {
        public EEventType       eventType;
        public ESessionError    errCode;        
        public TcpSession       session;
        public int              sessionID;
        public byte[]           msg = null;
    }

    public class TcpSession : TcpSessionBase
    {
        protected TcpSessionFactory _sessionFactory = null;
        protected Object            _userData;

		float _fAveragePing = 0.0f;
		int _iPingCount = 0;
		int _iLastPing = -1;

		System.Timers.Timer _pingTimer = new System.Timers.Timer(10000);

        public TcpSession(TcpSessionFactory factory)
        {
            _sessionFactory = factory;

			
        }
        
        public Object UserData
        {
            get
            {
                return _userData;
            }

            set
            {
                _userData = value;
            }
        }

		public void	BeginPing()
		{
			SendPingMessage();
			_pingTimer = new System.Timers.Timer(10000);
			_pingTimer.Elapsed += new System.Timers.ElapsedEventHandler(PingTimeout);
			//到达时间的时候执行事件；   
			_pingTimer.AutoReset = true;
			//设置是执行一次（false）还是一直执行(true)；   
			_pingTimer.Enabled = true;
			//是否执行System.Timers.Timer.Elapsed事件； 			
		}

		public void PingTimeout(object source, System.Timers.ElapsedEventArgs e)
		{   
			SendPingMessage();
		}  

	

		public int GetlastPing()
		{
			return _iLastPing;
		}

		public float GetAvgPing()
		{
			return _fAveragePing;
		}

		public void SendPingMessage()
		{
			byte[] msgPing = new byte[10];

			short sendMsgType = (short)EPacketType.ePacketHeartBeatPing;

			byte[] buf = BitConverter.GetBytes((int)4);
			buf.CopyTo(msgPing, 0);

			buf = BitConverter.GetBytes(sendMsgType);
			buf.CopyTo(msgPing, 4);

			TimeSpan ts = DateTime.Now - DateTime.Parse("2012-5-21");
			int curTime32 = (int)ts.TotalMilliseconds;

			buf = BitConverter.GetBytes(curTime32);
			buf.CopyTo(msgPing, 6);

			SendMessageToGame(msgPing);
		}

        public override void    OnConnect(ESessionError errCode)
        {
            SSessionEvent socketEvent   = new SSessionEvent();
            socketEvent.eventType       = EEventType.TEVENT_CONNECT;
            socketEvent.session         = this;
            socketEvent.errCode         = errCode;
            socketEvent.sessionID       = _sessoinID;

            _sessionFactory.PushSocketEvent(ref socketEvent);
        }

        public override bool    OnReceive(ESessionError errCode)
        {
            if (errCode != ESessionError.SUCCESS)
                return false;
                     
            int bodySize            = 0;
            int msgLen              = 0;
            byte[] bytesType        = null;
            byte[] bytesBodySize    = null;
            byte[] bytesBody        = null;
            short msgType           = 0;

            _receiveLock.AcquireReaderLock(-1);
            try
            {
                if (_msgBuffer.Length < 6)
                    return false;

                //读消息长度
                bytesBodySize = new byte[4];
                _msgBuffer.ReadData(ref bytesBodySize, 0);
                bodySize = BitConverter.ToInt32(bytesBodySize, 0);
                
                
                //读消息类型
                bytesType = new byte[2];
                _msgBuffer.ReadData(ref bytesType, 4);
                msgType = BitConverter.ToInt16(bytesType, 0);

                msgLen = bodySize + 6;

                //包未收完整
                if (_msgBuffer.Length < msgLen)
                {
                    return false;
                }

                //数据已经够整个包了，这次才真正的取出byteBodySize与btyesType
                _msgBuffer.SubmitReadData(ref bytesBodySize);
                _msgBuffer.SubmitReadData(ref bytesType);

                bytesBody = new byte[bodySize];
                _msgBuffer.FectchData(ref bytesBody);                               
            }
            catch
            {            	
            }
            finally
            {
                _receiveLock.ReleaseReaderLock();
            }

            if (msgType == (short)EPacketType.ePacketGameLogic)
            {
                //UnityEngine.Debug.Log("ePacketGameLogic");

                SSessionEvent socketEvent   = new SSessionEvent();
                socketEvent.eventType       = EEventType.TEVENT_RECEIVE;
                socketEvent.session         = this;
                socketEvent.errCode         = errCode;
                socketEvent.sessionID       = _sessoinID;
                socketEvent.msg             = new byte[msgLen];
                Array.Copy(bytesBody, 0, socketEvent.msg, 0, bodySize);

                _sessionFactory.PushSocketEvent(ref socketEvent);
            }
            else if (msgType == (short)EPacketType.ePacketHeartBeatPing)
            {
                byte[] msgPong = new byte[6];

                short sendMsgType = (short)EPacketType.ePacketHeartBeatPong;

                byte[] buf = BitConverter.GetBytes(sendMsgType);
                buf.CopyTo(msgPong, 4);

                SendMessageToGame(msgPong);

                //UnityEngine.Debug.Log("ePacketHeartBeatPong");
            }
			else if (msgType == (short)EPacketType.ePacketHeartBeatPong)
			{
 				//_iPingOTTimes = 0;
 
 				if (bodySize == 4)
 				{
 					int oldTime32;

					oldTime32 = BitConverter.ToInt32(bytesBody, 0);

					TimeSpan ts = DateTime.Now - DateTime.Parse("2012-5-21");
					int curTime32 = (int)ts.TotalMilliseconds;
				
 					_iLastPing = curTime32 - oldTime32;
 					if (_iLastPing < 0)
 						_iLastPing += 0x7FFFFFFF;
 
 					_iPingCount++;
 					_fAveragePing = ((_iPingCount - 1) * _fAveragePing + _iLastPing) * 1.0f / _iPingCount;
 				}
			}

            return true;
        }

        public override void    OnClose(int sessionID, ESessionError errCode)
        {
            SSessionEvent socketEvent   = new SSessionEvent();
            socketEvent.eventType       = EEventType.TEVENT_CLOSE;
            socketEvent.session         = this;
            socketEvent.errCode         = errCode;
            socketEvent.sessionID       = _sessoinID;
            socketEvent.msg             = null;

            _sessionFactory.PushSocketEvent(ref socketEvent);
        }

    }


    public interface INetworkEventHandler
    {
        void OnConnect(TcpSession socket, ESessionError errCode);
        bool OnReceive(TcpSession socket, byte[] byteMsg);
        void OnClose(TcpSession socket, ESessionError errCode);
    }

    public class TcpSessionFactory
    {
        private INetworkEventHandler _networkEventHandler;

        private Queue               _msgReceiveQueue	    = new Queue();
		private Queue				_msgOneUpdate			= new Queue();
		private ReaderWriterLock	_msgReceiveLock			= new ReaderWriterLock();


        private Hashtable           _socketTable			= new Hashtable();
        private ReaderWriterLock    _socketTableLock		= new ReaderWriterLock();


        public TcpSessionFactory(INetworkEventHandler handler)
        {
            _networkEventHandler = handler;
        }

        public ReaderWriterLock     MessageReceiveLock
        {
            get
            {
                return _msgReceiveLock;
            }
        }

        public Queue                MessageReceiveQueue
        {
            get
            {
                return _msgReceiveQueue;
            }
        }

        public void                 PushSocketEvent(ref SSessionEvent se)
        {
            _msgReceiveLock.AcquireWriterLock(-1);
            try
            {
                _msgReceiveQueue.Enqueue(se);
            }
            catch
            {
            }
            finally
            {
                _msgReceiveLock.ReleaseWriterLock();
            }

        }

        //创建Session
	    public TcpSession           CreateSession( string ip, int port )
        {
            TcpSession socket = new TcpSession(this);
            socket.Connect(ip, port);

            _socketTableLock.AcquireWriterLock(-1);
            try
            {
                _socketTable.Add(socket.SessionID, socket);            
            }
            catch
            {
            }
            finally
            {
                _socketTableLock.ReleaseWriterLock();
            }
            

            return socket;
        }

        //释放创建的Session
	    public void                 ReleaseSocket(TcpSession socket)
        {
            _socketTableLock.AcquireWriterLock(-1);
            try
            {
                _socketTable.Remove(socket.SessionID);                
            }
            catch
            {
            }
            finally
            {
                _socketTableLock.ReleaseWriterLock();
            }
        }

        public void                 ReleaseSocket(int sessionID)
        {
            _socketTableLock.AcquireWriterLock(-1);
            try
            {
                _socketTable.Remove(sessionID);
            }
            catch
            {
            }
            finally
            {
                _socketTableLock.ReleaseWriterLock();
            }
        }
	        
        //进程循环检查函数
	    public void                 Update()
        {
			_msgReceiveLock.AcquireWriterLock(-1);
            try
            {
                while (_msgReceiveQueue.Count > 0)
                {
					SSessionEvent socketEvent = (SSessionEvent)_msgReceiveQueue.Dequeue();
					_msgOneUpdate.Enqueue(socketEvent);
                }
            }
            catch
            {
            }
            finally
            {
                _msgReceiveLock.ReleaseWriterLock();
            }

			while (_msgOneUpdate.Count > 0)
			{
				SSessionEvent socketEvent = (SSessionEvent)_msgOneUpdate.Dequeue();

				switch (socketEvent.eventType)
				{
					case EEventType.TEVENT_CONNECT:
						if (socketEvent.errCode == ESessionError.SUCCESS)
						{
							//发送ping请求
							socketEvent.session.BeginPing();
						}
						_networkEventHandler.OnConnect(socketEvent.session, socketEvent.errCode);
						break;
					case EEventType.TEVENT_RECEIVE:
						_networkEventHandler.OnReceive(socketEvent.session, socketEvent.msg);
						break;
					case EEventType.TEVENT_CLOSE:
						_networkEventHandler.OnClose(socketEvent.session, socketEvent.errCode);
						break;
					default:
						Console.WriteLine("Invalidate message; message type:{0}", socketEvent.eventType);
						break;                
				}
			}
        }
    }
