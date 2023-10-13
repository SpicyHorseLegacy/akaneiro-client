 using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ManagedSteam;
using ManagedSteam.Exceptions;
using ManagedSteam.CallbackStructures;
using ManagedSteam.SteamTypes;
using ManagedSteam.Utility;
using System.Runtime.InteropServices;

namespace ClientCSharp.Logic
{
    class SessionGameClient : ProtocolGame_ICallbackReceiver
    {
		
		public String  ConvertGuidToString(ItemGuid guid) {
			String str = Convert.ToString(guid.worldID) + "-" + Convert.ToString(guid.gameID) + "-" + Convert.ToString(guid.serialID);
			return str;

		}

		//其他用户请求连接P2P游戏服务器
		// Param : id	请求进入的用户帐号ID
		// Param : roomTicket	用户的进入房间的门票
		// Param : characterInfo	用户的角色信息
		public void On_P2P_NewPlayerRequestLogin(int id, int roomTicket, SRoomCharacterInfo characterInfo){
			Console.WriteLine("On_P2P_NewPlayerRequestLogin, ID:{0}", id);
		}


		//玩家登陆成功
		// Param : reconnected	是否是断线重连
		// Param : isNeedCreateAccount	是否需要创建帐户信息
		// Param : accountInfo	account信息
		public  void  On_UserLoginOK(bool reconnected, SAccountInfo accountInfo, vectorCharacterInfoLogins characterDataList) {
#if NGUI
			GUILogManager.LogInfo("On_UserLoginOK");
			GameManager.Instance.SetDisconnectFlag(false);
			#region account
			DataManager.Instance.UpdateValue(DataListType.AccountData,"uid",VersionManager.Instance.GetUIDFormAccount(accountInfo.account));
			DataManager.Instance.Save(DataListType.AccountData);
			PlayerDataManager.Instance.SetAccountInfo(accountInfo);
			PlayerDataManager.Instance.SetCharaLoginListInfo(characterDataList);
			PlayerDataManager.Instance.SetKarmaVal(accountInfo.SK);
			PlayerDataManager.Instance.SetCrystalVal(accountInfo.FK);
			#endregion
			#region event info
			PlayerDataManager.Instance.BanAList.Clear();
			foreach (string data in accountInfo.eventData.Values){
				PlayerDataManager.Instance.AddToBanList(data);
			}
			#endregion
			PopUpBox.Hide(true);
			if(GameManager.Instance.GetReconnectFlag()) {
				GameManager.Instance.SetReconnectFlag(false);
				
			}else {
				UI_Fade_Control.Instance.FadeOutAndIn("SelectScreen");
			}
#else
			LogManager.Log_Warn("On_UserLoginOK");
			#region account
			ClientLogicCtrl.Instance.uid = ClientLogicCtrl.Instance.GetUID(accountInfo.account);
			#endregion
			#region reset player info
			_UI_CS_FightScreen.Instance.isCheckPing = true;
			_PlayerData.Instance.myAccountInfo = accountInfo;
			ChatBoxSettingSendType.Instance.InitSendType();
			MoneyBadgeInfo.Instance.fkText.Text = accountInfo.FK.ToString();
			#endregion
			#region reset reveive
			//Init revive num
			_PlayerData.Instance.playerReviveItemNum = accountInfo.reviveNum;
			_PlayerData.Instance.playerReviveItemNumText.Text = accountInfo.reviveNum.ToString();
//			_UI_CS_Revival.Instance.reviveItemCountText.Text = accountInfo.reviveNum.ToString();
			RevivePanel.Instance.UpdateRevivalItemCount(accountInfo.reviveNum);
			#endregion
			#region init platform info
			//Init pay menu info.
			BuyKarma.Instance.InitPlatformIcon();
			#endregion
			#region event info
			//账号事件数据同步
			EventSystem.Instance.BanAList.Clear();
			foreach (string data in accountInfo.eventData.Values){
				EventSystem.Instance.AddToBanList(data);
			}
			#endregion
			#region Login model
			int index = 0;
			if(_UI_CS_Ctrl.Instance.m_isReconnect){
				_UI_CS_Login.Instance.m_CS_loginPanel.Dismiss();
				_UI_CS_Ctrl.Instance.m_isReconnect = false;
				foreach (SCharacterInfoLogin logininfo in characterDataList){
					SelectChara.Instance.CharacterList[index] = logininfo;
					SelectChara.Instance.SelectBtn[index].SetStats(true,logininfo);
					index++;
				}
			}else{
				if(_UI_CS_TestModeCtrl.Instance.IsTestMode){
					_UI_CS_TestModeCtrl.Instance.Info.Text  = " --- user login ok,next auto select character ---";
					foreach (SCharacterInfoLogin logininfo in characterDataList){
						SelectChara.Instance.CharacterList[index] = logininfo;
						SelectChara.Instance.SelectBtn[index].SetStats(true,logininfo);
						index++;
					}
					int characterIdx = SelectChara.Instance.FindExistCharaIdx();
					if(-1 != characterIdx){
						LogManager.Log_Debug("--- SelectCharacter ---");
						CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolGame_SendRequest.UserSelectCharacter(SelectChara.Instance.SelectBtn[characterIdx].info.ID)
						);
					}else{
						SCharacterCreationData scd = new SCharacterCreationData();
						mapIntInt avatarTraits = new mapIntInt();
						string robotName = "robot" + UnityEngine.Random.Range(0,1000).ToString();
						scd.nickname = robotName;
						scd.style	 = 1;
						scd.avatarTraits = avatarTraits;
						scd.sex = new ESex();
						CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolGame_SendRequest.UserCreateCharacter(scd)
						);
					}	
				}else{
					MoneyBadgeInfo.Instance.skText.Text =accountInfo.SK.ToString();
					_UI_CS_Login.Instance.m_login_LogoPanel.Dismiss();
					foreach (SCharacterInfoLogin logininfo in characterDataList){
						SelectChara.Instance.CharacterList[index] = logininfo;
						SelectChara.Instance.SelectBtn[index].SetStats(true,logininfo);
						index++;
					}
					_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.Dismiss();
					_UI_CS_Login.Instance.m_CS_loginPanel.Dismiss();
					SelectChara.Instance.AwakeSelectChara();
					_UI_CS_NewsCtrl.Instance.InitNewsInfo();
					SelectChara.Instance.Wait1fStart();
					_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_SELECT);
				}
			}
			#endregion
			SelectChara.Instance.SetActiveCharaBtn(0);
#endif
		}

		//玩家角色数据列表
		// Param : characterDataList	角色数据列表
		public void On_SendCharacterInfoList(vectorCharacterInfoLogins characterDataList) {
#if NGUI
            PlayerDataManager.Instance.SetCharaLoginListInfo(characterDataList);
#endif
			foreach (SCharacterInfoLogin logininfo in characterDataList) {
				Console.WriteLine("CharacterInfo, CharacterID: {0}", logininfo.ID);
			}
		}


		//玩家登陆失败
		// Param : error	失败原因
		public void On_UserLoginFailed(EServerErrorType error) {	
#if NGUI	
			GUILogManager.LogInfo(string.Format(@"On_UserLoginFailed. errtype: {0}",error.GetString()));
			PopUpBox.PopUpErr(error);
#else			
			LogManager.Log_Warn("On_UserLoginFailed");
			_UI_CS_PopupBoxCtrl.PopUpError(error);
			_UI_CS_TestModeCtrl.Instance.Info.Text = error.GetString();
#endif			
		}

		//玩家创建角色成功
		// Param : characterInfo	角色信息
		public void On_UserCreateCharacterOK(SCharacterInfoLogin characterInfo) {
			#region kong
			Kong.Instance.KongRate();
			#endregion
			#region login model
#if NGUI
			GUILogManager.LogInfo("On_UserCreateCharacterOK");
			TutorialMan.Instance.SetTutorialFlag(true);
			PlayerDataManager.Instance.AddCharacterToList(characterInfo);
			CreateScreenCtrl.Instance.BackDelegate();
			PopUpBox.Hide(true);
#else
			if(_UI_CS_TestModeCtrl.Instance.IsTestMode){
				LogManager.Log_Debug("--- SelectCharacter ---");
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.UserSelectCharacter(characterInfo.ID)
				);
			}else{
				LogManager.Log_Debug("---  On_UserCreateCharacterOK ---");
				PlayerInfoBar.Instance.UpdateAllyPos();
				int idx = SelectChara.Instance.FindEmptyCharaIdx();
				SelectChara.Instance.CharacterList[idx] = characterInfo;
				SelectChara.Instance.SelectBtn[idx].SetStats(true,characterInfo);
				SelectChara.Instance.SetActiveCharaBtn(idx);
				_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.Dismiss();
				_UI_CS_CreateMenu.Instance.m_CS_CharacterCreatePanel.Dismiss();
				#region change 
//				SelectChara.Instance.AwakeSelectChara();
//				SelectChara.Instance.Wait1fStart();
//				if(-1 != idx){
//					_UI_CS_DownLoadPlayer.Instance.equipmentMan.DetachAllItems(SelectChara.Instance.SelectBtn[idx].info.sex);
//					SelectChara.Instance.UpdateModelEquip(idx);
//				}else{
//					SelectChara.Instance.Model.Hide(true);
//				}
				_UI_CS_CreateMenu.Instance.LeaveCreaterScenes(true);
//				SelectChara.Instance.TutorialMsgBoxPanel.BringIn();
//				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_SELECT);
				
				////////////////////////////////////////////////////////////////////////////////////////////
				TutorialMan.Instance.SetTutorialFlag(true);
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.UserSelectCharacter(characterInfo.ID)
				);
				
				#endregion
			}
#endif			
			#endregion

		} 

		//玩家创建角色失败
		// Param : error	错误类型
		public void On_UserCreateCharacterFailed(EServerErrorType error) {
#if NGUI
			GUILogManager.LogErr("On_UserCreateCharacterFailed: "+error.ToString());
			PopUpBox.PopUpErr(error);
#else
			LogManager.Log_Debug("On_UserCreateCharacterFailed");
			_UI_CS_PopupBoxCtrl.PopUpError(error);
#endif
		}
		
		//玩家选择角色成功
		// Param : characterData	角色数据
		// Param : attrVec	角色属性数组
		public void On_UserSelectCharacterOK(SCharacterInfoBasic characterData, vectorAttrChange attrVec) {
#if NGUI
			GUILogManager.LogInfo("On_UserSelectCharacterOK");
			PlayerDataManager.Instance.SetCurPlayerInfoBase(characterData);
			#region steam
			if (Steamworks.activeInstance != null) {
				Steamworks.activeInstance.DLCValidate(characterData.nickname);
			}
			#endregion
			#region event
			//event//
			PlayerDataManager.Instance.BanCList.Clear();
			foreach (string data in characterData.eventData.Values){
				PlayerDataManager.Instance.AddToBanList(data);
			}
			PlayerDataManager.Instance.CheckEvent(EM_EVENT_TYPE.EM_WEEK);
			#endregion
			
			#region PlayerData Init
            PlayerDataManager.Instance.InitData();
			PlayerDataManager.Instance.ChaName = characterData.nickname;
            PlayerDataManager.Instance.CurLV = characterData.level;
            PlayerDataManager.Instance.CurrentExperience = (long)characterData.exp;
			PlayerDataManager.Instance.Gender = characterData.sex;
			PlayerDataManager.Instance.EmptyAllFoodSlot();
            PlayerDataManager.Instance.SetAbiCoolDownList(characterData.cooldownData);
            Player.Instance.EquipementMan.DetachAllItems(characterData.sex);
			#endregion
			
			#region login mode normal/tutorial
			int missionID = 0;
			
			if(TutorialMan.Instance.GetTutorialFlag()){
				PlayerDataManager.Instance.SetScenseName("Hub_Village_Tutorial");
				CS_Main.Instance.g_commModule.SendMessage(
					ProtocolGame_SendRequest.TutorialEnterReq(SelectScreenCtrl.Instance.GetCurSelectCharaID())
				);
				int m_missionID = _UI_CS_MapInfo.Instance.SceneNameToMissionID("Hub_Village_Tutorial") + 1;
				PlayerDataManager.Instance.SetMissionID(m_missionID);
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.AcceptMission(1001, m_missionID));
			}else{
				missionID = _UI_CS_MapInfo.Instance.SceneNameToMissionID("Hub_Village") + 1;
				UI_Fade_Control.Instance.FadeOutAndIn("Hub_Village", "Hub_Village", missionID-1);
			}
			#endregion
#else	
			LogManager.Log_Debug("On_UserSelectCharacterOK");
			if (Steamworks.activeInstance != null) {
				Steamworks.activeInstance.DLCValidate(characterData.nickname);
			}
			#region abilities shop
			AbilitiesShop.Instance.SetAbiCoolDownList(characterData.cooldownData);
			#endregion
			
			#region player info
			_PlayerData.Instance.CharactorInfo = characterData;
            Player.Instance.EquipementMan.DetachAllItems(characterData.sex);
            foreach (SAttributeChange attr in attrVec){
                Player.Instance.AttrMan.Attrs[attr.attributeType.Get()] = attr.value;
            }
            CS_Main.Instance.SetUserCharactor(characterData, attrVec);
            _PlayerData.Instance.playerLevel = characterData.level;
            _PlayerData.Instance.playerCurrentExp = (long)characterData.exp;
			PlayerInfoBar.Instance.UpdatePlayerLevel(characterData.level);
			PlayerInfoBar.Instance.InitPlayerState(characterData.style,characterData.sex);
			_PlayerData.Instance.SetPlayerName(characterData.nickname);
			#endregion
			_UI_CS_EventRewards.Instance.IsFirstLogin = true;
			#region gift
			//req gift list
			CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.GetRedeemGift()
			);
			#endregion
			#region event
			//角色事件处理
			EventSystem.Instance.BanCList.Clear();
			foreach (string data in characterData.eventData.Values){
				EventSystem.Instance.AddToBanList(data);
			}
			EventSystem.Instance.CheckEvent(EM_EVENT_TYPE.EM_WEEK);
			#endregion
			#region mail
			//get mail info
			MailSystem.Instance.ResetMailSystem();
			MailSystem.Instance.GetMailList();
			#endregion
			#region login model
			//test mode
			if(_UI_CS_TestModeCtrl.Instance.IsTestMode){
				_UI_CS_TestModeCtrl.Instance.testModePanel.Dismiss();
				_UI_CS_LoadProgressCtrl.Instance.AwakeLoading(0);
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LOADING);
				vectorString mapnameVector = new vectorString();
				mapnameVector.Add(_UI_CS_TestModeCtrl.Instance.mapName.text);
				if(_UI_CS_TestModeCtrl.Instance.GetFixBtnState()){
					CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.ReloadMap(mapnameVector)
														 );
				}
				CS_Main.Instance.g_commModule.SendMessage(
		   			ProtocolGame_SendRequest.MissionListReq()
											 );
				EChatType chatType = new  EChatType(1);
				string chatInfo = ".SetD ";
				int idxLv = _UI_CS_TestModeCtrl.Instance.GetLvState()+1;
				chatInfo = chatInfo + idxLv.ToString();
				CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolGame_SendRequest.ChatRequest(chatType,0,0,chatInfo)
				);
				int MissionID = _UI_CS_TestModeCtrl.Instance.SceneNameToMissionID(_UI_CS_TestModeCtrl.Instance.mapName.text) + idxLv;
				MissionPanel.Instance.currentMissionID = MissionID;
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.AcceptMission(1001,MissionID)
													 );
				CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.EnterScene(_UI_CS_TestModeCtrl.Instance.mapName.text));
				_UI_CS_TestModeCtrl.Instance.Info.Text = "--- Go ---";
			}else{
				//normal mode
				SelectChara.Instance.Model.Hide(true);		
//				_UI_CS_EventRewards.Instance.IsFirstLogin = true;
				_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.Dismiss();
				SelectChara.Instance.RootPanel.Dismiss();
				MoneyBadgeInfo.Instance.Hide(true);
				_UI_MiniMap.Instance.isShowBigMap   = false;
				_UI_MiniMap.Instance.isShowSmallMap = false;
				_UI_CS_LoadProgressCtrl.Instance.AwakeLoading(0);
				_UI_CS_Ctrl.Instance.m_UI_Camera.enabled = true;
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LOADING);
				
				//tutorial
//				if(Tutorial.Instance.isTutorial){
				if(TutorialMan.Instance.GetTutorialFlag()){
//					SelectChara.Instance.TutorialMsgBoxPanel.Dismiss();
					CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolGame_SendRequest.TutorialEnterReq(SelectChara.Instance.SelectBtn[SelectChara.Instance.GetCurrentSelectIdx()].info.ID)
					);
				MissionPanel.Instance.currentMissionName = "Hub_Village_Tutorial";
				#region init mission complete info
					MissionComplete.Instance.InitMissionCompleteInfo("Welcome, Red Hunter",1);
				#endregion
				int MissionID = _UI_CS_TestModeCtrl.Instance.SceneNameToMissionID("Hub_Village_Tutorial") + 1;
				MissionPanel.Instance.currentMissionID = MissionID;
				CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.AcceptMission(1001,MissionID)
														 );
				}else{
					CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.EnterScene("Hub_Village"));
				}
			}
			#endregion
#endif
		}

		//玩家选择角色失败
		// Param : error	错误类型
		public void On_UserSelectCharacterFailed(EServerErrorType error) {
#if NGUI			
			GUILogManager.LogInfo(string.Format("On_UserSelectCharacterFailed. errtype: {0}",error.GetString()));
			PopUpBox.PopUpErr(error);
#else			
			LogManager.Log_Debug("On_UserSelectCharacterFailed");
			_UI_CS_PopupBoxCtrl.PopUpError(error);
#endif
		}

		//玩家删除角色相应
		// Param : characterID	角色ID
		// Param : reason	错误类型 0：表示成功
		public void On_UserDelCharacterResult(int characterID, EServerErrorType error) {
#if NGUI	
			if(EServerErrorType.eSuccess == error.Get()){
				PopUpBox.PopUpErr("Delete Character Success.");
				PlayerDataManager.Instance.RemoveCharaFromList(characterID);
				SelectScreenCtrl.Instance.InitCharaList();
			}else {
				PopUpBox.PopUpErr(error);
			}
#else		
			SelectChara.Instance.DelinfoFromCharaList(characterID);
			int currentIdx = SelectChara.Instance.GetCurrentSelectIdx();
			if(EServerErrorType.eSuccess == error.Get()){
				SelectChara.Instance.CharacterList[currentIdx].ID = 0;
				SelectChara.Instance.SelectBtn[currentIdx].SetStats(false,null);
				currentIdx = SelectChara.Instance.FindExistCharaIdx();
				if(-1 != currentIdx){
					SelectChara.Instance.Model.Hide(false);
					int scurrentIdx = SelectChara.Instance.FindExistCharaIdx();
					_UI_CS_DownLoadPlayer.Instance.equipmentMan.DetachAllItems(SelectChara.Instance.SelectBtn[currentIdx].info.sex);
					foreach(itemuuid equip in SelectChara.Instance.SelectBtn[currentIdx].info.equipinfo){
						Transform item = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(equip.itemID,0,equip.prefabID))as Transform;
                        SItemInfo itemInfo = new SItemInfo();
                        itemInfo.gem = equip.gemID;
                        itemInfo.element = equip.elementID;
                        itemInfo.enchant = equip.enchantID;
                        _UI_CS_DownLoadPlayer.Instance.equipmentMan.UpdateItemInfoBySlot((uint)equip.slotPart, item, itemInfo, true, SelectChara.Instance.SelectBtn[currentIdx].info.sex);
					}
                    _UI_CS_DownLoadPlayer.Instance.equipmentMan.UpdateEquipment(SelectChara.Instance.SelectBtn[currentIdx].info.sex);
                    _UI_CS_DownLoadPlayer.Instance.usingLatestConfig = true;
				}else{
					SelectChara.Instance.Model.Hide(true);	
				}
				SelectChara.Instance.SetActiveCharaBtn(currentIdx);
			}else{
				_UI_CS_PopupBoxCtrl.PopUpError(error);
			}
#endif
		}


		//update joule
		// Param : joule	joule
		public void On_UpdateJoule(ulong joule) {
			Console.WriteLine("On_UpdateJoule joule:{0}", joule);
		}

		//更新货币
		// Param : money	货币
		public  void On_UpdateMoneyInfo(SMoneyInfo money) {
#if NGUI
			PlayerDataManager.Instance.SetKarmaVal(money.Karma);
			PlayerDataManager.Instance.SetCrystalVal(money.FK);
#else
			//LogManager.Log_Debug("On_UpdateMoneyInfo");
			MoneyBadgeInfo.Instance.fkText.Text = money.FK.ToString();
			MoneyBadgeInfo.Instance.skText.Text = money.Karma.ToString();	
#endif
		}

		//level up
		// Param : level	current level
		public void On_LevelUp (int level, int catolog){
#if NGUI
			GUILogManager.LogInfo("On_LevelUp");
			if(catolog == 1) {
                PlayerDataManager.Instance.CurLV = level;
				PlayerDataManager.Instance.CheckEvent(EM_EVENT_TYPE.EM_LEVELUP);
                PlayerDataManager.Instance.UpdateCharacterInfoLogin();
			}
            if (catolog == 2)
            {
                // when player back to village, get this message from server.
                // pet level up.but we don't have design to show something to player.
            }
#else	
			Console.WriteLine("On_LevelUp level:{0} ", level);
			LogManager.Log_Debug("On_LevelUp");
			if(1 == catolog){
				levelUp.Instance.isLevelUp = true;
			    _PlayerData.Instance.playerLevel = level;
				MoneyBadgeInfo.Instance.UpDateBadge(0);	
				EventSystem.Instance.CheckEvent(EM_EVENT_TYPE.EM_LEVELUP);
			}
			_UI_CS_IngameMenu.Instance.UpdateInvBGColor();
#endif
		}
		
        //玩家角色信息
        // Param : characterData	角色数据
        public  void On_UserCharacterInfo(SCharacterInfoBasic characterData) {
            LogManager.Log_Debug("On_UserCharacterInfo");
        }
		
		//技能列表
		// Param : skillInfo	技能数据
		public void On_SendSkillInfo(SPlayerSkillInfo skillInfo) {
#if NGUI
            PlayerAbilityManager _playerAbiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _playerAbiManager.InitAbilitiesBySKillInfo(skillInfo);
            CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.RoleMasteryListReq());
#else
            // init abilities
            PlayerAbilityManager _playerAbiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _playerAbiManager.InitAbilitiesBySKillInfo(skillInfo);

			LogManager.Log_Debug("On_SendSkillInfo");
			_AbiMenuCtrl.Instance.ClearAbiList();
			
			foreach (SSkillInfo skill in skillInfo.skillInfoVec){
				//12001 & 12002 是复活技能
				if(10001 != skill.skillID && 12001 != skill.skillID && 12002 != skill.skillID){
					_AbiMenuCtrl.Instance.AddAbilitiesItem(skill.skillID);
				}
			}

			foreach (SSkillShortcut skill in skillInfo.skillShortcutVec){
				_AbiMenuCtrl.Instance.AddAbilitiesItemToUseGroup(skill.skillID,skill.groupIdx,skill.idx);			
			}
            _AbiMenuCtrl.Instance.InitAbiObjInfo(AbilityDetailInfo.EDisciplineType.EDT_Prowess);
			_AbiMenuCtrl.Instance.SetCurrentGroup(0);
			_AbiMenuCtrl.Instance.UpDateIngameAbilitiesIcon();
#endif
		}

		//背包列表
		// Param : baginfo	背包
		public void On_SendBagInfo(SBagGroup baggroupinfo) {
#if NGUI
			GUILogManager.LogInfo("On_SendBagInfo");
			#region bag
			foreach (SItemInfo iteminfo in baggroupinfo.basebag.iteminfos) {
				PlayerDataManager.Instance.SetSoltIsChange((int)(iteminfo.slot+1),true);
				PlayerDataManager.Instance.UpdateBagSlot(iteminfo);
			}
			#endregion
			#region equip
			foreach (SItemInfo iteminfo in baggroupinfo.equipbag.iteminfos) {
				PlayerDataManager.Instance.SetEquipSoltChange((int)(iteminfo.slot),true);
				PlayerDataManager.Instance.UpdateEquipSlot(iteminfo);
			}
			PlayerDataManager.Instance.InitPlayerModelEquips();
			#endregion
			#region stash
			foreach (SItemInfo iteminfo in baggroupinfo.stashbag.iteminfos) {
				PlayerDataManager.Instance.SetStashSoltChange((int)(iteminfo.slot+1),true);
				PlayerDataManager.Instance.UpdateStashSlot(iteminfo);
			}
			PlayerDataManager.Instance.SetStashMaxTab((int)baggroupinfo.stashbag.capacity);
			PlayerDataManager.Instance.SetCurStashTapIdx(1);
			#endregion
#else
			LogManager.Log_Debug("On_SendBagInfo");
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			#region bag
			foreach (SItemInfo iteminfo in baggroupinfo.equipbag.iteminfos) {	
				ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(iteminfo.ID,iteminfo.perfrab,iteminfo.gem,iteminfo.enchant,iteminfo.element,(int)iteminfo.level);
				Inventory.Instance.equipmentArray[iteminfo.slot].ItemStruct = tempItem;
				Inventory.Instance.equipmentArray[iteminfo.slot].m_ItemInfo = iteminfo;
				Inventory.Instance.equipmentArray[iteminfo.slot].setIsEmpty(false);
				Inventory.Instance.equipmentArray[iteminfo.slot].SetItemTypeID(tempItem._TypeID);
				ItemPrefabs.Instance.GetItemIcon(tempItem._ItemID,tempItem._TypeID,tempItem._PrefabID,Inventory.Instance.equipmentArray[iteminfo.slot].m_MyIconBtn);
				ItemPrefabs.Instance.GetItemIcon(tempItem._ItemID,tempItem._TypeID,tempItem._PrefabID,Inventory.Instance.equipmentArray[iteminfo.slot].ClonIcon);	
			}
			#endregion
			#region equip
			foreach (SItemInfo iteminfo in baggroupinfo.basebag.iteminfos) {
				ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(iteminfo.ID,iteminfo.perfrab,iteminfo.gem,iteminfo.enchant,iteminfo.element,(int)iteminfo.level);
				Inventory.Instance.bagItemArray[iteminfo.slot].ItemStruct = tempItem;
				Inventory.Instance.bagItemArray[iteminfo.slot].m_ItemInfo = iteminfo;
				Inventory.Instance.bagItemArray[iteminfo.slot].setIsEmpty(false);			
				Inventory.Instance.bagItemArray[iteminfo.slot].SetItemTypeID(tempItem._TypeID);
				ItemPrefabs.Instance.GetItemIcon(tempItem._ItemID,tempItem._TypeID,tempItem._PrefabID,Inventory.Instance.bagItemArray[iteminfo.slot].m_MyIconBtn);
				ItemPrefabs.Instance.GetItemIcon(tempItem._ItemID,tempItem._TypeID,tempItem._PrefabID,Inventory.Instance.bagItemArray[iteminfo.slot].ClonIcon);
			}
			#endregion
			#region stash
			Stash.Instance.InitStashInfo(baggroupinfo.stashbag);
			#endregion
			_UI_CS_IngameMenu.Instance.UpdateInvBGColor();
			_UI_CS_IngameMenu.Instance.isUpdateEquiptments = true;
			_UI_CS_IngameMenu.Instance.UpdateEquipIconState();
#endif
		}


        //买物品失败
        // Param : reason	失败原因
        public void On_BuyItemFailed(EServerErrorType reason) {
			LogManager.Log_Debug("On_BuyItemFailed reason: "+reason.ToString());
			PlayerDataManager.Instance.isUpdateRareShopItem = false;
#if NGUI
			PopUpBox.PopUpErr(reason);
#else
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			_UI_CS_PopupBoxCtrl.PopUpError(reason);
#endif
        }

		//试用物品失败
		// Param : reason	失败原因
		public void On_TryItemFailed(EServerErrorType reason)
		{
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			LogManager.Log_Debug("On_TryItemFailed");
			Console.WriteLine("On_BuyTrialItemFailed, Reason {0} ", reason.GetString());
		}

		//买试用物品失败
		// Param : reason	失败原因
		public void On_BuyTrialItemFailed(EServerErrorType reason)
		{
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			LogManager.Log_Debug("On_BuyTrialItemFailed");
			Console.WriteLine("On_BuyTrialItemFailed, Reason {0} ", reason.GetString());
		}


        //卖物品失败
        // Param : reason	失败原因
        public void On_SaleItemFailed(EServerErrorType reason)
        {
#if NGUI
			GUILogManager.LogInfo("On_SaleItemFailed");
			PopUpBox.PopUpErr(reason);
#else
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			LogManager.Log_Debug("On_SaleItemFailed");
			Console.WriteLine("On_SaleItemFailed, Reason {0} ", reason.GetString());
#endif
        }


		//拆分物品
		// Param : reason	失败原因
		public void On_SplitItemFailed(EServerErrorType reason)
		{
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			LogManager.Log_Debug("On_SwapItemFailed");
			LogManager.Log_Debug("On_SplitItemFailed");
			Console.WriteLine("On_SplitItemFailed, Reason {0} ", reason.GetString());
        }

		//交换物品失败
		// Param : reason	失败原因
		// Param : srcbagID	背包类型
		// Param : srcslot	源位置
		// Param : dstslot	目的位置
		public void On_SwapItemFailed(EServerErrorType reason)
		{
#if NGUI
			GUILogManager.LogInfo("On_SwapItemFailed reason:"+reason.ToString());
#else
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			LogManager.Log_Debug("On_SwapItemFailed:"+reason.GetString());
			//item位置重置
			switch(Inventory.Instance.preBagItmeIndex){
			case 1:
					Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].transform.position = 
					Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
						break;
			case 2:
					Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].transform.position = 
					Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
						break;
			case 4:
					Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).transform.position = 
					Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).m_StartPosition;
						break;
			default:
					break;
			}
#endif
		}

        //删除物品
        // Param : reason	失败原因
        // Param : bagID	背包类型
        // Param : slot	位置
		public void On_DelItemFailed(EServerErrorType reason) {
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			LogManager.Log_Debug("On_DelItemFailed");
        }


		//背包添加物品失败
		// Param : reason	失败原因
		public void On_ActiveBagAddItemFailed(EServerErrorType reason) {
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			LogManager.Log_Debug("On_ActiveBagAddItemFailed");
		}



        //更新物品通知
        // Param : bagID	背包类型
        // Param : iteminfo	更新物品
		public void On_NotifyItemUpdate(byte bagID, SItemInfo iteminfo) {
#if NGUI
			GUILogManager.LogInfo("On_NotifyItemUpdate");
			Debug.Log("BagID is ["+bagID+"]");
			if(bagID==2) {
				PlayerDataManager.Instance.SetSoltIsChange((int)iteminfo.slot+1,true);
				PlayerDataManager.Instance.UpdateBagSlot(iteminfo);
				if(InGameScreenCtrl.Instance) {
					if(InGameScreenCtrl.Instance.inventoryCtrl) {
						InGameScreenCtrl.Instance.inventoryCtrl.UpdateInventorySlot();
					}
				}
			}
			if(bagID==1) {
				PlayerDataManager.Instance.UpdateEquipSlot(iteminfo);
				if(InGameScreenCtrl.Instance) {
					if(InGameScreenCtrl.Instance.inventoryCtrl) {
						InGameScreenCtrl.Instance.inventoryCtrl.UpdateEquipSlot();
						InGameScreenCtrl.Instance.inventoryCtrl.UpdatePlayerModeEquip();
					}
				}
			}
			if(bagID==4) {
				PlayerDataManager.Instance.SetStashSoltChange((int)iteminfo.slot+1,true);
				PlayerDataManager.Instance.UpdateStashSlot(iteminfo);
				if(InGameScreenCtrl.Instance) {
					if(InGameScreenCtrl.Instance.inventoryCtrl) {
						InGameScreenCtrl.Instance.inventoryCtrl.UpdateStashSlot();
					}
				}
			}
#else
			LogManager.Log_Debug("On_NotifyItemUpdate");
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;

			 ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(iteminfo.ID,iteminfo.perfrab,iteminfo.gem,iteminfo.enchant,iteminfo.element,(int)iteminfo.level);
			
			switch(bagID){
			case 1:
					// ... ...
					break;
			case 2:
					{
				#region bag
						Inventory.Instance.bagItemArray[iteminfo.slot].setIsEmpty(false);
						Inventory.Instance.bagItemArray[iteminfo.slot].ItemStruct = tempItem;
						Inventory.Instance.bagItemArray[iteminfo.slot].m_ItemInfo = iteminfo;
						Inventory.Instance.bagItemArray[iteminfo.slot].SetItemTypeID(tempItem._TypeID);
						Inventory.Instance.bagItemArray[iteminfo.slot].m_MyCountText.Text = iteminfo.count.ToString();
						Inventory.Instance.bagItemArray[iteminfo.slot].UpdateItemHighLevel();
						ItemPrefabs.Instance.GetItemIcon(tempItem._ItemID,tempItem._TypeID,tempItem._PrefabID,Inventory.Instance.bagItemArray[iteminfo.slot].m_MyIconBtn);
						ItemPrefabs.Instance.GetItemIcon(tempItem._ItemID,tempItem._TypeID,tempItem._PrefabID,Inventory.Instance.bagItemArray[iteminfo.slot].ClonIcon);
				#endregion
					}
					break;
			case 4:
					stashInfo tempInfo = new stashInfo();
					tempInfo.serInfo = iteminfo;
					tempInfo.dropInfo = tempItem;
					tempInfo.isEmpty = false;
					Stash.Instance.AddItemToStash((int)iteminfo.slot,tempInfo);
					break;
			default:
					break;
			}
			_UI_CS_IngameMenu.Instance.UpdateInvBGColor();
#endif		
		}

        //删除物品
        // Param : bagID	背包类型
        // Param : slot	位置
		public void On_NotifyItemDestroy(byte bagID, uint slot) {
#if NGUI
			GUILogManager.LogInfo("On_NotifyItemDestroy");

			if(bagID==2) {  
				PlayerDataManager.Instance.SetSoltIsChange((int)(slot+1),true);
				PlayerDataManager.Instance.EmptyBagSlot((int)slot+1);

                if(InGameScreenCtrl.Instance.inventoryCtrl) 
					InGameScreenCtrl.Instance.inventoryCtrl.UpdateInventorySlot();

			}
			if(bagID==1) {
				PlayerDataManager.Instance.SetEquipSoltChange((int)(slot),true);
				PlayerDataManager.Instance.EmptyEquipSlot((int)slot);
				
                if(InGameScreenCtrl.Instance.inventoryCtrl) 
					InGameScreenCtrl.Instance.inventoryCtrl.UpdateEquipSlot();

			}
#else
			LogManager.Log_Debug("On_NotifyItemDestroy");
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			switch(bagID){
			case 1:
					// ... ...
					break;
			case 2:
					{
						Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].UpdateGroupElementPosition();
						
						Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].transform.position = 
						Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
						
						Inventory.Instance.bagItemArray[slot].setIsEmpty(true);
						Inventory.Instance.bagItemArray[slot].m_ItemInfo = null;
						Inventory.Instance.bagItemArray[slot].UpdateItemHighLevel();
						Inventory.Instance.bagItemArray[slot].SetIconTexture(null);
						Inventory.Instance.bagItemArray[slot].SetClonIconTexture(null);
					
						_UI_CS_IngameMenu.Instance.UpdateInvBGColor();
						
						CraftingDetailPanel.Instance.UpdateControlPanel();
					}
					break;
			case 4:
					Stash.Instance.DelItemFromStash((int)slot);
					Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).transform.position = 
					Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).m_StartPosition;
					break;
			default:
					break;
			}
//			SoundCue.PlayPrefabAndDestroy(Inventory.Instance.transmuteSound);
#endif
		}


		//试用物品到期通知
		// Param : slot	物品
		public void On_NotifyItemTrialEnded(int slot) {
			LogManager.Log_Debug("On_NotifyItemTrialEnded");
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
		}
		

				//装备物品失败
		// Param : reason	失败原因
		public void On_EquipErrorFailed(EServerErrorType reason) {
#if NGUI
			GUILogManager.LogInfo("On_EquipErrorFailed");
#else
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			LogManager.Log_Debug("On_EquipErrorFailed");
			LogManager.Log_Debug("reason:"+reason.GetString());
			LogManager.Log_Debug("m_IngameMenu_PreSlotItmeIndex:"+Inventory.Instance.preSlotItmeIndex);
			LogManager.Log_Debug("m_IngameMenu_PreBagItmeIndex:"+Inventory.Instance.preBagItmeIndex);
			//item位置重置
			switch(Inventory.Instance.preBagItmeIndex){
			case 1:
					Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].transform.position = 
					Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
						break;
			case 2:
					Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].transform.position = 
					Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
						break;
			case 4:
				Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).transform.position = 
				Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).m_StartPosition;
					break;
			default:
					break;
			}
#endif
		}

		//卸载物品失败
		// Param : reason	失败原因
		public void On_UnEquipErrorFailed(EServerErrorType reason) {
#if NGUI
			GUILogManager.LogInfo("On_UnEquipErrorFailed");
#else
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			LogManager.Log_Debug("On_EquipErrorFailed");
			LogManager.Log_Debug("m_IngameMenu_PreSlotItmeIndex:"+Inventory.Instance.preSlotItmeIndex);
			LogManager.Log_Debug("m_IngameMenu_PreBagItmeIndex:"+Inventory.Instance.preBagItmeIndex);
			LogManager.Log_Debug("reason:"+reason.GetString());
			Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].transform.position = 
			Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
			//item位置重置
			switch(Inventory.Instance.preBagItmeIndex){
			case 1:
					Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].transform.position = 
					Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
						break;
			case 2:
					Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].transform.position = 
					Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
						break;
			case 4:
					Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).transform.position = 
					Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).m_StartPosition;
					break;
			default:
					break;
			}
#endif			
		}


		//聊天回应
		// Param : chatType	聊天类型
		// Param : senderID	发送者ID
		// Param : senderName	发送者姓名
		// Param : chatMsg	聊天内容
		public void On_ChatResponse(EChatType chatType, int senderID, string senderName, string chatMsg) {
#if NGUI
	Debug.Log("chatok");
	//------------------------------------------------------------------------------->>mm
	chatBoxManager.Instance.ReceiveNewMsg(chatType,chatMsg,senderName,senderID);
			//------------------------------------------------------------------------------->>#mm
#else
			ChatBox.Instance.ReceiveNewMsg(chatType,chatMsg,senderName,senderID);
			
#endif
		}

		//卸载装备物品失败
		// Param : reason	失败原因
		public void On_ChatFailed(EServerErrorType reason)
		{
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			Console.WriteLine("On_ChatFailed, reason:{0}", reason.GetString());
		}

		//通知被加为好友，是否同意
		// Param : info	好友信息
		public void On_NotifyAddAccountFriend(SAccountFriendInfo info)
		{
			Console.WriteLine("On_NotifyAddAccountFriend, accountID:{0} name:{1} gender:{2} online:{3}",
					info.accountID, info.name, info.gender, info.onlineflag);

		}

		//添加好友成功
		// Param : info	信息
		public void On_AddAccountFriendOK(int accountID)
		{
			Console.WriteLine("On_AddAccountFriendOK, accountID:{0}", accountID);
		}

		//添加好友失败
		// Param : reason	失败原因
		public void On_AddAccountFriendFailed(EServerErrorType reason)
		{
			Console.WriteLine("On_AddAccountFriendFailed, reason:{0}", reason.GetString());
		}

		//删除好友成功
		// Param : accountID	好友ID
		public void On_DelAccountFriendOK(int accountID)
		{
			Console.WriteLine("On_DelAccountFriendOK, accountID:{0}", accountID);
		}

		//删除好友失败
		// Param : reason	失败原因
		public void On_DelAccountFriendFailed(EServerErrorType reason)
		{
			Console.WriteLine("On_DelAccountFriendFailed, reason:{0}", reason.GetString());
		}

		//通知被好友删除
		// Param : accountID	好友ID
		public void On_NotifyDelAccountFriendOk(int accountID)
		{
			Console.WriteLine("On_NotifyDelAccountFriendOk, accountID:{0}", accountID);
		}

		//好友上线通知
		// Param : accountID	好友ID
		public void On_OnlineAccountFriend(int accountID)
		{
			Console.WriteLine("On_OnlineAccountFriend, accountID:{0}", accountID);
		}

		//好友下线通知
		// Param : accountID	好友ID
		public void On_OfflineAccountFriend(int accountID)
		{
			Console.WriteLine("On_OfflineAccountFriend, accountID:{0}", accountID);
		}


		//关系列表
		// Param : friendlist	好友名单
		public void On_SendAccountRelationInfo(mapAccountFriendInfos friendlist)
		{
			Console.WriteLine("On_SendAccountRelationInfo");
			foreach (KeyValuePair<int, SAccountFriendInfo> m in friendlist)
			{
				SAccountFriendInfo info =m.Value;
				Console.WriteLine("friend, accountID:{0} name:{1} gender:{2} online:{3}",
					info.accountID, info.name, info.gender, info.onlineflag);
			}
		}

		//通知GameServer  通知更新账号好友
		// Param : friendlist	好友
		public void On_NotifyUpdateAccountFriend(mapAccountFriendInfos friendlist)
		{
			Console.WriteLine("On_NotifyUpdateAccountFriend");
			foreach (KeyValuePair<int, SAccountFriendInfo> m in friendlist)
			{
				SAccountFriendInfo info = m.Value;
				Console.WriteLine("friend, accountID:{0} name:{1} gender:{2} online:{3}",
					info.accountID, info.name, info.gender, info.onlineflag);
			}
		}
		
		//notify swap item
		// Param : srcbagID	bagID
		// Param : srcslot	srcslot
		// Param : dstbagID	bagID
		// Param : dstslot	dstslot
		public void On_NotifyItemSwap(byte srcbagID, uint srcslot, byte dstbagID, uint dstslot)
		{
#if NGUI
			GUILogManager.LogInfo("On_NotifyItemSwap");

			//b->b
			if(srcbagID == 2&&dstbagID == 2) {	
				PlayerDataManager.Instance.SetSoltIsChange((int)srcslot+1,true);
				PlayerDataManager.Instance.SetSoltIsChange((int)dstslot+1,true);
				PlayerDataManager.Instance.SwapBagToBagSlot((int)srcslot+1,(int)dstslot+1);
				PlayerDataManager.Instance.CheckFoodList((int)srcslot+1,(int)dstslot+1);
				if(InGameScreenCtrl.Instance.inventoryCtrl) {
					InGameScreenCtrl.Instance.inventoryCtrl.UpdateInventorySlot();
				}
				return;
			}
			//b->e
			if(srcbagID == 2&&dstbagID == 1) {	
				PlayerDataManager.Instance.SetSoltIsChange((int)srcslot+1,true);
				PlayerDataManager.Instance.SetEquipSoltChange((int)dstslot,true);
				PlayerDataManager.Instance.SwapEquipToBagSlot((int)dstslot,(int)srcslot+1);
				if(InGameScreenCtrl.Instance.inventoryCtrl) {
					InGameScreenCtrl.Instance.inventoryCtrl.UpdateInventorySlot();
					InGameScreenCtrl.Instance.inventoryCtrl.UpdateEquipSlot();
					InGameScreenCtrl.Instance.inventoryCtrl.UpdatePlayerModeEquip();
				}
				PlayerDataManager.Instance.InitPlayerModelEquips();
				return;
			}
			//e->b
			if(srcbagID == 1&&dstbagID == 2) {	
				PlayerDataManager.Instance.SetSoltIsChange((int)dstslot+1,true);
				PlayerDataManager.Instance.SetEquipSoltChange((int)srcslot,true);
				PlayerDataManager.Instance.SwapEquipToBagSlot((int)srcslot,(int)dstslot+1);
				if(InGameScreenCtrl.Instance.inventoryCtrl) {
					InGameScreenCtrl.Instance.inventoryCtrl.UpdateInventorySlot();
					InGameScreenCtrl.Instance.inventoryCtrl.UpdateEquipSlot();
					InGameScreenCtrl.Instance.inventoryCtrl.UpdatePlayerModeEquip();
				}
				PlayerDataManager.Instance.InitPlayerModelEquips();
				return;
			}
			//s->b
			if(srcbagID == 4&&dstbagID == 2) {	
				PlayerDataManager.Instance.SetSoltIsChange((int)dstslot+1,true);
				PlayerDataManager.Instance.SetStashSoltChange((int)srcslot+1,true);
				PlayerDataManager.Instance.SwapStashToBagSlot((int)srcslot+1,(int)dstslot+1);
				Debug.LogWarning("srcslot:"+(srcslot+1).ToString()+"dstslot:"+(dstslot+1).ToString());
				if(InGameScreenCtrl.Instance.inventoryCtrl) {
					InGameScreenCtrl.Instance.inventoryCtrl.UpdateInventorySlot();
					InGameScreenCtrl.Instance.inventoryCtrl.UpdateStashSlot();
				}
				return;
			}
			//b->s
			if(srcbagID == 2&&dstbagID == 4) {	
				PlayerDataManager.Instance.SetSoltIsChange((int)srcslot+1,true);
				PlayerDataManager.Instance.SetStashSoltChange((int)dstslot+1,true);
				PlayerDataManager.Instance.SwapStashToBagSlot((int)dstslot+1,(int)srcslot+1);
				Debug.LogWarning("srcslot:"+(srcslot+1).ToString()+"dstslot:"+(dstslot+1).ToString());
				if(InGameScreenCtrl.Instance.inventoryCtrl) {
					InGameScreenCtrl.Instance.inventoryCtrl.UpdateInventorySlot();
					InGameScreenCtrl.Instance.inventoryCtrl.UpdateStashSlot();
				}
				return;
			}
			//s->s
			if(srcbagID == 4&&dstbagID == 4) {	
				PlayerDataManager.Instance.SetStashSoltChange((int)srcslot+1,true);
				PlayerDataManager.Instance.SetStashSoltChange((int)dstslot+1,true);
				PlayerDataManager.Instance.SwapStashToStashSlot((int)srcslot+1,(int)dstslot+1);
				Debug.LogWarning("srcslot:"+(srcslot+1).ToString()+"dstslot:"+(dstslot+1).ToString());
				if(InGameScreenCtrl.Instance.inventoryCtrl) {
					InGameScreenCtrl.Instance.inventoryCtrl.UpdateStashSlot();
				}
				return;
			}
#else
			LogManager.Log_Debug("On_NotifyItemSwap");
			_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
			#region stash
			//stash logic
			if(srcbagID == 4 || dstbagID == 4) {
				_UI_CS_InventoryItem invItem;
				stashInfo	ids;
				stashInfo	idsd;
				stashInfo tempStashInfo = new stashInfo();
				//b_->s//
				if(dstbagID == 4 && srcbagID != 4) {
					ids = Stash.Instance.GetStashInfo((int)dstslot);
					invItem = _UI_CS_IngameMenu.Instance.GetBagSlotObject((int)srcbagID,(int)srcslot);
					tempStashInfo.dropInfo = invItem.ItemStruct;
					tempStashInfo.serInfo = invItem.m_ItemInfo;
					tempStashInfo.isEmpty = false;
					//clean or no clean bag item slot.//
					if(ids.isEmpty) {
						invItem.setIsEmpty(true);
						invItem.ItemStruct = null;
						invItem.m_ItemInfo = null;
						invItem.SetIconTexture(null);
						invItem.SetClonIconTexture(null);
						invItem.UpdateCountText();
					}else {
						invItem.setIsEmpty(false);
						invItem.ItemStruct = ids.dropInfo;
						invItem.m_ItemInfo = ids.serInfo;
						invItem.UpdateCountText();
						ItemPrefabs.Instance.GetItemIcon(ids.dropInfo._ItemID,ids.dropInfo._TypeID,ids.dropInfo._PrefabID,invItem.m_MyIconBtn);
						ItemPrefabs.Instance.GetItemIcon(ids.dropInfo._ItemID,ids.dropInfo._TypeID,ids.dropInfo._PrefabID,invItem.ClonIcon);
					}
					//add item to stash.//
					Stash.Instance.AddItemToStash((int)dstslot,tempStashInfo);
				//s->b//
				}else if(srcbagID == 4 && dstbagID != 4){
					ids = Stash.Instance.GetStashInfo((int)srcslot);
					invItem = _UI_CS_IngameMenu.Instance.GetBagSlotObject((int)dstbagID,(int)dstslot);
					tempStashInfo.dropInfo = invItem.ItemStruct;
					tempStashInfo.serInfo = invItem.m_ItemInfo;
					tempStashInfo.isEmpty = invItem.m_IsEmpty;
					//s->b stash 不可能为空//
					if(ids.isEmpty) {
						invItem.setIsEmpty(true);
						invItem.ItemStruct = null;
						invItem.m_ItemInfo = null;
						invItem.SetIconTexture(null);
						invItem.SetClonIconTexture(null);
						invItem.UpdateCountText();
					}else {
						invItem.setIsEmpty(false);
						invItem.ItemStruct = ids.dropInfo;
						invItem.m_ItemInfo = ids.serInfo;
						invItem.UpdateCountText();
						ItemPrefabs.Instance.GetItemIcon(ids.dropInfo._ItemID,ids.dropInfo._TypeID,ids.dropInfo._PrefabID,invItem.m_MyIconBtn);
						ItemPrefabs.Instance.GetItemIcon(ids.dropInfo._ItemID,ids.dropInfo._TypeID,ids.dropInfo._PrefabID,invItem.ClonIcon);
					}
					//add item to stash.//
					Stash.Instance.AddItemToStash((int)srcslot,tempStashInfo);
				}
				//stash -> stash//
				else {
					ids = Stash.Instance.GetStashInfo((int)srcslot);
					idsd = Stash.Instance.GetStashInfo((int)dstslot);
					tempStashInfo.dropInfo = ids.dropInfo;
					tempStashInfo.serInfo = ids.serInfo;
					tempStashInfo.isEmpty = ids.isEmpty;
					Stash.Instance.AddItemToStash((int)srcslot,idsd);
					Stash.Instance.AddItemToStash((int)dstslot,tempStashInfo);
				}
				_ItemTips.Instance.DismissItemTip();
				_UI_CS_IngameMenu.Instance.UpdateInvBGColor();
				_UI_CS_IngameMenu.Instance.UpdateEquipIconState();
				//item位置重置
				switch(Inventory.Instance.preBagItmeIndex){
				case 1:
						Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].transform.position = 
						Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
							break;
				case 2:
						Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].transform.position = 
						Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
							break;
				case 4:
						Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).transform.position = 
						Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).m_StartPosition;
							break;
				}
				return;
			}
			#endregion
			Transform itemEx = null;
			Transform itemEx2 = null;
			bool 		tempIsEmpty;
			SItemInfo	tempItemInfo;
			ItemDropStruct	tempItemDropStruct;
			int			tempItemType;
			//获取itme对象
			_UI_CS_InventoryItem srcItem = _UI_CS_IngameMenu.Instance.GetBagSlotObject((int)srcbagID,(int)srcslot);
			_UI_CS_InventoryItem destItem = _UI_CS_IngameMenu.Instance.GetBagSlotObject((int)dstbagID,(int)dstslot);
			//槽状态交换
			tempIsEmpty = srcItem.m_IsEmpty;
			srcItem.setIsEmpty(destItem.m_IsEmpty);
			destItem.setIsEmpty(tempIsEmpty);	
			//物品信息交换
			tempItemInfo = srcItem.m_ItemInfo;
			srcItem.m_ItemInfo  = destItem.m_ItemInfo;
			destItem.m_ItemInfo = tempItemInfo;
			tempItemDropStruct = srcItem.ItemStruct;
			srcItem.ItemStruct  = destItem.ItemStruct;
			destItem.ItemStruct = tempItemDropStruct;
			//Count
			srcItem.UpdateCountText();
			destItem.UpdateCountText();
//			if(srcItem.m_ItemInfo == null){
//				srcItem.m_MyCountText.Text = "";
//			}else{
//				srcItem.m_MyCountText.Text = srcItem.m_ItemInfo.count.ToString();
//			}
//			if(destItem.m_ItemInfo == null){
//				destItem.m_MyCountText.Text = "";
//			}else{
//				destItem.m_MyCountText.Text = destItem.m_ItemInfo.count.ToString();
//			}
			
			//交换类型
			tempItemType = srcItem.m_ItemTypeID;
			srcItem.m_ItemTypeID  = destItem.m_ItemTypeID;
			destItem.m_ItemTypeID = tempItemType;
			ItemDropStruct tempItem1;
			ItemDropStruct tempItem2;
			if(srcItem.m_ItemInfo == null){
				tempItem1 = null;
			}else{
//				if(srcItem.m_ItemInfo.ID != 0)
					tempItem1 = ItemDeployInfo.Instance.GetItemObject(srcItem.m_ItemInfo.ID,srcItem.m_ItemInfo.perfrab,srcItem.m_ItemInfo.gem,srcItem.m_ItemInfo.enchant,srcItem.m_ItemInfo.element,(int)srcItem.m_ItemInfo.level);
			}
			if(destItem.m_ItemInfo == null){
				tempItem2 = null;
			}else{
//				if(destItem.m_ItemInfo.ID != 0)
					tempItem2 = ItemDeployInfo.Instance.GetItemObject(destItem.m_ItemInfo.ID,destItem.m_ItemInfo.perfrab,destItem.m_ItemInfo.gem,destItem.m_ItemInfo.enchant,destItem.m_ItemInfo.element,(int)destItem.m_ItemInfo.level);
			}
			
			//设置icon
			if(!srcItem.m_IsEmpty){	
				ItemPrefabs.Instance.GetItemIcon(tempItem1._ItemID,tempItem1._TypeID,tempItem1._PrefabID,srcItem.m_MyIconBtn);
				ItemPrefabs.Instance.GetItemIcon(tempItem1._ItemID,tempItem1._TypeID,tempItem1._PrefabID,srcItem.ClonIcon);
			}else{
				srcItem.SetIconTexture(null);
				srcItem.SetClonIconTexture(null);
				//卸载物品
			}
			
			ItemDropStruct equipItem = null;
			SItemInfo equipItemInfo = null;
			
			if(!destItem.m_IsEmpty){	
				
//				destItem.SetIconTexture(ItemPrefabs.Instance.GetItemIcon(tempItem2._ItemID,tempItem2._TypeID,tempItem2._PrefabID));
				ItemPrefabs.Instance.GetItemIcon(tempItem2._ItemID,tempItem2._TypeID,tempItem2._PrefabID,destItem.m_MyIconBtn);
//				destItem.SetClonIconTexture(ItemPrefabs.Instance.GetItemIcon(tempItem2._ItemID,tempItem2._TypeID,tempItem2._PrefabID));
				ItemPrefabs.Instance.GetItemIcon(tempItem2._ItemID,tempItem2._TypeID,tempItem2._PrefabID,destItem.ClonIcon);
				
				//装备物品
				if(2 == srcbagID && 1 == dstbagID || 1 == srcbagID && 2 == dstbagID ){
					uint typeIdx;
					bool isAttach = false;
					if(2 == srcbagID && 1 == dstbagID){
						
						if(0 != tempItem2._PrefabID){
							
							itemEx = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(tempItem2._ItemID,tempItem2._TypeID,tempItem2._PrefabID))as Transform;	
							equipItem = tempItem2;
							equipItemInfo = destItem.m_ItemInfo;
						}
				
						typeIdx = dstslot;
						if(!destItem.m_IsEmpty){
							isAttach = true;
						}else{
							isAttach = false;
							itemEx = null;
						}
					}else{
						
						if(null != tempItem1){
							
							itemEx = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(tempItem1._ItemID,tempItem1._TypeID,tempItem1._PrefabID))as Transform;
                            equipItem = tempItem1;
							equipItemInfo = srcItem.m_ItemInfo;
						}
						
							
						typeIdx = srcslot;
						if(!srcItem.m_IsEmpty){
							isAttach = true;
						}else{
							isAttach = false;
							itemEx = null;
						}
					}
                    _UI_CS_IngameMenu.Instance.PlayEquipSound((int)typeIdx);
                    if (itemEx)
                        itemEx2 = UnityEngine.Object.Instantiate(itemEx) as Transform;
                    Player.Instance.EquipementMan.UpdateItemInfoBySlot(typeIdx, itemEx, equipItemInfo, isAttach, _PlayerData.Instance.CharactorInfo.sex);
                    Player.Instance.EquipementMan.UpdateEquipment(_PlayerData.Instance.CharactorInfo.sex);
                    Player.Instance.GetComponent<PreLoadPlayer>().usingLatestConfig = true;
                    _UI_CS_DownLoadPlayerForInv.Instance.equipmentMan.UpdateItemInfoBySlot(typeIdx, itemEx2, equipItemInfo, isAttach, _PlayerData.Instance.CharactorInfo.sex);
                    _UI_CS_DownLoadPlayerForInv.Instance.equipmentMan.UpdateEquipment(_PlayerData.Instance.CharactorInfo.sex);
                    _UI_CS_DownLoadPlayerForInv.Instance.usingLatestConfig = true;
				}
			}else{
				destItem.SetIconTexture(null);	
				destItem.SetClonIconTexture(null);	
			}
			
			_UI_CS_DebugInfo.Instance.CheckEquipArmor();
			
			//item位置重置
			switch(Inventory.Instance.preBagItmeIndex){
			case 1:
					Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].transform.position = 
					Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
						break;
			case 2:
					Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].transform.position = 
					Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
						break;
			case 4:
					Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).transform.position = 
					Stash.Instance.GetStashSlot(Inventory.Instance.preSlotItmeIndex%12).m_StartPosition;
						break;
			default:
					break;
			}
			_ItemTips.Instance.DismissItemTip();
			_UI_CS_IngameMenu.Instance.UpdateInvBGColor();
			_UI_CS_IngameMenu.Instance.UpdateEquipIconState();
#endif
		}	
		
		public void On_RequestDayRewardOK(SDayReward reward) {
#if NGUI
			GUILogManager.LogInfo("On_RequestDayRewardOK");
			if(reward.returnCode.Get() != EServerErrorType.eSuccess) {
				if(reward.returnCode.Get() == EServerErrorType.eDayRewardError_AlreadyGet){
					CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetFriendReward());
					Debug.Log("*****----------------------------******");
					return;
				}else{
					PopUpBox.PopUpErr(reward.returnCode);
					Debug.Log("*****xxxxxxxxxxxxxxxxxxxxxxxxxxxxx******");
				}	
			}else {
				PlayerDataManager.Instance.dailyWardData = reward;
				GUIManager.Instance.ChangeUIScreenState("DailyRewardScreen");
				Debug.Log("*****()()()()()()()()()()()()()()()()()()******");
			}
#else
			LogManager.Log_Debug("--- On_RequestDayRewardOK ---");
//			_UI_CS_EventRewards.Instance.IsFirstLogin = false;
			DailyRewardGift.Instance.AwakeDailyReward(reward.returnCode,reward.dayID,reward.dayRewardStatusVec);
#endif
		}
		
		public void On_RequestDayRewardFailed(EServerErrorType reason)
		{
			Debug.Log ("---------- FAILD --------------");
		}
		
		public void On_GetDayRewardResult(EServerErrorType reason)
		{
			Debug.Log ("---------- RESULT --------------");
		}
		
		//接任务OK
		// Param : returnCode	1:表示接任务成功
		public void On_AcceptMissionOk(int returnCode){
#if NGUI
		if(1 == returnCode){
			GUILogManager.LogInfo("On_AcceptMissionOk : <" + returnCode + ">");
			PlayerDataManager.Instance.InitMissionDataList();
            MissionTitleCtrl.Reset();
			UI_Fade_Control.Instance.FadeOutAndIn(PlayerDataManager.Instance.GetScenseName(), PlayerDataManager.Instance.GetScenseName(), PlayerDataManager.Instance.GetMissionID()-1);
			PlayerDataManager.Instance.SetRevivalCount(0);
			PlayerDataManager.Instance.RsetMissionScore();
            PlayerDataManager.Instance.ResetIngameMaterial();
		}else {
			LogManager.Log_Debug("On_AcceptMissionFaile: <"+returnCode.ToString()+">");	
		}
#else
	
			if(1 == returnCode){
				LogManager.Log_Debug("--- On_AcceptMissionOk ---");
				_UI_CS_MissionLogic.Instance.RsetMissionScore();
	            BGManager.Instance.ExitOutsideAudio();
				MissionPanel.Instance.LeaveMissionMap();
				
				#region loading
				_UI_CS_LoadProgressCtrl.Instance.AwakeLoading(MissionPanel.Instance.currentMissionID);
				_UI_CS_LoadProgressCtrl.Instance.m_LoadingMapName.Text = _UI_CS_MapScroll.Instance.SceneNameToMapName(MissionPanel.Instance.currentMissionName);
				#endregion
				
//				_UI_CS_MissionSummary.Instance.SetMissionCompleteTitle(_UI_CS_LoadProgressCtrl.Instance.m_LoadingMapName.text);
				
				#region mission complete menu
				MissionComplete.Instance.ResetMaterialList();
				#endregion
				
				#region mini map
				_UI_MiniMap.Instance.isShowBigMap = false;
				_UI_MiniMap.Instance.isShowSmallMap = false;
				#endregion
				
				if(_UI_CS_MapScroll.Instance.IsExistMission){
					_UI_CS_MissionLogic.Instance.InitMissionList(MissionPanel.Instance.currentMissionID);
				}
				_UI_CS_FightScreen.Instance.m_fightPanel.BringIn();
//				_UI_CS_Revival.Instance.RevivalCount = 0;
				RevivePanel.Instance.SetRevivalCount(0);
				CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.EnterScene(MissionPanel.Instance.currentMissionName));
			}else{
				LogManager.Log_Debug("--- On_AcceptMissionOk have problem --- " + returnCode.ToString());
			}
#endif
		}

		//一个task完成时的通知
		// Param : MisssionID	任务ID
		// Param : taskID	task ID
		// Param : exp	奖励XP
		// Param : karma	奖励karma
		public void On_taskCompleteNTF(int MisssionID, int branchID, int taskID, int exp, int karma){
#if NGUI
			GUILogManager.LogInfo("On_taskCompleteNTF");
			if(MissionObjectiveManager.Instance) {
				MissionObjectiveManager.Instance.PlayTaskCompletePanel(PlayerDataManager.Instance.GetCurMissionTaskName(branchID,taskID),exp,karma);
			}
			PlayerDataManager.Instance.SetMissionScore(exp);
#else
			LogManager.Log_Debug("--- On_taskCompleteNTF ---");
			_UI_CS_MissionLogic.Instance.SetMissionScore(exp);
#endif
		}

		//一个Mission完成时的通知
		// Param : MisssionID	任务ID
		// Param : exp	奖励XP
		// Param : karma	奖励karma
		public void On_MissionCompleteNTF(int MisssionID, int exp, int karma){
#if NGUI
			GUILogManager.LogInfo("On_MissionCompleteNTF");
			PlayerDataManager.Instance.SetThreatBoundExp(exp);
			PlayerDataManager.Instance.SetThreatBoundKarma(karma);
			PlayerDataManager.Instance.SetMissionCompleteFlag(true);
			if(MissionObjectiveManager.Instance) {
				MissionObjectiveManager.Instance.FadeInBackCrystal();
			}
#else
			LogManager.Log_Debug("--- On_MissionCompleteNTF ---");
			// only kong use.
			//Kong.Instance.KongRate();
			_UI_CS_MissionLogic.Instance.IstMissionFinish = true;
			MissionComplete.Instance.isCompleteMission = true;
			_UI_CS_Teleport.Instance.AwakeTel();
			_UI_CS_MissionLogic.Instance.MissionBgPanel.Dismiss();
			//setting exp and karma for mission success.
			MissionComplete.Instance.UpdateThreatBound(exp,karma);
#endif
		}

		//任务列表
		// Param : MisssionID	任务ID,最后两位代表任务的等级
		public void On_MissionListAck(vectorSAcceptMissionRelate SAcceptMissionRelateVec){
#if NGUI
			GUILogManager.LogInfo("On_MissionListAck");
			PlayerDataManager.Instance.RestSerMissDataList();
			foreach (SAcceptMissionRelate2 data in SAcceptMissionRelateVec){
				PlayerDataManager.Instance.AddSerMissDataEle(data);
			}	
			PlayerDataManager.Instance.UpdateReceiveSerMsgTime();
			GUIManager.Instance.ChangeUIScreenState("MissionSelectScreen");
#else
			LogManager.Log_Debug("--- On_MissionListAck ---");
			MissionPanel.Instance.missionInfoList.Clear();
			foreach (SAcceptMissionRelate2 mission in SAcceptMissionRelateVec){
				MissionPanel.Instance.missionInfoList.Add(mission);	
			}	
			MissionPanel.Instance.InitMissionMap();
#endif
		}
		
		public void On_MissionFailedNTF(int MisssionID, EServerErrorType reason){
#if NGUI
			GUILogManager.LogInfo("On_MissionFailedNTF");
#else
			LogManager.Log_Debug("--- On_MissionFailedNTF "+ reason.GetString() +"---");
			_UI_CS_FightScreen.Instance.m_fightPanel.Dismiss();
#endif
		}
		
		// Param : missionID	mission ID
		// Param : returnCode	
		public void On_MissionFailAck(int missionID, int returnCode)
		{
			LogManager.Log_Debug("--- On_MissionFailAck "+ missionID.ToString() +"---");
			_UI_CS_FightScreen.Instance.m_fightPanel.Dismiss();
		}


		//点击某个有任务的NPC发请求
		// Param : MisssionID	任务ID,最后两位代表任务的等级
		// Param : NpcID	NPC ID
		public void On_conversationNpcCheckReq(int MisssionID, int NpcID){
			LogManager.Log_Debug("--- On_conversationNpcCheckReq ---");
		}
		
		public void On_BuyPetAck(int PetID, int Num, EServerErrorType reason){
			LogManager.Log_Debug("--- On_BuyPetAck: "+reason.GetString()+" ---");
#if NGUI
			if(reason.Get() == EServerErrorType.eSuccess) {
                CS_Main.Instance.g_commModule.SendMessage(
                            ProtocolGame_SendRequest.choosePet(PetID)
                    );
			}else {
				PopUpBox.PopUpErr(reason);
			}
#else
			if(0 == reason.Get()){
				_UI_Spirit3DmodelCtrl.Instance.Hide(_UI_CS_SpiritTrainer.Instance.IconIdx);
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
				_UI_CS_SpiritTrainer.Instance.spiritTrainerPanel.Dismiss();
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
				CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.choosePet(_UI_CS_SpiritTrainer_Cost.Instance.PetID)
				);
			}else{
				_UI_CS_PopupBoxCtrl.PopUpError(reason);
//				if(84 == reason.Get()){
//					_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = "Sorry, you don't have enough Karma Shards.";
//				}else{
//					_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = reason.GetString();
//				}
//				_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
			}
#endif
		}

	
		public void On_choosePetAck(int  PetID, EServerErrorType reason){
			LogManager.Log_Debug("--- On_choosePetAck: "+reason.GetString()+" ---");
#if NGUI
			if(reason.Get() == EServerErrorType.eSuccess)
			{
                PlayerDataManager.Instance.CurrentPetId = PetID;

                if (InGameScreenShopSpriteCtrl.Instance)
                {
                    InGameScreenShopSpriteCtrl.Instance.ChoosePetSuccess(PetID);
					Debug.Log("PET!!!!!!------>>>" + PlayerDataManager.Instance.GetPetIcon(PetID));
					if (Steamworks.activeInstance.isSteamWork)
					{
						Steam.Instance.Stats.SetAchievement(PlayerDataManager.Instance.GetPetIcon(PetID));
						Steam.Instance.Stats.StoreStats();
					}
                }
			}
#else
			if(0 == reason.Get()){
				_UI_Spirit3DmodelCtrl.Instance.Hide(_UI_CS_SpiritTrainer.Instance.IconIdx);
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
				_UI_CS_SpiritTrainer.Instance.spiritTrainerPanel.Dismiss();
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				SoundCue.PlayPrefabAndDestroy(_UI_CS_SpiritShopSound.Instance.BGMS[_UI_CS_SpiritTrainer.Instance.IconIdx]);
				Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
				Player.Instance.CallOnSpirit((SpiritBase.eSpiriteType)PetID);
			}
#endif
		}
		
		public void On_RolePetListAck(vectorSPetInfo petInfoVec){
            LogManager.Log_Debug("On_RolePetListAck : Get pets information.");
#if NGUI
            List<SPetInfo> info = new List<SPetInfo>();
            foreach (SPetInfo petinfo in petInfoVec){
                info.Add(petinfo);
            }
            Player.Instance.PetManager.ServerPetsInfo = info.ToArray();
            info.Clear();
            info = null;

            GUIManager.Instance.ChangeUIScreenState("Shop_Sprite_Screen");
#else
            Console.WriteLine("On_RolePetListAck");
			int idx = 0;
			_UI_CS_SpiritTrainer.Instance.InitSpiritItem();
            foreach (SPetInfo petinfo in petInfoVec){
				idx = _UI_CS_SpiritTrainer.Instance.SearchSpiritItem(petinfo.petTypeID);
				if(-1 != idx){
					_UI_CS_SpiritTrainer.Instance.SpiritList[idx].m_LeaveTime 		= petinfo.leftTime.ToString();
					_UI_CS_SpiritTrainer.Instance.SpiritList[idx].m_BuyTime 		= petinfo.buypetBegTime.ToString();
					_UI_CS_SpiritTrainer.Instance.SpiritList[idx].m_IsSummoned 		= true;
					_UI_CS_SpiritTrainer.Instance.SpiritList[idx].m_isShow 			= true;
				}
            }
			_UI_CS_SpiritTrainer.Instance.InitSpiritTrainer();
			_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
			_UI_CS_SpiritTrainer.Instance.AwakeSpiritTrainer();
#endif
		}

		//租期结束通知
		// Param : PetID	宠物ID
		public void On_PetRentTimeEndNTF(int PetID){
			Console.WriteLine("On_PetRentTimeEndNTF:{0}", PetID);
			LogManager.Log_Debug("--- On_PetRentTimeEndNTF ---");

            PlayerDataManager.Instance.CurrentPetId = -1;
			Player.Instance.CallOffSpirit((SpiritBase.eSpiriteType)PetID);
		}
		
	    public void On_exploretaskcompleteAck(int missionID, int taskid, int trackType, int objectID, EServerErrorType reason)
        {
#if NGUI
			GUILogManager.LogInfo("On_exploretaskcompleteAck");
			
#else
            Console.WriteLine("On_exploretaskcompleteAck:missionID{0}, TASKID{1}", missionID, taskid);
			LogManager.Log_Debug("--- On_exploretaskcompleteAck ---");
			if(0 == reason.Get()){
				LogManager.Log_Debug("--- ok ---");		
			}else{
				LogManager.Log_Debug("--- err : " + reason.GetString()  + " ---");
			}
#endif
        }
		
		public void On_conversationNpcCheckAck(int MisssionID, int taskid, int trackType, int objectID, int NpcID, EServerErrorType reason)
        {
            Console.WriteLine("On_exploretaskcompleteAck:missionID{0}, TASKID{1}", MisssionID, taskid);

        }
		
		  //角色免费宠物列表
        // Param : freePetInfoVec	宠物id
        public void On_RoleFreePetListNTF(vectorInt freePetInfoVec)
        {
            Console.WriteLine("On_RoleFreePetListNTF");
            foreach (int pet in freePetInfoVec)
            {
                Console.WriteLine("petID :{0}", pet);
            }
        }

        //加经验通知
        // Param : catolog	1是人 2是宠物
        // Param : val	经验值
        public void On_AddExpNTF(int catolog, ulong val)
        {
#if NGUI
			GUILogManager.LogInfo("On_AddExpNTF: exp"+ val);
            PlayerDataManager.Instance.CurrentExperience += (long)val;
#else
            Console.WriteLine("On_AddExpNTF val:{0}", val);
			LogManager.Log_Debug("On_AddExpNTF");	
			
			if(1 == catolog){
				
				_PlayerData.Instance.playerCurrentExp += (long)val;
				
			}
#endif	
        }
		
		public void On_assignTalentAck(EServerErrorType reason){
#if NGUI
			GUILogManager.LogInfo("On_assignTalentAck");
			if(reason.Get() == EServerErrorType.eSuccess) {
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.hasAssignedTalentPointReq());
			}else {
				PopUpBox.PopUpErr(reason);
			}
#else	
			//some time no res,i dont konw how to do.So dont wait it.
//			AssignTalentAckLogic();
			LogManager.Log_Info("On_assignTalentAck");
			if(reason.Get() == EServerErrorType.eSuccess) {
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.hasAssignedTalentPointReq()
				);
			}else {
				_UI_CS_PopupBoxCtrl.PopUpError(reason);
			}
#endif
        }
		
		public void On_ChangeCharacterAttribNTF(vectorAttrChange attrVec)
        {
#if NGUI
			GUILogManager.LogInfo("On_ChangeCharacterAttribNTF");
			Player.Instance.AttrMan.UpdateAttrs(attrVec);			
#else
			LogManager.Log_Debug("On_ChangeCharacterAttribNTF");
            Player.Instance.AttrMan.UpdateAttrs(attrVec);
			_PlayerData.Instance.UpdatePlayerInfo();
			MoneyBadgeInfo.Instance.UpDateBadge(0);	
#endif
        }
		
		//学习技能
		// Param : npcID	npc ID
		// Param : abilityID	技能ID
		public void On_StudyAbilitySucceed(int npcID, int abilityID)
		{
			Console.WriteLine("On_StudyAbilitySucced, abilityID:{0}", abilityID);
			LogManager.Log_Debug("On_StudyAbilitySucceed" + abilityID);	
			

			// move to cool finish.//ffaaf
//			_AbiMenuCtrl.Instance.ChangeExistAbiInfo(abilityID-1,abilityID);
//			_AbiMenuCtrl.Instance.CheckEquipAbilitie(abilityID);
//			_AbiMenuCtrl.Instance.InitAbiObjInfo(_AbiMenuCtrl.Instance.CurrentDisciplineType);
//			 PlayerAbilityManager _tempAbiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
//            _tempAbiManager.AddSkill(abilityID);
#if NGUI
            if (InGameScreenShopAbilityCtrl.Instance)
                InGameScreenShopAbilityCtrl.Instance.LearnAbilitySuccess(true, abilityID);
#else
			SoundCue.PlayPrefabAndDestroy(_UI_CS_ItemVendor.Instance.BuySound);
			AbilitiesShop.Instance.SetTab(AbilitiesShop.Instance.currentType);
#endif 
			
//			_UI_CS_AbilitiesTrainer.Instance.UpdateAbil(abilityID);
//			_UI_CS_AbilitiesTrainer.Instance.DismissAbiInfo();
		}

		
		public void On_StudyAbilityFailed(EServerErrorType errType, int npcID, int abilityID){
#if NGUI
            PopUpBox.PopUpErr(errType);
#else
			LogManager.Log_Debug("On_StudyAbilityFailed");		
			 _UI_CS_PopupBoxCtrl.PopUpError(errType);
#endif
		}
		
		public void On_SendShopIteminfo(bool isUpdateIn12, vectorShopItemInfos vectorShopItem){
#if NGUI
			GUILogManager.LogInfo("On_SendShopIteminfo");	
			PlayerDataManager.Instance.ClearItemShopList(false);
			foreach (SShopItemInfo item in vectorShopItem) {
				PlayerDataManager.Instance.AddItemShopData(item,false);
			}
			if(!isUpdateIn12) {
				PlayerDataManager.Instance.AddInitShopDataFlag();
			}
#else
			
			int i = 0;
			_UI_CS_ItemVendor.Instance.m_isReceiveItemList = true;
			_UI_CS_ItemVendor.Instance.m_ShopItemList.Clear();
			_UI_CS_ItemVendorSpecials.Instance.ClearList();
			_UI_CS_ItemVendor_1hWeapon.Instance.ClearList();
			_UI_CS_ItemVendor_2hWeapon.Instance.ClearList();
			_UI_CS_ItemVendor_Accessory.Instance.ClearList();
			_UI_CS_ItemVendor_Chest.Instance.ClearList();
			_UI_CS_ItemVendor_Cloak.Instance.ClearList();
			_UI_CS_ItemVendor_Head.Instance.ClearList();
			_UI_CS_ItemVendor_Legs.Instance.ClearList();
			foreach (SShopItemInfo item in vectorShopItem){
					i++;
					_UI_CS_ItemVendor.Instance.m_ShopItemList.Add(item);
					_UI_CS_ItemVendorItem itemS = new _UI_CS_ItemVendorItem();
					ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(item.ID,item.perfrab,item.gem,item.enchant,item.element,(int)item.level);
					if(null == tempItem){
						LogManager.Log_Warn("service send item info err ");
						LogManager.Log_Warn(item.ID.ToString());
						continue;
					}
					itemS.m_shopItem = item;
					itemS.m_type = tempItem._TypeID;
					itemS.m_iconID = item.perfrab;
					itemS.m_ID = tempItem._ItemID;
					itemS.m_count = item.leftBuyCount;
					itemS.uuid	  = item.UUID;
					itemS.moneyType = item.moneyType;
					if(itemS.m_iconID <1){
						LogManager.Log_Warn("service send item info prefabId < 1. ");
						LogManager.Log_Warn(i.ToString());
						continue;
					}
					itemS.m_name = tempItem.info_EleName + " " + tempItem.info_GemName + " " + tempItem.info_EleName + " " + tempItem.info_TypeName;
					itemS.m_Val = item.price;
					itemS.info = tempItem;
					if(0 != item.isDiscount){
						_UI_CS_ItemVendorSpecials.Instance.SpecialsItemList.Add(itemS);
					}else{
						switch(itemS.m_type){
							case 1:
									_UI_CS_ItemVendor_Head.Instance.AddElement(itemS);
									break;
							case 2:
							case 5:
									_UI_CS_ItemVendor_Accessory.Instance.AddElement(itemS);
								break;
							case 3:
									_UI_CS_ItemVendor_Chest.Instance.AddElement(itemS);
									break;
							case 4:
									_UI_CS_ItemVendor_Cloak.Instance.AddElement(itemS);
								break;
							case 6:
									_UI_CS_ItemVendor_Legs.Instance.AddElement(itemS);
								break;
							case 7:
									_UI_CS_ItemVendor_1hWeapon.Instance.AddElement(itemS);
									break;
							case 8:
									_UI_CS_ItemVendor_2hWeapon.Instance.AddElement(itemS);
								break;
	
							default:
									break;
						}
					}
            }
			_UI_CS_ItemVendorSpecials.Instance.InitItem();
			_UI_CS_ItemVendor_1hWeapon.Instance.InitItemList();
			_UI_CS_ItemVendor_2hWeapon.Instance.InitItemList();
			_UI_CS_ItemVendor_Accessory.Instance.InitItemList();
			_UI_CS_ItemVendor_Chest.Instance.InitItemList();
			_UI_CS_ItemVendor_Cloak.Instance.InitItemList();
			_UI_CS_ItemVendor_Head.Instance.InitItemList();
			_UI_CS_ItemVendor_Legs.Instance.InitItemList();
			if(!isUpdateIn12){
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
				_UI_CS_ItemVendor.Instance.AwakeItemVendor();
			}
#endif			
		}
		
		public void On_hasAssignedTalentPointAck(int prowess, int fortitude, int cunning,int leftTalentPoint){
#if NGUI
			GUILogManager.LogInfo("On_hasAssignedTalentPointAck");
			if(PlayerDataManager.Instance.CurLV > (prowess + fortitude + cunning + 1) && leftTalentPoint > 0) {
				GUIManager.Instance.ChangeUIScreenState("LevelUpScreen");
			}else {
                PlayerDataManager.Instance.UpdateCharacterInfoLogin();

				if(PlayerDataManager.Instance.EventsList.Count > 0) {
					GUIManager.Instance.ChangeUIScreenState("EventsScreen");
				}else {
					Player.Instance.ReactivePlayer();
       				GameCamera.BackToPlayerCamera();
					GUIManager.Instance.ChangeUIScreenState("IngameScreen");
				}
			}
#else
			LogManager.Log_Info("On_hasAssignedTalentPointAck");
			if((_PlayerData.Instance.playerLevel - prowess - cunning - fortitude - 1)>0&&leftTalentPoint>0){
				LogManager.Log_Info("leftTalentPoint : "+leftTalentPoint);
				levelUp.Instance.AwakeLevelUp();
			}else{
				levelUp.Instance.LeaveLevelUp();
			}
#endif
		}
		
		public void On_BuyItemSuccAck(int UID, SBuyitemInfo info){
#if NGUI
			GUILogManager.LogInfo("On_BuyItemSuccAck");
			//0 != UID --> rare else shop
			bool isSpeial = false;
			if(0 == UID) {
				isSpeial = false;
			}else {
				isSpeial = true;
			}
			PlayerDataManager.Instance.RemoveItemShopData(info,isSpeial);
			if(ItemShopScreenCtrl.Instance) {
				ItemShopScreenCtrl.Instance.BuyItemSuc(info);
			}
#else
            Console.WriteLine("On_BuyItemSuccAck {0}", UID);
			//0 != UID --> rare else shop
			if(0 == UID){
				#region normal shop
				_UI_CS_ItemVendor.Instance.PopUpBuyOkMenu();
				#region clear
				_UI_CS_ItemVendor.Instance.m_ShopItemList.Remove(_UI_CS_ItemVendor.Instance.BuyOkoInfo);
				_UI_CS_ItemVendorSpecials.Instance.ClearList();
				_UI_CS_ItemVendor_1hWeapon.Instance.ClearList();
				_UI_CS_ItemVendor_2hWeapon.Instance.ClearList();
				_UI_CS_ItemVendor_Accessory.Instance.ClearList();
				_UI_CS_ItemVendor_Chest.Instance.ClearList();
				_UI_CS_ItemVendor_Cloak.Instance.ClearList();
				_UI_CS_ItemVendor_Head.Instance.ClearList();
				_UI_CS_ItemVendor_Legs.Instance.ClearList();
				#endregion
				#region one by one creat item
				for(int i =0;i<_UI_CS_ItemVendor.Instance.m_ShopItemList.Count;i++){
					_UI_CS_ItemVendorItem itemS = new _UI_CS_ItemVendorItem();
					ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(_UI_CS_ItemVendor.Instance.m_ShopItemList[i].ID,
					                                                                _UI_CS_ItemVendor.Instance.m_ShopItemList[i].perfrab,
					                                                                _UI_CS_ItemVendor.Instance.m_ShopItemList[i].gem,
					                                                                _UI_CS_ItemVendor.Instance.m_ShopItemList[i].enchant,
					                                                                _UI_CS_ItemVendor.Instance.m_ShopItemList[i].element,
					                                                                (int)_UI_CS_ItemVendor.Instance.m_ShopItemList[i].level);
					if(null == tempItem){
						LogManager.Log_Warn("service send item info err ");
						continue;
					}
					itemS.m_shopItem = _UI_CS_ItemVendor.Instance.m_ShopItemList[i];
					itemS.m_type = tempItem._TypeID;
					itemS.m_iconID =  _UI_CS_ItemVendor.Instance.m_ShopItemList[i].perfrab;
					itemS.m_ID = tempItem._ItemID;
					itemS.m_name = tempItem.info_EleName + " " + tempItem.info_GemName + " " + tempItem.info_EleName + " " + tempItem.info_TypeName;
					itemS.m_Val =  _UI_CS_ItemVendor.Instance.m_ShopItemList[i].price;
					itemS.m_count = _UI_CS_ItemVendor.Instance.m_ShopItemList[i].leftBuyCount;
					itemS.moneyType = _UI_CS_ItemVendor.Instance.m_ShopItemList[i].moneyType;
					itemS.info = tempItem;
					if(0 != _UI_CS_ItemVendor.Instance.m_ShopItemList[i].isDiscount){
						_UI_CS_ItemVendorSpecials.Instance.SpecialsItemList.Add(itemS);
					}else{
						switch(itemS.m_type){
							case 1:
									_UI_CS_ItemVendor_Head.Instance.AddElement(itemS);
									break;
							case 2:
							case 5:
									_UI_CS_ItemVendor_Accessory.Instance.AddElement(itemS);
								break;
							case 3:
									_UI_CS_ItemVendor_Chest.Instance.AddElement(itemS);
									break;
							case 4:
									_UI_CS_ItemVendor_Cloak.Instance.AddElement(itemS);
								break;
							case 6:
									_UI_CS_ItemVendor_Legs.Instance.AddElement(itemS);
								break;
							case 7:
									_UI_CS_ItemVendor_1hWeapon.Instance.AddElement(itemS);
									break;
							case 8:
									_UI_CS_ItemVendor_2hWeapon.Instance.AddElement(itemS);
								break;
							default:
									break;
						}
					}
		        }
				#endregion
				#region init
				_UI_CS_ItemVendorSpecials.Instance.InitItem();
				_UI_CS_ItemVendor_1hWeapon.Instance.InitItemList();
				_UI_CS_ItemVendor_2hWeapon.Instance.InitItemList();
				_UI_CS_ItemVendor_Accessory.Instance.InitItemList();
				_UI_CS_ItemVendor_Chest.Instance.InitItemList();
				_UI_CS_ItemVendor_Cloak.Instance.InitItemList();
				_UI_CS_ItemVendor_Head.Instance.InitItemList();
				_UI_CS_ItemVendor_Legs.Instance.InitItemList();
				_ItemTips.Instance.DismissItemTip();
				#endregion
				#endregion
				_ItemTips.Instance.DismissItemTip();
			}else{
				#region rare shop
				//rare shop
				_UI_CS_ItemVendorRare.Instance.PopUpBuyOkMenu();		
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.RequestPlayerShopInfo(true)
				);
				#endregion
				_ItemTips.Instance.DismissItemTip();
			}	
			SoundCue.PlayPrefabAndDestroy(_UI_CS_ItemVendor.Instance.BuySound);
#endif
		}
		
		  //稀有物品列表
        // Param : vectorShopRareItem	稀有物品列表
//        public void On_SendShopRareitemTrackInfo(mapIntInt mapShopRareItemTrack, int wDay) {
////            Console.WriteLine("On_SendShopRareitemTrackInfo  wday {0}",wDay);
//			LogManager.Log_Info("receive shopRareItemInfo.");
//        }

        //每天凌晨更新稀有商店列表
  		public void On_UpdateRareShopListNTF(int wDay) {
            Console.WriteLine("On_UpdateRareShopListNTF {0}", wDay);

        }
		
		//稀有物品列表
        // Param : vectorShopRareItem	稀有物品列表

        public void On_SendShopRareitemTrackInfo(bool isUpdateIn12, vectorShopItemInfos vectorShopItem, int wDay) {
#if NGUI
			GUILogManager.LogInfo("On_SendShopRareitemTrackInfo");	
			PlayerDataManager.Instance.ClearItemShopList(true);
			foreach (SShopItemInfo item in vectorShopItem) {
				PlayerDataManager.Instance.AddItemShopData(item,true);
			}
			if(!isUpdateIn12) {
				if(PlayerDataManager.Instance.isUpdateRareShopItem) {
					if(ItemShopScreenCtrl.Instance) {
						ItemShopScreenCtrl.Instance.UpdateCurItemShopList();
					}
				}else {
					PlayerDataManager.Instance.AddInitShopDataFlag();
				}
			}
			PlayerDataManager.Instance.isUpdateRareShopItem= false;
#else
			LogManager.Log_Info("receive shopRareItemInfo.");
			int i = 0;
			RareShopList.Instance.ClearList();	
            foreach (SShopItemInfo item in vectorShopItem) {
				i++;
				_UI_CS_ItemVendorItem itemS 	= new _UI_CS_ItemVendorItem();	
				ItemDropStruct tempItem 		= ItemDeployInfo.Instance.GetItemObject(item.ID,item.perfrab,item.gem,item.enchant,item.element,(int)item.level);
				if(tempItem != null){
					itemS.m_shopItem 				= item;
					itemS.info				        = tempItem;
					itemS.m_type 					= tempItem._TypeID;
					itemS.m_iconID 					= item.perfrab;
					itemS.m_ID 						= tempItem._ItemID;
					itemS.m_name 					= tempItem.info_EleName + " " + tempItem.info_GemName + " " + tempItem.info_EleName + " " + tempItem.info_TypeName;
					itemS.m_Val 					= item.price;
					itemS.m_count 					= item.leftBuyCount;
					itemS.m_time  					= item.leftBuyTime;
					itemS.uuid						= item.UUID;
					itemS.moneyType					= item.moneyType;
				}else{
					LogManager.Log_Error("sever send err rare item. idx: "+i);
					continue;
				}
				if(itemS.m_iconID <1){
					LogManager.Log_Error("service send item info prefabId < 1. ");
					LogManager.Log_Error(i.ToString());
					continue;
				}
				RareShopList.Instance.infoList.Add(itemS);
            }
			RareShopList.Instance.InitItemList();
			_UI_CS_ItemVendorRare.Instance.AwakeItemVendor();
#endif
        }
		
		//crafting item Ok
		// Param : bagID :	slot type
        // Param : item slot : slot id
		// Param : iteminfo :	item info
        public void On_CraftingItemOK(byte bagID, byte itemSlot, SItemInfo iteminfo)
		{
			Debug.Log("On_CraftingItemOK");
			
#if NGUI
			if(bagID == 1)
			{
            	PlayerDataManager.Instance.SetEquipSoltChange((int)iteminfo.slot, true);
				PlayerDataManager.Instance.UpdateEquipSlot(iteminfo);
			}else if(bagID == 2)
			{
				PlayerDataManager.Instance.SetSoltIsChange((int)iteminfo.slot+1, true);
            	PlayerDataManager.Instance.UpdateBagSlot(iteminfo);
			}

            if (InGameScreenShopCraftingCtrl.Instance)
            {
                InGameScreenShopCraftingCtrl.Instance.CraftingOK();
            }
#else
			
			#region set bag item new info.
			ItemDropStruct temp = ItemDeployInfo.Instance.GetItemObject(iteminfo.ID,iteminfo.perfrab,iteminfo.gem,iteminfo.enchant,iteminfo.element,(int)iteminfo.level);
			Inventory.Instance.bagItemArray[CraftingWeaponPanel.Instance.GetSourceItemSlot()].m_ItemInfo = iteminfo;
			Inventory.Instance.bagItemArray[CraftingWeaponPanel.Instance.GetSourceItemSlot()].ItemStruct = temp;
			
			CraftingWeaponPanel.Instance.craftingItem.m_ItemInfo = iteminfo;
			CraftingWeaponPanel.Instance.craftingItem.ItemStruct = temp;
			#endregion
			CraftingWeaponPanel.Instance.craftingItem.m_ItemInfo = iteminfo;
			CraftingDetailPanel.Instance.animPanel.StartAnim(true);
#endif
		}

		//crafting item Failed
		// Param : reason	reason
		public void On_CraftingItemFailed (EServerErrorType reason)
		{
			Debug.Log("On_CraftingItemFailed " + reason.Get());
#if NGUI
			if (reason.Get() == EServerErrorType.eItemCraftError_CraftFailure && InGameScreenShopCraftingCtrl.Instance)
            {
                InGameScreenShopCraftingCtrl.Instance.CraftingFailed();
            }else
			{
				PopUpBox.PopUpErr(reason);
			}
#else
			CraftingDetailPanel.Instance.animPanel.StartAnim(false);
#endif
		}

		//crafting item Ok
		// Param : charID	charID
		// Param : nickname	nickname
		// Param : iteminfo	item info
		public void On_BroadCastCraftingItemOK (int charID, string nickname, SItemShowInfo iteminfo)
		{
		}
		
		public void On_masteryLvlupAck(EMasteryType masteryType, EServerErrorType returnCode) {
            Console.WriteLine("On_masteryLvlupAck ");
			Debug.Log("On_masteryLvlupAck : " + masteryType.Get() + " || " + returnCode.GetString());
#if NGUI
            if (InGameScreenShopAbilityCtrl.Instance && returnCode.Get() == EServerErrorType.eSuccess)
                InGameScreenShopAbilityCtrl.Instance.LearnAbilitySuccess(false, masteryType.Get());
			else
				PopUpBox.PopUpErr(returnCode);
#else
			if(returnCode.Get() != EServerErrorType.eSuccess){
           	 	_UI_CS_PopupBoxCtrl.PopUpError(returnCode);
				SoundCue.PlayPrefabAndDestroy(_UI_CS_PopupBoxCtrl.Instance.failSound);
			}else {
				SoundCue.PlayPrefabAndDestroy(_UI_CS_ItemVendor.Instance.BuySound);
			}
#endif
		}

        public void On_RoleMasteryListAck(vectorMasteryLevelInfo MasteryLevelInfoVec)
        {
            Console.WriteLine("On_RoleMasteryListAck ");
			if(MasteryInfo.Instance)
				MasteryInfo.Instance.ResetAllMasteriesInfo(MasteryLevelInfoVec);
			else
				Debug.LogWarning("There is no mastery info manager in the scene!!");
        }
		
	    public void On_UpdateBadge(int badgeNum)
		{
			MoneyBadgeInfo.Instance.UpdateBadgeNum(badgeNum);
			MoneyBadgeInfo.Instance.GetBadgeTime();
			_UI_CS_DebugInfo.Instance.SetBadge(badgeNum);
			
		}
		
		public void On_branchCompleteNTF(int MisssionID, int branchID, int taskID, int exp, int karma)
        {
            Console.WriteLine("On_branchCompleteNTF ");
			LogManager.Log_Info("On_branchCompleteNTF");

        }

		 //玩家下线发送发送任务进度
        public void On_SendMissionProcess(SMissionProcess missionProcess)
        {
			LogManager.Log_Info("On_SendMissionProcess");
//            Console.WriteLine("On_SendMissionProcess ");
			
			#region update materials
			MissionComplete.Instance.ResetMaterialList();
			foreach(int id in missionProcess.materialMap.Keys) {
				ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(id,1,0,0,0,1);
				if(tempItem != null) {
					MissionComplete.Instance.materialsList.Add(tempItem);
					MissionComplete.Instance.UpdateMatListForMissionComplete(tempItem,missionProcess.materialMap[id]);
				}
			}
			#endregion
			
			int branchIdx 		= 0;
			int TaskIdx 		= 0;
//			int TrackIdx 		= 0;
			int TrackIdx2 		= 0;
			_UI_CS_MissionLogic.Instance.MissionBgPanel.BringIn();
			//set mission
			MissionPanel.Instance.currentMissionID = (int)missionProcess.missionID;	
			_UI_CS_MissionLogic.Instance.InitMissionList((int)missionProcess.missionID);
			
			foreach (SBranch branch in missionProcess.SBranchVec)
			{
				
				TaskIdx = 0;
				
				if(1 == branch.isBranchComplete){
	
					_UI_CS_MissionLogic.Instance.BranchList.RemoveAt(0);		
					
					
				}else{
					
					foreach (STask task in branch.STaskVector)
					{
						
//						TrackIdx  = 0;
						TrackIdx2 = 0;
						
						if(1 == task.isTaskComplete){
							
							_UI_CS_MissionLogic.Instance.BranchList[branchIdx].taskArray.RemoveAt(0);
							TaskIdx--;
							
						}else{
						
							foreach (STrack track in task.STrackVector)
							{
								
								if(1 == track.isTrackComplete){
									
									//this code have risk.if client list element will less and less. so if find element , function will break.
									//why todo it.becaus server if change will use more time.so...
									for(int i = 0;i <_UI_CS_MissionLogic.Instance.BranchList[branchIdx].taskArray[TaskIdx].SubObject.Count;i++) {
										if(_UI_CS_MissionLogic.Instance.BranchList[branchIdx].taskArray[TaskIdx].SubObject[i].objectID == track.objectID && 
											(int)_UI_CS_MissionLogic.Instance.BranchList[branchIdx].taskArray[TaskIdx].SubObject[i].typeID == track.trackType) {
											_UI_CS_MissionLogic.Instance.BranchList[branchIdx].taskArray[TaskIdx].SubObject.RemoveAt(i);
//											TrackIdx--;
											break;
										}
									}
				
								}else{
									
									for(int i = 0;i <_UI_CS_MissionLogic.Instance.BranchList[branchIdx].taskArray[TaskIdx].SubObject.Count;i++) {
										if(_UI_CS_MissionLogic.Instance.BranchList[branchIdx].taskArray[TaskIdx].SubObject[i].objectID == track.objectID && 
											(int)_UI_CS_MissionLogic.Instance.BranchList[branchIdx].taskArray[TaskIdx].SubObject[i].typeID == track.trackType) {
											_UI_CS_MissionLogic.Instance.BranchList[branchIdx].taskArray[TaskIdx].SubObject[i].CurrentVal = track.nowCount;
											break;
										}
									}
		
								}
								
//								TrackIdx++;
								TrackIdx2++;
							}
//							
						
						}
						
						TaskIdx++;
						
					}
					
					branchIdx++;
				}
			}
			
			//生成任务UI界面
			_UI_CS_MissionLogic.Instance.UpdateMissionUI();
			
        }
        public void On_AcceptMissionFail(int returnCode) {
#if NGUI
			GUILogManager.LogErr("On_AcceptMissionFail: "+returnCode);
			PopUpBox.PopUpErr("Accep Mission fial. plaease Contact YKF and YMW.");
#else
			LogManager.Log_Warn("On_AcceptMissionFail"+ returnCode);
			
//			_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = "err code: " + returnCode.ToString();
//			_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
#endif
        }
		
		//
		// Param : charN--ame	char name
		// Param : missionName	mission name
		public void On_UserReconnectNotify(string charName, string missionName)
		{
#if NGUI
			GUILogManager.LogInfo("On_UserReconnectNotify. CharaName: "+charName+"|MissionName: "+missionName);
			CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.UserReconnect(false));	
#else
			LogManager.Log_Info("On_UserReconnectNotify");
			
			if(_UI_CS_TestModeCtrl.Instance.IsTestMode){
				
				CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.UserReconnect(false)
				);	

			}else{	
				if(string.Compare(missionName,"")!=0) {
					_UI_CS_Ctrl.Instance.m_isReconnect = true;
					_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.Dismiss();
					_UI_CS_Login.Instance.m_ReconnectPanel.BringIn();
					_UI_CS_Login.Instance.CharaName.Text = charName;
					_UI_CS_Login.Instance.Mission.Text = missionName;
				}else {
					CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.UserReconnect(false)
					);
					_UI_CS_Ctrl.Instance.m_isReconnect = false;
				}
			}
#endif			
		}
		
		public void On_ChatRequestAck(EChatType chatType, int targetID, int itemID, string chatMsg, int eReturncode){
            Console.WriteLine("On_ChatRequestAck ");
			LogManager.Log_Info("On_ChatRequestAck");
        }
		
		//leaderboard
		// Param : leaderboardType	
		// Param : leaderboardInfo	
		public void On_UpdateLeaderboardInfo(ELeaderboardType leaderboardType, SLeaderboardTable leaderboardInfo, bool isClientGet)
		{
			LogManager.Log_Info("On_UpdateLeaderboardInfo");
		}
		
		// Param : elementAtrrVec	element attr vec
		public void On_UpdateElementAttr(vectorElementValue elementAtrrVec)
		{
            Player.Instance.AttrMan.UpdateEleAttrs(elementAtrrVec);
		}

		public void On_UpdateElementChance(vectorElementValue elementAttrVec)
		{
			Player.Instance.AttrMan.UpdateEleChanceAttrs(elementAttrVec);
		}
		
		public void On_SendFriendRewardInfo(mapFriendHireReward rewardInfo)
		{
#if NGUI
			Debug.Log ("DEBUG :: Condition <<0>>");
			GUILogManager.LogInfo("On_SendFriendRewardInfo");
			PlayerDataManager.Instance.summonReward = rewardInfo;
			if(rewardInfo.Count != 0) {
				GUIManager.Instance.ChangeUIScreenState("SummonRewrdScreen");
				Debug.Log ("DEBUG :: Condition <<1>>");
			}else {
				if(PlayerDataManager.Instance.GetMissionCompleteFlag()) {
					GUIManager.Instance.ChangeUIScreenState("SuccessScreen");
					Debug.Log ("DEBUG :: Condition <<2>>");
				}else {
					CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.hasAssignedTalentPointReq());
					Debug.Log ("DEBUG :: Condition <<3>>");
				}
				Debug.Log ("DEBUG :: Condition <<4>>");
			}
#else
			LogManager.Log_Info("On_SendFriendRewardInfo");
			SummonRewardPanel.Instance.AwakeSummonReward(rewardInfo);
#endif
		}
		
		public void On_SendShopConsumeIteminfo(bool isUpdateIn12, vectorShopConsumeItemInfos vectorShopConsumeItem)
        {
            Console.WriteLine("On_SendShopConsumeIteminfo");
        }
		
		public void On_SendShopConsumeIteminfo(bool isUpdateIn12, int shopType,  vectorShopConsumeItemInfos vectorShopConsumeItem)
        {
            Console.WriteLine("On_SendShopConsumeIteminfo shopType", shopType);
        }

        public void On_BuyConsumeItemSuccAck(int itemType, int itemID, int buycount)
        {
			LogManager.Log_Info("On_BuyConsumeItemSuccAck itemType : "+itemType.ToString());
#if NGUI
			if (InGameScreenShopCraftingCtrl.Instance) InGameScreenShopCraftingCtrl.Instance.BuyMaterialSuc(itemType);
            if (InGameScreenShopConsumableItemCtrl.Instance) InGameScreenShopConsumableItemCtrl.Instance.BuyItemSuccess(itemType, itemID, buycount);
#else
			_UI_CS_Consumable.Instance.CostPanel.Dismiss();
			_UI_CS_Consumable.Instance.ThanksPanel.BringIn();
			CraftingDetailPanel.Instance.UpdateControlPanel();
#endif
        }
		
		public void On_UpdateCacheMoneyInfo(SMoneyInfo money)
        {
			LogManager.Log_Info("On_UpdateCacheMoneyInfo Karma:{0} FK:{1}"+money.Karma.ToString()+":"+ money.FK.ToString());
			//this function is on us.//
			_UI_CS_MissionLogic.Instance.MissionKarma = money.Karma;
			
        }
		
		//update base attributes
		// Param : attrVec	base attr vec
		public void On_UpdateBaseAttr(vectorAttrChange attrVec) {
#if NGUI
			GUILogManager.LogInfo("On_UpdateBaseAttr");
			foreach (SAttributeChange c in attrVec){
				PlayerDataManager.Instance.SetBaseAttrs(c.attributeType.Get(),c.value);
			}
#else
			LogManager.Log_Info("On_UpdateBaseAttr");
			 foreach (SAttributeChange c in attrVec){
	            _PlayerData.Instance.BaseAttrs[c.attributeType.Get()] = c.value;
	        }
#endif
		}
		
		public void On_MissionAreaInfoAck(vectorSMissionAreaRelate SMissionAreaRelateVec){
//			_UI_CS_MissionMaster.Instance.UpdateMapLockState(SMissionAreaRelateVec);
//			_UI_CS_MissionMaster.Instance.initMissionMap();
//			_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
			//---------------------------------------------------------------------------------->>mm
			#if NGUI
			
			#else
			//for (int i = 0; i< SMissionAreaRelateVec.Capacity; i++){
				foreach (SMissionAreaRelate stat in SMissionAreaRelateVec){
					if (stat.areaID == 5100){
						if (stat.isBuy == true){
							if (GameObject.Find("UI manager").gameObject.GetComponent<isFacebookBuild>().isFacebook == true){
								isFacebookBuild.NeedToPay = true ;
							Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ status is : ["+ stat.isBuy + "] for the area ID is : [" + stat.areaID +"]");
							}
						}
					}else{
						if (GameObject.Find("UI manager").gameObject.GetComponent<isFacebookBuild>().isFacebook == true){
							Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ MONEY ALREADY PAID ");
						}else{
						
						}
					}
				}
			//}
			Debug.Log ("Count is : [" + SMissionAreaRelateVec.Capacity + "]");
			#endif
			//---------------------------------------------------------------------------------->>#mm
        }
		
		public void On_unLockAreaAck(int areaID, EServerErrorType returnCode){
//			if(0 == returnCode.Get()){
//				UnlockArea.Instance.basePanel.Dismiss();
//				CS_Main.Instance.g_commModule.SendMessage(
//			   		ProtocolGame_SendRequest.MissionListReq()
//												 );
//			}else{
//				_UI_CS_PopupBoxCtrl.PopUpError(returnCode);
//			}
        }
		
		//update char stat data
		// Param : statData	stat data
		public void On_UpdateCharStatData(SCharStatData statData){

		}
		
		 public void On_TutorialEnterAck(int charID, int returnCode){
           Console.WriteLine("On_unLockAreaAck "); 
			
        }
        public void On_UpdateTutorialInfo(bool istutorial){
            Console.WriteLine("On_UpdateTutorialInfo ");
        }
		
		//cheat
		public void On_YouCheat(){
			_UI_CS_Ctrl.Instance.m_isCheat = true;
		}
		
		public void On_buyThreatLevelByKarmaAck(EServerErrorType returnCode, int missionID, int subOrAdd, int nextThreatTime){
//			ThreatTips.Instance.DismissTip();
//			if(returnCode.Get() == EServerErrorType.eSuccess){
//				_UI_CS_MissionLevelSelect.Instance.ChangeMissionThreat(MissionPanel.Instance.currentMissionID+_UI_CS_MissionLevelSelect.Instance.threatChangVal,nextThreatTime);
//			}else{
//				_UI_CS_PopupBoxCtrl.PopUpError(returnCode);
//			}
        }
		
		public void On_buyAllAreaByKarmaAck(EServerErrorType returnCode){
        }
		
		public void On_GetGiftAck(SGift gift, EServerErrorType result){
			LogManager.Log_Info("On_GetGiftAck: "+ gift.karma + " result :" + result.Get());
#if NGUI
            if (result.Get() == EServerErrorType.eSuccess)
            {
                string str = LocalizeManage.Instance.GetDynamicText("YOURECEIVE");
                str += gift.karma;
                PopUpBox.PopUpErr(str);
            }
            else
                PopUpBox.PopUpErr(result);
#else
            if(result.Get() == EServerErrorType.eSuccess){
				LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"YOURECEIVE");
				_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text += gift.karma;
//				OptionCtrl.Instance.giftMsg.Text += gift.karma;
				_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();	
			}else{
//				LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"INVALIDCODE");
				_UI_CS_PopupBoxCtrl.PopUpError(result);
			}
#endif
        }
		
		public void On_pingRequestAck(){
#if NGUI
			if(InGameScreenCtrl.Instance.inventoryCtrl) {
				InGameScreenCtrl.Instance.inventoryCtrl.UpdatePingRender();
			}
#else
			_UI_CS_FightScreen.Instance.ChangePingState();
#endif
        }
		
		//
		// Param : reason	
		public void On_BuyReviveItemFailed(EServerErrorType reason)
		{
#if NGUI
            PopUpBox.PopUpErr(reason);
#else
                _UI_CS_PopupBoxCtrl.PopUpError(reason);
#endif
		}

		//
		// Param : num	
		public void On_BuyReviveItemSucc(int num)
		{

		}
		
		//update revive num
		// Param : num	
		public void On_UpdateReviveNum(int num)
		{
#if NGUI
			GUILogManager.LogInfo("On_UpdateReviveNum");
			PlayerDataManager.Instance.SetReviveCount(num);
#else
			_PlayerData.Instance.playerReviveItemNum = num;
			_PlayerData.Instance.playerReviveItemNumText.Text = num.ToString();
			RevivePanel.Instance.UpdateRevivalItemCount(num);
#endif
		}
		
		public void On_NewRedeemGift(SRedeemGift gift){
			LogManager.Log_Warn("On_NewRedeemGift gifID:"+gift.title);
			TakeGiftPanel.Instance.giftItemList.Add(gift);
			TakeGiftPanel.Instance.InitGiftList();
			TakeGiftPanel.Instance.AwakeTakeGiftPanel();
		}
		
		public void On_GetRedeemGiftResult(mapRedeemGift gift){
			LogManager.Log_Warn("On_GetRedeemGiftResult count:"+gift.Count);
			TakeGiftPanel.Instance.ClearList();
			foreach (SRedeemGift giftInfo in gift.Values){
				TakeGiftPanel.Instance.giftItemList.Add(giftInfo);
			}
			if(gift.Count > 0){
				TakeGiftPanel.Instance.ShowGiftIcon(true);
			}
			TakeGiftPanel.Instance.InitGiftList();
		}
		
		public void On_ProcessRedeemGiftResult(SRedeemGift gift , EServerErrorType err){
			LogManager.Log_Warn("On_ProcessRedeemGiftResult err:"+err.ToString());
			if(err.Get() == EServerErrorType.eSuccess){
				for(int i = 0;i<TakeGiftPanel.Instance.giftItemList.Count;i++) {
					if(string.Compare(TakeGiftPanel.Instance.giftItemList[i].redeemCode,gift.redeemCode) == 0) {
						TakeGiftPanel.Instance.giftItemList.Remove(TakeGiftPanel.Instance.giftItemList[i]);
						break;
					}
				}
				
			}else{
#if NGUI
                PopUpBox.PopUpErr(err);
#else
                _UI_CS_PopupBoxCtrl.PopUpError(err);
#endif
			}
			TakeGiftPanel.Instance.InitGiftList();
		}
		
		//get well data result
		// Param : wellData	well data
		// Param : curTime	current time
		public void On_GetWellDataResult(SWellData wellData, ulong curTime)
		{
            ShopNpc _wellnpc = NpcInfo.GetNPCByType(NpcInfo.NPCType.WELL);
            NPC_Well _well = (NPC_Well)_wellnpc;
			_well.LastGetDataTime = curTime;
            _well.WellData = wellData;
		}

		//upgrade well result
		// Param : wellType	well type
		// Param : level	level after upgrade
		// Param : result	result type
		public void On_UpgradeWellResult(EWellType wellType, int level, EServerErrorType result)
		{

		}

		//get well karma result
		// Param : result	result type
		public void On_GetWellKarmaResult(EServerErrorType result) {
            if (result.Get() == EServerErrorType.eSuccess) {
                ShopNpc _wellnpc = NpcInfo.GetNPCByType(NpcInfo.NPCType.WELL);
                NPC_Well _well = (NPC_Well)_wellnpc;
                _well.GetKarmaSuccess();
            }else {
#if NGUI
				PopUpBox.PopUpErr(result);
#else
                _UI_CS_PopupBoxCtrl.PopUpError(result);
#endif
            }
		}
		
		public void On_SelectDayRewardNTF(int DayRewardNum)
        {
#if NGUI
			PlayerDataManager.Instance.dailyRewarType = DayRewardNum;
			Debug.Log("dayreword idx : " + DayRewardNum);
#else
           DailyRewardGift.Instance.dailyRewardFileIndex = DayRewardNum;
#endif

        }
		
		//change name result
		// Param : newName	new name
		// Param : result	result
		public void On_ChangeNameResult(int charID, string newName, EServerErrorType result)
		{

		}

		//change gender result
		// Param : sex	gender
		// Param : result	result
		public void On_ChangeGenderResult(int charID, int sex, EServerErrorType result)
		{

		}
		
		public void On_decreaseCoolDownAck(EServerErrorType result, int missionID, int bType) {
			LogManager.Log_Warn("On_decreaseCoolDownAck result:"+result.Get().ToString());
			if(result.Get() == EServerErrorType.eSuccess) {
				MissionPanel.Instance.ResetCoolTime(missionID,bType);
				MissionSelect.Instance.coolDownFinishPanel.Dismiss();
			}else {
#if NGUI
                PopUpBox.PopUpErr(result);
#else
                _UI_CS_PopupBoxCtrl.PopUpError(result);
#endif
			}
        }
		
		// Param : result	result
		// Param : mailInfo	mail info
		public void On_SendMailResult(EServerErrorType result, SMailInfo mailInfo)
		{
			if(result.Get() != EServerErrorType.eSuccess) {
#if NGUI
                PopUpBox.PopUpErr(result);
#else
                _UI_CS_PopupBoxCtrl.PopUpError(result);
#endif
			}
			MailSystem.Instance.UpdateMailExpireTime();
		}

		//GetMailMoneyResult
		// Param : mailID	
		// Param : result	result
		public void On_GetMailMoneyResult(int mailID, EServerErrorType result)
		{
			if(result.Get() != EServerErrorType.eSuccess) {
#if NGUI
                PopUpBox.PopUpErr(result);
#else
                _UI_CS_PopupBoxCtrl.PopUpError(result);
#endif
			}
			MailSystem.Instance.UpdateMailExpireTime();
		}

		// Param : vectorMailInfo	mail info vector
		public void On_RecvNewMail(vectorMailInfos vectorMailInfo)
		{
			LogManager.Log_Info("On_RecvNewMail");

#if NGUI
            List<SMailInfo> mails = new List<SMailInfo>();
            foreach (SMailInfo mail in vectorMailInfo)
            {
                mails.Add(mail);
            }
            PlayerDataManager.Instance.MailList = mails.ToArray();
            mails.Clear();
            mails = null;
#else
//			MailSystem.Instance.ResetMailSystem();
			foreach (SMailInfo mail in vectorMailInfo){
				MailSystem.Instance.mailList.Add(mail);
			}
			MailSystem.Instance.InitMailInfo();
			MailSystem.Instance.UpdateMailExpireTime();
#endif
		}

		// Param : vectorMailInfo	mail info vector
		public void On_GetPlayerMailResult(vectorMailInfos vectorMailInfo)
		{
			LogManager.Log_Info("On_GetPlayerMailResult");

#if NGUI
            List<SMailInfo> mails = new List<SMailInfo>();
            foreach (SMailInfo mail in vectorMailInfo)
            {
                mails.Add(mail);
            }
            PlayerDataManager.Instance.MailList = mails.ToArray();
            mails.Clear();
            mails = null;

            if (UI_Mailbox_Manager.Instance){
                UI_Mailbox_Manager.Instance.UpdateList(PlayerDataManager.Instance.MailList);
				Debug.Log ("UI_Mailbox_Manager.Instance");	
			}
#else
			foreach (SMailInfo mail in vectorMailInfo){
				MailSystem.Instance.mailList.Add(mail);
			}
			MailSystem.Instance.InitMailInfo();
			MailSystem.Instance.UpdateMailExpireTime();
#endif
		}

		//DeleteMail
		// Param : result	result
		// Param : mailID	id
		public void On_DeleteMailResult(EServerErrorType result, int mailID)
		{
			LogManager.Log_Info("On_DeleteMailResult");
#if NGUI
            if (result.Get() == EServerErrorType.eSuccess)
            {
                SMailInfo info = null;
                List<SMailInfo> list = new List<SMailInfo>(PlayerDataManager.Instance.MailList);
                foreach (SMailInfo mail in list)
                    if (mail.id == mailID)
                        info = mail;
                
                list.Remove(info);

                PlayerDataManager.Instance.MailList = list.ToArray();


                if (UI_Mailbox_Manager.Instance)
                    UI_Mailbox_Manager.Instance.UpdateList(PlayerDataManager.Instance.MailList);
            }
            else
            {
                PopUpBox.PopUpErr(result);
            }
#else
			if(result.Get() == EServerErrorType.eSuccess) {
				MailLeftPanel.Instance.DelMail(mailID);
			}else {
				_UI_CS_PopupBoxCtrl.PopUpError(result);
			}
			MailSystem.Instance.UpdateMailExpireTime();
#endif
		}

		public void On_missionShopNTF(vectorShopItemInfos missionShopItemVec)
        {

        }
		
		//DeleteMail
		// Param : result	result
		// Param : mailIDVec	id vector
		public void On_DeleteMailResult(EServerErrorType result, vectorInt mailIDVec) {
			LogManager.Log_Info("On_DeleteMailResult");
#if NGUI
            if (result.Get() == EServerErrorType.eSuccess) 
            {
                if (UI_Mailbox_Manager.Instance)
                {
                    SMailInfo info = null;
                    List<SMailInfo> list = new List<SMailInfo>(PlayerDataManager.Instance.MailList);

                    foreach (int i in mailIDVec)
                    {
                        foreach (SMailInfo mail in list)
                            if (mail.id == i)
                                info = mail;

                        list.Remove(info);                       
                    }

                    PlayerDataManager.Instance.MailList = list.ToArray();
                    UI_Mailbox_Manager.Instance.UpdateList(PlayerDataManager.Instance.MailList);
                }
            }
            else
            {
                PopUpBox.PopUpErr(result);
            }		
#else
            if (result.Get() == EServerErrorType.eSuccess) 
            {
                MailLeftPanel.Instance.DelMail(mailIDVec);
            }
            else
            {
                _UI_CS_PopupBoxCtrl.PopUpError(result);
            }
			MailSystem.Instance.UpdateMailExpireTime();
#endif
		}
		
		//GetMailAllItemsResult
		// Param : mailID	mail ID
		// Param : result	result
		public void On_GetMailAllItemsResult(int mailID, EServerErrorType result)
		{
			LogManager.Log_Info("On_GetMailAllItemsResult");
//			MailRightPanel.Instance.DelAllItem();
//			MailSystem.Instance.GetMailList();
			if(result.Get() != EServerErrorType.eSuccess) {
#if NGUI
                PopUpBox.PopUpErr(result);
#else
                _UI_CS_PopupBoxCtrl.PopUpError(result);
#endif
			}
			MailLeftPanel.Instance.GetAllItem(mailID);
			MailSystem.Instance.UpdateMailExpireTime();
		}

		//GetMailAllResult
		// Param : mailID	mail ID
		// Param : result	result
		public void On_GetMailAllResult(int mailID, EServerErrorType result)
		{
			LogManager.Log_Info("On_GetMailAllResult");

#if NGUI
            if (result.Get() == EServerErrorType.eSuccess)
            {
                if (UI_Mailbox_Manager.Instance)
                    UI_Mailbox_Manager.Instance.GetAllSuc(mailID);
            }
            else
            {
                PopUpBox.PopUpErr(result);
            }
#else
			if(result.Get() == EServerErrorType.eSuccess) {
				MailLeftPanel.Instance.GetAllItem(mailID);
				MailRightPanel.Instance.DelAllItem();
//				MailSystem.Instance.GetMailList();
			}else {
				_UI_CS_PopupBoxCtrl.PopUpError(result);
			}
			
			MailSystem.Instance.UpdateMailExpireTime();
#endif
		}

        //GetMailKarmaResult
        // Param : mailID	
        // Param : result	result
        public void On_GetMailKarmaResult(int mailID, EServerErrorType result)
        {
            LogManager.Log_Info("On_GetMailKarmaResult");

#if NGUI
            if (result.Get() == EServerErrorType.eSuccess)
            {
                if (UI_Mailbox_Manager.Instance)
                    UI_Mailbox_Manager.Instance.GetKarmaSuc(mailID);
            }
            else
            {
                PopUpBox.PopUpErr(result);
            }
#else
            if(result.Get() == EServerErrorType.eSuccess) {
				MailRightPanel.Instance.DelItem(0,MAILITEMType.KARMA);
            }else {
				_UI_CS_PopupBoxCtrl.PopUpError(result);
			}
			MailSystem.Instance.UpdateMailExpireTime();
#endif

        }

        //GetMailcrystalResult
        // Param : mailID	
        // Param : result	result
        public void On_GetMailCrystalResult(int mailID, EServerErrorType result)
        {
            LogManager.Log_Info("On_GetMailCrystalResult");

#if NGUI
            if (result.Get() == EServerErrorType.eSuccess)
            {
                if (UI_Mailbox_Manager.Instance)
                    UI_Mailbox_Manager.Instance.GetCrystalSuc(mailID);
            }
            else
            {
                PopUpBox.PopUpErr(result);
            }
#else

			if(result.Get() == EServerErrorType.eSuccess) {
				MailRightPanel.Instance.DelItem(0,MAILITEMType.CRYSTAL);
			}else {
				_UI_CS_PopupBoxCtrl.PopUpError(result);
			}
			MailSystem.Instance.UpdateMailExpireTime();
#endif
        }

        // Param : result	result
        // Param : mailID	mail ID
        // Param : itemSlotID	mail item slot ID
        public void On_GetMailItemResult(EServerErrorType result, int mailID, int itemSlotID)
        {
            LogManager.Log_Info("On_GetPlayerMailResult");

#if NGUI
            if (result.Get() == EServerErrorType.eSuccess)
            {
                if (UI_Mailbox_Manager.Instance)
                    UI_Mailbox_Manager.Instance.GetMailItemSuc(mailID, itemSlotID);
            }
            else
            {
                PopUpBox.PopUpErr(result);
            }
#else
            if (result.Get() == EServerErrorType.eSuccess)
            {
				MailRightPanel.Instance.DelItem(itemSlotID,MAILITEMType.ITEM);
            }
            else
            {
                _UI_CS_PopupBoxCtrl.PopUpError(result);
            }
#endif
        }
		
		// Param : result	result
		// Param : mailID	mail ID
		// Param : petSlotID	mail pet slot ID
		public void On_GetMailPetResult(EServerErrorType result, int mailID, int petSlotID)
		{
			LogManager.Log_Info("On_GetMailPetResult");
#if NGUI
            if (result.Get() == EServerErrorType.eSuccess)
            {
                if (UI_Mailbox_Manager.Instance)
                    UI_Mailbox_Manager.Instance.GetPetSuc(mailID, petSlotID);
            }
            else
            {
                PopUpBox.PopUpErr(result);
            }
#else
			if(result.Get() == EServerErrorType.eSuccess) {
				MailRightPanel.Instance.DelItem(petSlotID,MAILITEMType.PET);
			}else {
				_UI_CS_PopupBoxCtrl.PopUpError(result);
			}
			MailSystem.Instance.UpdateMailExpireTime();
#endif
		}

		public void On_MaterialMailNotify()
		{
		}
		
		// Param : result	result
		public void On_BuyCharSlotResult(EServerErrorType result)
		{
			if(result.Get() != EServerErrorType.eSuccess) {
#if NGUI
                PopUpBox.PopUpErr(result);
#else
                _UI_CS_PopupBoxCtrl.PopUpError(result);
#endif
			}
		}
		
		public void On_buyStashAck(int stashNum, EServerErrorType result)
        {
#if NGUI
			GUILogManager.LogInfo("On_buyStashAck");
			if(result.Get() == EServerErrorType.eSuccess) {
				PlayerDataManager.Instance.SetStashMaxTabForCount(stashNum);
				if(StashManager.Instance) {
                    StashManager.Instance.BuyStashSuccess();
				}
			}else {
                if (StashManager.Instance)
                {
                    StashManager.Instance.BuyStashFailed();
                }
				PopUpBox.PopUpErr(result);
			}
#else
          	LogManager.Log_Info("On_buyStashAck");
			if(result.Get() == EServerErrorType.eSuccess) {
				Stash.Instance.SetMaxTab((stashNum));
				Stash.Instance.UpdateTabState();
				Stash.Instance.DismissBuyBoxPanel();
			}else {
				_UI_CS_PopupBoxCtrl.PopUpError(result);
			}
#endif		
		}
		// Param : abilityID	
		public void On_AddNewAbilityNotify(int abilityID) {
			LogManager.Log_Info("On_AddNewAbilityNotify : " + abilityID);
#if NGUI
			PlayerAbilityManager _tempAbiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _tempAbiManager.AddSkill(abilityID);
            if (InGameScreenShopAbilityCtrl.Instance) InGameScreenShopAbilityCtrl.Instance.AbilityLevelUp(abilityID);
#else
			_AbiMenuCtrl.Instance.ChangeExistAbiInfo(abilityID-1,abilityID);
			_AbiMenuCtrl.Instance.CheckEquipAbilitie(abilityID);
			_AbiMenuCtrl.Instance.InitAbiObjInfo(_AbiMenuCtrl.Instance.CurrentDisciplineType);
			 PlayerAbilityManager _tempAbiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _tempAbiManager.AddSkill(abilityID);
#endif
		}
		
		//
		// Param : cooldownType	cooldown type
		// Param : id	id
		// Param : targetTime	
		public void On_UpdateCoolDown(ECooldownType cooldownType, int id, long targetTime) {
			LogManager.Log_Info("On_UpdateCoolDown");
#if NGUI
			if(PlayerDataManager.Instance) PlayerDataManager.Instance.SetAbiCoolDownList(cooldownType.Get(),id,targetTime);
			if(InGameScreenShopAbilityCtrl.Instance) InGameScreenShopAbilityCtrl.Instance.CooldownUpdated(cooldownType, id, targetTime);
#else
			AbilitiesShop.Instance.SetAbiCoolDownList(cooldownType.Get(),id,targetTime);
			AbilitiesShop.Instance.leftPanel.Dismiss();
#endif
		}
		
		//speed up cooldown result
		// Param : result	result
		// Param : cooldownType	cooldown type
		// Param : id	id
		// Param : targetTime	
		public void On_SpeedUpCoolDownResult(EServerErrorType result, ECooldownType cooldownType, int id, long targetTime) {
			LogManager.Log_Info("On_SpeedUpCoolDownResult : " + id + " || " + targetTime);
#if NGUI
			if(result.Get() == EServerErrorType.eSuccess) {
                if (InGameScreenShopAbilityCtrl.Instance) InGameScreenShopAbilityCtrl.Instance.CooldownUpdated(cooldownType, id, targetTime);
			}else{
				PopUpBox.PopUpErr(result);
			}
#else
			if(result.Get() == EServerErrorType.eSuccess) {
				AbilitiesShop.Instance.leftPanel.Dismiss();
				AbilitiesShop.Instance.speedUpPanel.Dismiss();
				AbilitiesShop.Instance.SetTab(AbilitiesShop.Instance.currentType);
			}else {
				_UI_CS_PopupBoxCtrl.PopUpError(result);
			}
#endif
		}
		
		public void On_AddNewMasteryNotify(int id) {
			LogManager.Log_Info("On_AddNewMasteryNotify : " + id);
            // request to renew all mastery
            CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.RoleMasteryListReq());
            MasteryInfo.Instance.MasteryLevelUp(id);
            if (InGameScreenShopAbilityCtrl.Instance) InGameScreenShopAbilityCtrl.Instance.MasteryLevelup(id);
		}
		
		// Param : curTime	current system time
		public void On_ServerSystemTimeNotify(long curTime)
		{
#if NGUI
			long t = PlayerDataManager.Instance.Update1970OffestTime();
			PlayerDataManager.Instance.offest1970Time = curTime-t;
#else
			long t = _PlayerData.Instance.Update1970OffestTime();
			_PlayerData.Instance.offest1970Time = curTime-t;
#endif
		}
		
    }
}
