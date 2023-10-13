using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_Summon : MonoBehaviour {
	
	//Instance
	public static _UI_CS_Summon Instance = null;
	
	public UIPanel		SummonPanel;
	
	public  List<SFriendCharInfo> 		FriendInfoList   = new List<SFriendCharInfo>();
	
	public SummonPlayer [] ModelList;
	
	public int CurrentModelIndex = 0;
	//0: all 1: friends
	public int CurrentSearchIndex = 0;
	
	public UIButton 	LeftBtn;
	public UIButton 	RightBtn;
	public UIButton 	SummonBtn;
	public UIButton 	SummonIconBtn;
	public SpriteText	SummonTextBtn;
	public UIButton 	BackBtn;
	public UIButton 	BackIconBtn;
	public SpriteText	BackTextBtn;

	public UIButton 	AllBtn;
	public UIButton 	FriendBtn;
	public UIButton []	EquipListBtn;
	
	public UIButton 	EquipBtn;
	public bool 		IsEquipShow = true;
	public UIPanel		EquipPanel;
	
	public UIButton  		bg;
	
	void Awake()
	{
		Instance = this;                                                                                                
	}
	
	// Use this for initialization
	void Start () {
		LeftBtn.AddInputDelegate(LeftBtnDelegate);
		RightBtn.AddInputDelegate(RightBtnDelegate);
		BackBtn.AddInputDelegate(BackBtnDelegate);
		SummonBtn.AddInputDelegate(SummonBtnDelegate);
		EquipBtn.AddInputDelegate(EquipBtnDelegate);
		EquipPanel.BringIn();
		IsEquipShow = false;
		for(int i = 0;i<3;i++) {
			ModelList[i].Model.OnLoadOKDelegate += NotifyModeLoadOK;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AwakeSummon(){
		InitImage();
		CS_Main.Instance.g_commModule.SendMessage(
	   		ProtocolBattle_SendRequest.GetFriendList()
		);
		SummonPanel.BringIn();
		MoneyBadgeInfo.Instance.Hide(true);
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
		LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"SEARCHFRIENDLIST");
		_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
	}
	
	private void InitImage(){
		bg.SetUVs(new Rect(0,0,1,1));
		//downloading image
		TextureDownLoadingMan.Instance.DownLoadingTexture("Chara_Bk",bg.transform);
	}
	
	public void ShowFriendInfo(){
	
		int count = 0;
		count = FriendInfoList.Count;
		
		// only one friend 
		if(1 == count){
			
			ModelList[0].MyPanel.Dismiss();
			ModelList[0].IsShow = false;
			ModelList[2].MyPanel.Dismiss();
			ModelList[2].IsShow = false;
			
			ModelList[1].SetFriendModelInfo(FriendInfoList[0]);
			ModelList[1].MyPanel.BringIn();
			ModelList[1].IsShow = true;
			
		}else{
		
			for(int i =0;i<3;i++){
				
				if(i < count){
					int idx = CurrentModelIndex+i;
					if(idx >= count){
						idx = (idx - count);
					}
					ModelList[i].SetFriendModelInfo(FriendInfoList[idx]);
					ModelList[i].MyPanel.BringIn();
					ModelList[i].IsShow = true;
				}else{
					ModelList[i].MyPanel.Dismiss();
					ModelList[i].IsShow = false;
				}
			}
			
		}
		
		if(0 != count)
			UpdateEquipInfo();
		
	}
	
	private void HideArrow() {
		curLoadOkCount = 0;
		LeftBtn.gameObject.SetActive(false);
		RightBtn.gameObject.SetActive(false);
		
	}
	private void ShowArrow() {
		LeftBtn.gameObject.SetActive(true);
		RightBtn.gameObject.SetActive(true);
	}
	
	int curLoadOkCount = 0;
	private void NotifyModeLoadOK() {
		curLoadOkCount++;
		if(curLoadOkCount >= FriendInfoList.Count||curLoadOkCount >= 3) {
			ShowArrow();
		}
//		LogManager.Log_Warn("FriendInfoList.Count:"+FriendInfoList.Count+"|curLoadOkCount:"+curLoadOkCount);
	}
	
	public void UpdateEquipInfo(){
		
		for(int i = 0;i<9;i++){
			EquipListBtn[i].SetUVs(new Rect(0,0,1,1));
			EquipListBtn[i].SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[0]);
		
		}
		
		if(ModelList[1].IsShow){
			foreach(itemuuid equip in ModelList[1].MyInfo.equipinfo){
				EquipListBtn[equip.slotPart].SetUVs(new Rect(0,0,1,1));
				ItemPrefabs.Instance.GetItemIcon(equip.itemID,0,equip.prefabID,EquipListBtn[equip.slotPart]);	
			}
		}else{
			EquipPanel.Dismiss();
			IsEquipShow = false;
		}
	
	}
	
	void RightBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CurrentModelIndex++;
				if(CurrentModelIndex > (FriendInfoList.Count-1)){
					CurrentModelIndex = 0;
				}
				ShowFriendInfo();
				HideArrow();
		  	 break;
		   default:
				break;
		}
	}
	
	void LeftBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CurrentModelIndex--;
				if(CurrentModelIndex < 0){
					CurrentModelIndex = (FriendInfoList.Count-1);
				}
				ShowFriendInfo();
				HideArrow();
		  	 break;
		   default:
				break;
		}	
	}
	
	public void ShowModel(){
	
		for(int i = 0;i<3;i++){
			
			if(ModelList[i].IsShow){
				Vector3 tPos = _UI_CS_Ctrl.Instance.m_UI_Camera.WorldToScreenPoint(new Vector3(ModelList[i].PosObj.position.x,ModelList[i].PosObj.position.y,1));
				ModelList[i].SCamera.ShowAt(new Vector2(tPos.x,tPos.y), new Vector2(Screen.width/4, Screen.height));
			}else{
				ModelList[i].SCamera.ShutDown();
			}
		
		}
	
	}
	
	public void HideModel(){
	
		for(int i =0;i<3;i++){
			ModelList[i].IsShow = false;
			ModelList[i].MyPanel.Dismiss();
			ModelList[i].SCamera.ShutDown();
		}
	
	}
	
	void BackBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.PRESS:
					BackIconBtn.SetColor(_UI_Color.Instance.color1);
					BackTextBtn.SetColor(_UI_Color.Instance.color1);	
			
				break;
			
			case POINTER_INFO.INPUT_EVENT.MOVE:
					BackIconBtn.SetColor(_UI_Color.Instance.color1);
					BackTextBtn.SetColor(_UI_Color.Instance.color1);	
				break;
			
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
					
				BackIconBtn.SetColor(_UI_Color.Instance.color2);
				BackTextBtn.SetColor(_UI_Color.Instance.color4);
			
			break;
			
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				HideModel();
				SummonPanel.Dismiss();
				MoneyBadgeInfo.Instance.Hide(false);
				BackIconBtn.SetColor(_UI_Color.Instance.color2);
				BackTextBtn.SetColor(_UI_Color.Instance.color4);

                Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
				break;
		   default:
				break;
		}	
	}
		
	void SummonBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.PRESS:
					SummonIconBtn.SetColor(_UI_Color.Instance.color1);
					SummonTextBtn.SetColor(_UI_Color.Instance.color1);	
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
					SummonIconBtn.SetColor(_UI_Color.Instance.color1);
					SummonTextBtn.SetColor(_UI_Color.Instance.color1);	
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:	
				SummonIconBtn.SetColor(_UI_Color.Instance.color2);
				SummonTextBtn.SetColor(_UI_Color.Instance.color4);
			break;
		   case POINTER_INFO.INPUT_EVENT.TAP:

				if(FriendInfoList.Count != 0){
					if(FriendInfoList.Count == 1){
						CS_Main.Instance.g_commModule.SendMessage(
				   			ProtocolBattle_SendRequest.SelectFriendAlly(FriendInfoList[0].ID)
						);
					}else{
						int tempIdx = 0;
						if((CurrentModelIndex+1)<FriendInfoList.Count){
							tempIdx = CurrentModelIndex+1;
						}else{
							//last one;
							tempIdx = 0;
						}
						CS_Main.Instance.g_commModule.SendMessage(
				   			ProtocolBattle_SendRequest.SelectFriendAlly(FriendInfoList[tempIdx].ID)
						);
					}
				}
				SummonIconBtn.SetColor(_UI_Color.Instance.color2);
				SummonTextBtn.SetColor(_UI_Color.Instance.color4);
				break;
		}	
	}
	
	void EquipBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
	
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
				if(IsEquipShow){
					EquipPanel.Dismiss();
					IsEquipShow = false;
				}else{
					EquipPanel.BringIn();
					IsEquipShow = true;
				}
			
				break;
		   default:
				break;
		}	
	}
}
