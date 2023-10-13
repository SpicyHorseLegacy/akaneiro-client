using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum TutorialEventsType{
	Empty = 0,
	Event1,
	Event2,
	Event3,
	Event4,
	Event5,
	Event6,
	Event7,
	Event8,
	Event9,
	Event10,
	Event11,
	Event12,
	Event13,
	Event14,
	Event15,
	Event16,
	Event17,
	Event18,
	Event19,
	Max
}

public class STutorialHunt {
	public int huntID;
	public int huntCount;
	public int curHuntCount;
	public TutorialEventsType nextEventType;
}

public class STutorialCollect {
	public int itemID;
	public int count;
	public int curCount;
	public TutorialEventsType nextEventType;
}

public class SObjectArrow {
	public int id;
	public string arrowKey;
	public TutorialEventsType nextEventType;
}


public class TutorialMan : MonoBehaviour {
	
	public static TutorialMan Instance = null;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		InitTutorialStr();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	[SerializeField]
	private TutorialEventsType curEventMode = TutorialEventsType.Empty;
	public void StartEvent(TutorialEventsType events) {
		curEventMode = events;
		curEvent = null;
		curEvent = GetCurEventObj(curEventMode);
		if(curEvent == null) {
			LogManager.Log_Error("This event type have problem. type: "+curEventMode.ToString());
			return;
		}
		curEvent.Init();
	}
	
	public void EndEvent() {
		if(curEvent == null) {
			return;
		}
		curEvent.End();
	}
	
	public void AddBranchEndFlag() {
		if(curEvent == null) {
			return;
		}
		curEvent.AddCompleteBranch();
	}
	
	[SerializeField]
	private bool isTutorialMode = false;
	public bool GetTutorialFlag() {
		return isTutorialMode;
	}
	public void SetTutorialFlag(bool isTut) {
		isTutorialMode = isTut;
	}
	
	[SerializeField]
	private  List<TutorialEvent> eventList = new List<TutorialEvent>();
	public void AddEvent(TutorialEvent tEvent) {
		if(curRoot == null){
			CreateRoot();
		}
		tEvent.transform.parent = curRoot;
		tEvent.transform.localPosition = Vector3.zero; 
		tEvent.transform.localScale = new Vector3(1,1,1);
		eventList.Add(tEvent);
//		GUILogManager.LogErr("AddEvent");
	}
	public void DelEvent(TutorialEvent tEvent) {
		eventList.Remove(tEvent);
		if(tEvent!=null) {
			Destroy(tEvent.gameObject);
		}
	}
	
	public void Reset() {
//		GUILogManager.LogErr("Reset");
		if(curRoot != null) {
			Destroy(curRoot.gameObject);
			//WTF ! FUCK ! missing != null//
			curRoot = null;
		}
		eventList.Clear();
		if(curRoot == null) {
			CreateRoot();
		}
	}
	
	#region Collect
	[SerializeField]
	private  List<STutorialCollect> collectList = new List<STutorialCollect>();
	public void AddCollectTarget(int id,int count,TutorialEventsType nextType) {
		STutorialCollect tCollect = new STutorialCollect();
		tCollect.curCount = 0;
		tCollect.count = count;
		tCollect.itemID = id;
		tCollect.nextEventType = nextType;
		if(!CheckCollectID(id)) {
			GUILogManager.LogErr("you want add already exist id.faile!");
			return;
		}
		collectList.Add(tCollect);
	}
	private bool CheckCollectID(int id) {
		foreach(STutorialCollect ele in collectList) {
			if(ele.itemID == id) {
				return false;
			}
		}
		return true;
	}
	public void UpdateTutorialCollectState(int id) {
		if(collectList.Count <= 0) {
			return;
		}
		foreach(STutorialCollect ele in collectList) {
			if(ele.itemID == id) {
				ele.curCount++;
//				GUILogManager.LogWarn("++:"+ele.curCount);
				if(ele.curCount >= ele.count) {
					StartEvent(ele.nextEventType);
					DelCollect(ele);
					return;
				}
			}
		}
	} 
	public void DelCollect(STutorialCollect ev) {
		collectList.Remove(ev);
	}
	public void ResetCollectList() {
		collectList.Clear();
	}
	#endregion
	
	#region Hunt
	[SerializeField]
	private  List<STutorialHunt> huntList = new List<STutorialHunt>();
	public void AddHuntTarget(int id,int count,TutorialEventsType nextType) {
		STutorialHunt tHunt = new STutorialHunt();
		tHunt.curHuntCount = 0;
		tHunt.huntCount = count;
		tHunt.huntID = id;
		tHunt.nextEventType = nextType;
		if(!CheckHuntID(id)) {
			GUILogManager.LogErr("you want add already exist id.faile!");
			return;
		}
		huntList.Add(tHunt);
	}
	private bool CheckHuntID(int id) {
		foreach(STutorialHunt ele in huntList) {
			if(ele.huntID == id) {
				return false;
			}
		}
		return true;
	}
	public void UpdateTutorialHuntState(int id) {
		if(huntList.Count <= 0) {
			return;
		}
		foreach(STutorialHunt ele in huntList) {
			if(ele.huntID == id) {
				ele.curHuntCount++;
//				GUILogManager.LogWarn("++:"+ele.curHuntCount);
				if(ele.curHuntCount >= ele.huntCount) {
					StartEvent(ele.nextEventType);
					DelHunt(ele);
					return;
				}
			}
		}
	}      
	public void DelHunt(STutorialHunt ev) {
		huntList.Remove(ev);
	}
	public void ResetHuntList() {
		huntList.Clear();
	}
	#endregion
	
	#region Obj Arrow
	[SerializeField]
	private  List<SObjectArrow> objArrowList = new List<SObjectArrow>();
	public void AddObjArrow(int id,string key,TutorialEventsType type) {
		SObjectArrow temp = new SObjectArrow();
		temp.id = id;
		temp.arrowKey = key;
		temp.nextEventType = type;
		objArrowList.Add(temp);
	}
	public void DelObjArrow(SObjectArrow obj) {
		objArrowList.Remove(obj);
	}
	public void ClearObjArrowList() {
		objArrowList.Clear();
	}
	public void UpdateObjArrow(int id,Transform tran,bool isWait) {
		if(objArrowList.Count <= 0) {
			return;
		}
		foreach(SObjectArrow tObj in objArrowList) {
			if(tObj.id == id) {
				if(isWait) {
					StartCoroutine(WaitAMoment(tObj.arrowKey,new Vector3(0,1,0),0,tran,"Default"));
				}else {
					CreateArrow3D(tObj.arrowKey,new Vector3(0,1,0),0,tran,"Default");
				}
				return;
			}
		}
	}
	public IEnumerator WaitAMoment(string key,Vector3 pos,float time,Transform parent,string layer) {	
		yield return new WaitForSeconds(0.5f);
		CreateArrow3D(key,pos,time,parent,layer);
	}
	
	public void UpdateObjArrowRemove(int id) {
		if(objArrowList.Count <= 0) {
			return;
		}
		foreach(SObjectArrow tObj in objArrowList) {
			if(tObj.id == id) {
				DelArrow3D(tObj.arrowKey);
				StartEvent(tObj.nextEventType);
				DelObjArrow(tObj);
				return;
			}
		}
	}
	#endregion
	
	#region 3D Arrow
	[SerializeField]
	private Transform Arrow3D;
	[SerializeField]
	private  List<Arrow3d> arrow3DList = new List<Arrow3d>();
	public void CreateArrow3D(string key,Vector3 pos,float time,Transform parent,string layer) {
		Transform obj  = (Transform)UnityEngine.Object.Instantiate(Arrow3D);
		obj.gameObject.AddComponent<Arrow3d>();
		obj.GetComponent<Arrow3d>().SetKey(key);
//		obj.GetComponent<Arrow3d>().SetPos(pos);
		obj.GetComponent<Arrow3d>().SetTime(time);
//		obj.GetComponent<Arrow3d>().gameObject.layer = LayerMask.NameToLayer(layer);
		foreach(Transform child in obj) {
			child.gameObject.layer = LayerMask.NameToLayer(layer);
		}
		
		obj.localScale = new Vector3(2,2,2);
		
		if(parent != null) {
			obj.parent = parent;
		}
//		Vector3 temp = new Vector3();
//		temp.x = pos.x;temp.y = pos.y+1;temp.z = pos.z;
		obj.GetComponent<Arrow3d>().SetPos(pos);
		arrow3DList.Add(obj.GetComponent<Arrow3d>());
	}
	public void DelArrow3D(string key) {
		foreach(Arrow3d arrow in arrow3DList) {
			if(string.Compare(key,arrow.GetKey()) == 0) {
				arrow3DList.Remove(arrow);
				Destroy(arrow.gameObject);
				return;
			}
		}
	}
	public void ClearArrowList() {
		for(int i = arrow3DList.Count-1;i>=0;i--) {
			Destroy(arrow3DList[i].gameObject);
			arrow3DList.RemoveAt(i);
		}
	}
	#endregion

	#endregion
	
	#region Local
	[SerializeField]
	private TutorialEvent curEvent = null;
	private TutorialEvent GetCurEventObj(TutorialEventsType type) {
		foreach(TutorialEvent ev in eventList) {
			if(ev.CompareEventType(type)) {
				return ev;
			}
		}
		return null;
	}
	
	private Transform curRoot = null;
	private void CreateRoot(){
//		LogManager.Log_Error("CreateRoot");
		GameObject obj  = new GameObject();
		obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero; 
        obj.transform.localScale= new Vector3(1,1,1);
		curRoot = obj.transform;
	}
	
	public struct STutorialStr {
		public int id;
		public string  title;
		public string  content;
	}
	public List<STutorialStr> _STutorialStrList = new List<STutorialStr>();	
	private void InitTutorialStr() {
		_STutorialStrList.Clear();
		string 		_fileName = "";
		TextAsset 	item;
		_fileName = LocalizeManage.Instance.GetLangPath("TutorialString.string");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i) {
			STutorialStr temp = new STutorialStr();
			string pp = itemRowsList[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			if(string.Compare(vals[0],"") != 0) {
				temp.id = int.Parse(vals[0]);
			}else {
				temp.id = 0;
			}
			temp.title		= vals[1];
			temp.content	= vals[2];
			_STutorialStrList.Add(temp);
		}
	}
	public string GetTutorialStrTitle(int id) {
		foreach(STutorialStr temp in _STutorialStrList) {
			if(temp.id == id) {
				return temp.title;
			}
		}
		return "";
	}
	public string GetTutorialStrContent(int id) {
		foreach(STutorialStr temp in _STutorialStrList) {
			if(temp.id == id) {
				return temp.content;
			}
		}
		return "";
	}
	#endregion
	
}
