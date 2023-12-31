/*=============================================================================
*	Copyright (C) 2006-2011, Zhang Kun. All Rights Reserved.
*	Generated by: ProtocolGen 1.3
=============================================================================*/
//
using System;
//
using System.Threading;
//
using System.Collections.Generic;

	//
	public class vectorInt : System.Collections.ArrayList
	{
		public static void Write(Packet os, vectorInt v)
		{
			os.WriteInt(v.Count);
			foreach (int m in v)
			{
				os.WriteInt(m);
			}
		}
		public static void Read(Packet ins, out vectorInt v)
		{
			v = new vectorInt();
			int count = ins.ReadInt();
			for (int i = 0; i < count; i++)
			{
				int	m;
				m = ins.ReadInt();
				v.Add(m);
			}
		}
	}

	//Short
	public class vectorShort : System.Collections.ArrayList
	{
		public static void Write(Packet os, vectorShort v)
		{
			os.WriteInt(v.Count);
			foreach (short m in v)
			{
				os.WriteShort(m);
			}
		}
		public static void Read(Packet ins, out vectorShort v)
		{
			v = new vectorShort();
			int count = ins.ReadInt();
			for (int i = 0; i < count; i++)
			{
				short	m;
				m = ins.ReadShort();
				v.Add(m);
			}
		}
	}

	//int 64
	public class vectorInt64 : System.Collections.ArrayList
	{
		public static void Write(Packet os, vectorInt64 v)
		{
			os.WriteInt(v.Count);
			foreach (long m in v)
			{
				os.WriteLong(m);
			}
		}
		public static void Read(Packet ins, out vectorInt64 v)
		{
			v = new vectorInt64();
			int count = ins.ReadInt();
			for (int i = 0; i < count; i++)
			{
				long m;
				m = ins.ReadLong();
				v.Add(m);
			}
		}
	}

	//unsigned int 64
	public class vectorUint64 : System.Collections.ArrayList
	{
		public static void Write(Packet os, vectorUint64 v)
		{
			os.WriteInt(v.Count);
			foreach (ulong m in v)
			{
				os.WriteUint64(m);
			}
		}
		public static void Read(Packet ins, out vectorUint64 v)
		{
			v = new vectorUint64();
			int count = ins.ReadInt();
			for (int i = 0; i < count; i++)
			{
				ulong m;
				m = ins.ReadUint64();
				v.Add(m);
			}
		}
	}

	//Cactus::String
	public class vectorString : System.Collections.ArrayList
	{
		public static void Write(Packet os, vectorString v)
		{
			os.WriteInt(v.Count);
			foreach (string m in v)
			{
				os.WriteString(m);
			}
		}
		public static void Read(Packet ins, out vectorString v)
		{
			v = new vectorString();
			int count = ins.ReadInt();
			for (int i = 0; i < count; i++)
			{
				string m;
				m = ins.ReadString();
				v.Add(m);
			}
		}
	}

	//int-int map
	public class mapIntInt : Dictionary<int, int>
	{
		public static void Write(Packet os, mapIntInt v)
		{
			os.WriteInt(v.Count);
			foreach (KeyValuePair<int, int> m in v)
			{
				os.WriteInt(m.Key);
				os.WriteInt(m.Value);
			}
		}
		public static void Read(Packet ins, out mapIntInt v)
		{
			v = new mapIntInt();
			int count = ins.ReadInt();
			for (int i = 0; i < count; i++)
			{
				int	tempKey;
				tempKey = ins.ReadInt();

				int	tempValue;
				tempValue = ins.ReadInt();

				v.Add(tempKey, tempValue);
			}
		}
	}


