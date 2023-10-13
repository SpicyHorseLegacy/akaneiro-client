using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MissionObjectiveManager : MonoBehaviour {
	
	public static MissionObjectiveManager Instance;

	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		//for test//
//		UpdateMissionObjList();
//		InitFirstAniData();
//		InitFirstListObjPos();
	}
	
	// Update is called once per frame
	void Update () {
//		if(Input.GetKeyDown(KeyCode.A)) {
//			PlayMissionAni();
//		}
	}
	
	#region Interface 
	public void RemoveDelegate() {
		foreach(BranchCtrl obj in list) {
			obj.RemoveDelegate();
		}
	}
	
	[SerializeField]
	private Transform root;
	private List<BranchCtrl> list = new List<BranchCtrl>();
	
	public int elementCount = 0;
	
	[SerializeField]
	private Transform branchPrefab;
	public void UpdateMissionObjList() {
		GUILogManager.LogInfo("UpdateMissionObjList branchList count|"+PlayerDataManager.Instance.branchList.Count);
		elementCount = 0;
		DestoryMissionList();
		for(int i = 0;i<PlayerDataManager.Instance.branchList.Count;i++) {
			GameObject obj  =(GameObject)Instantiate(branchPrefab.gameObject);
			obj.transform.parent = root.transform;
			obj.transform.localPosition = new Vector3(0,-(elementCount*50),0);
			obj.transform.localScale= new Vector3(1,1,1);
			obj.GetComponent<BranchCtrl>().move.localPosition = new Vector3(0,0,0);
			obj.GetComponent<BranchCtrl>().InitBranch(PlayerDataManager.Instance.branchList[i]);
			
			obj.GetComponent<BranchCtrl>().OnMoveInDelegate += BranchMoveDelegate;
			obj.GetComponent<BranchCtrl>().OnTaskMoveOutDelegate += TaskMoveOutDelegate;
			
			list.Add(obj.GetComponent<BranchCtrl>());
			elementCount++;
		}
	}
	
	public List<int> branchAniList = new List<int>();
	public List<TaskCompareData> taskAniList = new List<TaskCompareData>();
	public void SetBranchAniID(int id) {
		branchAniList.Add(id);
	}
	public void SetTaskAniID(TaskCompareData data) {
		taskAniList.Add(data);
	}
	public void ResetAniList() {
		branchAniList.Clear();
		taskAniList.Clear();
	}
	private bool CheckBranchAniID(int id) {
		foreach(int _id in branchAniList) {
			if(id == _id) {
				return true;
			}
		}
		return false;
	}
	private bool CheckTaskAniID(int type,int objID,int recycle) {
		foreach(TaskCompareData data in taskAniList) {
			if(data.type == type&&data.objID == objID&&data.recycke == recycle) {
				return true;
			}
		}
		return false;
	}
	public void PlayMissionAni() {
		foreach(BranchCtrl branchObj in list) {
			if(CheckBranchAniID(branchObj.branchData.BranchID)) {
				branchObj.ShowBranch();
			}else {
				foreach(TaskCtrl taskObj in branchObj.list) {
					if(CheckTaskAniID((int)taskObj.data.typeID,taskObj.data.objectID,taskObj.data.recycle)) {
						taskObj.FadeIn();
					}
				}
			}
		}
		StartCoroutine(MissionIconAniStart());
	}
	public void InitFirstAniData() {
		ResetAniList();
		SetIsPlatIconAni(true);
		foreach(BranchCtrl branch in list) {
			branchAniList.Add(branch.branchData.BranchID);
		}
	}
	
	[SerializeField]
	private Transform icon;
	public bool isPlayIconAni = false;
	public void SetIsPlatIconAni(bool isPlay) {
		isPlayIconAni = isPlay;
		if(isPlay) {
			icon.localScale = new Vector3(2,2,1);
		}
	}
	public void PlayIconAni() {
		GUILogManager.LogInfo("Play Mission Icon Animation");
		if(!isPlayIconAni) {
			return;
		}
		icon.animation.Play();
	}
	
	public void InitFirstListObjPos() {
		foreach(BranchCtrl branch in list) {
			branch.move.localPosition = new Vector3(300,0,0);
			foreach(TaskCtrl task in branch.list) {
				task.task.localPosition = new Vector3(300,0,0);
			}
		}
	}
	
	public void PlayWantedPanel(string name,int xp,int karma) {
		WantedkilledCtrl.Instance.SetWantedName(name);
		WantedkilledCtrl.Instance.SetXpVal(xp.ToString());
		WantedkilledCtrl.Instance.SetKarmaVal(karma.ToString());
		WantedkilledCtrl.Instance.Play();
	}
	
	public void PlayTaskCompletePanel(string name,int xp,int karma) {
		TaskCompleteCtrl.Instance.SetXpVal(xp.ToString());
		TaskCompleteCtrl.Instance.SetTaskName(name);
		TaskCompleteCtrl.Instance.SetKarmaVal(karma.ToString());
		TaskCompleteCtrl.Instance.Play();
	}
	
	public void ChangeTaskCurVal(int branchIdx,int taskIdx,int val) {
//		list[branchIdx].list[taskIdx].data.CurrentVal = val;
//		list[branchIdx].list[taskIdx].SetContent(val,list[branchIdx].list[taskIdx].data.count,list[branchIdx].list[taskIdx].data.description);
		UpdateMissionObjList();
	}
	
	public void RemoveTask(int branchIdx,int taskIdx) {
		list[branchIdx].list[taskIdx].FadeOut();
		if(list[branchIdx].list[taskIdx].data != null) {
			PlayTaskCompletePanel(list[branchIdx].list[taskIdx].data.description,0,0);
		}
		
	}
	
	public void FadeInBackCrystal() {
		BackCrystalCtrl.Instance.Play();
	}
	#endregion
	
	#region Local
	private void BranchMoveDelegate() {
		
	}
	
	public bool isUpdateMissionList = false;
	private void TaskMoveOutDelegate() {
		if(isUpdateMissionList) {
			UpdateMissionObjList();
		}
	}
	
	private IEnumerator MissionIconAniStart() {
		yield return new WaitForSeconds(1);
		PlayIconAni();
	}
	
	private void DestoryMissionList() {
		for(int i = list.Count-1;i>=0;i--) {
			Destroy(list[i].gameObject);
		}
		list.Clear();
	}
	#endregion
}

public class TaskCompareData {
	public int type;
	public int objID;
	public int recycke;
}
