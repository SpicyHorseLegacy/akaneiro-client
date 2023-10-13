using UnityEngine;
using System.Collections;

public class _UI_CS_FightScreen : MonoBehaviour {
	
	//Instance
	public static _UI_CS_FightScreen Instance = null;
	
	public UIPanel		 m_CS_Ingame_NormalPanel;
	
	public bool 		m_isLogout = false;

	public UIPanel		m_DisconnectPanel;
	public SpriteText	disConnectText;
	public UIButton     m_DisconnectBtn;
	
	public _UI_CS_BagCtrl BagPanel;
	//Ingame Normal
	public UIButton 	 m_IngameNormal_Menu;
	public UIButton []	 m_IngameNormal_FastMenuBtn;
	
	public UIButton 	  []	 m_IngameNormal_AbilitiesBtn;
	public UIButton 	  []	 m_IngameNormal_AbilitiesFrameBtn;
	public SpriteText	  [] 	 m_IngameNormal_AbilitiesCoolDown;
	public RectMaskEffect [] 	 m_abiMaskEffest;
	public UIButton 	  []  	 NoEnBtn;
	public UIButton 	 m_IngameNormal_SwitchBtn;
	
	public UIPanel      m_fightPanel;
	public bool         isIngameBring = true;
	public UIButton  SummonBtn;
	
	public UIButton   pingIcon;
	public UIButton   pingBG;
	public float	  SendPingTime = 0f;
	public float	  pingTime = 0f;
	private Color 	  pingColor = new Color();
	public Color 	  pingColor1 = new Color();
	public Color 	  pingColor2 = new Color();
	public Color 	  pingColor3 = new Color();
	public bool		  isCheckPing = false;
	public float 	  pingMaxTime = 2;
	private float 	  pingCurTime = 0;
	private Vector3   mousePos;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		
		m_DisconnectBtn.AddInputDelegate(DisconnectDelegate);
		
		m_IngameNormal_Menu.AddInputDelegate(IngameNormalMenuDelegate);
		m_IngameNormal_FastMenuBtn[0].AddInputDelegate(IngameNormalFMB0Delegate);
		m_IngameNormal_FastMenuBtn[1].AddInputDelegate(IngameNormalFMB1Delegate);
		m_IngameNormal_FastMenuBtn[2].AddInputDelegate(IngameNormalFMB2Delegate);
		
		m_IngameNormal_AbilitiesFrameBtn[0].AddInputDelegate(IngameNormalAbilities0Delegate);
		m_IngameNormal_AbilitiesFrameBtn[1].AddInputDelegate(IngameNormalAbilities1Delegate);
		m_IngameNormal_AbilitiesFrameBtn[2].AddInputDelegate(IngameNormalAbilities2Delegate);
		pingBG.AddInputDelegate(PingIconDelegate);
		
		SummonBtn.AddInputDelegate(SummonBtnDelegate);
		
		m_IngameNormal_SwitchBtn.AddInputDelegate(IngameNormalSwitchDelegate);		
		m_IngameNormal_AbilitiesCoolDown[0].Text = "";
		m_IngameNormal_AbilitiesCoolDown[1].Text = "";
		m_IngameNormal_AbilitiesCoolDown[2].Text = "";
		
		pingColor.r = 0;
		pingColor.g = 1;
		pingColor.b = 0;
		pingColor.a = 255;
		isCheckPing = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(null != Player.Instance){
			UpDateAbilNoEn();
		}
		if(isCheckPing){
			pingCurTime += Time.deltaTime;
			if(pingCurTime > pingMaxTime){
				pingCurTime = 0;
				SendPingToServer();
			}
		}
	}
	
	#region main menu btn ctrl
	public void SetMainMenuBtnState(int state) {
		for(int i = 0;i<m_IngameNormal_FastMenuBtn.Length;i++) {
			m_IngameNormal_FastMenuBtn[i].controlIsEnabled = false;
		}
		_UI_CS_KeyboardCtrl.Instance.AddKey(KeyCode.I);
		_UI_CS_KeyboardCtrl.Instance.AddKey(KeyCode.A);
		_UI_CS_KeyboardCtrl.Instance.AddKey(KeyCode.C);
		if(state == -1) {
			return;
		}
		if(state >=0 && state < m_IngameNormal_FastMenuBtn.Length) {
			  m_IngameNormal_FastMenuBtn[state].controlIsEnabled = true;
		}
		switch(state) {
		case 0:
			_UI_CS_KeyboardCtrl.Instance.DelKey(KeyCode.A);
			break;
		case 1:
			_UI_CS_KeyboardCtrl.Instance.DelKey(KeyCode.C);
			break;
		case 2:
			_UI_CS_KeyboardCtrl.Instance.DelKey(KeyCode.I);
			break;
		}
	}
	#endregion
	
	public void SendPingToServer(){
		SendPingTime = Time.realtimeSinceStartup;
		CS_Main.Instance.g_commModule.SendMessage(
		   		ProtocolGame_SendRequest.pingRequest()
		);
	}
	
	public void ChangePingState() {
		pingTime = ((Time.realtimeSinceStartup - SendPingTime)*1000);
		// MD no like this color.So chang to 3 color.
//		if(pingTime < 255){
//			pingColor.r = (pingTime/255);
//			pingColor.g = 1;
//		}else if(pingTime < 510){
//			pingColor.r = 1;
//			pingColor.g = 1 - (pingTime-255)/255;
//		}else{
//			pingColor.r = 1;
//			pingColor.g = 0;
//		}
		if(pingTime < 255){
			pingColor= pingColor1;
		}else if(pingTime < 510){
			pingColor= pingColor2;
		}else{
			pingColor= pingColor3;
		}
		pingIcon.SetColor(pingColor);
	}
	
	//清空玩家装备背包
	public void ClearBag(){
		
		for(int j = 0;j<Inventory.Instance.equipmentArray.Length;j++){
				
			Inventory.Instance.equipmentArray[j].m_IsEmpty = true;

		}
		
		for(int i = 0;i<Inventory.Instance.bagItemArray.Length;i++){
				
			Inventory.Instance.bagItemArray[i].m_IsEmpty = true;

		}
		
	}

	void DisconnectDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				m_DisconnectPanel.Dismiss();
				m_fightPanel.Dismiss();
				DismissAllPanel();
				_UI_CS_Login.Instance.AwakeLogin();
				_UI_MiniMap.Instance.isShowBigMap   = false;
				_UI_MiniMap.Instance.isShowSmallMap = false;	
				//todo: reset ally.
				PlayerInfoBar.Instance.ClearAllAllyState();
				Application.LoadLevelAsync("EmptyScenes");
		   	 	CS_SceneInfo.Instance.ClearALLObjects();
				break;
		}	
	}
	
	//if disconect , dismiss all panel;
	public void DismissAllPanel(){
		m_CS_Ingame_NormalPanel.Dismiss();
		_UI_CS_AbilitiesTrainer.Instance.AbilitiesTrainerPanel.Dismiss();
//		_UI_CS_MissionMaster.Instance.MissionBasePanel.Dismiss();
		BuyKarmaPanel.Instance.basePanel.Dismiss();
		BuyCrystalPanel.Instance.basePanel.Dismiss();
		_UI_CS_Consumable.Instance.ConsumablePanel.Dismiss();
		_UI_CS_CreateMenu.Instance.m_CS_CharacterCreatePanel.Dismiss();
		_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.Dismiss();
		_UI_CS_ItemVendor.Instance.ItemVendorPanel.Dismiss();
		_UI_CS_ItemVendorRare.Instance.ItemVendorRarePanel.Dismiss();
//		_UI_CS_LevelUp.Instance.LevelUpBgPanel.Dismiss();
		levelUp.Instance.basePanel.Dismiss();
		_UI_CS_LoadProgressCtrl.Instance.m_CS_Ingame_LoadMapPanel.Dismiss();
		MoneyBadgeInfo.Instance.InfoPanel.Dismiss();
//		_UI_CS_Revival.Instance.RevivalPanel.Dismiss();
		RevivePanel.Instance.basePanel.Dismiss();
		SelectChara.Instance.RootPanel.Dismiss();
		_UI_CS_SpiritTrainer.Instance.spiritTrainerPanel.Dismiss();
		_UI_CS_Summon.Instance.SummonPanel.Dismiss();
//		Tutorial.Instance.BGpanel.Dismiss();
	}
	
	void IngameNormalMenuDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				 MoneyBadgeInfo.Instance.Hide(false);
				_UI_CS_IngameMenu.Instance.SetIngameMenuState(5);
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
				_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.BringIn();
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_OPT);
			break;
		}	
	}

	void showBigMapDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
			if(_UI_MiniMap.Instance.isShowBigMap){
					
				_UI_MiniMap.Instance.isShowBigMap = false;
			
			}else{
			
				_UI_MiniMap.Instance.isShowBigMap = true;
			
			}
			
				break;
		   default:
				break;
		}	
	}
			
	void IngameNormalFMB0Delegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				MoneyBadgeInfo.Instance.Hide(false);
				_UI_CS_IngameMenu.Instance.SetIngameMenuState(2);
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
				_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.BringIn();		
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_ABI);
				break;
		}	
	}
	
	void IngameNormalFMB1Delegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				MoneyBadgeInfo.Instance.Hide(false);
				_UI_CS_IngameMenu.Instance.SetIngameMenuState(3);
				m_CS_Ingame_NormalPanel.Dismiss();
				_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.BringIn();
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INFO);
				break;
		}	
	}
	
	void IngameNormalFMB2Delegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				MoneyBadgeInfo.Instance.Hide(false);
				_UI_CS_IngameMenu.Instance.SetIngameMenuState(1);
				m_CS_Ingame_NormalPanel.Dismiss();
				_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.BringIn();
				BagPanel.ShowBag(Inventory.Instance.bagPosition);
				_UI_CS_IngameMenu.Instance.SetTransmuteState(false);
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INV);
				_UI_CS_IngameMenu.Instance.ShowPlayerModel(_UI_CS_IngameMenu.Instance.PlayerPos);
				break;
		   default:
				break;
		}	
	}
	
	void IngameNormalAbilities0Delegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
					_UI_CS_UseAbilities abilitiesObject2  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(0);
					if(null != abilitiesObject2)
						AbilitieTip.Instance.AbiTipShow(m_IngameNormal_AbilitiesFrameBtn[0].transform.position,abilitiesObject2.m_abilitiesInfo,AbiPosOffestType.LEFT_TOP);
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
					AbilitieTip.Instance.HideTip();
				break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
				_UI_CS_UseAbilities abilitiesObject  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(0);
				//LOGIC
				if(null != abilitiesObject)
				{
					if(abilitiesObject.m_isCoolDownFinish)
					{
                        PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                        _abiManager.UseAbility(abilitiesObject, false);
					}
				}
				break;
		   default:
				break;
		}	
	}
	
	void IngameNormalAbilities1Delegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		  case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
					_UI_CS_UseAbilities abilitiesObject2  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(1);
					if(null != abilitiesObject2)
						AbilitieTip.Instance.AbiTipShow(m_IngameNormal_AbilitiesFrameBtn[1].transform.position,abilitiesObject2.m_abilitiesInfo,AbiPosOffestType.LEFT_TOP);
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
					AbilitieTip.Instance.HideTip();
				break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
				_UI_CS_UseAbilities abilitiesObject  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(1);
				//LOGIC
				if(null != abilitiesObject)
				{
					if(abilitiesObject.m_isCoolDownFinish)
					{
                        PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                        _abiManager.UseAbility(abilitiesObject, false);
					}
				}
				break;
		   default:
				break;
		}	
	}
	
	void IngameNormalAbilities2Delegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
					_UI_CS_UseAbilities abilitiesObject2  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(2);
					if(null != abilitiesObject2)
						AbilitieTip.Instance.AbiTipShow(m_IngameNormal_AbilitiesFrameBtn[2].transform.position,abilitiesObject2.m_abilitiesInfo,AbiPosOffestType.LEFT_TOP);
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
					AbilitieTip.Instance.HideTip();
				break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
				_UI_CS_UseAbilities abilitiesObject  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(2);
				if(null != abilitiesObject){
					if(abilitiesObject.m_isCoolDownFinish){
                        PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                        _abiManager.UseAbility(abilitiesObject, false);
					}
				}
				break;
		   default:
				break;
		}	
	}
	
	void SummonBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				Stash.Instance.AwakeStash();
//				_UI_CS_Summon.Instance.AwakeSummon();
//				_UI_CS_MissionSummary.Instance.AwakeCompleteSummary();
//				_UI_CS_LevelUp.Instance.AwakeLevelUp();
//				_UI_CS_Wanted.Instance.AwakeRewardWanted(0,0,0,"xxxx");
				break;
		   default:
				break;
		}	
	}
	
	void PingIconDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
 			case POINTER_INFO.INPUT_EVENT.MOVE:	
		    case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
				{
					mousePos = UIManager.instance.uiCameras[0].camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,UIManager.instance.uiCameras[0].camera.nearClipPlane + 1));
					PingTips.Instance.ShowPingTip(mousePos,pingBG.width,pingBG.height);
				}
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		   case POINTER_INFO.INPUT_EVENT.RELEASE:
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				{
					PingTips.Instance.DismissTip();
				}
				break;
		}	
	}

	void IngameNormalSwitchDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
//				_UI_CS_AbilitiesCtrl.Instance.m_UseAbilitiesGroupIndex_Ingame++;
//				if(_UI_CS_AbilitiesCtrl.Instance.m_UseAbilitiesGroupIndex_Ingame > 2){
//						_UI_CS_AbilitiesCtrl.Instance.m_UseAbilitiesGroupIndex_Ingame = 0;
//				}
				_AbiMenuCtrl.Instance.SwitchGroup();
				_AbiMenuCtrl.Instance.UpDateIngameAbilitiesIcon();
			
				break;
		   default:
				break;
		}	
	}

	public void UpDateAbilNoEn(){
		_UI_CS_UseAbilities abilitiesObject0  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(0);	
		if(null != abilitiesObject0){
            if (abilitiesObject0.m_abilitiesInfo.m_EnergyCost < Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurMP]){
				NoEnEffect(0,false);	
			}else{		
				NoEnEffect(0,true);	
			}
		}else{
			NoEnEffect(0,false);	
		}
		_UI_CS_UseAbilities abilitiesObject1  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(1);
		if(null != abilitiesObject1){
            if (abilitiesObject1.m_abilitiesInfo.m_EnergyCost < Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurMP]){
				NoEnEffect(1,false);
			}else{	
				NoEnEffect(1,true);	
			}
		}else{
			NoEnEffect(1,false);	
		}
		_UI_CS_UseAbilities abilitiesObject2  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(2);
		if(null != abilitiesObject2){
            if (abilitiesObject2.m_abilitiesInfo.m_EnergyCost < Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurMP]){
				NoEnEffect(2,false);
			}else{
				NoEnEffect(2,true);	
			}
		}else{
			NoEnEffect(2,false);
		}
	}
	
	public void NoEnEffect(int idx,bool isShow){
		
		switch(idx){
			
		case 0:
			
			if(isShow){
				
				NoEnBtn[0].transform.position = new  Vector3(NoEnBtn[0].transform.position.x,NoEnBtn[0].transform.position.y,-1);
				
			}else{
				
				NoEnBtn[0].transform.position = new  Vector3(NoEnBtn[0].transform.position.x,NoEnBtn[0].transform.position.y,100);
				
			}
			
			break;
			
		case 1:
			
			if(isShow){
				
				NoEnBtn[1].transform.position = new  Vector3(NoEnBtn[1].transform.position.x,NoEnBtn[1].transform.position.y,-1);
				
			}else{
				
				NoEnBtn[1].transform.position = new  Vector3(NoEnBtn[1].transform.position.x,NoEnBtn[1].transform.position.y,100);
				
			}
			
			break;
			
		case 2:
			
			if(isShow){
				
				NoEnBtn[2].transform.position = new  Vector3(NoEnBtn[2].transform.position.x,NoEnBtn[2].transform.position.y,-1);
				
			}else{
				
				NoEnBtn[2].transform.position = new  Vector3(NoEnBtn[2].transform.position.x,NoEnBtn[2].transform.position.y,100);
				
			}
			
			break;
			
		default:
			break;
			
		}
		
	}
	
}

 