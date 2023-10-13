using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_AbilitiesTrainer : MonoBehaviour {
	public static _UI_CS_AbilitiesTrainer Instance;
	public UIPanel AbilitiesTrainerPanel;
	public UIButton 	fareWellBtn;
	public UIButton 	fareWellIcon;
	public SpriteText 	fareWellText;
	public UIPanelTab AbilitiesTab;
	public UIPanelTab WeaponTab;
	public UIPanelTab ArmorTab;
	public UIPanel AbilitiesPanel;
	public UIPanel WeaponPanel;
	public UIPanel ArmorPanel;
	public  List<_UI_CS_AbilitiesTrainerItem> 		AbilShopList   = new List<_UI_CS_AbilitiesTrainerItem>();
	public  List<int> 								AbilExistList  = new List<int>();
	public int CurrentAbilID;
	public EMoneyType MoneyType = new  EMoneyType();
	public UIPanel 		TrainMsgPanel;
	public UIButton 	IconBtn;
//	public UIButton 	PassBtn;
//	public UIButton 	SkBtn;
//	public UIButton 	SfBtn;
	public UIButton 	SBGBtn;
	public SpriteText   levelText;
	public SpriteText   nameText;
	public SpriteText   Des1Text;
	public Transform    Description2Group;
	public SpriteText	Des2TitleText;
	public SpriteText	Des2SubTitleText;
	public SpriteText   Des2Text;    
	public SpriteText   StatText;
    public Transform    BuyBTN;
	public SpriteText   countText;
	public SpriteText   PaySkText;
//	public SpriteText   PaySfText;
	public int CurItemTypeInMSG = 0;		// 0 是代表在确认框中的是技能Ability， 1代表确认框中的是Mastery， 用这个来区分点击购买按钮发送消息的方式
    public _UI_CS_AbilitiesTrainerItem CurrentAbiInfo;
	public Transform trainPos;
//	public UIButton  npc;
	public UIButton  smallBg;
	
	public  List<int> AllBaseAbiList  = new List<int>();
	
	public void AwakeAbilitiesTrainer(){
		AbilitiesTrainerPanel.BringIn();
		MoneyBadgeInfo.Instance.Hide(false);
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_ABILITIES_TRAINER);
		InitImage();
	}
	
	private void InitImage(){
		//downloading image
//		TextureDownLoadingMan.Instance.DownLoadingTexture("Figure_use7",npc.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Dia_Box3",smallBg.transform);
	}
	
	public int IsExistAbil(int id){
		for(int i = 0;i<AbilExistList.Count;i++){
			if(id == ((AbilExistList[i]/100) * 100 + 1)){
				return AbilExistList[i];
			}
		}
		return id;
	}
	
	public int GetAbiLevelFromProwessExistAbiIdx(int id){
		for(int i = 0;i < _AbiMenuCtrl.Instance.ExistProwessList.Count;i++){
			if(id == (_AbiMenuCtrl.Instance.ExistProwessList[i].m_AbilitieID/100*100+1)){
				return i;
			}
		}
		return -1;
	}
	
	public int GetAbiLevelFromFortitudeExistAbiIdx(int id){
		for(int i = 0;i < _AbiMenuCtrl.Instance.ExistFortitudeList.Count;i++){
			if(id == (_AbiMenuCtrl.Instance.ExistFortitudeList[i].m_AbilitieID/100*100+1)){
				return i;
			}
		}
		return -1;
	}
	
	public int GetAbiLevelFromCunningExistAbiIdx(int id){
		for(int i = 0;i < _AbiMenuCtrl.Instance.ExistCunningList.Count;i++){
			if(id == (_AbiMenuCtrl.Instance.ExistCunningList[i].m_AbilitieID/100*100+1)){
				return i;
			}
		}
		return -1;
	}
	
	public void AbiShopListObjInit(int id){
		int index;
		for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
			AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];	
			_UI_CS_AbilitiesTrainerItem temp = new _UI_CS_AbilitiesTrainerItem();
			if(id+1 == ability.id){
				if(ability.Info != null) {
					_UI_CS_AbilitiesTrainer.Instance.ReadAbilitiesLearnInfo(id+1,temp);
					AbilShopList.Add(temp);
				}
			}
		}	
	}
	
	public void InitAbilitiesTrainer(){
		AbilShopList.Clear();
		for(int i =0;i<AllBaseAbiList.Count;i++){
			if(-1 != GetAbiLevelFromProwessExistAbiIdx(AllBaseAbiList[i])){
				AbiShopListObjInit(_AbiMenuCtrl.Instance.ExistProwessList[GetAbiLevelFromProwessExistAbiIdx(AllBaseAbiList[i])].m_AbilitieID);
			}else if(-1 != GetAbiLevelFromFortitudeExistAbiIdx(AllBaseAbiList[i])){
				AbiShopListObjInit(_AbiMenuCtrl.Instance.ExistFortitudeList[GetAbiLevelFromFortitudeExistAbiIdx(AllBaseAbiList[i])].m_AbilitieID);
			}else if(-1 != GetAbiLevelFromCunningExistAbiIdx(AllBaseAbiList[i])){
				AbiShopListObjInit(_AbiMenuCtrl.Instance.ExistCunningList[GetAbiLevelFromCunningExistAbiIdx(AllBaseAbiList[i])].m_AbilitieID);
			}else{
				AbiShopListObjInit(AllBaseAbiList[i]-1);
			}
		}	
		_UI_CS_AbilitiesTrainer_AllAbilities.Instance.m_AAItemList.Clear();
		_UI_CS_AbilitiesTrainer_AllAbilities.Instance.m_List.ClearList(true);
		_UI_CS_AbilitiesTrainer_ProwessAbilities.Instance.m_AAItemList.Clear();
		_UI_CS_AbilitiesTrainer_ProwessAbilities.Instance.m_List.ClearList(true);
		_UI_CS_AbilitiesTrainer_FortitudeAbilities.Instance.m_AAItemList.Clear();
		_UI_CS_AbilitiesTrainer_FortitudeAbilities.Instance.m_List.ClearList(true);
		_UI_CS_AbilitiesTrainer_CunningAbilities.Instance.m_AAItemList.Clear();
		_UI_CS_AbilitiesTrainer_CunningAbilities.Instance.m_List.ClearList(true);
		for(int i =0;i< AbilShopList.Count;i++){	
			_UI_CS_AbilitiesTrainer_AllAbilities.Instance.AddAbilitiesAllListElement(AbilShopList[i]);
		}
		_UI_CS_AbilitiesTrainer.Instance.InitAbilities();
	}
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		fareWellBtn.AddInputDelegate(FareWellBtnDelegate);
		AbilitiesTab.AddInputDelegate(AbilitiesTabDelegate);
		WeaponTab.AddInputDelegate(WeaponTabDelegate);
		ArmorTab.AddInputDelegate(ArmorTabDelegate);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//初始化技能 
	public void InitAbilities(){
		_UI_CS_AbilitiesTrainer_AllAbilities.Instance.InitAbilitiesAllList();
		for(int i=0;i<_UI_CS_AbilitiesTrainer_AllAbilities.Instance.m_count;i++){	
			switch(_UI_CS_AbilitiesTrainer_AllAbilities.Instance.m_AAItemList[i].m_type){
			case 1:
					_UI_CS_AbilitiesTrainer_ProwessAbilities.Instance.AddAbilitiesAllListElement(_UI_CS_AbilitiesTrainer_AllAbilities.Instance.m_AAItemList[i]);
					break;
			case 2:
					_UI_CS_AbilitiesTrainer_FortitudeAbilities.Instance.AddAbilitiesAllListElement(_UI_CS_AbilitiesTrainer_AllAbilities.Instance.m_AAItemList[i]);
					break;
			case 4:
					_UI_CS_AbilitiesTrainer_CunningAbilities.Instance.AddAbilitiesAllListElement(_UI_CS_AbilitiesTrainer_AllAbilities.Instance.m_AAItemList[i]);
					break;
			default:
					break;
			}
		}
		_UI_CS_AbilitiesTrainer_ProwessAbilities.Instance.InitAbilitiesAllList();
		_UI_CS_AbilitiesTrainer_FortitudeAbilities.Instance.InitAbilitiesAllList();
		_UI_CS_AbilitiesTrainer_CunningAbilities.Instance.InitAbilitiesAllList();

	}
	
	void FareWellBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.PRESS:
					fareWellIcon.SetColor(_UI_Color.Instance.color1);
					fareWellText.SetColor(_UI_Color.Instance.color1);	
			
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
					fareWellIcon.SetColor(_UI_Color.Instance.color1);
					fareWellText.SetColor(_UI_Color.Instance.color1);	
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:	
				fareWellIcon.SetColor(_UI_Color.Instance.color2);
				fareWellText.SetColor(_UI_Color.Instance.color4);
			break;
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				PopMsg(0);
				BGMInfo.Instance.isPlayUpGradeEffectSound = true;
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				AbilitiesTrainerPanel.Dismiss();
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
                Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
				fareWellIcon.SetColor(_UI_Color.Instance.color2);
				fareWellText.SetColor(_UI_Color.Instance.color4);		
				break;
		   default:
				break;
		}	
	}
	
	void AbilitiesTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				PopMsg(0);
				AbilitiesPanel.BringIn();
				WeaponPanel.Dismiss();
				ArmorPanel.Dismiss();
				break;
		   default:
				break;
		}	
	}
	
	void WeaponTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				PopMsg(0);
				AbilitiesPanel.Dismiss();
				WeaponPanel.BringIn();
				ArmorPanel.Dismiss();
				break;
		   default:
				break;
		}	
	}
	
	void ArmorTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				PopMsg(0);
				AbilitiesPanel.Dismiss();
				WeaponPanel.Dismiss();
				ArmorPanel.BringIn();
				break;
		   default:
				break;
		}	
	}
	
	public int ReadAbilitiesLearnInfo(int ID, _UI_CS_AbilitiesTrainerItem abiObj){
		int aLevel = ID % 100 + 1;
		int index;
		for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
			AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
			if(ability.id == ID){
				abiObj.m_icon        = ability.icon;
				abiObj.m_name        = ability.name;
				abiObj.m_level       = aLevel;
				abiObj.m_type        =  (int)ability.Info.Discipline;
            	abiObj.m_Des1        = ability.Info.Description1;
				abiObj.m_Des2        = "";
				abiObj.m_playerLevel = ability.Info.Level;
				abiObj.m_skVal		 = ability.Info.Karma;
				abiObj.m_id 		 = ID;
				return 1;
			}
		}
		return 2;
	}
	
	public void PopMsg(int msg){
	
		switch(msg){
			
		case 0:
			TrainMsgPanel.Dismiss();
			break;
		case 1:
			TrainMsgPanel.BringIn();
			break;
		default:
			break;
		
		}
	
	}
	
//	void PassBtnDelegate(ref POINTER_INFO ptr)
//	{
//		switch(ptr.evt)
//		{
//		   case POINTER_INFO.INPUT_EVENT.TAP:
//				PopMsg(0);
//				break;
//		   default:
//				break;
//		}	
//	}
	
//	void SkBuyBtnDelegate(ref POINTER_INFO ptr)
//	{
//		MoneyType.Set(EMoneyType.eMoneyType_SK);
//		
//		switch(ptr.evt)
//		{
//		   case POINTER_INFO.INPUT_EVENT.TAP:
//				PopMsg(0);
//
//                Debug.Log(CurItemTypeInMSG);
//				if(CurItemTypeInMSG == 0)
//				{
//					CS_Main.Instance.g_commModule.SendMessage(
//					   		ProtocolGame_SendRequest.StudyAbility(CurrentAbilID,_UI_CS_AbilitiesTrainer.Instance.CurrentAbilID,MoneyType)
//					);
//				}
//			
//				if(CurItemTypeInMSG == 1)
//				{
//					EMasteryType type = new EMasteryType();
//					type.Set(CurrentMasteryID);
//					CS_Main.Instance.g_commModule.SendMessage(
//						   		ProtocolGame_SendRequest.masteryLvlupReq(type)
//						);
//					
//				}
//				break;
//		   default:
//				break;
//		}	
//	}
	
	public void RightClickLogic(){
		if(Input.GetButtonDown("Fire2")){
			MoneyType.Set(EMoneyType.eMoneyType_SK);
			//Debug.Log(CurItemTypeInMSG);
			if(CurItemTypeInMSG == 0){
				CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.StudyAbility(CurrentAbilID,_UI_CS_AbilitiesTrainer.Instance.CurrentAbilID,MoneyType)
				);
			}
			if(CurItemTypeInMSG == 1){
                if (CurrentAbiInfo != null && !CurrentAbiInfo.m_isMaxLevel)
                {
                    CS_Main.Instance.g_commModule.SendMessage(
                                ProtocolGame_SendRequest.masteryLvlupReq(new EMasteryType(CurrentAbiInfo.m_MasteryClass))
                        );
                }
			}
		}
	}
	
//	void SfBuyBtnDelegate(ref POINTER_INFO ptr)
//	{
//		MoneyType.Set(EMoneyType.eMoneyType_FK);
//		
//		switch(ptr.evt)
//		{
//		   case POINTER_INFO.INPUT_EVENT.TAP:
//				PopMsg(0);
//				CS_Main.Instance.g_commModule.SendMessage(
//				   		ProtocolGame_SendRequest.StudyAbility(1003,_UI_CS_AbilitiesTrainer.Instance.CurrentAbilID,MoneyType)
//				);
//				break;
//		   default:
//				break;
//		}	
//	}
	
	public void UpdateAbil(int id){
	
		_AbiMenuCtrl.Instance.ChangeExistAbiInfo(id-1,id);

		InitAbilitiesTrainer();
	
	}
	
	public int CompareObjPriceLowToHigh(_UI_CS_AbilitiesTrainerItem src,_UI_CS_AbilitiesTrainerItem dest){
		if(src.m_skVal < dest.m_skVal){
			return -1;
		}else if(src.m_skVal > dest.m_skVal){
			return 1;
		}else{
			return 0;
		}
	}
	
	public void ShowAbiInfo(_UI_CS_AbilitiesTrainerItem info,Color colorBtn){
		InitAbiInfo(info,colorBtn);
		TrainMsgPanel.transform.position = trainPos.position;
	}
	
	public void DismissAbiInfo(){
		TrainMsgPanel.transform.position = new Vector3(999,999,999);
	}
	
	private void InitAbiInfo(_UI_CS_AbilitiesTrainerItem info,Color colorBtn){
        CurrentAbiInfo = info;
		CurrentAbilID = info.m_id;
		IconBtn.SetUVs(new Rect(0,0,1,1));
		IconBtn.SetTexture(info.m_icon);
		levelText.Text = info.m_level.ToString();
		nameText.Text = info.m_name;
		Des1Text.Text = info.m_Des1;
		Des2Text.Text = info.m_Des2;
        CurItemTypeInMSG = info.m_IsAbilityOrMastery;
        BuyBTN.gameObject.SetActive(true);
        if (info.m_IsAbilityOrMastery == 0){
			//------------------------------------------------
			 switch (info.m_type) {
                case 1:
					LocalizeManage.Instance.GetDynamicText(StatText,"REQPOWER");
					 countText.Text = info.m_playerLevel.ToString();
                    break;
                case 2:
                   LocalizeManage.Instance.GetDynamicText(StatText,"REQDEFENSE");
					 countText.Text = info.m_playerLevel.ToString();
                    break;
                case 4:
                    LocalizeManage.Instance.GetDynamicText(StatText,"REQSKILL");
					 countText.Text = info.m_playerLevel.ToString();
                    break;
            }
			//---------------------------------------------------
           
            Description2Group.localPosition = new Vector3(1000, 1000, 1000);
			LocalizeManage.Instance.GetDynamicText(Des2TitleText,"FURTHERTA");
			LocalizeManage.Instance.GetDynamicText(Des2SubTitleText,"FLAMET");
            Vector3 _tempPos = Des2Text.transform.localPosition;
            _tempPos.y = -0.3f;
            Des2Text.transform.localPosition = _tempPos;
        }else {
//			// level requid info
//			LocalizeManage.Instance.GetDynamicText(StatText,"REQLEVEL");
//			StatText.Text += info.m_playerLevel.ToString();
//			countText.Text = "";
			switch (info.m_type) {
                case 1:
					LocalizeManage.Instance.GetDynamicText(StatText,"REQPOWER");
					 countText.Text = info.m_playerLevel.ToString();
                    break;
                case 2:
                   LocalizeManage.Instance.GetDynamicText(StatText,"REQDEFENSE");
					 countText.Text = info.m_playerLevel.ToString();
                    break;
                case 4:
                    LocalizeManage.Instance.GetDynamicText(StatText,"REQSKILL");
					 countText.Text = info.m_playerLevel.ToString();
                    break;
            }
			
            Description2Group.localPosition = new Vector3(-5.559143f, -2.9151f, -1f);
			LocalizeManage.Instance.GetDynamicText(Des2TitleText,"AVAILABLETRA");
            Des2SubTitleText.Text = "";
            Vector3 _tempPos = Des2Text.transform.localPosition;
            _tempPos.y = 0.1f;
            Des2Text.transform.localPosition = _tempPos;
        }
        

        if (info.m_isMaxLevel)
        {
            SBGBtn.SetColor(Color.gray);
            PaySkText.Text = "------";
            BuyBTN.gameObject.SetActive(false);
        }
        else
        {
            SBGBtn.SetColor(colorBtn);
            if (-1 != info.m_skVal)
            {
                PaySkText.Text = info.m_skVal.ToString();
            }
        }
	}
}
