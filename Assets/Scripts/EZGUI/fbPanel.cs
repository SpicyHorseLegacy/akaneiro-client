using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class fbPanel : MonoBehaviour {
	
	public static fbPanel Instance = null;
	public UIButton closeButton;
	public UIPanel mapBgPanel;

	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		closeButton.AddInputDelegate(ExitDelegate);
		//InitMapDescription();
	}

	public void LeaveMissionMap() {
		//MissionSelect.Instance.LeaveSelectMssion();
		transform.GetComponent<UIPanel>().Dismiss();
		GameObject.Find("mission map panel").gameObject.GetComponent<UIPanel>().Dismiss();
	}

#region Local	
	void ExitDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				MoneyBadgeInfo.Instance.Hide(false);
				LeaveMissionMap();
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
				MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
				BGManager.Instance.ExitOutsideAudio();
				break;
		}	
	}
#endregion

}