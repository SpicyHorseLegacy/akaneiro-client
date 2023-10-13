using UnityEngine;
using System.Collections;

public class OptionCtrl : MonoBehaviour {

	public static OptionCtrl Instance;
	
	public UIStateToggleBtn autoAttack;
	public UIStateToggleBtn fadeLootTips;
	public UIStateToggleBtn fullScreen;
	public UIStateToggleBtn shadows;
	public UIButton 		qualityList;
	public UIButton 		qualityLow;
	public UIButton 		qualityNormal;
	public UIButton 		qualityHigh;
	public UIButton 		qualityBest;  
	public Transform		qualityListRoot;
	public Transform		qualityListPos;
	private bool			isShow = false;
	private bool			isAutoAttack = false;
	private bool			isFadeLootTips = true;
	public  bool			isFullScreen   = false;
	public UIButton 		BackToSelectBtn;
	public UIButton 		BackToVillageBtn;
	public UIButton 		ExitBtn;
	public UIPanel			exitPanel;
	public UIButton			exitYes;
	public UIButton			exitNo;
	public UIButton 		BugReportBtn;
	public UISlider			soundFx;
//	public bool				isMuteSoundFX = false;
	public UIStateToggleBtn muteSoundFx;
	public float			saveSoundFxValue;
	public UISlider			music;
//	public bool				isMuteMusic = false;
	public UIStateToggleBtn muteMusic;
	public float			saveMusicValue;
	public bool				isChangeBtnState = false;
	public UIPanelTab	 	giftCodeTab;
	public UITextField	 	giftCodeInput;
	public UIButton	 		giftCodeSend;
	public SpriteText 		giftMsg;
	public bool				isInputGiftCode;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		qualityList.AddInputDelegate(QualityListDelegate);
		qualityLow.AddInputDelegate(QualityLowDelegate);
		qualityNormal.AddInputDelegate(QualityNormalDelegate);
		qualityHigh.AddInputDelegate(QualityHighDelegate);
		qualityBest.AddInputDelegate(QualityBestDelegate);
		qualityListRoot.position = new Vector3 (999,999,999);
		autoAttack.AddInputDelegate(AutoAttackDelegate);
		fadeLootTips.AddInputDelegate(FadeLootTipsDelegate);
		isAutoAttack = true;
		SetAutoAttack(true);
		isFadeLootTips = false;
		fullScreen.AddInputDelegate(FullScreen1Delegate);
		//fullScreen.AddValueChangedDelegate(FullScreen2Delegate);
		BackToSelectBtn.AddInputDelegate(BackToSelectBtnDelegate);
		BackToVillageBtn.AddInputDelegate(BackToVillageBtnDelegate);
		ExitBtn.AddInputDelegate(ExitBtnDelegate);
		exitYes.AddInputDelegate(exitYesDelegate);
		exitNo.AddInputDelegate(exitNoDelegate);
		BugReportBtn.AddInputDelegate(BugReportBtnDelegate);
		//hide quit button ,if steam no need hide.
		if(!ClientLogicCtrl.Instance.isClientVer){
			if(Steamworks.activeInstance!= null) {
				if(!Steamworks.activeInstance.isSteamWork) {
					ExitBtn.transform.position = new Vector3(999,999,999);
				}
			}
		}
//		isMuteSoundFX = false;
//		isMuteMusic   = false;
		music.AddValueChangedDelegate(MusicDelegate);
		muteMusic.AddInputDelegate(MuteMusicDelegate);
		soundFx.AddValueChangedDelegate(SoundFxDelegate);
		muteSoundFx.AddInputDelegate(MuteSoundFxDelegate);
		StartCoroutine(InitOptionInfo());
		giftCodeTab.AddInputDelegate(GiftCodeTabDelegate);
		giftCodeSend.AddInputDelegate(GiftCodeSendDelegate);
		isInputGiftCode = false;
		giftCodeInput.SetCommitDelegate(MyCommitDelegate);
		giftCodeInput.AddInputDelegate(InputStateDelegate);
		
	}
	
	// Update is called once per frame
	void Update () {
//		if(isChangeBtnState){
//			isChangeBtnState= false;
//		
//		}
	}
	
#region Local
	private bool GetIsAutoAtk() {
		return GameConfig.IsAutoAttack;
	}
	
	private void InitAutoAck() {
		if(GetIsAutoAtk()) {
			isAutoAttack = true;
			autoAttack.SetToggleState(0);
			SetAutoAttack(true);
//			LogManager.Log_Error("SetAutoAttack: true");
		}else {
			isAutoAttack = false;
			autoAttack.SetToggleState(1);
			SetAutoAttack(false);
//			LogManager.Log_Error("SetAutoAttack: false");
		}
	}
	
	private IEnumerator InitOptionInfo() {
		yield return new WaitForSeconds(0.5f);
		InitSoundInfo();
		InitAutoAck();
		if(!Screen.fullScreen) {
			//todo: because statr will delegate FullScreenDelegate;So...
			IsFullScreen(true);
		}else {
			IsFullScreen(false);
		}
	}
	
	void GiftCodeTabDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
//				giftMsg.Text = "Please input your key for redemption.";
				LocalizeManage.Instance.GetDynamicText(giftMsg,"PLEINPUTREDEEM");
				isInputGiftCode = true;
				break;
		}	
	}
	
	void GiftCodeSendDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
			#region new solution
			//	So the fix for handling codes is do this:
			//	Send code to Alex's system
			//	If redemption is successful:
			//	Tell user it worked
			//	Exit
			//	If redemption is not successful in Alex's system:
			//	Send code to WLZ's system
			//	If redemption is successful:
			//	Tell user it worked
			//	Exit
			//	If redemption is not successful:
			//	Tell user the code is invalid
			//	Exit
			//  ~(*-*)~
			TakeGiftPanel.Instance.backUpRedeemCode = giftCodeInput.text;
			StartCoroutine(TakeGiftPanel.Instance.SendMsgToServerForRedeemCode(giftCodeInput.text));
			#endregion
			#region no use
//			if(giftCodeInput.text.Length < 32) {
//				#region wlz redeemer
//				CS_Main.Instance.g_commModule.SendMessage(
//				   		ProtocolGame_SendRequest.ProcessRedeemGift(giftCodeInput.text)
//				);
//				//this is old cod. wlz say save it.
//				CS_Main.Instance.g_commModule.SendMessage(
//				   		ProtocolGame_SendRequest.GetGiftReq(giftCodeInput.text)
//				);
//				#endregion
//			}else {
//				#region alex redeemer
//				StartCoroutine(TakeGiftPanel.Instance.SendMsgToServerForRedeemCode(giftCodeInput.text));
//				#endregion
//			}
			#endregion
			giftCodeInput.Text = "";
				break;
		}	
	}
	
	void InitSoundInfo(){
		music.Value = GameConfig.MusicVolumn;
		soundFx.Value = GameConfig.SFXVolumn;
		if(GameConfig.IsMusic) {
			muteMusic.SetToggleState(1);
//			muteSoundFx.SetState(0);
		}else {
			muteMusic.SetToggleState(0);
//			muteSoundFx.SetState(1);
		}
		if(GameConfig.IsSFX) {
			muteSoundFx.SetToggleState(1);
//			muteMusic.SetState(0);
		}else {
			muteSoundFx.SetToggleState(0);
//			muteMusic.SetState(1);
		}
	}
	
	void MusicDelegate(IUIObject obj){
        if(obj != null){
//			if(!isMuteMusic){
				GameConfig.MusicVolumn 	= music.Value;
//			}
		}
    }
	
	void MuteMusicDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				SetMuteMuisc(GameConfig.IsMusic);
//				if(!GameConfig.IsMusic){
////					isMuteMusic = false;
//					SetMuteMuisc(false);
//				}else{
////					isMuteMusic = true;
//					SetMuteMuisc(true);
//				}
				break;
		}	
	}
	
	void FullScreen2Delegate(IUIObject obj){
        if(obj != null){
//			IsFullScreen(isFullScreen);
		}
    }
	
	void FullScreen1Delegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				IsFullScreen(isFullScreen);
				break;
		}	
	}
	
	void SoundFxDelegate(IUIObject obj){
        if(obj != null){
//			if(!isMuteSoundFX){
				GameConfig.SFXVolumn = soundFx.Value;
//			}
		}
    }
	
	void MuteSoundFxDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				SetMuteSoundFX(GameConfig.IsSFX);
			
//				if(!GameConfig.IsSFX){
////					isMuteSoundFX = false;
//					SetMuteSoundFX(false);
//				}else{
////					isMuteSoundFX = true;
//					SetMuteSoundFX(true);
//				}
				break;
		}	
	}
	
	// 1 low 2 normal 3 high 4 best
	void QualityLogic(int qlt){
		switch(qlt){
		case 0:
                UnityEngine.QualitySettings.currentLevel = QualityLevel.Fastest;
			break;
		case 1:
                UnityEngine.QualitySettings.currentLevel = QualityLevel.Simple;
			break;
		case 2:
			    UnityEngine.QualitySettings.currentLevel = QualityLevel.Beautiful;
			break;
		case 3:
                UnityEngine.QualitySettings.currentLevel = QualityLevel.Fantastic;
			break;
		}
	}
	
	void QualityLowDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				IsHideQualityList(true);
//				qualityList.spriteText.Text = "Low";
				LocalizeManage.Instance.GetDynamicText(qualityList.spriteText,"LOW");
				QualityLogic(0);
				break;
		   default:
				break;
		}	
	}
	
	void QualityNormalDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				IsHideQualityList(true);
//				qualityList.spriteText.Text = "Normal";
				LocalizeManage.Instance.GetDynamicText(qualityList.spriteText,"NORMAL");
				QualityLogic(1);
				break;
		   default:
				break;
		}	
	}
	
	void QualityHighDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				IsHideQualityList(true);
//				qualityList.spriteText.Text = "High";
				LocalizeManage.Instance.GetDynamicText(qualityList.spriteText,"HIGH");
				QualityLogic(2);
				break;
		   default:
				break;
		}	
	}
	
	void QualityBestDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				IsHideQualityList(true);
				qualityList.spriteText.Text = "Best";
				LocalizeManage.Instance.GetDynamicText(qualityList.spriteText,"BEST");
				QualityLogic(3);
				break;
		   default:
				break;
		}	
	}

	void BackToSelectBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				// is player is dead, reset state back to idle
//                _UI_CS_Revival.Instance.RevivalPanel.Dismiss();
				RevivePanel.Instance.basePanel.Dismiss();
                Player.Instance.FSM.ChangeState(Player.Instance.IS);
				_UI_CS_FightScreen.Instance.m_isLogout = true;
				MoneyBadgeInfo.Instance.Hide(false);
				_UI_CS_IngameMenu.Instance.BackToIngame();	
				//to do: Hide teleport btn
				_UI_CS_Teleport.Instance.HideTelport();
				//todo:	Hide pet
                Player.Instance.BuffMan.RemoveAllBuffs();
				Player.Instance.CallOffSpirit((SpiritBase.eSpiriteType)_UI_CS_SpiritTrainer_Cost.Instance.PetID);
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
				_UI_MiniMap.Instance.isShowBigMap   = false;
				_UI_MiniMap.Instance.isShowSmallMap = false;	
				SelectChara.Instance.SavePlayerInfoTOList();
				SelectChara.Instance.SetActiveCharaBtn(SelectChara.Instance.GetCurrentSelectIdx());
				_UI_CS_FightScreen.Instance.ClearBag();
				_UI_CS_ItemVendor.Instance.m_isReceiveItemList = false;
				_UI_CS_LoadProgressCtrl.Instance.AwakeLoading(0);
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LOADING);
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.LeaveWorld());	
				CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.EnterScene("EmptyScenes"));	
				SelectChara.Instance.AwakeSelectChara();
				break;
		   default:
				break;
		}	
	}
		
	void BackToVillageBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
                // is player is dead, reset state back to idle
//                _UI_CS_Revival.Instance.RevivalPanel.Dismiss();
				RevivePanel.Instance.basePanel.Dismiss();
                Player.Instance.FSM.ChangeState(Player.Instance.IS);
				_UI_CS_MissionLogic.Instance.MissionBgPanel.Dismiss();
//				_UI_CS_MissionLogic.Instance.RsetMissionScore();
				//to do: Hide teleport btn
				_UI_CS_Teleport.Instance.HideTelport();
				_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.Dismiss();
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
				_UI_CS_FightScreen.Instance.m_fightPanel.Dismiss();
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LOADING);
				_UI_CS_LoadProgressCtrl.Instance.m_LoadingMapName.Text = _UI_CS_MapScroll.Instance.SceneNameToMapName("Hub_Village");
				_UI_CS_LoadProgressCtrl.Instance.AwakeLoading(0);
				_UI_MiniMap.Instance.isShowBigMap = false;
				_UI_MiniMap.Instance.isShowSmallMap = false;
//				Tutorial.Instance.isTutorial = false;
				TutorialMan.Instance.SetTutorialFlag(false);
				CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.EnterScene("Hub_Village"));
                Player.Instance.ReactivePlayer();
				break;
		}	
	}

	void BugReportBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(ClientLogicCtrl.Instance.isClientVer){
					UrlOpener.Open("https://spicyhorsegames.zendesk.com");
				}else{
					Application.ExternalEval("window.open('https://spicyhorsegames.zendesk.com','_blank')");
				}
				break;
		   default:
				break;
		}	
	}
		
	void ExitBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				exitPanel.BringIn();
//				Application.Quit();
				break;
		   default:
				break;
		}	
	}
	
	void exitYesDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				exitPanel.Dismiss();
				Application.Quit();
				break;
		   default:
				break;
		}	
	}
	
	void exitNoDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				exitPanel.Dismiss();
				break;
		   default:
				break;
		}	
	}
	
	void QualityListDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				IsHideQualityList(false);
				qualityList.spriteText.Text = "";
				break;
		   default:
				break;
		}	
	}
	
	void AutoAttackDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(isAutoAttack){
					isAutoAttack = false;
					SetAutoAttack(false);
				}else{
					isAutoAttack = true;
					SetAutoAttack(true);
				}
				break;
		   default:
				break;
		}	
	}
	
	void FadeLootTipsDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(isFadeLootTips){
					isFadeLootTips = false;
					SetFadeLootTips(false);
				}else{
					isFadeLootTips = true;
					SetFadeLootTips(true);
				}
				break;
		   default:
				break;
		}	
	}
	
	void InputStateDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				isInputGiftCode  = true;
				ClipBoardHelp.Instance.SetCurrentObj(giftCodeInput.transform);
//				ClipBoardHelp.Instance.currentInput = giftCodeInput;
				break;
		}	
	}
#endregion

#region Interface 
	public void SetMuteMuisc(bool isPlay){
		GameConfig.IsMusic = !isPlay;
//		if(!isPlay){
//			GameConfig.MusicVolumn = music.Value;
//		}else{
//			saveMusicValue = GameConfig.MusicVolumn;
//			GameConfig.MusicVolumn = 0;
//		}
	}
	
	public void SetMuteSoundFX(bool isPlay){
		GameConfig.IsSFX = !isPlay;
//		if(isPlay){
//            GameConfig.SFXVolumn = saveMusicValue;
//		}else{
//			saveMusicValue = GameConfig.SFXVolumn;
//			GameConfig.SFXVolumn = 0;
//		}
	}
	
	public void SetAutoAttack(bool isAuto){
		GameConfig.IsAutoAttack = isAuto;
	}
	
	public void SetFadeLootTips(bool isFade){
		if(isFade){
			_UI_CS_IngameToolTipMan.Instance.Hide();
		}else{
			_UI_CS_IngameToolTipMan.Instance.Show();
		}
	}

	public void IsHideQualityList(bool isHide){
		if(isHide){
			qualityListRoot.position = new Vector3 (999,999,999);
		}else{
			qualityListRoot.position = qualityListPos.position;
		}
	}			
	
	public void ToWindows(){
		Screen.SetResolution(1280, 720, false);
		_UI_CS_Ctrl.Instance.m_UI_Camera.AutoResize(1280, 720);
		//because delegate will next call.So...
		isFullScreen = false;
		fullScreen.SetToggleState(0);
		fullScreen.SetState(1);
		SelectChara.Instance.fullScreen.SetToggleState(1);
		SelectChara.Instance.fullScreen.SetState(0);
		isChangeBtnState = true;
	}
	
	public void IsFullScreen(bool bIsFullScreen){
		isChangeBtnState = true;
		if(bIsFullScreen){
			 Screen.SetResolution(1280, 720, false);
			_UI_CS_Ctrl.Instance.m_UI_Camera.AutoResize(1280, 720);
			isFullScreen = false;
			fullScreen.SetToggleState(0);
			fullScreen.SetState(1);
			SelectChara.Instance.fullScreen.SetToggleState(1);
			SelectChara.Instance.fullScreen.SetState(0);
			
		}else{
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
			_UI_CS_Ctrl.Instance.m_UI_Camera.AutoResize(Screen.currentResolution.width, Screen.currentResolution.height);	
			isFullScreen = true;
			fullScreen.SetToggleState(1);
			fullScreen.SetState(0);
			SelectChara.Instance.fullScreen.SetToggleState(0);
			SelectChara.Instance.fullScreen.SetState(1);
		}
	}
	
	public void MyCommitDelegate(IKeyFocusable control){
		isInputGiftCode = false;
		ClipBoardHelp.Instance.SetCurrentObj(null);
//		ClipBoardHelp.Instance.currentInput = null;
	}
#endregion
}
