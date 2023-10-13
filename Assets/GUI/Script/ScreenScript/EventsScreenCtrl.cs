using UnityEngine;
using System.Collections;

public class EventsScreenCtrl : MonoBehaviour {

	public static EventsScreenCtrl Instance;
	
	void Awake() {
		Instance = this;
		RegisterInitEvent();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Local
	#region event create and destory
	private void RegisterInitEvent() {
		GUIManager.Instance.OnInitEndDelegate += TemplateInitEnd;
		GUIManager.Instance.OnScreenManagerDestroy += DestoryAllEvent;
	}
	
	//MAX template count.//
	private int initDelegateCount = 1;
	private void TemplateInitEnd() {
		Debug.Log("template init end ");
		if(GUIManager.Instance.GetTemplateInitEndCount() >= initDelegateCount) {
			RegisterTemplateEvent();
			Init();
//			GUILogManager.LogErr("DailyRewardScreenCtrl template init ok.");
		}
	}
	
	private void RegisterTemplateEvent() {
		Debug.Log("template register");
		if(EventsManager.Instance) {
			EventsManager.Instance.OnThanksDelegate += this.ThanksDelegate;
		}
	}
	
	private void DestoryAllEvent() {
		if(EventsManager.Instance) {
			EventsManager.Instance.OnThanksDelegate -= this.ThanksDelegate;
		}
		GUIManager.Instance.OnInitEndDelegate -= TemplateInitEnd;
		GUIManager.Instance.OnScreenManagerDestroy -= DestoryAllEvent;
	}
	#endregion
	private void Init() {
		UpdateEventData(0);
	}
	
	private void UpdateEventData(int idx) {
		if(PlayerDataManager.Instance.EventsList.Count > 0) {
			EventsManager.Instance.InitEventData(PlayerDataManager.Instance.EventsList[idx].title,PlayerDataManager.Instance.EventsList[idx].description1,PlayerDataManager.Instance.EventsList[idx].description2);
			PlayerDataManager.Instance.EventsList.RemoveAt(idx);
		}else {
			Player.Instance.ReactivePlayer();
       		GameCamera.BackToPlayerCamera();
			GUIManager.Instance.ChangeUIScreenState("IngameScreen");
		}
	}
			
	private void ThanksDelegate() {
		UpdateEventData(0);
	}
	#endregion
}
