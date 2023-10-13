using UnityEngine;
using System.Collections;

public class _UI_CS_AinuElder : MonoBehaviour {
	
	public static _UI_CS_AinuElder Instance;
	
	public UIPanel AinuElderPanel;
	public UIButton fareWellBtn;
	
	public _UI_CS_BagCtrl BagPanel;
	
	public UIPanelTab MailTab;
	public UIPanelTab AidTab;
	public UIPanelTab StorageTab;
	
	public UIPanelTab InboxTab;
	public UIPanelTab SendTab;
	
	private bool CurrentItemType  = false;
	
	void Awake()
	{
		Instance = this;
	}
	
	public void AwakeAinuElder(){
		if(CurrentItemType){
			BagPanel.Hide(false);
		}
		AinuElderPanel.BringIn();
		MoneyBadgeInfo.Instance.Hide(false);
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_AINU_ELDER);
	}
	
	public void InitAinuElder(){
//		for(int i = 0;i< 20;i++){
//			
//			_UI_CS_ItemVendorItem temp1W = new _UI_CS_ItemVendorItem();
//			temp1W.m_iconID = 1;
//			temp1W.m_ID = i;
//			temp1W.m_name = "aid";
//			temp1W.m_type = 1;
//			temp1W.m_Val = 100;
//
//			_UI_CS_AinuElder_Aid.Instance.AddElement(temp1W);
//		}
//		_UI_CS_AinuElder_Aid.Instance.InitItemList();
//		
//		for(int i = 0;i<2;i++){
//			_UI_CS_AbilitiesTrainerItem temp3 = new _UI_CS_AbilitiesTrainerItem();
//			
//			temp3.m_iconID = 1;
//			temp3.m_level = 0;
//			temp3.m_name = "MyNameSkill5";
//			temp3.m_type = 0;
//			temp3.m_val = 500;
//			
//			_UI_CS_AinuElder_Storage.Instance.AddElement(temp3);
//		}
//		
//		_UI_CS_AinuElder_Storage.Instance.InitItemList();
//		
//		for(int i = 0;i<9;i++){
//			_UI_CS_AinuElderItem temp3 = new _UI_CS_AinuElderItem();
//			
//			temp3.m_IconID = 1;
//			temp3.m_Info = "info...";
//			temp3.m_name = "name";
//			temp3.m_time = "1 day";
//			temp3.m_IsNew = false;
//			
//			_UI_CS_AinuElder_Mail.Instance.AddElement(temp3);
//		}
//		
//		_UI_CS_AinuElder_Mail.Instance.InitItemList();
//		
	}
	
	// Use this for initialization
	void Start () {
		fareWellBtn.AddInputDelegate(FareWellBtnDelegate);
		MailTab.AddInputDelegate(MailTabDelegate);
		AidTab.AddInputDelegate(AidTabDelegate);
		StorageTab.AddInputDelegate(StorageTabDelegate);
		InboxTab.AddInputDelegate(InboxTabDelegate);
		SendTab.AddInputDelegate(SendTabDelegate);
		CurrentItemType = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    void FareWellBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				BGMInfo.Instance.isPlayUpGradeEffectSound = true;
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				BagPanel.Hide(true);
				AinuElderPanel.Dismiss();
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);

                Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
			
				break;
		   default:
				break;
		}	
	}
	
	void AidTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
				BagPanel.Hide(true);
			
				break;
		   default:
				break;
		}	
	}
	
	void StorageTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
				
				BagPanel.Hide(true);
			
				break;
		   default:
				break;
		}	
	}
	
	void MailTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
			if(CurrentItemType){
				BagPanel.Hide(false);
			}
			
				break;
		   default:
				break;
		}	
	}
	
	void InboxTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
				CurrentItemType = true;
				BagPanel.Hide(false);
			
				break;
		   default:
				break;
		}	
	}
	
	void SendTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:

				CurrentItemType = false;
				BagPanel.Hide(true);
			
				break;
		   default:
				break;
		}	
	}
	
	
}
