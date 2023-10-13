using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum DataListType {
	PlayerData = 0,
	AccountData = 1,
	DataMax
}

public class DataManager : MonoBehaviour {
	
	public static DataManager	Instance;
	public Transform template;
	public List<Transform> DataList = new List<Transform>();
	
	void Awake () {
		Instance = this;
		InitDataObj();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
#region Local
	private string fileName = "DataManger.DataObj";
	private void InitDataObj() {
		DataList.Clear();
		#region use file
		TextAsset file = null;
		file = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
		if(file == null) {
			GUILogManager.LogErr("data file not exist. file name: <"+fileName+">");
		}
		string[] fileRowList = file.text.Split('\n');
		for (int j = 3; j < fileRowList.Length -1; j++) {
			string pp = fileRowList[j];
			string[] vals = pp.Split(new char[] { '	', '	' });
			if(vals[0] != null) {
				Transform tran = (Transform)UnityEngine.Object.Instantiate(template);
				tran.GetComponent<DataObj>().dataFileName = vals[0];
				tran.gameObject.AddComponent<DontDestoryOnLoad>();
				tran.parent = transform;
				DataList.Add(tran);
				tran.GetComponent<DataObj>().InitData();
				#region show list
				//for test, show data list.//
//				tran.GetComponent<DataObj>().ShowMapElementInfo();
				#endregion
			}
		}
		#endregion
	}
#endregion	
	
#region Interface
	#region save
	public void Save(string dataName) {
		foreach(Transform obj in DataList) {
			if(string.Compare(obj.GetComponent<DataObj>().dataFileName,dataName) == 0) {
				obj.GetComponent<DataObj>().SaveData();
				return;
			}
		}
		GUILogManager.LogErr("cant find data: <"+dataName+">");
	}
	public void Save(DataListType type) {
		try{
			DataList[(int)type].GetComponent<DataObj>().SaveData();
		}catch(System.Exception e) {
			GUILogManager.LogErr("Save fail: <"+type+">");
		}
	}
	#endregion
	
	public static string GetDataFilePath() {
		string path =	"Assets"+Path.DirectorySeparatorChar+
						"GUI"+Path.DirectorySeparatorChar+
//						"DataFile";
						"Resources";
		return path;
	}
	
	#region Get Element Value
	public string GetMapValue(string dataName,string key) {
		foreach(Transform obj in DataList) {
			if(string.Compare(obj.GetComponent<DataObj>().dataFileName,dataName) == 0) {
				return obj.GetComponent<DataObj>().GetElementVal(key);
			}
		}
		GUILogManager.LogErr("cant find data: <"+dataName+">|<"+key+">");
		return "0";
	}
	public string GetMapValue(DataListType type,string key) {
		try{
			return DataList[(int)type].GetComponent<DataObj>().GetElementVal(key);
		}catch(System.Exception e) {
			GUILogManager.LogErr("GetMapValue fail: <"+type+">|<"+key+">");
			return "0";
		}
	}
	#endregion
	#region Update Element Value
	public void UpdateMapValue(string dataName,string key,string val) {
		foreach(Transform obj in DataList) {
			if(string.Compare(obj.GetComponent<DataObj>().dataFileName,dataName) == 0) {
				obj.GetComponent<DataObj>().UpdateMapElement(key,val);
				return;
			}
		}
		GUILogManager.LogErr("cant find data: <"+dataName+">|<"+key+">|<"+val+">");
	}
	public void UpdateValue(DataListType type,string key,string val) {
		try{
			DataList[(int)type].GetComponent<DataObj>().UpdateMapElement(key,val);
		}catch(System.Exception e) {
			GUILogManager.LogErr("UpdateValue fail: <"+type+">|<"+key+">|<"+val+">");
		}
	}
	#endregion
#endregion
	
}
