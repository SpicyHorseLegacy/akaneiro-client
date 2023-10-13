using UnityEngine;
using System.Collections;

public class _UI_CS_Revival : MonoBehaviour {
	
	public  static _UI_CS_Revival Instance;
	public  UIPanel 	RevivalPanel;
	public  UIButton 	VillageBtn;
	public  UIButton 	VillageIcon;
	public  UIButton 	VillageTitleBG;
	public  UIButton 	VillagePayBtn;
	public  SpriteText 	VillageTitleText;
	public  UIButton 	VillageBG;
	public  UIButton 	VillageKarma;
	public  SpriteText 	VillageKarmaText;
	public  SpriteText 	VillageInfo1Text;
	public  SpriteText 	VillageInfo2Text;
	public  SpriteText 	VillageInfo3Text;
	public  UIButton 	RevivalBtn;
	public  UIButton 	RevivalPayBtn;
	public  UIButton 	RevivalIcon;
	public  UIButton 	RevivalTitleBG;
	public  SpriteText 	RevivalTitleText;
	public  UIButton 	RevivalBG;
	public  UIButton 	RevivalKarma;
	public  SpriteText 	RevivalKarmaText;
	public  SpriteText 	RevivalInfo1Text;
	public  SpriteText 	RevivalInfo2Text;
	public  SpriteText 	RevivalInfo3Text;
	public  UIButton 	ItemBtn;
	public  UIButton 	ItemPay1Btn;
	public  UIButton 	ItemPay2Btn;
	public  UIButton 	ItemIcon;
	public  UIButton 	ItemTitleBG;
	public  SpriteText 	ItemTitleText;
	public  UIButton 	ItemBG;
	public  UIButton 	ItemKarma;
	public  UIButton 	ItemKarma2;
	public  SpriteText 	ItemKarmaText;
	public  SpriteText 	ItemKarma2Text;
	public  SpriteText 	ItemInfo1Text;
	public  SpriteText 	ItemInfo2Text;
	public  SpriteText 	ItemInfo3Text;
	public  SpriteText 	ItemInfo4Text;
	private EReviveType reviveType = new  EReviveType();
	public  int 		RevivalCount = 0;
	public SpriteText 	reviveItemCountText;
	
	public void AwakeRevival(){
		int reviveKarma = 0;
		IngameMenuBack();
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_REVIVAL_MENU);
		RevivalPanel.BringIn();
		_UI_MiniMap.Instance.isShowSmallMap	 = false;
		_UI_MiniMap.Instance.isShowBigMap	 = false;
		reviveKarma = GetRevivalKarma();
		if(reviveKarma == 0){
//			RevivalKarmaText.Text = "Free";
			LocalizeManage.Instance.GetDynamicText(RevivalKarmaText,"FREE");
		}else{
			RevivalKarmaText.Text = reviveKarma.ToString();
		}
	}
	
	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		VillageBtn.AddInputDelegate(VillageDelegate);
		VillagePayBtn.AddInputDelegate(VillagePayBtnDelegate);
		RevivalBtn.AddInputDelegate(RevivalDelegate);
		RevivalPayBtn.AddInputDelegate(RevivalPayBtnDelegate);
		ItemBtn.AddInputDelegate(ItemBtnDelegate);
		ItemPay1Btn.AddInputDelegate(ItemPay1BtnDelegate);
		ItemPay2Btn.AddInputDelegate(ItemPay2BtnDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void IngameMenuBack(){
		_AbiMenuCtrl.Instance.UpDateIngameAbilitiesIcon();
		_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.Dismiss();
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
		Inventory.Instance.BagPanel.HideBag();
		Inventory.Instance.bagItemArray[0].CancelPress();
		SurveillanceCamera.Instance.ShutDown();
		if(null != ScenesLightCtrl.Instance)
		ScenesLightCtrl.Instance.OpenLight();
		_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
		_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
	}
	
	public int GetRevivalKarma(){
		string _fileName = LocalizeManage.Instance.GetLangPath("RevivalCost.Costs");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		int idx = 0;
		idx = RevivalCount;
		for (int i = 3; i < itemRowsList.Length; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[0]) == _PlayerData.Instance.playerLevel){
				if((idx)>7){
					idx = 7;
				}
				return int.Parse(vals[(idx)]);
			}
		}
		return 0;
	}
	
	void ItemPay1BtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.PRESS:
				ItemIcon.SetColor(_UI_Color.Instance.color1);
				ItemTitleBG.SetColor(_UI_Color.Instance.color1);
				ItemBG.SetColor(_UI_Color.Instance.color1);
				ItemKarma.SetColor(_UI_Color.Instance.color1);
				ItemKarma2.SetColor(_UI_Color.Instance.color1);
				ItemTitleText.SetColor(_UI_Color.Instance.color29);
				ItemKarmaText.SetColor(_UI_Color.Instance.color1);
				ItemKarma2Text.SetColor(_UI_Color.Instance.color1);
				reviveItemCountText.SetColor(_UI_Color.Instance.color1);
				ItemInfo1Text.SetColor(_UI_Color.Instance.color23);
				ItemInfo2Text.SetColor(_UI_Color.Instance.color1);
				ItemInfo3Text.SetColor(_UI_Color.Instance.color1);
				ItemInfo4Text.SetColor(_UI_Color.Instance.color1);
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
				ItemIcon.SetColor(_UI_Color.Instance.color1);
				ItemTitleBG.SetColor(_UI_Color.Instance.color1);
				ItemBG.SetColor(_UI_Color.Instance.color1);
				ItemKarma.SetColor(_UI_Color.Instance.color1);
				ItemKarma2.SetColor(_UI_Color.Instance.color1);
				ItemTitleText.SetColor(_UI_Color.Instance.color29);
				ItemKarmaText.SetColor(_UI_Color.Instance.color1);
				ItemKarma2Text.SetColor(_UI_Color.Instance.color1);
				reviveItemCountText.SetColor(_UI_Color.Instance.color1);
				ItemInfo1Text.SetColor(_UI_Color.Instance.color23);
				ItemInfo2Text.SetColor(_UI_Color.Instance.color1);
				ItemInfo3Text.SetColor(_UI_Color.Instance.color1);
				ItemInfo4Text.SetColor(_UI_Color.Instance.color1);
				break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
				reviveType.Set(EReviveType.eReviveType_Crystal);
				 CS_Main.Instance.g_commModule.SendMessage(
							ProtocolBattle_SendRequest.ReviveReq(reviveType)
															);
				break;
		   default:
				break;
		}	
	}
	
	void ItemPay2BtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.PRESS:
				ItemIcon.SetColor(_UI_Color.Instance.color1);
				ItemTitleBG.SetColor(_UI_Color.Instance.color1);
				ItemBG.SetColor(_UI_Color.Instance.color1);
				ItemKarma.SetColor(_UI_Color.Instance.color1);
				ItemKarma2.SetColor(_UI_Color.Instance.color1);
				ItemTitleText.SetColor(_UI_Color.Instance.color29);
				ItemKarmaText.SetColor(_UI_Color.Instance.color1);
				ItemKarma2Text.SetColor(_UI_Color.Instance.color1);
				reviveItemCountText.SetColor(_UI_Color.Instance.color1);
				ItemInfo1Text.SetColor(_UI_Color.Instance.color23);
				ItemInfo2Text.SetColor(_UI_Color.Instance.color1);
				ItemInfo3Text.SetColor(_UI_Color.Instance.color1);
				ItemInfo4Text.SetColor(_UI_Color.Instance.color1);
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
				ItemIcon.SetColor(_UI_Color.Instance.color1);
				ItemTitleBG.SetColor(_UI_Color.Instance.color1);
				ItemBG.SetColor(_UI_Color.Instance.color1);
				ItemKarma.SetColor(_UI_Color.Instance.color1);
				ItemKarma2.SetColor(_UI_Color.Instance.color1);
				ItemTitleText.SetColor(_UI_Color.Instance.color29);
				ItemKarmaText.SetColor(_UI_Color.Instance.color1);
				ItemKarma2Text.SetColor(_UI_Color.Instance.color1);
				reviveItemCountText.SetColor(_UI_Color.Instance.color1);
				ItemInfo1Text.SetColor(_UI_Color.Instance.color23);
				ItemInfo2Text.SetColor(_UI_Color.Instance.color1);
				ItemInfo3Text.SetColor(_UI_Color.Instance.color1);
				ItemInfo4Text.SetColor(_UI_Color.Instance.color1);
				break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
				reviveType.Set(EReviveType.eReviveType_Item);
				 CS_Main.Instance.g_commModule.SendMessage(
							ProtocolBattle_SendRequest.ReviveReq(reviveType)
															);
				break;
		   default:
				break;
		}	
	}
	
	void ItemBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.PRESS:
				ItemIcon.SetColor(_UI_Color.Instance.color1);
				ItemTitleBG.SetColor(_UI_Color.Instance.color1);
				ItemBG.SetColor(_UI_Color.Instance.color1);
				ItemKarma.SetColor(_UI_Color.Instance.color1);
				ItemKarma2.SetColor(_UI_Color.Instance.color1);
				ItemTitleText.SetColor(_UI_Color.Instance.color29);
				ItemKarmaText.SetColor(_UI_Color.Instance.color1);
				ItemKarma2Text.SetColor(_UI_Color.Instance.color1);
				reviveItemCountText.SetColor(_UI_Color.Instance.color1);
				ItemInfo1Text.SetColor(_UI_Color.Instance.color23);
				ItemInfo2Text.SetColor(_UI_Color.Instance.color1);
				ItemInfo3Text.SetColor(_UI_Color.Instance.color1);
				ItemInfo4Text.SetColor(_UI_Color.Instance.color1);
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
				ItemIcon.SetColor(_UI_Color.Instance.color1);
				ItemTitleBG.SetColor(_UI_Color.Instance.color1);
				ItemBG.SetColor(_UI_Color.Instance.color1);
				ItemKarma.SetColor(_UI_Color.Instance.color1);
				ItemKarma2.SetColor(_UI_Color.Instance.color1);
				ItemTitleText.SetColor(_UI_Color.Instance.color29);
				ItemKarmaText.SetColor(_UI_Color.Instance.color1);
				ItemKarma2Text.SetColor(_UI_Color.Instance.color1);
				reviveItemCountText.SetColor(_UI_Color.Instance.color1);
				ItemInfo1Text.SetColor(_UI_Color.Instance.color23);
				ItemInfo2Text.SetColor(_UI_Color.Instance.color1);
				ItemInfo3Text.SetColor(_UI_Color.Instance.color1);
				ItemInfo4Text.SetColor(_UI_Color.Instance.color1);
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:	
				ItemIcon.SetColor(_UI_Color.Instance.color2);
				ItemTitleBG.SetColor(_UI_Color.Instance.color2);
				ItemBG.SetColor(_UI_Color.Instance.color2);
				ItemKarma.SetColor(_UI_Color.Instance.color2);
				ItemKarma2.SetColor(_UI_Color.Instance.color2);
				ItemTitleText.SetColor(_UI_Color.Instance.color30);
				ItemKarmaText.SetColor(_UI_Color.Instance.color4);
				ItemKarma2Text.SetColor(_UI_Color.Instance.color4);
				reviveItemCountText.SetColor(_UI_Color.Instance.color4);
				ItemInfo1Text.SetColor(_UI_Color.Instance.color24);
				ItemInfo2Text.SetColor(_UI_Color.Instance.color4);
				ItemInfo3Text.SetColor(_UI_Color.Instance.color4);
				ItemInfo4Text.SetColor(_UI_Color.Instance.color4);
			break;
		}	
	}
		
	void RevivalPayBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.PRESS:
				RevivalIcon.SetColor(_UI_Color.Instance.color1);
				RevivalTitleBG.SetColor(_UI_Color.Instance.color1);
				RevivalBG.SetColor(_UI_Color.Instance.color1);
				RevivalKarma.SetColor(_UI_Color.Instance.color1);
				RevivalTitleText.SetColor(_UI_Color.Instance.color23);
				RevivalKarmaText.SetColor(_UI_Color.Instance.color1);
				RevivalInfo1Text.SetColor(_UI_Color.Instance.color23);
				RevivalInfo2Text.SetColor(_UI_Color.Instance.color1);
				RevivalInfo3Text.SetColor(_UI_Color.Instance.color1);
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
				RevivalIcon.SetColor(_UI_Color.Instance.color1);
				RevivalTitleBG.SetColor(_UI_Color.Instance.color1);
				RevivalBG.SetColor(_UI_Color.Instance.color1);
				RevivalKarma.SetColor(_UI_Color.Instance.color1);
				RevivalTitleText.SetColor(_UI_Color.Instance.color23);
				RevivalKarmaText.SetColor(_UI_Color.Instance.color1);
				RevivalInfo1Text.SetColor(_UI_Color.Instance.color23);
				RevivalInfo2Text.SetColor(_UI_Color.Instance.color1);
				RevivalInfo3Text.SetColor(_UI_Color.Instance.color1);
				break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
				reviveType.Set(EReviveType.eReviveType_NoMoney);
				 CS_Main.Instance.g_commModule.SendMessage(
							ProtocolBattle_SendRequest.ReviveReq(reviveType)
															);
				break;
		}	
	}	
		
	void RevivalDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.PRESS:
				RevivalIcon.SetColor(_UI_Color.Instance.color1);
				RevivalTitleBG.SetColor(_UI_Color.Instance.color1);
				RevivalBG.SetColor(_UI_Color.Instance.color1);
				RevivalKarma.SetColor(_UI_Color.Instance.color1);
				RevivalTitleText.SetColor(_UI_Color.Instance.color23);
				RevivalKarmaText.SetColor(_UI_Color.Instance.color1);
				RevivalInfo1Text.SetColor(_UI_Color.Instance.color23);
				RevivalInfo2Text.SetColor(_UI_Color.Instance.color1);
				RevivalInfo3Text.SetColor(_UI_Color.Instance.color1);
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
				RevivalIcon.SetColor(_UI_Color.Instance.color1);
				RevivalTitleBG.SetColor(_UI_Color.Instance.color1);
				RevivalBG.SetColor(_UI_Color.Instance.color1);
				RevivalKarma.SetColor(_UI_Color.Instance.color1);
				RevivalTitleText.SetColor(_UI_Color.Instance.color23);
				RevivalKarmaText.SetColor(_UI_Color.Instance.color1);
				RevivalInfo1Text.SetColor(_UI_Color.Instance.color23);
				RevivalInfo2Text.SetColor(_UI_Color.Instance.color1);
				RevivalInfo3Text.SetColor(_UI_Color.Instance.color1);
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:	
				RevivalIcon.SetColor(_UI_Color.Instance.color2);
				RevivalTitleBG.SetColor(_UI_Color.Instance.color2);
				RevivalBG.SetColor(_UI_Color.Instance.color2);
				RevivalKarma.SetColor(_UI_Color.Instance.color4);
				RevivalTitleText.SetColor(_UI_Color.Instance.color24);
				RevivalKarmaText.SetColor(_UI_Color.Instance.color4);
				RevivalInfo1Text.SetColor(_UI_Color.Instance.color24);
				RevivalInfo2Text.SetColor(_UI_Color.Instance.color4);
				RevivalInfo3Text.SetColor(_UI_Color.Instance.color4);
			break;
		}	
	}
	
	void VillagePayBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.PRESS:
				VillageIcon.SetColor(_UI_Color.Instance.color1);
				VillageTitleBG.SetColor(_UI_Color.Instance.color1);
				VillageBG.SetColor(_UI_Color.Instance.color1);
				VillageKarma.SetColor(_UI_Color.Instance.color1);
				VillageTitleText.SetColor(_UI_Color.Instance.color21);
				VillageKarmaText.SetColor(_UI_Color.Instance.color1);
				VillageInfo1Text.SetColor(_UI_Color.Instance.color21);
				VillageInfo2Text.SetColor(_UI_Color.Instance.color1);
				VillageInfo3Text.SetColor(_UI_Color.Instance.color1);
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
				VillageIcon.SetColor(_UI_Color.Instance.color1);
				VillageTitleBG.SetColor(_UI_Color.Instance.color1);
				VillageBG.SetColor(_UI_Color.Instance.color1);
				VillageKarma.SetColor(_UI_Color.Instance.color1);
				VillageTitleText.SetColor(_UI_Color.Instance.color21);
				VillageKarmaText.SetColor(_UI_Color.Instance.color1);
				VillageInfo1Text.SetColor(_UI_Color.Instance.color21);
				VillageInfo2Text.SetColor(_UI_Color.Instance.color1);
				VillageInfo3Text.SetColor(_UI_Color.Instance.color1);
				break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
				_UI_CS_MissionLogic.Instance.MissionBgPanel.Dismiss();
				_UI_CS_Revival.Instance.RevivalPanel.Dismiss();
				Player.Instance.FSM.ChangeState(Player.Instance.IS);
				_UI_MiniMap.Instance.isShowSmallMap	 = true;
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LOADING);
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
				_UI_MiniMap.Instance.isShowSmallMap	 = false;
				_UI_MiniMap.Instance.isShowBigMap	 = false;
				_UI_CS_MissionLogic.Instance.MissionBgPanel.Dismiss();
				_UI_CS_MissionLogic.Instance.RsetMissionScore();
				_UI_CS_Teleport.Instance.HideTelport();
				_UI_CS_LoadProgressCtrl.Instance.m_LoadingMapName.Text = _UI_CS_MapScroll.Instance.SceneNameToMapName("Hub_Village");
				_UI_CS_LoadProgressCtrl.Instance.AwakeLoading(0);
				CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.EnterScene("Hub_Village"));	
				break;
		}	
	}
	
	void VillageDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.PRESS:
				VillageIcon.SetColor(_UI_Color.Instance.color1);
				VillageTitleBG.SetColor(_UI_Color.Instance.color1);
				VillageBG.SetColor(_UI_Color.Instance.color1);
				VillageKarma.SetColor(_UI_Color.Instance.color1);
				VillageTitleText.SetColor(_UI_Color.Instance.color21);
				VillageKarmaText.SetColor(_UI_Color.Instance.color1);
				VillageInfo1Text.SetColor(_UI_Color.Instance.color21);
				VillageInfo2Text.SetColor(_UI_Color.Instance.color1);
				VillageInfo3Text.SetColor(_UI_Color.Instance.color1);
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
				VillageIcon.SetColor(_UI_Color.Instance.color1);
				VillageTitleBG.SetColor(_UI_Color.Instance.color1);
				VillageBG.SetColor(_UI_Color.Instance.color1);
				VillageKarma.SetColor(_UI_Color.Instance.color1);
				VillageTitleText.SetColor(_UI_Color.Instance.color21);
				VillageKarmaText.SetColor(_UI_Color.Instance.color1);
				VillageInfo1Text.SetColor(_UI_Color.Instance.color21);
				VillageInfo2Text.SetColor(_UI_Color.Instance.color1);
				VillageInfo3Text.SetColor(_UI_Color.Instance.color1);
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:	
				VillageIcon.SetColor(_UI_Color.Instance.color2);
				VillageTitleBG.SetColor(_UI_Color.Instance.color2);
				VillageBG.SetColor(_UI_Color.Instance.color2);
				VillageKarma.SetColor(_UI_Color.Instance.color2);
				VillageTitleText.SetColor(_UI_Color.Instance.color22);
				VillageKarmaText.SetColor(_UI_Color.Instance.color4);
				VillageInfo1Text.SetColor(_UI_Color.Instance.color22);
				VillageInfo2Text.SetColor(_UI_Color.Instance.color4);
				VillageInfo3Text.SetColor(_UI_Color.Instance.color4);
			break;
		}	
	}
}
