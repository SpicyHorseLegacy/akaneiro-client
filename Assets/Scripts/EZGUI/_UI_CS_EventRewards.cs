using UnityEngine;
using System.Collections;

// this file is old file.

public class _UI_CS_EventRewards : MonoBehaviour {
	
	//Instance
	public static _UI_CS_EventRewards Instance = null;
	//first
	public bool IsFirstLogin;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//
	public void AwkeBonusMenu(int idx){
		switch(idx){
//		case 0:
//				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
//				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
//				_UI_CS_MapScroll.Instance.EnterMap(_UI_CS_LoadProgressCtrl.Instance.MapName);
//			break;
		case 3:
				_UI_CS_FightScreen.Instance.m_isLogout = false;
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_SELECT);	
				SelectChara.Instance.AwakeSelectChara();
			break;
		case 4:
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);	
				_UI_CS_MapScroll.Instance.EnterMap(_UI_CS_LoadProgressCtrl.Instance.MapName);
			break;
		}
	}
	
}
