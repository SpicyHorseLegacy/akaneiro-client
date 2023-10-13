using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class DataObj : MonoBehaviour {
	
	public Dictionary<string,string> map = new Dictionary<string,string>();  
	
	public string dataFileName = "";
	
	void Awake() {
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
#region Local
	/// <summary>
	/// Inits the data.
	/// format key#value/n
	/// if no find dataFile file, init map use default val.
	/// </summary>
	/// <returns>
	/// success/fail
	/// </returns>
	private bool InitDefaultData() {
		GUILogManager.LogWarn("InitDefaultData");
		map.Clear();
		#region use unity player setting
//		GetFormPlayerSetting();
//		return true;
		#endregion
		#region use file
		TextAsset file = (TextAsset)Resources.Load(dataFileName, typeof(TextAsset));
		if(file == null) {
			GUILogManager.LogErr(dataFileName+"not exist.");
			return false;
		}
		string[] fileRowList = file.text.Split('\n');
		for (int i = 0; i < fileRowList.Length - 1; i++) {
			string pp = fileRowList[i];
			string[] vals = pp.Split('#');
			if(vals[0] != null) {
				map.Add(vals[0],vals[1].Substring(0,vals[1].Length-1));
			}
		}
		if(map.Count > 0) {
			return true;
		}else {
			GUILogManager.LogErr(dataFileName+"element count is 0");
			return false;
		}
		#endregion
	}
	
	/// <summary>
	/// Saves the data.
	/// save to file.
	/// </summary>
	public void SaveData() {
		#region use unity player setting
		SaveToPlayerSetting();
		#endregion
		
		#region use file
//		string filePath = DataManager.GetDataFilePath();
//		if (!Directory.Exists(filePath)) {
//            Directory.CreateDirectory(filePath);
//        }
//		DeleteFile(filePath,dataFileName);
//		CreateFile(filePath,dataFileName);
//		GUILogManager.LogWarn("save data ok. data name:"+dataFileName);
		#endregion
	}
	
	private void DeleteFile(string path,string name) {
		File.Delete(path+Path.DirectorySeparatorChar+ name+".txt");
	}
	
	private void SaveToPlayerSetting() {
		string infoLine = "";
		foreach(string key in map.Keys) {
			PlayerPrefs.SetString(key,map[key]);
		}
	}
	private void GetFormPlayerSetting() {
		List<string> list = new List<string>();
		list.AddRange(map.Keys);
		foreach(string key in list) {
			map[key] = PlayerPrefs.GetString(key);
		}
	}
	
	private void CreateFile(string path,string name) {
		StreamWriter sw;
		FileInfo t = new FileInfo(path+Path.DirectorySeparatorChar+ name+".txt");
		if(!t.Exists) {
			sw = t.CreateText();
		}else {
			sw = t.AppendText();
		}
		// save map info to txt
		string infoLine = "";
		foreach(string key in map.Keys) {
			infoLine = key+"#"+map[key];
			sw.WriteLine(infoLine);
		}
		sw.Close();
		sw.Dispose();
	}
	
	private bool LoadFile(string path,string name) {
		StreamReader sr =null;
		try {
			sr = File.OpenText(path+Path.DirectorySeparatorChar+ name+".txt");
		}catch(Exception e) {
			GUILogManager.LogErr("no find file:+"+path+Path.DirectorySeparatorChar+ name+".txt <"+e.ToString()+">");
			return false;
		}
		
			string line = "";
			while((line = sr.ReadLine()) != null) {
				string[] vals = line.Split('#');
				if(vals[0] != null) {
					try {
						map.Add(vals[0],vals[1]);
					}catch(Exception e) {
						GUILogManager.LogErr(e+"key:"+vals[0]+"#val:"+vals[1]);
					}
				}
			}
			sr.Close();
			sr.Dispose();
		
		return true;
	}
#endregion	
	
#region Interface
	public void InitData() {
		#region use unity player setting
		InitDefaultData();
		GetFormPlayerSetting();
		#endregion
		#region use file
//		string filePath = DataManager.GetDataFilePath();
//		bool isLoadOk = LoadFile(filePath,dataFileName);
//		if(!isLoadOk) {
//			InitDefaultData();
//		}
		#endregion
	}
	
	public void ShowMapElementInfo() {
		string info = "";
		GUILogManager.LogInfo("---<"+dataFileName+" element info"+">---");
		foreach(string key in map.Keys) {
			info = "[Key]: "+key+"[Val]: "+map[key];
			GUILogManager.LogInfo(info);
		}
		GUILogManager.LogInfo("------------------------------");
	}
	
	public void UpdateMapElement(string key,string val) {
		map[key] = val;
//		SaveData();
	}
	
	public string GetElementVal(string key) {
		return map[key];
	}
#endregion

}
