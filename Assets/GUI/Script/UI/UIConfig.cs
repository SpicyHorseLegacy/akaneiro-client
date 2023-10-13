using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class STemplateInfo {
	//this name is bundle name.
	public string 		templateName;
	//belong to 
	public string 		screenName;
	public Vector3 pos;
	public Vector3 scale;
	public bool isHide;
	public bool isDrag;
	public bool preLoad;
}

public class UIConfig : MonoBehaviour {
	
	public static UIConfig	Instance;
	
	void Awake () {
		Instance = this;
		InitConfigFile(filePath);
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

#region Init
	private const string fileRootStr = "UIConfig.";
	public static string filePath = "UIConfig.root";
	
	private List<string> screenNameList	= new List<string>();
	
	private bool InitConfigFile(string filePath) {
		screenNameList.Clear();
		TextAsset file = (TextAsset)Resources.Load(filePath, typeof(TextAsset));
		if(file == null) {
			GUILogManager.LogErr("UIConfig not exist.");
			return false;
		}
		string[] fileRowList = file.text.Split('\n');
		for (int i = 3; i < fileRowList.Length - 1; i++) {
			string pp = fileRowList[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			if(vals[0] != null) {
				screenNameList.Add(vals[0]);
			}
		}
		if(screenNameList.Count > 0) {
			return true;
		}else {
			GUILogManager.LogErr("screen file count is 0");
			return false;
		}
	}

	private List<STemplateInfo> templateInfoList = new List<STemplateInfo>();
	
	public void InitScreenFile(string screenName) {
		templateInfoList.Clear();
		Vector3 tempVec = Vector3.zero;
		string strFileName = fileRootStr+screenName;
		TextAsset file = null;
		file = (TextAsset)Resources.Load(strFileName, typeof(TextAsset));
		if(file == null) {
			GUILogManager.LogErr("screen file not exist. file name: <"+strFileName+">");
		}
		string[] fileRowList = file.text.Split('\n');
		for (int j = 3; j < fileRowList.Length - 1; j++) {
			string pp = fileRowList[j];
			string[] vals = pp.Split(new char[] { '	', '	' });
			if(vals[0] != null) {
				STemplateInfo temp  = new  STemplateInfo();
				temp.screenName		= screenName;
				temp.templateName	= vals[0];
				//Pos//
				tempVec.x = float.Parse(vals[1]);tempVec.y = float.Parse(vals[2]);tempVec.z = float.Parse(vals[3]);
				temp.pos = tempVec;
				//isHide//
				if(int.Parse(vals[4])==1) {
					temp.isHide = true;
				}else {
					temp.isHide = false;
				}
				//isDrag//
				if(int.Parse(vals[5])==1) {
					temp.isDrag = true;
				}else {
					temp.isDrag = false;
				}
				//Scale//
				tempVec.x = float.Parse(vals[6]);tempVec.y = float.Parse(vals[7]);tempVec.z = float.Parse(vals[8]);
				temp.scale = tempVec;
				//preload//
				if(int.Parse(vals[9]) == 1) {
					temp.preLoad = true;
				}else {
					temp.preLoad = false;
				}
				templateInfoList.Add(temp);
			}
		}
	}
#endregion
	
#region Interface
	public void GetCurrentScreenTemplateList(string screenName) {
		GUIManager.Instance.currScreenTemplateInfoList.Clear();		
		for(int i = 0;i<templateInfoList.Count;i++) {
			if(string.Compare(templateInfoList[i].screenName,screenName) == 0) {
				GUIManager.Instance.currScreenTemplateInfoList.Add(templateInfoList[i]);
			}
		}
	}
#endregion
	
}
