using UnityEngine;
using System.Collections;

public class _UI_CS_UpgradeVendor : MonoBehaviour {
	
	public static _UI_CS_UpgradeVendor Instance;
	
	public _UI_CS_BagCtrl BagPanel;
	
	public UIPanel UpgradeVendorPanel;
	public UIButton fareWellBtn;
	
	
	
	void Awake()
	{
		Instance = this;
	}
	
	public void AwakeItemVendor(){
		UpgradeVendorPanel.BringIn();
		MoneyBadgeInfo.Instance.Hide(false);
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_UPGRADE_VENDOR);
	}
	
	public void InitItemVendor(){
		
	}
	
	// Use this for initialization
	void Start () {
		fareWellBtn.AddInputDelegate(FareWellBtnDelegate);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FareWellBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				BagPanel.Hide(true);
				UpgradeVendorPanel.Dismiss();
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);

                Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
//				_UI_CS_ToolsTip.Instance.DismissToolTips();	
			
				BGMInfo.Instance.isPlayUpGradeEffectSound = true;
				
				break;
		   default:
				break;
		}	
	}
	
	
}
