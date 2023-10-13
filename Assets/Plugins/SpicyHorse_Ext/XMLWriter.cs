using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class XMLFileWriter
{
	public bool BindWithFile(string filename)
	{
        _iIdent = 0;

		FileStream file;

		try
		{
			if (File.Exists(filename))
			{
				file = new FileStream(filename, FileMode.Truncate, FileAccess.ReadWrite, FileShare.Read);
				
			}
			else
			{
				file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
			}

			_sw = new StreamWriter(file);
		}
		catch (System.Exception)
		{
			return false;
		}

		return true;
	}

	public void Flush()
	{
		_sw.Flush();
	}
	
	public void ShutDown()
	{
		_sw.Close();
	}

	public void NodeBegin(string strName)
	{
		if (_strCurNode.Length > 0)
			_sw.WriteLine(">");

		int i = _iIdent++;
		while (i-- != 0)
		{
			_sw.Write("\t");
		}

		_sw.Write("<" + strName + " ");
		_strCurNode = strName;
	}

	public void NodeEnd(string strName)
	{
		_iIdent--;

		if (_strCurNode == strName)
		{
			_sw.WriteLine("/>");
		}
		else
		{
			int i = _iIdent;
			while (i-- != 0)
			{
				_sw.Write("\t");
			}
			_sw.WriteLine("</" + strName + ">");
		}

		_strCurNode = "";
	}

	public void AddAttribute<T>(string strKey, T val)
	{
		_sw.Write(strKey + "=\"" + val.ToString() + "\" ");
	}

	public void AddAttribute(string strKey, float val)
	{
		_sw.Write(strKey + "=\"" + val.ToString("F4") + "\" ");
	}

	public void AddContent(string strContent)
	{
		if(_strCurNode.Length != 0)
			_sw.WriteLine(">");
		
		_strCurNode = "";

		_iIdent++;
		_sw.Write(strContent);
	}

	StreamWriter _sw;
    protected int _iIdent;
	protected string _strCurNode = "";
}

[ExecuteInEditMode]
public class XMLStringWriter
{
	public XMLStringWriter()
	{
		_iIdent = 0;
	}

	public void NodeBegin(string strName)
	{
		if (_strCurNode.Length > 0)
			Result += ">";

		int i = _iIdent++;
		while (i-- != 0)
		{
			Result += "\t";
		}

		Result += "<" + strName + " ";
		_strCurNode = strName;
	}

	public void NodeEnd(string strName)
	{
		_iIdent--;
		if(_iIdent < 0)
			_iIdent = 0;

		if (_strCurNode == strName)
		{
			Result += "/>";
		}
		else
		{
			int i = _iIdent;
			while (i-- != 0)
			{
				Result += "\t";
			}
			Result += "</" + strName + ">";
		}

		_strCurNode = "";
	}

	public void AddAttribute<T>(string strKey, T val)
	{
		Result += strKey + "=\"" + val.ToString() + "\" ";
	}

	public void AddAttribute(string strKey, float val)
	{
		Result += strKey + "=\"" + val.ToString("F4") + "\" ";
	}

	public string Result = "";

	protected int _iIdent;
	protected string _strCurNode = "";
}

