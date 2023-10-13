using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum MAILITEMType {
        ITEM = 1,
		PET,
		KARMA,
		CRYSTAL,
        MAXTYPE
}

public class MailSystem : MonoBehaviour {
	
	//Instance
	public static MailSystem Instance = null;
	
	public UIPanel	basePanel;
	public UIPanel	rightPanel;
	
	public UIButton exit;
	
	public Texture2D systemIcon;
	public Texture2D UserIcon;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		exit.AddInputDelegate(ExitDelegate);
		mailIcon.AddInputDelegate(MailIconDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
#region Interface
	public void AwakeMailSystem() {
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_MAIL_SYSYTEM);
		basePanel.BringIn ();
		MoneyBadgeInfo.Instance.Hide(false);
		Player.Instance.FreezePlayer();
		MailSystem.Instance.UpdateMailExpireTime();
//		GetMailList();
	}
	
	public  List<SMailInfo> 		mailList   = new List<SMailInfo>();

	public void InitMailInfo() {
		MailLeftPanel.Instance.InitList();
		UpdateUnReadMailCount();
		rightPanel.Dismiss();
	}
	
	public void ResetMailSystem() {
		mailList.Clear();
	}
	
	public void GetMailList() {
		CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetPlayerMail());
	}
	
	public UIButton mailIcon;
	public SpriteText newMailCount;
	public void UpdateUnReadMailCount() {
		int unReadCount = 0;
		for(int i = 0;i<MailLeftPanel.Instance.list.Count;i++) {
			if(MailLeftPanel.Instance.list.GetItem(i).gameObject.GetComponent<MailItem>().info.state.Get() == EMailState.eMailState_Unread) {
				unReadCount++;
			}
		}
		newMailCount.Text = unReadCount.ToString();
	}
	
	public void IsHideMailIcon(bool isHide) {
		mailIcon.Hide(isHide);
		newMailCount.Hide(isHide);
	}
	
	
	public void UpdateMailExpireTime() {
		long t1970 = MailLeftPanel.Instance.Get1970Time();
		for(int i = 0;i<MailLeftPanel.Instance.list.Count;i++) {
			if(MailLeftPanel.Instance.list.GetItem(i).gameObject.GetComponent<MailItem>().info.timeout != 0) {
				long t1970s = MailLeftPanel.Instance.list.GetItem(i).gameObject.GetComponent<MailItem>().info.timeout - t1970;
				MailLeftPanel.Instance.list.GetItem(i).gameObject.GetComponent<MailItem>().time.Text = (t1970s/86400).ToString() + "d "+(t1970s/3600%24).ToString() + "h ";
			}else {
				MailLeftPanel.Instance.list.GetItem(i).gameObject.GetComponent<MailItem>().time.Text = "Forever";
			}
		}
	}
#endregion
	
#region Local
	void ExitDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				basePanel.Dismiss();
				Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
			break;
		}	
	}
	
	void MailIconDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
				AwakeMailSystem();	
			break;
		}	
	}
#endregion
}
