using UnityEngine;
using System.Collections;

public class MailItem : MonoBehaviour {
	
	public UIButton 	icon;
	public UIButton 	bg;
	public UIButton 	newIcon;
	public SpriteText 	name;
	public SpriteText 	fromPlayer;
	public SpriteText 	time;
	
	public SMailInfo info;
	
	// Use this for initialization
	void Start () {
		 bg.AddInputDelegate(BgDelegate);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void BgDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				// send msg for server . this mail already open.
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.OpenMail(info.id));
				info.state.Set(EMailState.eMailState_Read);
				MailRightPanel.Instance.curMailInfo = info;
				MailSystem.Instance.UpdateUnReadMailCount();
				IsHideNewIcon(true);
				MailRightPanel.Instance.Init();
				MailRightPanel.Instance.basePanel.BringIn();
				break;
		}	
	}
	
	public void IsHideNewIcon(bool isHide) {
		if(isHide) {
			newIcon.gameObject.layer = LayerMask.NameToLayer("Default");
		}else {
			newIcon.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
		}
	}
}
