using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class CSVHelper{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static void WriteCSV(string filePathName,List<string[]>ls)
	{
		WriteCSV(filePathName,false,ls);
	}
	
	public static void WriteCSV(string filePathName,bool append,List<string[]>ls)
	{
		StreamWriter fileWriter = new StreamWriter(filePathName,append,Encoding.Default);
		foreach(string[] strArr in ls)
		{
			fileWriter.WriteLine(string.Join(",",strArr));
		}
		fileWriter.Flush();
		fileWriter.Close();
	}
	
	public static List<string[]> ReadCSV(string filePathName)
	{
		List<string[]>ls = new List<string[]>();
		StreamReader fileReader = new StreamReader(filePathName);
		string strLine="";
		while(strLine != null)
		{
			strLine = fileReader.ReadLine();
			if(strLine != null && strLine.Length > 0)
			{
				ls.Add(strLine.Split(','));
				 
			}
		}
		fileReader.Close();
		return ls;
	}
	
	public static List<string[]> ReadCSVAssert(string strName)
	{
		TextAsset item = (TextAsset)Resources.Load(strName, typeof(TextAsset));
		
		List<string[]>ls = new List<string[]>();
		
        if(item != null)
		{
	        string[] itemRowsList = item.text.Split('\n');
			
			foreach( string it in itemRowsList)
			{
				ls.Add(it.Split('\t'));
			}
		}
	
		return ls;
	}
	
   
	
	
}
