using UnityEngine;
using System.Collections;

public class _UI_CS_IngameMenu : MonoBehaviour {
	
	//Instance
	public static _UI_CS_IngameMenu Instance = null;
	public UIPanel		 m_CS_Ingame_MenuPanel;
	
	public UIButton 	 backGameButton;
	
	public UIPanelTab	 invTab;
	public UIPanelTab	 proFileTab;
	public UIPanelTab	 abilitiesTab;
	public UIPanelTab    akaneTrials;
	public UIPanelTab    option;
	public UIPanelTab    m_Player;
	public UIPanelTab    m_Mission;
	
	//item logic flag
	public bool 		 IsMustPTap = false;
	public bool			 isLockInvLogic = false;
	
	//invertory player model pos
	public Transform 	 PlayerPos;
	public Transform	 currentPlayerPos;

	public Transform	pickUpnSound;
	
	void Awake() {
		Instance = this;
		if (localizeMgr_ == null) {
            localizeMgr_ = LocalizeFontManager.ManagerInstance;
        }
		localizeMgr_.OnLangChanged += this.UpdateBagAndEquipInfo;
	}
	
	// Use this for initialization
	void Start () {
        backGameButton.AddInputDelegate(IngameMenuBackGameButtonDelegate);	
		invTab.AddInputDelegate(IngameMenu_InvTabDelegate);
		proFileTab.AddInputDelegate(IngameMenu_ProFileTabDelegate);
		akaneTrials.AddInputDelegate(IngameMenu_AchievementTabDelegate);
		option.AddInputDelegate(OptionTabDelegate);
		abilitiesTab.AddInputDelegate(IngameMenu_AbilitiesTabDelegate);
		m_Player.AddInputDelegate(PlayerPanelTabDelegate);
		m_Mission.AddInputDelegate(MissionPanelTabDelegate);
		InitEquipStateIcon();
	}

	// Update is called once per frame
	void Update () {
		UpdateEquip();
	}
	
#region Local
	private static LocalizeManage localizeMgr_ = null;
	private int bagCount = 40;
	private void UpdateBagAndEquipInfo(LocalizeManage.Language _lang) {
		for(int i = 0;i<bagCount;i++) {
			if(Inventory.Instance.bagItemArray[i].m_IsEmpty) {
				continue;
			}
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.bagItemArray[i].m_ItemInfo.ID,Inventory.Instance.bagItemArray[i].m_ItemInfo.perfrab,
										Inventory.Instance.bagItemArray[i].m_ItemInfo.gem,Inventory.Instance.bagItemArray[i].m_ItemInfo.enchant,Inventory.Instance.bagItemArray[i].m_ItemInfo.element,
										(int)Inventory.Instance.bagItemArray[i].m_ItemInfo.level);
			Inventory.Instance.bagItemArray[i].ItemStruct = tempItem;
		}
		for(int i = 0;i<Inventory.Instance.equipmentCount;i++) {
			if(Inventory.Instance.equipmentArray[i].m_IsEmpty) {
				continue;
			}
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[i].m_ItemInfo.ID,Inventory.Instance.equipmentArray[i].m_ItemInfo.perfrab,
				Inventory.Instance.equipmentArray[i].m_ItemInfo.gem,Inventory.Instance.equipmentArray[i].m_ItemInfo.enchant,Inventory.Instance.equipmentArray[i].m_ItemInfo.element,(int)Inventory.Instance.equipmentArray[i].m_ItemInfo.level);
			Inventory.Instance.equipmentArray[i].ItemStruct = tempItem;
		}
	}
	
	public bool isUpdateEquiptments = false;
	private void UpdateEquip() {
		if(null != Player.Instance&&Player.Instance.bAssetBundleReady){
			if(isUpdateEquiptments){
				isUpdateEquiptments = false;
                Player.Instance.EquipementMan.DetachAllItems(_PlayerData.Instance.CharactorInfo.sex);
                _UI_CS_DownLoadPlayerForInv.Instance.equipmentMan.DetachAllItems(_PlayerData.Instance.CharactorInfo.sex);
				for(int i = 0;i<Inventory.Instance.equipmentArray.Length;i++){
                    bool isAttach = true;
                    Transform itemEx = null;
					if(!Inventory.Instance.equipmentArray[i].m_IsEmpty){
                        isAttach = true;
                        ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[i].m_ItemInfo.ID, Inventory.Instance.equipmentArray[i].m_ItemInfo.perfrab,Inventory.Instance.equipmentArray[i].m_ItemInfo.gem, Inventory.Instance.equipmentArray[i].m_ItemInfo.enchant,Inventory.Instance.equipmentArray[i].m_ItemInfo.element,(int)Inventory.Instance.equipmentArray[i].m_ItemInfo.level);
						itemEx = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(tempItem._ItemID,tempItem._TypeID,tempItem._PrefabID))as Transform;  
					}else{
                        isAttach = false;
					}
                    if (itemEx) {
                        Transform itemEx2 = Instantiate(itemEx) as Transform;
                        Player.Instance.EquipementMan.UpdateItemInfoBySlot((uint)i, itemEx, Inventory.Instance.equipmentArray[i].m_ItemInfo, isAttach, _PlayerData.Instance.CharactorInfo.sex);
                        _UI_CS_DownLoadPlayerForInv.Instance.equipmentMan.UpdateItemInfoBySlot((uint)i, itemEx2, Inventory.Instance.equipmentArray[i].m_ItemInfo, isAttach, _PlayerData.Instance.CharactorInfo.sex);
                    }
				}
                Player.Instance.EquipementMan.UpdateEquipment(_PlayerData.Instance.CharactorInfo.sex);
                Player.Instance.GetComponent<PreLoadPlayer>().usingLatestConfig = true;
                _UI_CS_DownLoadPlayerForInv.Instance.equipmentMan.UpdateEquipment(_PlayerData.Instance.CharactorInfo.sex);
                _UI_CS_DownLoadPlayerForInv.Instance.usingLatestConfig = true;
			}
		}
	}
	
	private void InitEquipStateIcon(){
		for(int i = 0;i<Inventory.Instance.equipmentIconArray.Length;i++ ){
				Inventory.Instance.equipmentIconArray[i].gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
		}
	}
	void IngameMenu_InvTabDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(invIsShow){
					MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
					MoneyBadgeInfo.Instance.Hide(false);
					BackToIngame();	
					_AbiMenuCtrl.Instance.LeaveAbiSetting();
				}else{
					MoneyBadgeInfo.Instance.Hide(false);
					SetIngameMenuState(1);
					Inventory.Instance.BagPanel.ShowBag(Inventory.Instance.bagPosition);
					Inventory.Instance.bagItemArray[0].CancelPress();
					_UI_CS_IngameMenu.Instance.ShowPlayerModel(_UI_CS_IngameMenu.Instance.PlayerPos);
					SetTransmuteState(false);
					_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INV);
				}
				break;
		}	
	}
	
	void IngameMenu_AchievementTabDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
				if(trialsIsShow){
					MoneyBadgeInfo.Instance.Hide(false);
					BackToIngame();	
					_AbiMenuCtrl.Instance.LeaveAbiSetting();
				}else{
					MoneyBadgeInfo.Instance.Hide(false);
					SetIngameMenuState(4);
					Inventory.Instance.BagPanel.HideBag();
					Inventory.Instance.bagItemArray[0].CancelPress();
	                SurveillanceCamera.Instance.ShutDown();
					if(null != ScenesLightCtrl.Instance)
						ScenesLightCtrl.Instance.OpenLight();
					_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
					_AbiMenuCtrl.Instance.LeaveAbiSetting();
					_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_ACH);
				}
				break;
		}	
	}
	
	void OptionTabDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
				if(optionIsShow){
					optionIsShow = false;
					MoneyBadgeInfo.Instance.Hide(false);
					BackToIngame();	
				}else{
					MoneyBadgeInfo.Instance.Hide(false);
					SetIngameMenuState(5);
					Inventory.Instance.BagPanel.HideBag();
					Inventory.Instance.bagItemArray[0].CancelPress();
					SurveillanceCamera.Instance.ShutDown();
					if(null != ScenesLightCtrl.Instance)
						ScenesLightCtrl.Instance.OpenLight();
					_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
					_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_OPT);
				}
			break;
		}
	}
	
	void IngameMenu_ProFileTabDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
				if(profileIsShow){
					MoneyBadgeInfo.Instance.Hide(false);
					BackToIngame();	
					_AbiMenuCtrl.Instance.LeaveAbiSetting();
				}else{
					MoneyBadgeInfo.Instance.Hide(false);
					SetIngameMenuState(3);
					Inventory.Instance.BagPanel.HideBag();
					Inventory.Instance.bagItemArray[0].CancelPress();	
					 SurveillanceCamera.Instance.ShutDown();
					if(null != ScenesLightCtrl.Instance)
						ScenesLightCtrl.Instance.OpenLight();
					_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
					_AbiMenuCtrl.Instance.LeaveAbiSetting();
					_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INFO);
				}
				break;
		}	
	}
	
	void IngameMenu_AbilitiesTabDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
				if(abiIsShow){
					MoneyBadgeInfo.Instance.Hide(false);
					BackToIngame();	
					_AbiMenuCtrl.Instance.LeaveAbiSetting();
				}else{
					MoneyBadgeInfo.Instance.Hide(false);
					SetIngameMenuState(2);
					Inventory.Instance.BagPanel.HideBag();
					Inventory.Instance.bagItemArray[0].CancelPress();
					SurveillanceCamera.Instance.ShutDown();
					if(null != ScenesLightCtrl.Instance)
						ScenesLightCtrl.Instance.OpenLight();
					_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
					_AbiMenuCtrl.Instance.LeaveAbiSetting();
					_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_ABI);
				}
				break;
		}	
	}
	
	void PlayerPanelTabDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				break;
		}	
	}

	void MissionPanelTabDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				break;
		}	
	}
	
	void AkaneTrialsPanelTabDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				SurveillanceCamera.Instance.ShutDown();
				if(null != ScenesLightCtrl.Instance)
					ScenesLightCtrl.Instance.OpenLight();
				_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
				break;
		}	
	}

	void IngameMenuBackGameButtonDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				MoneyBadgeInfo.Instance.Hide(false);
				BackToIngame();	
				_AbiMenuCtrl.Instance.LeaveAbiSetting();
				break;
		}	
	}
	
	private IEnumerator ShowPlayerModelBC() {
		yield return null;
		if(null != ScenesLightCtrl.Instance)
			ScenesLightCtrl.Instance.CloseLight();
		_UI_CS_DownLoadPlayerForInv.Instance.OpenLight();
		Vector3 tPos = _UI_CS_Ctrl.Instance.m_UI_Camera.WorldToScreenPoint(currentPlayerPos.position);
		SurveillanceCamera.Instance.ShowAt(new Vector2(tPos.x,tPos.y), new Vector2(Screen.width/5, Screen.height));
	}
	
	
#endregion
	
#region Interface	
	public UIPanel profilePanel;
	public UIPanel abilitiesPanel;
	public UIPanel achievementPanel;
	public UIPanel optionPanel;
	private bool invIsShow	  = false;
	private bool abiIsShow 	  = false;
	private bool profileIsShow = false;
	private bool trialsIsShow  = false;
	private bool optionIsShow  = false;
	public void SetIngameMenuState(int idx){
		if(TutorialMan.Instance.GetTutorialFlag()) {
			TutorialMan.Instance.ClearArrowList();
		}
		switch(idx){
		case 1:
			invTab.SetState (0);
			proFileTab.SetState (1);
			abilitiesTab.SetState (1);
			akaneTrials.SetState (1);
			option.SetState (0);
			invIsShow	  = true;
			abiIsShow	  = false;
			profileIsShow = false;
			trialsIsShow =  false;
			optionIsShow  = false;
			Inventory.Instance.panel.BringIn();
			profilePanel.Dismiss();
			achievementPanel.Dismiss();
			abilitiesPanel.Dismiss();
			optionPanel.Dismiss();
			break;
		case 2:
			invTab.SetState (1);
			proFileTab.SetState (1);
			abilitiesTab.SetState (0);
			akaneTrials.SetState (1);
			option.SetState (0);
			invIsShow	  = false;
			abiIsShow	  = true;
			profileIsShow = false;
			trialsIsShow =  false;
			optionIsShow  = false;
			Inventory.Instance.panel.Dismiss();
			profilePanel.Dismiss();
			achievementPanel.Dismiss();
			abilitiesPanel.BringIn();
			optionPanel.Dismiss();
			break;
		case 3:
			invTab.SetState (1);
			proFileTab.SetState (0);
			abilitiesTab.SetState (1);
			akaneTrials.SetState (1);
			option.SetState (0);
			invIsShow	  = false;
			abiIsShow	  = false;
			profileIsShow = true;
			trialsIsShow =  false;
			optionIsShow  = false;
			Inventory.Instance.panel.Dismiss();
			profilePanel.BringIn();
			achievementPanel.Dismiss();
			abilitiesPanel.Dismiss();
			optionPanel.Dismiss();
			break;
		case 4:
			invTab.SetState (1);
			proFileTab.SetState (1);
			abilitiesTab.SetState (1);
			akaneTrials.SetState (0);
			option.SetState (0);
			invIsShow	  = false;
			abiIsShow	  = false;
			profileIsShow = false;
			trialsIsShow =  true;
			optionIsShow  = false;
			Inventory.Instance.panel.Dismiss();
			profilePanel.Dismiss();
			achievementPanel.BringIn();
			abilitiesPanel.Dismiss();
			optionPanel.Dismiss();
			break;
		case 5:
			invTab.SetState (1);
			proFileTab.SetState (1);
			abilitiesTab.SetState (1);
			option.SetState (0);
			akaneTrials.SetState (1);
			invIsShow	  = false;
			abiIsShow	  = false;
			profileIsShow = false;
			trialsIsShow  = false;
			optionIsShow  = true;
			Inventory.Instance.panel.Dismiss();
			profilePanel.Dismiss();
			achievementPanel.Dismiss();
			abilitiesPanel.Dismiss();
			optionPanel.BringIn();
			break;
		default:
			break;
		}
	}
	
	public void UpdateEquipIconState(){
		for(int i = 0;i<Inventory.Instance.equipmentArray.Length;i++ ){
			if(Inventory.Instance.equipmentArray[i].m_IsEmpty){
				Inventory.Instance.equipmentIconArray[i].Hide(false);
			}else{
				Inventory.Instance.equipmentIconArray[i].Hide(true);
			}
		}
	}
	
	public void BackToIngame(){
		MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
		_AbiMenuCtrl.Instance.UpDateIngameAbilitiesIcon();
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
		m_CS_Ingame_MenuPanel.Dismiss();
		Inventory.Instance.BagPanel.HideBag();
		Inventory.Instance.bagItemArray[0].CancelPress();
		SurveillanceCamera.Instance.ShutDown();
		if(null != ScenesLightCtrl.Instance)
			ScenesLightCtrl.Instance.OpenLight();
		_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
		_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
        Player.Instance.ReactivePlayer();
	}
	
	public _UI_CS_InventoryItem GetBagSlotObject(int bag,int slot){
		switch(bag){
		case 1:
			return	Inventory.Instance.equipmentArray[slot];
		case 2:
			return 	Inventory.Instance.bagItemArray[slot];
		default:
			return null;
		}
	}
	
	public int EmptyBagIndex(int bagID){
		int i;
		if(bagID == 1) {
			for(i = 0;i< 20;i++) {
				if(true == Inventory.Instance.bagItemArray[i].m_IsEmpty) {
					return i;
				}
			}
			return -1;
		}else{
			for(i = 20;i< bagCount;i++) {
				if(true == Inventory.Instance.bagItemArray[i].m_IsEmpty) {
					return i;
				}
			}
			return -1;
		}
	}
	
	public int EmptyBagIndex() {
		int i;
		for(i = 0;i< bagCount;i++) {
			if(true == Inventory.Instance.bagItemArray[i].m_IsEmpty) {
				return i;
			}
		}
		return -1;
	}	
	
	public int EmptyBagIndexSkipIdx(int skipIdx) {
		int i;
		for(i = 0;i< bagCount;i++) {
			if(i != skipIdx){
				if(true == Inventory.Instance.bagItemArray[i].m_IsEmpty) {
					return i;
				}
			}
		}
		return -1;
	}
	
	public void UpdateInvBGColor(){
		for(int i = 0;i<Inventory.Instance.bagItemArray.Length;i++) {
			if(!Inventory.Instance.bagItemArray[i].m_IsEmpty){
				float itemVal = (Inventory.Instance.bagItemArray[i].ItemStruct.info_gemVal + Inventory.Instance.bagItemArray[i].ItemStruct.info_encVal + Inventory.Instance.bagItemArray[i].ItemStruct.info_eleVal);
				_UI_Color.Instance.SetNameColor(itemVal,Inventory.Instance.bagItemArray[i].BG);
			}else {
				Inventory.Instance.bagItemArray[i].BG.SetColor(_UI_Color.Instance.color10);
			}
			Inventory.Instance.bagItemArray[i].UpdateItemHighLevel();
		}
		for(int j = 0;j<Inventory.Instance.equipmentArray.Length;j++) {
			if(!Inventory.Instance.equipmentArray[j].m_IsEmpty){
				float itemVal = (Inventory.Instance.equipmentArray[j].ItemStruct.info_gemVal + Inventory.Instance.equipmentArray[j].ItemStruct.info_encVal + Inventory.Instance.equipmentArray[j].ItemStruct.info_eleVal);
				_UI_Color.Instance.SetNameColor(itemVal,Inventory.Instance.equipmentArray[j].BG);
			}else {
				Inventory.Instance.equipmentArray[j].BG.SetColor(_UI_Color.Instance.color10);
			}
		}
	}
	
	public GameObject 	 accesorrySound;
	public GameObject	 bodySound;
	public GameObject	 headSound;
	public GameObject	 underwearSound;
	public GameObject	 mediumSound;
	public GameObject 	 dropAccSound;
	public GameObject	 dropWpnSound;
	public GameObject	 dropBodySound;
	public void PlayEquipSound(int typeIdx){
		switch(typeIdx){
		case 0:
			SoundCue.PlayPrefabAndDestroy(headSound.transform);
			break;
		case 1:
			SoundCue.PlayPrefabAndDestroy(accesorrySound.transform);
			break;
		case 2:
			SoundCue.PlayPrefabAndDestroy(bodySound.transform);
			break;
		case 3:
			SoundCue.PlayPrefabAndDestroy(bodySound.transform);
			break;
		case 4:
			SoundCue.PlayPrefabAndDestroy(accesorrySound.transform);
			break;
		case 5:
			SoundCue.PlayPrefabAndDestroy(accesorrySound.transform);
			break;
		case 6:
			SoundCue.PlayPrefabAndDestroy(mediumSound.transform);
			break;
		case 7:
			SoundCue.PlayPrefabAndDestroy(mediumSound.transform);
			break;
		case 8:
			SoundCue.PlayPrefabAndDestroy(underwearSound.transform);
			break;
		default:
			break;
		}
	}
	
	public void PlayDropSound(int typeIdx){
		switch(typeIdx){
		case 0:
			SoundCue.PlayPrefabAndDestroy(dropBodySound.transform);
			break;
		case 1:
			SoundCue.PlayPrefabAndDestroy(dropBodySound.transform);
			break;
		case 2:
			SoundCue.PlayPrefabAndDestroy(dropAccSound.transform);
			break;
		case 3:
			SoundCue.PlayPrefabAndDestroy(dropBodySound.transform);
			break;
		case 4:
			SoundCue.PlayPrefabAndDestroy(dropBodySound.transform);
			break;
		case 5:
			SoundCue.PlayPrefabAndDestroy(dropAccSound.transform);
			break;
		case 6:
			SoundCue.PlayPrefabAndDestroy(dropBodySound.transform);
			break;
		case 7:
			SoundCue.PlayPrefabAndDestroy(dropWpnSound.transform);
			break;
		case 8:
			SoundCue.PlayPrefabAndDestroy(dropWpnSound.transform);
			break;
		default:
			break;
		}
	}
	
	public void BagEquipShadow(int equipIdx,bool isShow) {
		if(!isShow){
			for(int i = 0;i<Inventory.Instance.equipmentArray.Length;i++) {
				Inventory.Instance.equipmentArray[i].Shadow.Hide(true);
			}
			return;
		}
		for(int i = 0;i<Inventory.Instance.equipmentArray.Length;i++) {
			Inventory.Instance.equipmentArray[i].Shadow.Hide(false);
		}
		Inventory.Instance.equipmentArray[equipIdx].Shadow.Hide(true);
	}
	
	public void ShowPlayerModel(Transform obj) {
		 currentPlayerPos = obj;
		 StartCoroutine(ShowPlayerModelBC());
	}
	
	
	
	public _UI_CS_InventoryItem  [] motionlessArray;
	public UIButton	bgImg;
	public Color colorBlack;
	public Color colorPurple;
	public bool	isTransmute = false;
	public void SetTransmuteState(bool isAct) {
		isTransmute = isAct;
		if(isAct){
			motionlessArray[0].GetComponent<UIButton>().SetColor(_UI_Color.Instance.color19);
			MouseCtrl.Instance.SetMouseStats(MouseIconType.SELL);
			bgImg.SetColor(colorPurple);
		}else{
			motionlessArray[0].GetComponent<UIButton>().SetColor(_UI_Color.Instance.color20);
			MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
			bgImg.SetColor(colorBlack);
		}
	}
	
	///item位置重置
	public void ResetItemPos() {	
		switch(Inventory.Instance.preBagItmeIndex) {
		case 1:
				Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].transform.position = Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
					break;
		case 2:
				Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].transform.position = Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
					break;
		case 4:
					Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).transform.position = 
					Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).m_StartPosition;
						break;
		}
	}
#endregion

}
