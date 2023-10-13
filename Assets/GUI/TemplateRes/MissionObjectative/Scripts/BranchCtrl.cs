using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BranchCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private TaskCtrl taskPrefab;
	public List<TaskCtrl> list = new List<TaskCtrl>();
	
	#region Interface
	[SerializeField]
	private Transform root;

	public void InitBranch(_UI_CS_Branch data) {
		branchData = data;
		SetContent(data.BranchName);
		//init tasks//
		for(int i = 0;i<data.taskArray[0].SubObject.Count;i++) {
			AddTasks(data.taskArray[0].SubObject[i],i);
		}
	}
	
	public void AddTasks(_UI_CS_RamusTask info,int idx) {
		MissionObjectiveManager.Instance.elementCount++;
		//create obj//
		GameObject obj  =(GameObject)Instantiate(taskPrefab.gameObject);
		obj.transform.parent = root.transform;
		obj.transform.localPosition = new Vector3(0,-((idx)*50),0);
//        obj.transform.localPosition = new Vector3(0,-((list.Count+1)*50),0); 
        obj.transform.localScale= new Vector3(1,1,1);
		obj.GetComponent<TaskCtrl>().task.localPosition = new Vector3(0,0,0);
		//update data//
		obj.GetComponent<TaskCtrl>().data = info;
		obj.GetComponent<TaskCtrl>().SetContent(info.CurrentVal,info.count,info.description);
		//delegate
		obj.GetComponent<TaskCtrl>().OnMoveInDelegate += TaskMoveInDelegate;
		obj.GetComponent<TaskCtrl>().OnMoveOutDelegate += TaskMoveOutDelegate;
		//add to list//
		list.Add(obj.GetComponent<TaskCtrl>());  
	}
	
	[SerializeField]
	private UILabel content;
	public void SetContent(string str) {
		content.text = str;
        PlayerDataManager.Instance.SetMissionName(str);
	}
	
	public _UI_CS_Branch branchData;
	
	public Transform move;
	public void ShowBranch() {
		move.localPosition = new Vector3(300,0,0);
		SetAllTaskAniPos();
		TweenPosition.Begin(move.gameObject,0.5f,new Vector3(0,0,0));
		move.GetComponent<TweenPosition>().eventReceiver = gameObject;
		move.GetComponent<TweenPosition>().callWhenFinished = "TasksMove";
	}
	
	private void SetAllTaskAniPos() {
		foreach(TaskCtrl taskObj in list) {
			taskObj.SetTaskAniPos();
		}
	}
	
	private void TaskMoveInDelegate() {
	}
	
	public delegate void Handle_TaskMoveOutDelegate();
    public event Handle_TaskMoveOutDelegate OnTaskMoveOutDelegate;
	private void TaskMoveOutDelegate() {
		if(OnTaskMoveOutDelegate != null) {
			OnTaskMoveOutDelegate();
		}
	}
	
	public void RemoveDelegate() {
		foreach(TaskCtrl obj in list) {
			obj.OnMoveInDelegate -= TaskMoveInDelegate;
			obj.OnMoveOutDelegate -= TaskMoveOutDelegate;
		}
	}
	
	public delegate void Handle_MoveInDelegate();
    public event Handle_MoveInDelegate OnMoveInDelegate;
	private void MoveInDelegate() {
		if(OnMoveInDelegate != null) {
			OnMoveInDelegate();
		}
	}
	
	public void TasksMove() {
		int i = 0;
		for(i = 0;i< list.Count;i++) {
			StartCoroutine(TaskStart(list[i],i));
		}
		StartCoroutine(TaskMoveInSuccess(i));
	}
	
	private IEnumerator TaskStart(TaskCtrl task,float time) {
		yield return new WaitForSeconds(time);
		task.FadeIn();
	}
	private IEnumerator TaskMoveInSuccess(float time) {
		yield return new WaitForSeconds(time);
		MoveInDelegate();
	}
	#endregion
	#region Local
	
	#endregion
}
