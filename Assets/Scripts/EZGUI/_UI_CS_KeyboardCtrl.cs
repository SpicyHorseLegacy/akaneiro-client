using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_KeyboardCtrl : MonoBehaviour {
	
	//Instance
	public static _UI_CS_KeyboardCtrl Instance = null;
	
	public float  PreTime;
	public float  offestTime = 0.2f;
	public float  offestFastTime = 0.05f;
	public Transform clickSound;
	
	public bool IsDoubleInput(){
		if(Time.realtimeSinceStartup <= PreTime + offestTime ){
//			PreTime = Time.realtimeSinceStartup;
			return true;
		}else{
			PreTime = Time.realtimeSinceStartup;
			return false;
		}
	}
	
	public bool IsFastDoubleInput(){
		if(Time.realtimeSinceStartup <= PreTime + offestFastTime ){
//			PreTime = Time.realtimeSinceStartup;
			return true;
		}else{
			PreTime = Time.realtimeSinceStartup;
			return false;
		}
	}
	
	#region ban key
	[SerializeField]
	private  List<KeyCode> banKeyList = new List<KeyCode>();
	public void AddKey(KeyCode kc) {
		if(banKeyList.Count > 0) {
			foreach(KeyCode key in banKeyList) {
				if(kc == key) {
					return;
				}
			}	
		}
		banKeyList.Add(kc);
	}
	public void DelKey(KeyCode kc) {
		banKeyList.Remove(kc);
	}
	public void ResetBanKeyList() {
		banKeyList.Clear();
	}
	public bool IsBan(KeyCode kc) {
		if(banKeyList.Count > 0) {
			foreach(KeyCode key in banKeyList) {
				if(kc == key) {
					return true;
				}
			}	
			return false;
		}else {
			return false;
		}
	}
	#endregion
	
	void Awake()
	{
		Instance = this;
	}
	
//	KeyCode
	
	// Use this for initialization
	void Start () {
		PreTime     = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
		//full to screen,hot key,only web ver
		if(WebLoginCtrl.Instance.IsWebLogin){
			if (Input.GetKeyDown (KeyCode.Escape)){
				if(!IsDoubleInput()){
					SoundCue.PlayPrefabAndDestroy(clickSound);
					if(OptionCtrl.Instance.isFullScreen){
						OptionCtrl.Instance.ToWindows();
					}
				}
			}
		}
		
		if(!WebLoginCtrl.Instance.IsWebLogin&&!ClientLogicCtrl.Instance.isClientVer){
			if (Input.GetKeyDown (KeyCode.Escape)){
				if(!IsDoubleInput()){
					SoundCue.PlayPrefabAndDestroy(clickSound);
					if(OptionCtrl.Instance.isFullScreen){
						OptionCtrl.Instance.ToWindows();
					}
				}
			}
		}

		///use return btn login.
		switch((int)_UI_CS_ScreenCtrl.Instance.currentScreenType){
			case (int)_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LOGIN	:
				if (Input.GetKey (KeyCode.Return)){
					if(!IsDoubleInput()){
						SoundCue.PlayPrefabAndDestroy(clickSound);
						if(ClientLogicCtrl.Instance.isClientVer){
							if(ClientLogicCtrl.Instance.GetEmailValid()){
								StartCoroutine(ClientLogicCtrl.Instance.SendMsgToServerForLogin());
							}else{
								LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"LOGININPUTERR");
								_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
							}
						}else{
							CS_Main.Instance.g_commModule.Connect(_UI_CS_Login.Instance.m_login_IPEditText.text,7001);
							LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"CON");
							_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
						}
					}
				}
			
				if (Input.GetKey(KeyCode.I)){
					if(Input.GetKeyDown(KeyCode.LeftControl)){
						if(!IsDoubleInput()){
							if(ClientLogicCtrl.Instance.isConnectTestServer) {
								ClientLogicCtrl.Instance.isConnectTestServer = false;
								_UI_CS_PopupBoxCtrl.PopUpError("Connect Normal Server");
							}else {
								ClientLogicCtrl.Instance.isConnectTestServer = true;;
								_UI_CS_PopupBoxCtrl.PopUpError("Connect Test Server");
							}
						}
					}
				}
			
				if (Input.GetKey (KeyCode.Tab)){
					if(!IsDoubleInput()){
						SoundCue.PlayPrefabAndDestroy(clickSound);
						_UI_CS_Ctrl.Instance.m_UI_Manager.FocusObject = _UI_CS_Login.Instance.m_login_PassWordEditText;
					}
				}

			
				break;
			case (int)_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_SELECT:
				if (Input.GetKey (KeyCode.Return)){
					if(!IsDoubleInput()){
						SoundCue.PlayPrefabAndDestroy(clickSound);
						LogManager.Log_Debug("--- SelectCharacter ---");
						CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolGame_SendRequest.UserSelectCharacter(SelectChara.Instance.GetCurrentSelectIdx())
						);
//						_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = "Loading...";
						LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"LOADING");
						_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
					}
				}
				break;
			case (int)_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL:
				if (Input.GetKeyDown (KeyCode.Escape)){
					if(!ChatBox.Instance.isInputState){
						SoundCue.PlayPrefabAndDestroy(clickSound);
						 MoneyBadgeInfo.Instance.Hide(false);
						_UI_CS_IngameMenu.Instance.SetIngameMenuState(5);
						_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
						_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.BringIn();
						_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_OPT);
                        Player.Instance.ReactivePlayer();
					}
				}

				if (Input.GetKeyDown (KeyCode.Return)){
					if(ChatBox.Instance.isInputState){
						SoundCue.PlayPrefabAndDestroy(clickSound);
						if(!ChatBox.Instance.isSend){
							ChatBox.Instance.SendMsg();
						}
					}
				}
			
				if (Input.GetKeyDown (KeyCode.Return)){
					if(!ChatBox.Instance.isInputState){
						ChatBox.Instance.PopUpCharBox();
					}
				}
			
				if (Input.GetKeyDown (KeyCode.F1)){
					if(!IsDoubleInput()){
						SoundCue.PlayPrefabAndDestroy(clickSound);
						if(_UI_CS_FightScreen.Instance.isIngameBring){
							_UI_CS_FightScreen.Instance.isIngameBring = false;
							_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
						}else{
							_UI_CS_FightScreen.Instance.isIngameBring = true;
							_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
						}
					}
				}

                if (Input.GetKeyDown(KeyCode.F5)) {
                    if (!IsDoubleInput()){
						SoundCue.PlayPrefabAndDestroy(clickSound);
                        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl)) {
                            _UI_CS_DebugInfo.Instance.NotifyDebugInfo();
                            BattleInfoPanel.Instance.NotifyDebugInfo();
                        }
                    }

                }
				
	
				if (Input.GetKeyDown(KeyCode.Tab)){
					if(!IsFastDoubleInput()){
						SoundCue.PlayPrefabAndDestroy(_AbiMenuCtrl.Instance.GroupSound);
						_AbiMenuCtrl.Instance.SwitchGroup();
						_AbiMenuCtrl.Instance.UpDateIngameAbilitiesIcon();			
					}
				}

				if (Input.GetKeyDown (KeyCode.M)){
					if(!ChatBox.Instance.isInputState){
						SoundCue.PlayPrefabAndDestroy(clickSound);
						if(_UI_MiniMap.Instance.isShowBigMap){
							_UI_MiniMap.Instance.isShowBigMap = false;
						
						}else{
							_UI_MiniMap.Instance.isShowBigMap = true;
						}
						_UI_MiniMap.Instance.MapPicCopy();
					}
				}
			
				if (Input.GetKeyDown (KeyCode.A) && !GameCamera.Instance.IsFreeCameraMode){
					if(!IsBan(KeyCode.A)) {	
						if(!ChatBox.Instance.isInputState){
							if(!IsDoubleInput()){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								_UI_CS_IngameMenu.Instance.SetIngameMenuState(2);
								_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
								_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.BringIn();
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_ABI);						
							}
						}
					}
				}
			
				if (Input.GetKeyDown (KeyCode.C)){
					if(!IsBan(KeyCode.C)) {	
						if(!ChatBox.Instance.isInputState){
							if(!IsDoubleInput()){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								_UI_CS_IngameMenu.Instance.SetIngameMenuState(3);
								_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
								_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.BringIn();
								_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INFO);		
							}
						}
					}
				}
			
				if (Input.GetKeyDown (KeyCode.I)){
					if(!IsBan(KeyCode.I)) {	
						if(!ChatBox.Instance.isInputState){
							if(!IsDoubleInput()){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.SetIngameMenuState(1);
								_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
								_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.BringIn();
								_UI_CS_FightScreen.Instance.BagPanel.ShowBag(Inventory.Instance.bagPosition);
								_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INV);
								_UI_CS_IngameMenu.Instance.ShowPlayerModel(_UI_CS_IngameMenu.Instance.PlayerPos);
								_UI_CS_IngameMenu.Instance.SetTransmuteState(false);
	                            Player.Instance.FreezePlayer();
							}
						}
					}
				}
			
//				if (Input.GetKey(KeyCode.Q)){
//					if(Input.GetKeyDown(KeyCode.LeftAlt)){
//						if(Input.GetKeyDown(KeyCode.LeftControl)){
//							if(!IsDoubleInput()){
//								if(ChatBox.Instance.isInputState){
//									EChatType chatType = new  EChatType(1);
//									_UI_CS_ChatBox.Instance.perString = _UI_CS_ChatBox.Instance.m_Input.text;
//									CS_Main.Instance.g_commModule.SendMessage(
//										   	ProtocolGame_SendRequest.ChatRequest(chatType,0,0,_UI_CS_ChatBox.Instance.m_Input.text)
//									);
//									_UI_CS_ChatBox.Instance.m_Input.Text = "";
//								}else{
//									ChatBox.Instance.isInputState = true;
//									_UI_CS_ChatBox.Instance.m_ChatBoxPanel.BringIn();
//								}
//							}
//						}
//					}
//				}
			
				if (Input.GetKey(KeyCode.F)){
					if(!IsDoubleInput()){
						if(!ChatBox.Instance.isInputState){
							SoundCue.PlayPrefabAndDestroy(clickSound);
							if(CS_SceneInfo.Instance.AllyNpcList.Count < 0){
								_UI_CS_Summon.Instance.AwakeSummon();
							}
						}
					}
				}
			
				if (Input.GetKeyDown (KeyCode.UpArrow)){
					if(!IsDoubleInput()){
						if(ChatBox.Instance.isInputState){
							SoundCue.PlayPrefabAndDestroy(clickSound);
							ChatBox.Instance.currentSveIdx--;
							if(ChatBox.Instance.saveMsgList.Count > 0){
								if(ChatBox.Instance.currentSveIdx<0){
									ChatBox.Instance.currentSveIdx = 0;
								}
								ChatBox.Instance.inputEdit.Text = ChatBox.Instance.saveMsgList[ChatBox.Instance.currentSveIdx].msg;
							}
						}
					}
				}
			
				if (Input.GetKeyDown (KeyCode.DownArrow)){
					if(!IsDoubleInput()){
						if(ChatBox.Instance.isInputState){
							SoundCue.PlayPrefabAndDestroy(clickSound);
							ChatBox.Instance.currentSveIdx++;
							if(ChatBox.Instance.saveMsgList.Count > 0){
								if(ChatBox.Instance.currentSveIdx >= ChatBox.Instance.saveMsgList.Count){
									ChatBox.Instance.currentSveIdx = ChatBox.Instance.saveMsgList.Count-1;
								}
								ChatBox.Instance.inputEdit.Text = ChatBox.Instance.saveMsgList[ChatBox.Instance.currentSveIdx].msg;
							}
						}
					}
				}

				if (Input.GetKeyDown (KeyCode.Alpha1)){
					if(!ChatBox.Instance.isInputState){
						if(!IsDoubleInput()){
							//SoundCue.PlayPrefabAndDestroy(clickSound);
							_UI_CS_UseAbilities abilitiesObject  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(0);
							if(null != abilitiesObject){
								if(abilitiesObject.m_isCoolDownFinish){
                                    PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                                    _abiManager.UseAbility(abilitiesObject, true);
								}
							}
						}
					}
				}
			
				if(Input.GetKeyUp(KeyCode.Alpha1)){
					if(!ChatBox.Instance.isInputState){
						//SoundCue.PlayPrefabAndDestroy(clickSound);
						_UI_CS_UseAbilities abilitiesObject  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(0);
						if(null != abilitiesObject){
                            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
							_abiManager.KeyboardKeyUp(abilitiesObject);
						}
					}
				}
				
				if (Input.GetKeyDown (KeyCode.Alpha2)){
					if(!ChatBox.Instance.isInputState){
						//SoundCue.PlayPrefabAndDestroy(clickSound);
						_UI_CS_UseAbilities abilitiesObject  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(1);
						if(null != abilitiesObject){
							if(abilitiesObject.m_isCoolDownFinish){
                                PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                                _abiManager.UseAbility(abilitiesObject, true);
							}
						}
					}
				}
			
				if(Input.GetKeyUp(KeyCode.Alpha2))
				{
					if(!ChatBox.Instance.isInputState){
						//SoundCue.PlayPrefabAndDestroy(clickSound);
						_UI_CS_UseAbilities abilitiesObject  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(1);
						if(null != abilitiesObject){
                            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                            _abiManager.KeyboardKeyUp(abilitiesObject);
						}
					}
				}
			
				if (Input.GetKeyDown(KeyCode.Alpha3)){
					if(!ChatBox.Instance.isInputState){
						//SoundCue.PlayPrefabAndDestroy(clickSound);
						_UI_CS_UseAbilities abilitiesObject  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(2);
						if(null != abilitiesObject){
							if(abilitiesObject.m_isCoolDownFinish){
                                PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                                _abiManager.UseAbility(abilitiesObject, true);
							}
						}
					}
				}
			
				if(Input.GetKeyUp(KeyCode.Alpha3)){
					if(!ChatBox.Instance.isInputState){
						//SoundCue.PlayPrefabAndDestroy(clickSound);
						_UI_CS_UseAbilities abilitiesObject  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(2);
						if(null != abilitiesObject){
                            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                            _abiManager.KeyboardKeyUp(abilitiesObject);
						}
					}
				}
			
				break;
			
			case (int)_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INV:
					if (Input.GetKeyDown (KeyCode.I)){
						if(!ChatBox.Instance.isInputState){
							if(!IsDoubleInput()){
								_UI_CS_IngameMenu.Instance.ResetItemPos();
								_UI_CS_IngameMenu.Instance.BagEquipShadow(0,false);
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.BackToIngame();
							}
						}
					}
			
					if (Input.GetKeyDown (KeyCode.Escape)){
						if(!IsDoubleInput()){
							SoundCue.PlayPrefabAndDestroy(clickSound);
							MoneyBadgeInfo.Instance.Hide(false);
							_UI_CS_IngameMenu.Instance.BackToIngame();	
							_AbiMenuCtrl.Instance.LeaveAbiSetting();
						}
					}
					
					if (Input.GetKeyDown (KeyCode.A)){
						if(!IsBan(KeyCode.A)) {	
							if(!IsDoubleInput()){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.SetIngameMenuState(2);
								Inventory.Instance.BagPanel.HideBag();
								Inventory.Instance.bagItemArray[0].CancelPress();
								SurveillanceCamera.Instance.ShutDown();
								if(null != ScenesLightCtrl.Instance)
									ScenesLightCtrl.Instance.OpenLight();
								_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
								_AbiMenuCtrl.Instance.LeaveAbiSetting();
								_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_ABI);
							}
						}
					}
			
					if (Input.GetKeyDown (KeyCode.C)){
						if(!IsBan(KeyCode.C)) {	
							if(!IsDoubleInput()){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.SetIngameMenuState(3);
								Inventory.Instance.BagPanel.HideBag();
								Inventory.Instance.bagItemArray[0].CancelPress();	
								SurveillanceCamera.Instance.ShutDown();
								if(null != ScenesLightCtrl.Instance)
									ScenesLightCtrl.Instance.OpenLight();
								_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
								_AbiMenuCtrl.Instance.LeaveAbiSetting();
								_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INFO);
							}
						}
					}
			
					if (Input.GetKeyDown (KeyCode.Escape)){
						if(!IsDoubleInput()){
							SoundCue.PlayPrefabAndDestroy(clickSound);
							MoneyBadgeInfo.Instance.Hide(false);
							_UI_CS_IngameMenu.Instance.SetIngameMenuState(5);
							Inventory.Instance.BagPanel.HideBag();
							Inventory.Instance.bagItemArray[0].CancelPress();
							SurveillanceCamera.Instance.ShutDown();
							if(null != ScenesLightCtrl.Instance)
								ScenesLightCtrl.Instance.OpenLight();
							_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
							_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_OPT);	
						}
					}
			
				break;
			case (int)_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_ABI:
			
					if (Input.GetKeyDown (KeyCode.A)){
						if(!ChatBox.Instance.isInputState){
							if(!IsDoubleInput()){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.BackToIngame();	
								
							}
						}
					}
			
					if (Input.GetKeyDown (KeyCode.Escape)){
						if(!IsDoubleInput()){
							SoundCue.PlayPrefabAndDestroy(clickSound);
							MoneyBadgeInfo.Instance.Hide(false);
							_UI_CS_IngameMenu.Instance.BackToIngame();	
							_AbiMenuCtrl.Instance.LeaveAbiSetting();
						}
					}
			
					if (Input.GetKeyDown (KeyCode.Tab)){
						if(!IsDoubleInput()){	
						}
					}
			
					if (Input.GetKeyDown (KeyCode.I)){
						if(!IsBan(KeyCode.I)) {	
							if(!IsDoubleInput()){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.SetIngameMenuState(1);
								Inventory.Instance.BagPanel.ShowBag(Inventory.Instance.bagPosition);
								Inventory.Instance.bagItemArray[0].CancelPress();
								_UI_CS_IngameMenu.Instance.ShowPlayerModel(_UI_CS_IngameMenu.Instance.PlayerPos);
								_UI_CS_IngameMenu.Instance.SetTransmuteState(false);
								_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INV);
							}
						}
					}
			
					if (Input.GetKeyDown (KeyCode.C)){
						if(!IsBan(KeyCode.C)) {	
							if(!IsDoubleInput()){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.SetIngameMenuState(3);
								Inventory.Instance.BagPanel.HideBag();
								Inventory.Instance.bagItemArray[0].CancelPress();	
								SurveillanceCamera.Instance.ShutDown();
								if(null != ScenesLightCtrl.Instance)
									ScenesLightCtrl.Instance.OpenLight();
								_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
								_AbiMenuCtrl.Instance.LeaveAbiSetting();
								_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INFO);
							}
						}
					}
			
					if (Input.GetKeyDown (KeyCode.Escape)){
						if(!IsDoubleInput()){
							SoundCue.PlayPrefabAndDestroy(clickSound);
							MoneyBadgeInfo.Instance.Hide(false);
							_UI_CS_IngameMenu.Instance.SetIngameMenuState(5);
							Inventory.Instance.BagPanel.HideBag();
							Inventory.Instance.bagItemArray[0].CancelPress();
							SurveillanceCamera.Instance.ShutDown();
							if(null != ScenesLightCtrl.Instance)
								ScenesLightCtrl.Instance.OpenLight();
							_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
							_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_OPT);	
						}
					}
			
			
				break;
			
			case (int)_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INFO:
					if (Input.GetKeyDown (KeyCode.C)){
						if(!IsDoubleInput()){
							SoundCue.PlayPrefabAndDestroy(clickSound);
							MoneyBadgeInfo.Instance.Hide(false);
							_UI_CS_IngameMenu.Instance.BackToIngame();	
						}
					}
			
					if (Input.GetKeyDown (KeyCode.Escape)){
						if(!IsDoubleInput()){
							SoundCue.PlayPrefabAndDestroy(clickSound);
							MoneyBadgeInfo.Instance.Hide(false);
							_UI_CS_IngameMenu.Instance.BackToIngame();	
						}
					}
			
					if (Input.GetKeyDown (KeyCode.I)){
						if(!IsBan(KeyCode.I)) {	
							if(!IsDoubleInput()){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.SetIngameMenuState(1);
								Inventory.Instance.BagPanel.ShowBag(Inventory.Instance.bagPosition);
								Inventory.Instance.bagItemArray[0].CancelPress();
								_UI_CS_IngameMenu.Instance.ShowPlayerModel(_UI_CS_IngameMenu.Instance.PlayerPos);
								_UI_CS_IngameMenu.Instance.SetTransmuteState(false);
								_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INV);
							}
						}
					}
			
					if (Input.GetKeyDown (KeyCode.A)){
						if(!IsBan(KeyCode.A)) {	
							if(!IsDoubleInput()){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.SetIngameMenuState(2);
								Inventory.Instance.BagPanel.HideBag();
								Inventory.Instance.bagItemArray[0].CancelPress();
								SurveillanceCamera.Instance.ShutDown();
								if(null != ScenesLightCtrl.Instance)
									ScenesLightCtrl.Instance.OpenLight();
								_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
								_AbiMenuCtrl.Instance.LeaveAbiSetting();
								_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_ABI);
							}
						}
					}
			
					if (Input.GetKeyDown (KeyCode.Escape)){
						if(!IsDoubleInput()){
							SoundCue.PlayPrefabAndDestroy(clickSound);
							MoneyBadgeInfo.Instance.Hide(false);
							_UI_CS_IngameMenu.Instance.SetIngameMenuState(5);
							Inventory.Instance.BagPanel.HideBag();
							Inventory.Instance.bagItemArray[0].CancelPress();
							SurveillanceCamera.Instance.ShutDown();
							if(null != ScenesLightCtrl.Instance)
								ScenesLightCtrl.Instance.OpenLight();
							_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
							_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_OPT);	
						}
					}
			
				break;
				case (int)_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_OPT:
					if (Input.GetKeyDown (KeyCode.Escape)){
						if(!IsDoubleInput()){
							SoundCue.PlayPrefabAndDestroy(clickSound);
							MoneyBadgeInfo.Instance.Hide(false);
							_UI_CS_IngameMenu.Instance.BackToIngame();	
						}
					}
			
					if (Input.GetKeyDown (KeyCode.I)){
						if(!IsDoubleInput()){
							if(!OptionCtrl.Instance.isInputGiftCode){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.SetIngameMenuState(1);
								Inventory.Instance.BagPanel.ShowBag(Inventory.Instance.bagPosition);
								Inventory.Instance.bagItemArray[0].CancelPress();
								_UI_CS_IngameMenu.Instance.ShowPlayerModel(_UI_CS_IngameMenu.Instance.PlayerPos);
								_UI_CS_IngameMenu.Instance.SetTransmuteState(false);
								_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INV);
							}
						}
					}
			
					if (Input.GetKeyDown (KeyCode.A)){
						if(!IsDoubleInput()){
							if(!OptionCtrl.Instance.isInputGiftCode){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.SetIngameMenuState(2);
								Inventory.Instance.BagPanel.HideBag();
								Inventory.Instance.bagItemArray[0].CancelPress();
								SurveillanceCamera.Instance.ShutDown();
								if(null != ScenesLightCtrl.Instance)
									ScenesLightCtrl.Instance.OpenLight();
								_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
								_AbiMenuCtrl.Instance.LeaveAbiSetting();
								_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_ABI);
							}	
						}
					}
			
					if (Input.GetKeyDown (KeyCode.C)){
						if(!IsDoubleInput()){
							if(!OptionCtrl.Instance.isInputGiftCode){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.SetIngameMenuState(3);
								Inventory.Instance.BagPanel.HideBag();
								Inventory.Instance.bagItemArray[0].CancelPress();	
								SurveillanceCamera.Instance.ShutDown();
								if(null != ScenesLightCtrl.Instance)
									ScenesLightCtrl.Instance.OpenLight();
								_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
								_AbiMenuCtrl.Instance.LeaveAbiSetting();
								_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INFO);
							}
						}
					}
			
					if (Input.GetKeyDown (KeyCode.Escape)){
						if(!IsDoubleInput()){
							if(!OptionCtrl.Instance.isInputGiftCode){
								SoundCue.PlayPrefabAndDestroy(clickSound);
								MoneyBadgeInfo.Instance.Hide(false);
								_UI_CS_IngameMenu.Instance.BackToIngame();	
							}
						}
					}
				break;
		}
	}
}







