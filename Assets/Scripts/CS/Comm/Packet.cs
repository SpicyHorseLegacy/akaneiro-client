using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

/*
	base_ptr	rd_ptr		   wr_ptr
	|			|			   |
	|___________|___remaining__|____________________
	|_________used_size________|_______space_______|
	|_________________alloc_space__________________|
*/


public class OutOfBoundException : System.Exception
{
    public OutOfBoundException()
        : base("Exceeds the size of the buf")
    {
    }
}


public class Packet
{
	//用于写
    public Packet()
    {
        _pbase       = 0;
        _readpointer = 0;
        _used_size   = 0;
        _writer      = new BinaryWriter(new MemoryStream());
    }

	//用于读
    public Packet(byte[] stream)
    {
        _pbase          = 0;
        _readpointer    = 0;
        _used_size      = stream.Length;
        _reader         = new BinaryReader(new MemoryStream(stream));

		_readRawData = stream;
    }

    BinaryReader        _reader;
    BinaryWriter        _writer;


    /* 空间的起始位置 */
    protected int           _pbase;

    /* 当前的位置 */
    protected int           _readpointer;

    /* 实际数据块的大小，也是写指针的位置 */
    protected int           _used_size;

    protected byte[]        _stringBytes;

    protected byte[] _readRawData;


	public byte[]	GetReadRawData()
	{
		return _readRawData;
	}

    public static System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding(false, true);

	//打包好后，取出数据
    public byte[]   GetByteStream()
    {
        byte[] bytestr = new byte[_used_size];
        System.Buffer.BlockCopy(((MemoryStream)_writer.BaseStream).GetBuffer(), 0, bytestr, 0, _used_size);

        return bytestr;
    }

    public void     SetByteStream(byte[] arr)
    {
        _writer.Write(arr);
        _used_size = arr.Length;
    }

    public int      get_remaining()
    {
        return _used_size - (_readpointer - _pbase);

    }

    public int      get_used_size()
    {
        return _used_size;
    }

    public int      Patch(int v)
    {
        return Patch(v, 0);
    }

    public int      Patch(int v, int pos)
    {
        Int64 curpos = _writer.Seek(pos, SeekOrigin.Begin);
        if (curpos > _used_size)
        {
            return 0;
        }

        _writer.Write(v);

        return 1;
    }

	// 将Packet内容转换为Base64，写入str
	public void		readOutToBase64String(out string str)
	{
		byte[] bytestr = new byte[_used_size];
		System.Buffer.BlockCopy(((MemoryStream)_writer.BaseStream).GetBuffer(), 0, bytestr, 0, _used_size);

		str = System.Convert.ToBase64String(bytestr);
	}

	// 将Base6编码的str写入Packet
	public void		writeIntoBase64String(string str)
	{
		byte[] stream = System.Convert.FromBase64String(str);

		_pbase = 0;
		_readpointer = 0;
		_used_size = stream.Length;
		_reader = new BinaryReader(new MemoryStream(stream));

		_readRawData = stream;
	}


    protected void  checkUnderflow(int size)
    {
        if (_readpointer + size > _used_size)
            throw new OutOfBoundException();
    }


    #region Specific types of reading and writing
    /// <summary>
    /// Specific types of reading and writing
    /// </summary>
    /// <param name="v"></param>
    public void         WriteInt(int v)
    {
        _writer.Write(v);
        _used_size += 4;
    }
    public int          ReadInt()
    {
        checkUnderflow(4);
        int ret = _reader.ReadInt32();
        _readpointer += 4;
        return ret;
    }

    public void         WriteUint(uint v)
    {
        _writer.Write(v);
        _used_size += 4;
    }
    public uint         ReadUint()
    {
        checkUnderflow(4);
        uint ret;
        ret = _reader.ReadUInt32();
        _readpointer += 4;
        return ret;
    }

    public float        ReadFloat()
    {
        checkUnderflow(4);   
        float ret = _reader.ReadSingle();
        _readpointer += 4;
        return ret;
    }
    public void         WriteFloat(float v)
    {
        _writer.Write(v);
        _used_size += 4;
    }

    public double       ReadDouble()
    {
        checkUnderflow(8);
        double ret;
        ret = _reader.ReadDouble();
        _readpointer += 8;
        return ret;
    }
    public void         WriteDouble(double v)
    {
        _writer.Write(v);
        _used_size += 8;
    }

    public long         ReadLong()
    {
        checkUnderflow(8);
        long ret = _reader.ReadInt64();
        _readpointer += 8;
        return ret;
    }
    public void         WriteLong(long v)
    {
        _writer.Write(v);
        _used_size += 8;
    }

    // char不一定是占2个字节，具体要看stream编码
    //public char         ReadChar()
    //{
    //    checkUnderflow(2);
    //    char ret = _reader.ReadChar();
    //    _readpointer += 2;
    //    return ret;
    //}
    //public void         WriteChar(char v)
    //{
    //    _writer.Write(v);
    //    _used_size += 2;
    //}

    public byte         ReadByte()
    {
        checkUnderflow(1);
        byte ret = _reader.ReadByte();
        return ret;
    }
    public void         WriteByte(byte v)
    {

        _writer.Write(v);
        _used_size += 1;
    }

    public short        ReadShort()
    {
        checkUnderflow(2);
        short ret = _reader.ReadInt16(); ;
        _readpointer += 2;
        return ret;
    }
    public void         WriteShort(short v)
    {
        _writer.Write(v);
        _used_size += 2;
    }

    public ushort       ReadUshort()
    {
        checkUnderflow(2);
        ushort ret = _reader.ReadUInt16(); ;
        _readpointer += 2;
        return ret;
    }
    public void         WriteUshort(ushort v)
    {
        _writer.Write(v);
        _used_size += 2;
    }

    public bool         ReadBool()
    {
        checkUnderflow(1);
        bool ret = _reader.ReadBoolean();
        _readpointer += 1;
        return ret;
    }

    public void         WriteBool(bool v)
    {
        _writer.Write(v);
        _used_size += 1;
    }

    public string       ReadString()
    {
        checkUnderflow(4);
        int length = ReadInt();
        if (length > get_remaining())
        {
            throw new OutOfBoundException();
        }
        if (length == 0)
            return "";

        if (_stringBytes == null || length > _stringBytes.Length)
        {
            _stringBytes = new byte[length];
        }
        _stringBytes = _reader.ReadBytes(length);

        return utf8.GetString(_stringBytes, 0, length);
    }
    public void         WriteString(string v)
    {
        if (v == null)
            v = "";
        byte[] arr = utf8.GetBytes(v);
        WriteInt(arr.Length);
        _writer.Write(arr);
        _used_size += arr.Length;
    }

    public SByte        ReadInt8()
    {
        checkUnderflow(1);
        SByte ret = _reader.ReadSByte();
        _readpointer += 1;

        return ret;
    }
    public void         WriteInt8(SByte v)
    {
        _writer.Write(v);
        _used_size += 1;
    }

    public Byte         ReadUint8()
    {
        return ReadByte();
    }
    public void         WriteUint8(Byte v)
    {
        WriteByte(v);
    }

    public Int16        ReadInt16()
    {
        return ReadShort();
    }
    public void         WriteInt16(Int16 v)
    {
        WriteShort(v);
    }

    public UInt16       ReadUint16()
    {
        return ReadUshort();
    }
    public void         WriteUint16(UInt16 v)
    {
        WriteUshort(v);
    }

    public Int64        ReadInt64()
    {
        checkUnderflow(8);
        Int64 ret = _reader.ReadInt64();
        _readpointer += 8;

        return ret;
    }
    public void         WriteInt64(Int64 v)
    {
        _writer.Write(v);
        _used_size += 8;
    }

    public UInt64       ReadUint64()
    {
        checkUnderflow(8);
        UInt64 ret = _reader.ReadUInt64();
        _readpointer += 8;

        return ret;
    }
    public void         WriteUint64(UInt64 v)
    {
        _writer.Write(v);
        _used_size += 8;
    }


    public NatureTime   ReadNatureTime()
    {
        return new NatureTime(ReadInt());
    }
    public void         WriteNatureTime(NatureTime v)
    {
        WriteInt(v.GetTotalSecond());
    }


    public Packet       ReadPacket()
    {
        int length = ReadInt();
        if (length > get_remaining())
        {
            throw new OutOfBoundException();
        }
        if (length == 0)
            return null;

        byte[] bitearry = _reader.ReadBytes(length);
        Packet v = new Packet(bitearry);

        _readpointer += length;

        return v;

    }
    public void         WritePacket(Packet v)
    {
        byte[] arr = v.GetByteStream();
        WriteInt(arr.Length);
        _writer.Write(arr);
        _used_size += arr.Length;

    }

    #endregion



}
