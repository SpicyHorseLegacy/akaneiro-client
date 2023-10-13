using System;
using System.Collections.Generic;

    public class CircleBuffer
    {
        private int     _circleBegin = 0;
        private int     _circleEnd = 0;
        private int     _circleLength = 0;
        private int     _circleSuffLength = 0; //buffer中存有的数据长度
        private byte[]  _circleBuffer;

        public      CircleBuffer(int circleLen)
        {
            _circleLength = circleLen;
            _circleBuffer = new byte[circleLen];
        }

        /// <summary>
        /// 添加数据到内存
        /// </summary>
        /// <param name="bytesAdd"></param>
        /// <returns></returns>
        public bool AppendData(byte[] bytesAdd)
        {
            return AppendData(bytesAdd, bytesAdd.Length);
        }

        /// <summary>
        /// 添加数据到内存
        /// </summary>
        /// <param name="bytesAdd"></param>
        /// <param name="addLen"></param>
        /// <returns></returns>
        public bool AppendData(byte[] bytesAdd, int addLen)
        {
            if(_circleLength - _circleSuffLength < addLen)
            {
                //空间不够了
                return false;
            }
            if (_circleEnd - _circleBegin >= 0)
            {
                //环内存的起始指针小于等于结尾指针，代表buffer没有被正向填充满。
                //---begin-------------------------end---------------//
                if (_circleEnd + addLen > _circleLength)
                {
                    //到尾部不够用新添加到长度了
                    int excessDataLen = _circleEnd + addLen - _circleLength;
                    if (_circleBegin > excessDataLen)
                    {
                        //在头部可以将尾部不够的填充满
                        //-------------------end------begin------------------//
                        Array.Copy(bytesAdd, 0, _circleBuffer, _circleEnd, addLen - excessDataLen);//拷贝到尾部
                        Array.Copy(bytesAdd, addLen - excessDataLen, _circleBuffer, 0, excessDataLen);//拷贝到头部
                        _circleEnd = excessDataLen;
                    }
                    else
                    {
                        //Buffer没有足够空间了
                        return false;
                    }
                }
                else
                {
                    //尾部有足够的空间啊
                    //begin-------------------------end------------------//
                    Array.Copy(bytesAdd, 0, _circleBuffer, _circleEnd, addLen);                    
                    _circleEnd += addLen;
                }
            }
            else
            {
                //正向填充满了，进行反向回环添加
                //---end-------------------------begin------------------//
                if (_circleBegin - _circleEnd < addLen)
                {//Buffer没有足够空间了
                    return false;
                }
                else
                {
                    Array.Copy(bytesAdd, 0, _circleBuffer, _circleEnd, addLen);                    
                    _circleEnd += addLen;
                }
            }

            _circleSuffLength += addLen;

            return true;
        }

        /// <summary>
        /// 读取数据，但不删除内存中的数据。（只读不取）
        /// </summary>
        /// <param name="byteOut"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public bool ReadData(ref byte[] byteOut, int startIndex)
        {
            return ReadData(ref byteOut, startIndex, byteOut.Length);
        }

        /// <summary>
        /// 读取数据，但不删除内存中的数据。（只读不取）
        /// </summary>
        /// <param name="byteOut"></param>
        /// <param name="startIndex">是以Buffer限定的指针为起始位置</param>
        /// <param name="readLen"></param>
        /// <returns></returns>
        public bool ReadData(ref byte[] byteOut, int startIndex, int readLen)
        {
            try
            {
                if (_circleSuffLength < readLen)
                {
                    return false; //空间没有足够要读数据了
                }


                int tmpBegin = _circleBegin + startIndex;
                if( tmpBegin > _circleLength )
                {
                    //已经超出要读指针的结尾了，应该将begin移到头部去啊。
                    tmpBegin -= _circleLength;
                }

                if (_circleEnd - tmpBegin + startIndex >= readLen)
                {
                    //------begin-----------------end-----//
                    //Console.WriteLine("ReadData 1;  begin:{0}; end:{1}; readlen:{2}", tmpBegin, _circleEnd, readLen);
                    Array.Copy(_circleBuffer, tmpBegin, byteOut, 0, readLen);                    
                }
                else if (_circleEnd - tmpBegin <= 0 && _circleLength - tmpBegin >= readLen)
                {
                    //------end-----------------begin-----//
                    //Console.WriteLine("ReadData 2;  begin:{0}; end:{1}; readlen:{2}", tmpBegin, _circleEnd, readLen);
                    Array.Copy(_circleBuffer, tmpBegin, byteOut, 0, readLen);                    
                }
                else if (_circleEnd - tmpBegin <= 0 && _circleLength - System.Math.Abs(_circleEnd - tmpBegin) >= readLen)
                {
                    //-end-------------------------begin--//
                    Array.Copy(_circleBuffer, tmpBegin, byteOut, 0, _circleLength - tmpBegin);
                    Array.Copy(_circleBuffer, 0, byteOut, _circleLength - tmpBegin, readLen - (_circleLength - tmpBegin));
                    //Console.WriteLine("ReadData 3;  begin:{0}; end:{1}; readlen:{2}", tmpBegin, _circleEnd, readLen);
                }
                else
                {
                    //没有足够的空间可以读了
                    //Console.WriteLine("ReadData 4;  begin:{0}; end:{1}; readlen:{2}", tmpBegin, _circleEnd, readLen);
                    return false;
                }               

            }
            catch (System.Exception e)
            {
                //Console.WriteLine("ReadData 5;  begin:{0}; end:{1}; readlen:{2}", tmpBegin, _circleEnd, readLen);
                Console.WriteLine(e.Message);
            }

            return true;
        }

        /// <summary>
        /// 与ReadData函数配对的，将ReadData部分数据在内存中删除，否则ReadData将不删除内存中数据。
        /// </summary>
        /// <param name="byteOut"></param>
        /// <returns></returns>
        public bool SubmitReadData(ref byte[] byteOut)
        {
            return SubmitReadData(ref byteOut, byteOut.Length);
        }

        public bool SubmitReadData(ref byte[] byteOut, int readLen)
        {
            try
            {
                if (_circleSuffLength < readLen)
                {
                    return false; //空间没有足够要读数据了
                }
                if (_circleEnd - _circleBegin >= readLen)
                {
                    //------begin-----------------end-----//
                    //Console.WriteLine("SubmitReadData 1;  begin:{0}; end:{1}; readlen:{2}", _circleBegin, _circleEnd, readLen);
                    _circleBegin += readLen;
                }
                else if (_circleEnd - _circleBegin <= 0 && _circleLength - _circleBegin >= readLen)
                {
                    //------end-----------------begin-----//
                    //Console.WriteLine("SubmitReadData 2;  begin:{0}; end:{1}; readlen:{2}", _circleBegin, _circleEnd, readLen);
                    _circleBegin += readLen;
                }
                else if (_circleEnd - _circleBegin <= 0 && _circleLength - System.Math.Abs(_circleEnd - _circleBegin) >= readLen)
                {
                    //-end-------------------------begin--//
                    //Console.WriteLine("SubmitReadData 3;  begin:{0}; end:{1}; readlen:{2}", _circleBegin, _circleEnd, readLen);
                    _circleBegin = readLen - (_circleLength - _circleBegin);                    
                }
                else
                {
                    //没有足够的空间可以读了
                    //Console.WriteLine("SubmitReadData 4;  begin:{0}; end:{1}; readlen:{2}", _circleBegin, _circleEnd, readLen);
                    return false;
                }

                _circleSuffLength -= readLen;

            }
            catch (System.Exception e)
            {
                //Console.WriteLine("SubmitReadData 5;  begin:{0}; end:{1}; readlen:{2}", _circleBegin, _circleEnd, readLen);
                Console.WriteLine(e.Message);
            }

            return true;
        }

        /// <summary>
        /// 读取数据，同时在内存中删除这段数据
        /// </summary>
        /// <param name="byteOut"></param>
        /// <returns></returns>
        public bool FectchData(ref byte[] byteOut)
         {
             return FectchData(ref byteOut, byteOut.Length);
         }

        /// <summary>
        /// 读取数据，同时在内存中删除这段数据
        /// </summary>
        /// <param name="byteOut"></param>
        /// <param name="readLen"></param>
        /// <returns></returns>
        public bool FectchData(ref byte[] byteOut, int readLen)
        {
            try
            {
                if (_circleSuffLength < readLen)
                {
                    return false; //空间没有足够要读数据了
                }
                if (_circleEnd - _circleBegin >= readLen)
                {
                    //------begin-----------------end-----//
                    //Console.WriteLine("FectchData 1;  begin:{0}; end:{1}; readlen:{2}", _circleBegin, _circleEnd, readLen);
                    Array.Copy(_circleBuffer, _circleBegin, byteOut, 0, readLen);
                    _circleBegin += readLen;                    
                }
                else if (_circleEnd - _circleBegin <= 0 && _circleLength - _circleBegin >= readLen)
                {
                    //------end-----------------begin-----//
                    //Console.WriteLine("FectchData 2;  begin:{0}; end:{1}; readlen:{2}", _circleBegin, _circleEnd, readLen);
                    Array.Copy(_circleBuffer, _circleBegin, byteOut, 0, readLen);
                    _circleBegin += readLen;                    
                }
                else if (_circleEnd - _circleBegin <= 0 && _circleLength - System.Math.Abs(_circleEnd - _circleBegin) >= readLen)
                {
                    //-end-------------------------begin--//
                    Array.Copy(_circleBuffer, _circleBegin, byteOut, 0, _circleLength - _circleBegin);
                    Array.Copy(_circleBuffer, 0, byteOut, _circleLength - _circleBegin, readLen - (_circleLength - _circleBegin));
                    _circleBegin = readLen - (_circleLength - _circleBegin);
                    //Console.WriteLine("FectchData 3;  begin:{0}; end:{1}; readlen:{2}", _circleBegin, _circleEnd, readLen);
                }
                else
                {
                    //没有足够的空间可以读了
                    //Console.WriteLine("FectchData 4;  begin:{0}; end:{1}; readlen:{2}", _circleBegin, _circleEnd, readLen);
                    return false;
                }

                _circleSuffLength -= readLen;

            }
            catch (System.Exception e)
            {
                //Console.WriteLine("FectchData 5;  begin:{0}; end:{1}; readlen:{2}", _circleBegin, _circleEnd, readLen);
                Console.WriteLine(e.Message);
            }

            return true;
        }

        public int  Length
        {
            get
            {
                return _circleSuffLength;
            }
        }
    }

