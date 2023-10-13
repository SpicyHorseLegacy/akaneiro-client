using UnityEngine;
using System.Collections;

public class TaskCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
//		if(Input.GetKeyDown(KeyCode.A)) {
//			FadeIn();
//		}
	}
	
	#region Interface
	public _UI_CS_RamusTask data;

	[SerializeField]
	private Transform root;
	[SerializeField]
	private UILabel content;
	public void SetContent(int cur,int max,string str) {
		if(max != 0 && max != 1) {
			content.text = cur.ToString() + "/" + max.ToString() + " " +str;
		}else {
			content.text = str;
		}
	}
	
	public void FadeIn() {
		ShowTask();
	}
	public void FadeOut() {
		ShowTick();
	}
	
	public delegate void Handle_MoveInDelegate();
    public event Handle_MoveInDelegate OnMoveInDelegate;
	private void MoveInDelegate() {
		if(OnMoveInDelegate != null) {
			OnMoveInDelegate();
		}
	}
	
	public delegate void Handle_MoveOutDelegate();
    public event Handle_MoveOutDelegate OnMoveOutDelegate;
	private void MoveOutDelegate() {
		if(OnMoveOutDelegate != null) {
			OnMoveOutDelegate();
		}
	}
	
	public void SetTaskAniPos() {
		task.localPosition = new Vector3(300,0,0);
	}
	#endregion
	
	#region Local
	#region new
	[SerializeField]
	private Transform newImg;
	private void ShowNew() {
		TweenAlpha.Begin(newImg.gameObject,1,1);
		newImg.GetComponent<TweenAlpha>().eventReceiver = gameObject;
		newImg.GetComponent<TweenAlpha>().callWhenFinished = "NewBringDelegate";
		
	}
	private void NewBringDelegate() {
		TweenDelay.Begin(gameObject,2,"NewDelayDelegate",null);
	}
	private void NewDelayDelegate() {
		TweenAlpha.Begin(newImg.gameObject,1,0);
		newImg.GetComponent<TweenAlpha>().eventReceiver = gameObject;
		newImg.GetComponent<TweenAlpha>().callWhenFinished = "NewDismissDelegate";
	}
	private void NewDismissDelegate() {
		MoveInDelegate();
	}
	#endregion	
	#region Tick
	[SerializeField]
	private UISprite tick;
	private void ShowTick() {
		TweenAlpha.Begin<TweenAlpha>(tick.gameObject,1);
		tick.GetComponent<TweenAlpha>().eventReceiver = gameObject;
		tick.GetComponent<TweenAlpha>().callWhenFinished = "TickDelegate";
		TweenColor.Begin(content.gameObject,1,Color.yellow);
	}
	private void TickDelegate() {
		DismissTask();
	}
	#endregion
	#region bg
	public Transform task;
	private void ShowTask() {
		SetTaskAniPos();
		TweenPosition.Begin(task.gameObject,0.2f,new Vector3(0,0,0));
		task.GetComponent<TweenPosition>().eventReceiver = gameObject;
		task.GetComponent<TweenPosition>().callWhenFinished = "TaskBringDelegate";
	}
	private void TaskBringDelegate() {
		ShowNew();
	}
	private void DismissTask() {
		TweenPosition.Begin(task.gameObject,0.2f,new Vector3(350,0,0));
		task.GetComponent<TweenPosition>().eventReceiver = gameObject;
		task.GetComponent<TweenPosition>().callWhenFinished = "TaskDismissDelegate";
	}
	private void TaskDismissDelegate() {
		MoveOutDelegate();
	}
	#endregion
	#endregion
	
}
