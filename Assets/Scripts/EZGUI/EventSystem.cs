using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public enum EM_EVENT_TYPE {
		EM_LEVELUP  = 0,
		EM_WEEK,
		EM_MONEY,
		EM_MAX,
}

public class EventSystem : MonoBehaviour {
	
	//Instance
	public static EventSystem Instance = null;
	
	public UIPanel		 			basePanel;
	private Rect 					rect = new Rect(0,0,1,1);
	public UIButton     			greatBtn;
	public SpriteText 				titleText;
	public SpriteText 				giftDescriptionText;	
	public SpriteText 				infoDescriptionText;	
	public UIPanel		 			itemPanel;
	public SpriteText 				itemNameText;	
	public UIButton     			icon;
	public UIButton     			thanksBtn;
	
	public  List<EventStruct> 		EventsList = new List<EventStruct>();
	public  List<EventStruct> 		BanAList   = new List<EventStruct>();
	public  List<EventStruct> 		BanCList   = new List<EventStruct>();
//	public UIButton  npc;
	
	public void AwakeEventPanel(){	
		if(EventsList.Count > 0&&!_UI_CS_MapScroll.Instance.IsExistMission){
			titleText.Text = EventsList[0].title;
			giftDescriptionText.Text = EventsList[0].description1;
			infoDescriptionText.Text = EventsList[0].description2;
			EventsList.RemoveAt(0);
			MoneyBadgeInfo.Instance.Hide(true);
			_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_EVENT_NEWS);
			_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
			basePanel.BringIn();
		}else{
			_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
			basePanel.Dismiss();
			MoneyBadgeInfo.Instance.Hide(false);
			_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
			Player.Instance.ReactivePlayer();
		}
//		npc.SetUVs(new Rect(0,0,1,1));
//		//downloading image
//		TextureDownLoadingMan.Instance.DownLoadingTexture("Figure_use6",npc.transform);
	}
	
//	public void AwakeEventPanel(int eventId){	
//	}
//	
//	public void AwakeEventPanel(ItemDropStruct itmeInfo,string title,string description1,string description2){	
//	}
//	
//	public void AwakeEventPanel(ItemDropStruct itmeInfo){	
//	}
	
	public void InitEventPanel(ItemDropStruct itmeInfo,string title,string description1,string description2){
		if(itmeInfo != null){
			_UI_CS_ItemVendor.Instance.SetColorForName(itemNameText,itmeInfo);
			ItemPrefabs.Instance.GetItemIcon(itmeInfo._ItemID,itmeInfo._TypeID,itmeInfo._PrefabID,icon);
			itemPanel.BringIn ();
		}
		titleText.Text = title;
		giftDescriptionText.Text = description1;
		infoDescriptionText.Text = description2;
		basePanel.BringIn();
	}
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		icon.SetUVs(rect);
		greatBtn.AddInputDelegate(GreatBtnDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddToBanList(string scrStr){
		string[] vals = scrStr.Split('@');
		EventStruct temp = new EventStruct();
		if(vals[0] != null) {
			temp.type 	 = int.Parse(vals[0]);
		}else {
			temp.type = 0;
		}
		if(vals[1] != null) {
			temp.eventId = int.Parse(vals[1]);
		}else {
			temp.eventId = 0;
		}
		if(vals[2] != null) {
			temp.count 	 = int.Parse(vals[2]);
		}else {
			temp.count 	 = 0;
		}
		if(vals[3] != null) {
			if(int.Parse(vals[3]) == 1) {
				BanAList.Add(temp);
			}else {
				BanCList.Add(temp);
			}
		}
	}
	
	void IntoEventsList(EventStruct temp) {
		
		string sendData = "";
		sendData = temp.type.ToString()+"@"+temp.eventId.ToString()+"@"+temp.count.ToString()+"@"+temp.eventType.ToString();
		
		for(int i = 0;i<BanAList.Count;i++){
			if(BanAList[i].eventId == temp.eventId){	
				return;
			}
		}
		for(int i = 0;i<BanCList.Count;i++){
			if(BanCList[i].eventId == temp.eventId){
				return;
			}
		}
		
		if(temp.eventType == 1){
			BanAList.Add(temp);
			EEventDBType eedbta = new EEventDBType(EEventDBType.eEventType_Account);
			
			CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.UpdateEvent(eedbta,temp.eventId,sendData)
													 );
		}else if(temp.eventType == 2){
			BanCList.Add(temp);
			EEventDBType eedbtc = new EEventDBType(EEventDBType.eEventType_Character);
			CS_Main.Instance.g_commModule.SendMessage(
				   	ProtocolGame_SendRequest.UpdateEvent(eedbtc,temp.eventId,sendData)
													 );
		}
		
		EventsList.Add(temp);
	}
	
//	public void RsetList(){
//		EventsList.Clear();
//		BanList.Clear();
//	}
	
	public void CheckEvent(EM_EVENT_TYPE events){
		switch((int)events){
		case (int)EM_EVENT_TYPE.EM_LEVELUP:
			CheckLevelUpEvent();
			break;
		case (int)EM_EVENT_TYPE.EM_WEEK:
			CheckWeekEvent();
			break;
		case (int)EM_EVENT_TYPE.EM_MONEY:
			CheckMoneyEvent();
			break;
		}
	}
	
	public void CheckLevelUpEvent(){
		string _fileName = LocalizeManage.Instance.GetLangPath("EventSystem.levelUp");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[4]) == _PlayerData.Instance.playerLevel){
				EventStruct temp = new EventStruct();
				temp.eventId = int.Parse(vals[0]);
				temp.title = vals[1];
				temp.description1 = vals[2];
				temp.description2 = vals[3];
				temp.eventType = int.Parse(vals[5]);
				IntoEventsList(temp);
			}
		}
	}
	
	public void CheckWeekEvent(){
		string _fileName = LocalizeManage.Instance.GetLangPath("EventSystem.Week");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[4]) == (int)System.DateTime.Now.DayOfWeek){
				EventStruct temp = new EventStruct();
				temp.eventId = int.Parse(vals[0]);
				temp.title = vals[1];
				temp.description1 = vals[2];
				temp.description2 = vals[3];
				temp.eventType = int.Parse(vals[5]);
				IntoEventsList(temp);
			}
		}
	}
	
	public void CheckMoneyEvent(){
		string _fileName = LocalizeManage.Instance.GetLangPath("EventSystem.money");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[4]) <= int.Parse(MoneyBadgeInfo.Instance.skText.text)){
				EventStruct temp = new EventStruct();
				temp.eventId = int.Parse(vals[0]);
				temp.title = vals[1];
				temp.description1 = vals[2];
				temp.description2 = vals[3];
				temp.eventType = int.Parse(vals[5]);
				IntoEventsList(temp);
			}
		}
	}
	
	void GreatBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				AwakeEventPanel();
				break;
		}	
	}
}
