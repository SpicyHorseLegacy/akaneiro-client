using UnityEngine;
using System.Collections;

public class ChatBoxSettingFilter : MonoBehaviour {
	//Instance
	public static ChatBoxSettingFilter Instance = null;
	public Transform			root;
	public Transform			maxPos;
	public Transform			minPos;
	public UIButton 			minBtn;
	public UIStateToggleBtn		publicBtn;
//	public UIStateToggleBtn		teamBtn;
//	public UIStateToggleBtn		privateBtn;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		minBtn.AddInputDelegate(MinBtnDelegate);
		root.position = minPos.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitFilterSetting(){
		root.position = maxPos.position;
	}
	
	void MinBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				root.position = minPos.position;
				break;
		}	
	}
	
	public int GetPublicState(){
		return publicBtn.StateNum;
	}
	
//	public int GetTeamState(){
//		return teamBtn.StateNum;
//	}
	
//	public int GetPrivateState(){
//		return privateBtn.StateNum;
//	}
}
