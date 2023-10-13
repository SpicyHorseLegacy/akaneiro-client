using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LogicManager : MonoBehaviour {
	
	public static LogicManager	Instance;
	
	//screen obj,this obj is screen logic
	public List<ScreenObj> screenLogicObjList = new List<ScreenObj>();
	
	void Awake() {
		Instance = this;
		CheckScreenLogicObjIsExsit();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
#region Local
	/// <summary>
	/// Checks the screen logic object is exsit.
	/// </summary>
	/// <returns>
	/// The screen logic object is exsit.
	/// </returns>
	private void CheckScreenLogicObjIsExsit() {
		TextAsset file = (TextAsset)Resources.Load(UIConfig.filePath, typeof(TextAsset));
		if(file == null) {
			GUILogManager.LogErr("UIConfig not exist.");
			return;
		}
		string[] fileRowList = file.text.Split('\n');
		for (int i = 3; i < fileRowList.Length - 1; i++) {
			string pp = fileRowList[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			if(vals[0] != null) {
				ObjIsExist(vals[0]);
			}
		}
	}	
	private void ObjIsExist(string screenName) {
		foreach(ScreenObj obj in screenLogicObjList) {
			if(string.Compare(obj.screenName,screenName) == 0) {
				return;
			}
		}
		GUILogManager.LogErr("can't find screen obj ["+screenName+"]");
	}
#endregion
}
