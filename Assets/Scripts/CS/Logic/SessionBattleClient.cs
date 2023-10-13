using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ClientCSharp.Logic
{
	class SessionBattleClient : ProtocolBattle_ICallbackReceiver
	{
		//??????????????????
		// Param : mapName	map name
		public void On_EnterSceneOk(string mapName) {
#if NGUI
			GUILogManager.LogInfo("On_EnterSceneOk mapName: "+mapName);

            // Fix assigning mission ID before the loading screen
            if (mapName == "Hub_Village_Tutorial")
                PlayerDataManager.Instance.SetMissionID(5011);
			//mm
			if(PlayerLightCtrl.Instance != null){
				PlayerLightCtrl.Instance.DestroyObj();
			}
			//#mm
			CS_SceneInfo.Instance.ClearALLObjects();
			LoadingScreenCtrl.Instance.DownloadScense(mapName);
			BGMInfo.Instance.PlayBGMByStageName(mapName);
#else
            LogManager.Log_Info("On_EnterSceneOk : " + mapName);
			if(PlayerLightCtrl.Instance != null)
				PlayerLightCtrl.Instance.DestroyObj();
			CS_SceneInfo.Instance.ClearALLObjects();
			//todo: reset ally.
			PlayerInfoBar.Instance.ClearNpcState();
			_UI_CS_KillChain.Instance.HideCombInfo();
			_UI_MiniMap.Instance.SetMiniMapIdx(mapName);
			_UI_CS_LoadProgressCtrl.Instance.LoadLevel(mapName);
			_UI_CS_DebugInfo.Instance.SetMapName(mapName);
			BGMInfo.Instance.PlayBGMByStageName(mapName);
#endif
		}

		//????????????
		// Param : objectID	objectID
		// Param : pos	
		// Param : rot1	
		// Param : rot2	
		// Param : continueTime	
		public void On_UpdateMoveResult(int objectID, UnityEngine.Vector3 pos, float rot, uint continueTime) {
			//Debug.Log("Object ID: "+objectID + " move too fast!!! Cancel invaild movement!");
			//Aka's ID = 1
			if(objectID==1)
			{
				Player.Instance.GetComponent<PlayerMovement>().CancelInvaildMovement(pos,rot,continueTime);
			}
		}

		//??????object??????????????		// Param : objectID	objectID
		// Param : pos	??????
		public void On_SetObjectPosition(int objectID, UnityEngine.Vector3 pos)
		{
			CS_SceneInfo.Instance.On_SetObjectPosition(objectID,pos);
		}
		
		public void On_SetObjectVecPosition(float time, int objectID, vectorServerPosition vecpos)
		{
			CS_SceneInfo.Instance.On_SetObjectVecPosition(time,objectID,vecpos);
		}

		//????????????
		// Param : errType	????????????
		// Param : direction	??????
		// Param : pos	??????
		public void On_UpdateMoveFail(EServerErrorType errType, float direction, UnityEngine.Vector3 pos)
		{

		}

		//??????????????????
		// Param : objectID	objectID
		// Param : moveType	????????????
		// Param : direction	??????
		// Param : pos	??????
		public void On_SetMoveTypeResult(int objectID, EObjectMoveType moveType, float direction, UnityEngine.Vector3 pos)
		{

		}

		//??????????????????
		// Param : errType	????????????
		// Param : moveType	????????????
		// Param : direction	??????
		// Param : pos	??????
		public void On_SetMoveTypeFail(EServerErrorType errType, EObjectMoveType moveType, float direction, UnityEngine.Vector3 pos)
		{

		}

		//????????????
		// Param : objectID	objectID
		// Param : animationID	??????ID
		public void On_UpdateAnimationNotify(int objectID, int TargetID,short animationID)
		{
			CS_SceneInfo.Instance.On_UpdateAnimationNotify(objectID,TargetID,animationID);

		}

		//
		// Param : continueTime	
		public void On_UpdateGameTime(int continueTime)
		{

		}

		//???????????????????????		// Param : skillID	skillID
		// Param : reason	????????????
		public void  On_UseFightSkillFailed(int objectID, uint skillID, EServerErrorType reason)
		{
            //Debug.Log("On_UseFightSkillFailed: objID : " + objectID + " || skill id = " + skillID + " || reason : " + reason.GetString());
			
			if(objectID == 1)
			{
			   Player.Instance.abilityManager.On_UseAbilityFailed(skillID,reason);
			}
			else
			{
				for(int i = 0; i < CS_SceneInfo.Instance.AllyNpcList.Count;i++)
				{
					if( objectID == CS_SceneInfo.Instance.AllyNpcList[i].ObjID)
					{
						CS_SceneInfo.Instance.AllyNpcList[i].abilityManager.On_UseAbilityFailed(skillID,reason);
						break;
					}
				}
			}
		}
		
		public void On_BeginUseSkill(SUseSkillResult useSkillResult)
		{
			CS_SceneInfo.Instance.OnBeginSkill(useSkillResult);
		}
		
		public void On_StopBeginUseSkill(uint skillID, uint souceObjectID)
		{
		}

		//?????????????????		// Param : useSkillResult	?????????????????????
		public void On_UseFightSkillResult(SUseSkillResult useSkillResult)
		{
			CS_SceneInfo.Instance.On_UseFightSkillResult(useSkillResult);
		}

		public void On_UpdateStatusEffect(vectorStatusEffects statusEffectVec)
		{
			CS_SceneInfo.Instance.On_UpdateStatusEffect(statusEffectVec);
		}

		//object????????????????????????????????		// Param : objectID	objectID
		public void On_ObjectEnterTest(int objectID)
		{

		}

		//??????????????????
		// Param : characterUpdateInfo	??????????????????
		public void On_CharacterUpdate(SCharacterUpdateInfo characterUpdateInfo)
		{

		}

		public void On_ObjectLeave(vectorInt objectIDVec)
		{
			foreach (int eachID in objectIDVec)
			{
				CS_SceneInfo.Instance.OnObjectLeave(eachID);
			}
			
		}

		public void On_CharacterPrivateInfoUpdate(SCharacterPrivateInfo characterUpdateInfo)
		{

		}

		public void On_PlayerChangeWeaponOk(sbyte slotID)
		{

		}
		
		public void On_ItemEnter(SItemEnter mapitemInfo)
		{
			CS_SceneInfo.Instance.On_EnterItem(mapitemInfo);
		}

        /// <summary>
        /// NPC enters scene.
        /// </summary>
        /// <param name="npcInfo"></param>
		public void On_npcEnter(SNpcEnter npcInfo)
		{
            ShopNpc _perfab = NpcInfo.GetPrefabByID(npcInfo.npcID);
            if (_perfab)
            {
                ShopNpc _npc = GameObject.Instantiate(_perfab) as ShopNpc;
                _npc.transform.position = npcInfo.pos;
                _npc.ObjID = npcInfo.objectID;
                NpcInfo.AddNPC(_npc);
            }
            else
            {
                LogManager.Log_Warn(" --- On_npcEnter npcID null !!!! --- ");
            }
		}
		
		public void On_BreakableActorEnter(SBreakableActorEnter BreakableActorInfo)
		{
			CS_SceneInfo.Instance.On_BreakableActorEnter(BreakableActorInfo);
		}
		
		public void On_monsterEnter(SMonsterEnter monsterInfo)
		{
			CS_SceneInfo.Instance.On_EnterMonster(monsterInfo);
		}
		
		/// <summary>
		/// ???????????????????????
		/// </summary>
		/// <param name="health">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="energy">
		/// A <see cref="System.Int32"/>
		/// </param>
		public void On_PlayerRegenerate(int health,int energy)
		{
            if (Player.Instance)
			{
                PlayerAttributionManager attrMan = (PlayerAttributionManager)Player.Instance.AttrMan;
                attrMan.Attrs[EAttributeType.ATTR_CurHP] = (int)Mathf.Clamp(health, 0f, attrMan.Attrs[EAttributeType.ATTR_MaxHP]);
                attrMan.Attrs[EAttributeType.ATTR_CurMP] = (int)Mathf.Clamp(energy, 0f, attrMan.Attrs[EAttributeType.ATTR_MaxMP]);
                attrMan.nextRegenTime = Time.time + 1;

                //Debug.Log("Refresh HP : " + attrMan.Attrs[EAttributeType.ATTR_CurHP] + " and MP : " + attrMan.Attrs[EAttributeType.ATTR_CurMP]);

                if (attrMan.Attrs[EAttributeType.ATTR_CurHP] <= 0 && !Player.Instance.FSM.IsInState(Player.Instance.DS))
                    Player.Instance.FSM.ChangeState(Player.Instance.DS);
			}
		}
		
		/// <summary>
		/// ?????????????????? 
		/// </summary>
		/// <param name="objectID">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="error">
		/// A <see cref="EServerErrorType"/>
		/// </param>
		public void On_PickupItemFailed(int objectID,EServerErrorType error)
		{
			CS_SceneInfo.Instance.On_PickupItemFailed(objectID,error);
			LogManager.Log_Info("On_PickupItemFailed");
			if(EServerErrorType.eItemError_BagFull == error.Get()){
#if NGUI
				if(BottomTipManager.Instance) {
					BottomTipManager.Instance.Show("Inventory Full Item Dropped");
				}
#else
				IngamePopMsg.Instance.AwakeIngameMsg("Inventory Full Item Dropped");
#endif
			}
		}
		
		public void On_CharacterStartInfo(SCharacterStartInfo charStartInfo)
		{
            //Debug.LogError("characterenter");
#if NGUI
            UI_BlackBackground_Control.CloseBlackBackground();
#endif

			if(Player.Instance != null)
			{
                Player.Instance.ObjID = charStartInfo.objectID;

			    RaycastHit hit;
                Player.Instance.transform.position = CS_SceneInfo.pointOnTheGround(charStartInfo.pos);

                if (GameCamera.Instance)
                {
                    GameCamera.Instance.ResetCamera();
                }
                if (Player.Instance.AttachedSpirit != null)
                {
                    Player.Instance.CallOnSpirit(Player.Instance.AttachedSpirit.mSpiritType);
                }
                // update attribution
                Player.Instance.AttrMan.UpdateAttrs(charStartInfo.attrVec);

                if (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurHP] > 0)
                {
                    Player.Instance.FSM.ChangeState(Player.Instance.IS);
                }
                else
                {
                    if (Player.Instance.FSM.IsInState(Player.Instance.DS))
                    {
						RevivePanel.Instance.AwakeRevival();
                    }
                    else
                    {
                        Player.Instance.FSM.ChangeState(Player.Instance.DS);
                    }
                }
			}

#if NGUI
			if(LoadingScreenCtrl.Instance) {
				LoadingScreenCtrl.Instance.ExitLoadScreen();
				Debug.Log("ExitLoadScreen CALLED");
			}
#else
			//if test mode into game.the screen type is master//
            if (_UI_CS_LoadProgressCtrl.Instance&&(_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LOADING)
				||_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_BOUNTY_MASTER)))
            {
                _UI_CS_LoadProgressCtrl.Instance.LeaveLoading();
            }
#endif
		}
		
		public void On_BulletHit(int objectID, SSkillEffect bulletEffect)
		{
			CS_SceneInfo.Instance.On_BulletHit(objectID,bulletEffect);
		}
		
		public void On_TrapHit(int objectID, SSkillEffect trapEffect)
		{
			CS_SceneInfo.Instance.On_TrapHit(objectID, trapEffect);
		}
		
		public void On_SkillObjectEnter(SSkillObjectEnter skillObjectInfo)
		{
			CS_SceneInfo.Instance.On_SkillObjectEnter(skillObjectInfo);
		}

        public void On_SkillObjectActive(int objectID)
        {
            CS_SceneInfo.Instance.On_SkillObjectActive(objectID);
        }
		
		public void On_RushHit(int objectID, SSkillEffect hitEffect)
		{
			CS_SceneInfo.Instance.On_RushHit(objectID, hitEffect);
		}
		
		public void On_AllyNpcEnter(int objectID)
		{
			CS_SceneInfo.Instance.On_AllyNpcEnter(objectID);
		}
		
		public void On_AllyRegenerate(int objectID, int CurHealth, int CurEnergy)
		{
			
			foreach( AllyNpc it in CS_SceneInfo.Instance.AllyNpcList)
		    {
				if(it.ObjID == objectID)
				{
					it.AttrMan.Attrs[EAttributeType.ATTR_CurHP] = CurHealth;
					it.AttrMan.Attrs[EAttributeType.ATTR_CurMP] = CurEnergy;
					
					//Debug.Log("AllyRegenerate HP is " + CurHealth.ToString() + " MP is " + CurEnergy.ToString());
					
					if(CurHealth <= 0)
					{
						if( !it.FSM.IsInState(it.DS))
							it.FSM.ChangeState(it.DS);
						
						PlayerInfoBar.Instance.RemoveNpc(it.stateObj);
				        CS_SceneInfo.Instance.AllyNpcList.Remove(it);
					}
					break; 
				}
			}
				
		}
		
		public void On_TelePortMoveResult(int objectID, UnityEngine.Vector3 pos, float rot, uint continueTime, float fadeinTime, float fadeoutTime, bool cameraFollow)
		{
			CS_SceneInfo.Instance.mTeleportData.triggerID = objectID;
			
			CS_SceneInfo.Instance.mTeleportData.bStart = true;
			 
			CS_SceneInfo.Instance.mTeleportData.bCameraFollow = cameraFollow;
			
			CS_SceneInfo.Instance.mTeleportData.fadeintime = fadeinTime;
			
			CS_SceneInfo.Instance.mTeleportData.fadeouttime = fadeoutTime;
			
			CS_SceneInfo.Instance.mTeleportData.teleportPos = pos;
			
			CS_SceneInfo.Instance.mTeleportData.bEnterScene = false;
			
			CS_SceneInfo.Instance.mTeleportData.step = 1;

            Player.Instance.FreezePlayer();

		}
		
		//item??????????????????
		// Param : moneyInfo	moneyInfo
		public void On_moneyEnter(SMoneyEnter moneyInfo)
		{
			 CS_SceneInfo.Instance.On_moneyEnter(moneyInfo);
		}
		
		//????????????????????????
		// Param : objectID	objectID
		// Param : result	result
        public void On_PickupMoneyResult(int objectID, EServerErrorType result) {
            if (result.Get() == EServerErrorType.eSuccess) {
                if (CS_SceneInfo.Instance != null && CS_SceneInfo.Instance.KarmaMap.ContainsKey(objectID)) {

					KarmainfoData karmaData = new KarmainfoData();
#if NGUI
                    CS_SceneInfo.Instance.KarmaMap.TryGetValue(objectID, out karmaData);
                    CS_SceneInfo.Instance.KarmaMap.Remove(objectID);
					
					PlayerDataManager.Instance.AddMissionKarma(karmaData.KarmaInfo.money);
#else
					CS_SceneInfo.Instance.KarmaMap.TryGetValue(objectID, out karmaData);
                    CS_SceneInfo.Instance.KarmaMap.Remove(objectID);
					
					 _UI_CS_DebugInfo.Instance.AddMissionKarma(karmaData.KarmaInfo.money);
					 _UI_CS_MissionLogic.Instance.MissionKarma += karmaData.KarmaInfo.money;
#endif   
                }
				GUILogManager.LogInfo("PickupMoney " + objectID.ToString() + " sucess");
            }else {
                if (CS_SceneInfo.Instance != null && CS_SceneInfo.Instance.KarmaMap.ContainsKey(objectID)) {
                    if (CS_SceneInfo.Instance.KarmaMap[objectID].KarmaInfo != null) {
                        CS_SceneInfo.Instance.On_moneyEnter(CS_SceneInfo.Instance.KarmaMap[objectID].KarmaInfo);
					}
                }
                GUILogManager.LogInfo("PickupMoney " + objectID.ToString() + " failed");
            }
        }

		//????????????????????????
        // Param : objectID	objectID
        // Param : result	??????
        public void On_PickupItemSuccAck(int objectID, EServerErrorType result)
        {
			LogManager.Log_Info("On_PickupItemSuccAck");
			#region mission 
#if NGUI
			PlayerDataManager.Instance.CheckMissionProgress((int)_UI_CS_RamusTask.MISSION_TYPE.COLLECT,CS_SceneInfo.Instance.ItemInfoList[objectID].iteminfo.ID,CS_SceneInfo.Instance.ItemInfoList[objectID].iteminfo.perfrab);
#else
			_UI_CS_MissionLogic.Instance.CheckMissionProgress(_UI_CS_MissionLogic.MissionType.COLLECT,CS_SceneInfo.Instance.ItemInfoList[objectID].iteminfo.ID,CS_SceneInfo.Instance.ItemInfoList[objectID].iteminfo.perfrab);
#endif			
			#endregion
			#region mat list
			if(CS_SceneInfo.Instance!=null) {
				if(CS_SceneInfo.Instance.ItemInfoList.ContainsKey(objectID)) {
					int id = CS_SceneInfo.Instance.ItemInfoList[objectID].iteminfo.ID;
					int perfab = CS_SceneInfo.Instance.ItemInfoList[objectID].iteminfo.perfrab;
					int gem = CS_SceneInfo.Instance.ItemInfoList[objectID].iteminfo.gem;
					int enchant = CS_SceneInfo.Instance.ItemInfoList[objectID].iteminfo.enchant;
					int element = CS_SceneInfo.Instance.ItemInfoList[objectID].iteminfo.element;
					int level = (int)CS_SceneInfo.Instance.ItemInfoList[objectID].iteminfo.level;
					ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(id,perfab,gem,enchant,element,level);
					if( tempItem == null)
						return;
					//11.12.13 is material item type
					if(tempItem._TypeID == 11 || tempItem._TypeID == 12 || tempItem._TypeID == 13) {
#if NGUI
						PlayerDataManager.Instance.AddIngameMaterial(tempItem);
						if(InGameScreenCtrl.Instance) {
							if(InGameScreenCtrl.Instance.inventoryCtrl) {
								Debug.Log ("there is ctrl !!!");
								InGameScreenCtrl.Instance.inventoryCtrl.UpdateInventorySlot();
								Debug.Log ("slot updated now !!!");
							}
						}
						Debug.Log ("tempItemID ["+tempItem._TypeID+"]");
#else
						MissionComplete.Instance.AddMaterialItem(tempItem);
#endif
					}
				}
			}
			#endregion
        }
		
		public void On_TriggerAnimationResult(int objectID, string AnimName, bool IsLoop, float AnimDelayTime,bool bCollision,bool bDisplay)
		{
			CS_SceneInfo.TriggerAnimData theTriggerAnimData = new CS_SceneInfo.TriggerAnimData();
		
			theTriggerAnimData.id = objectID;
			theTriggerAnimData.Aniname = AnimName;
			theTriggerAnimData.IsLoopAnim = IsLoop;
			theTriggerAnimData.AnimDelayTime = AnimDelayTime;
			theTriggerAnimData.bCollision = bCollision;
			theTriggerAnimData.bDisplay = bDisplay;
			
			CS_SceneInfo.Instance.AddTriggerAnimation(theTriggerAnimData);
		}
		
		public void On_SetKnockBackSpeed(int objectID, UnityEngine.Vector3 speed, int behavior)
		{
			NpcBase theMonster = CS_SceneInfo.Instance.GetMonsterByID(objectID);
			speed.y = 0;
			if( theMonster != null)
			{
				if( behavior == 1)
				{
					if(theMonster.KnockbackState != null)
					{
						theMonster.KnockbackState.KnockBackSpeed = speed;
						
					}
				}
				else if(behavior == 2)
				{
					if(theMonster.flee_state != null)
					{
					   theMonster.FleeState.KnockBackSpeed = speed;
					}
				}
			}
		}

        /// <summary>
        /// Player and ally do normal attack , this function could be called.
        /// </summary>
        /// <param name="isCritical">actually, this variable is included in useskillresult.skillvector, so we ignore it.</param>
        /// <param name="useSkillResult"></param>
        public void On_UseNormalAttackResult(bool isCritical, SUseSkillResult useSkillResult)
        {
            CS_SceneInfo.Instance.On_UseFightSkillResult(useSkillResult);
        }

        public void On_UseNormalAttackFailed(uint targetObjectID, EServerErrorType reason)
        {
            if (targetObjectID == 1)
            {
                Player.Instance.abilityManager.On_UseAbilityFailed((uint)AbilityIDs.NormalAttack_1H_ID, reason);
            }
            else
            {
                foreach (AllyNpc it in CS_SceneInfo.Instance.AllyNpcList)
                {
                    if (targetObjectID == it.ObjID)
                    {
                        it.abilityManager.On_UseAbilityFailed((uint)AbilityIDs.NormalAttack_1H_ID, reason);
                        break;
                    }
                }
            }
        }

		public void On_ReviveResult(EReviveType reviveType, EServerErrorType result, vectorAttrChange playerAttrVec)
		{	
			if(result.Get() == EServerErrorType.eSuccess){
#if NGUI
				GUIManager.Instance.ChangeUIScreenState("IngameScreen");
#else
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
				RevivePanel.Instance.basePanel.Dismiss();
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				_UI_MiniMap.Instance.isShowSmallMap	 = true;
#endif
	            Player.Instance.AttrMan.UpdateAttrs(playerAttrVec);
                if (reviveType.Get() == EReviveType.eReviveType_Item || reviveType.Get() == EReviveType.eReviveType_Crystal)
                {
                    Player.Instance.abilityManager.UseAbilityFromID((int)AbilityIDs.ReviveEX_ID);
                }
                else
                {
                    Player.Instance.abilityManager.UseAbilityFromID((int)AbilityIDs.Revive_ID);
                }
			}else {
#if NGUI
				PopUpBox.PopUpErr(result);
#else
				_UI_CS_PopupBoxCtrl.PopUpError(result);
#endif
			}
		}
		
		public void On_SpeechResult(int npcID, bool loop, bool bRandom,float time, vectorInt indexVec, bool inside, SServerPosition minpoint, SServerPosition maxpoint)
		{
			CS_SceneInfo.cSpeechCont newSpeechCont = new CS_SceneInfo.cSpeechCont();
			
			newSpeechCont.bRandom = bRandom;
			
			newSpeechCont.bLoop = loop;
			
			newSpeechCont.NpcTypeID = npcID;
			
		    newSpeechCont.SpeekTime = time;
			
			newSpeechCont.WordList = new List<int>();
			
			newSpeechCont.WordList.Clear();
			
			newSpeechCont.bInside = inside;
			
			newSpeechCont.minPoint = Vector2.zero;
			
			newSpeechCont.minPoint.x = minpoint.fx;
			
			newSpeechCont.minPoint.y = minpoint.fz;
			
			newSpeechCont.maxPoint = Vector2.zero;
			
			newSpeechCont.maxPoint.x = maxpoint.fx;
			
			newSpeechCont.maxPoint.y = maxpoint.fz;
			
			for(int i = 0; i < indexVec.Count; i++)
				newSpeechCont.WordList.Add((int)indexVec[i]);
			
			if(CS_SceneInfo.Instance != null)
			  CS_SceneInfo.Instance.PushNewContent(newSpeechCont);
			
		}
		
		public void On_UpdateSerialKillNum(int serialNum)
		{
			LogManager.Log_Info("On_UpdateSerialKillNum");
#if NGUI
			if(Hud_KillChain_Manager.Instance != null)
			{
				Hud_KillChain_Manager.Instance.StartKillChain(serialNum);
			}
#else
			
			_UI_CS_KillChain.Instance.isServerCalc = true;
			_UI_CS_KillChain.Instance.CalcKillChainTime();
#endif
		}
		
		public void On_SerialKillOver(int serialNum, int serialExp)
		{
#if NGUI
			GUILogManager.LogInfo("On_SerialKillOver");
			PlayerDataManager.Instance.SetMissionScore(serialExp);
			
			if(Hud_KillChain_Manager.Instance != null)
			{
                Hud_KillChain_Manager.Instance.EndKillChain();
				Hud_KillChain_Manager.Instance.UpdateBonus(serialNum, serialExp);
			}
#else
			LogManager.Log_Info("On_SerialKillOver");
			_UI_CS_KillChain.Instance.isServerCalc = false;
			if(serialNum >= _UI_CS_KillChain.Instance.goodFlag){
				if(serialNum >= _UI_CS_KillChain.Instance.lethalFlag){
					if(serialNum >= _UI_CS_KillChain.Instance.slayerFlag){
						_UI_CS_KillChain.Instance.CallKillChain(3,serialExp);
					}else{
						_UI_CS_KillChain.Instance.CallKillChain(2,serialExp);	
					}
				}else{
					_UI_CS_KillChain.Instance.CallKillChain(1,serialExp);
				}
			}else{
				_UI_CS_MissionLogic.Instance.SetMissionScore(serialExp);
				_UI_CS_KillChain.Instance.DismissKillChainMsg();
			}
#endif
		}
		
		public void On_SpawnnerKilled(int SpawnerID) {	
#if NGUI
			GUILogManager.LogInfo("On_SpawnnerKilled");
			PlayerDataManager.Instance.CheckMissionProgress((int)_UI_CS_RamusTask.MISSION_TYPE.SURVIVE,0,0);
#else			
			LogManager.Log_Info("On_SpawnnerKilled");
			_UI_CS_MissionLogic.Instance.CheckMissionProgress(_UI_CS_MissionLogic.MissionType.SURVIVE,0,0);
#endif
		}

        public void On_FriendAllyEnter(SFriendAllyEnter FriendAllyInfo)
        {
            if (CS_SceneInfo.Instance != null)
            {
                if (CS_SceneInfo.Instance.AllyNpcPrefab != null)
                {
                    //Vector3 thePosition =  Player.Instance.transform.position; //FriendAllyInfo.pos;
                    Vector3 thePosition = CS_SceneInfo.pointOnTheGround(FriendAllyInfo.pos);

                    AllyNpc tNewAllyNpc = (AllyNpc)GameObject.Instantiate(CS_SceneInfo.Instance.AllyNpcPrefab, thePosition, Quaternion.identity);

                    if (tNewAllyNpc != null)
                    {
                        ObjStatePrefab ObjState = (ObjStatePrefab)GameObject.Instantiate(PlayerInfoBar.Instance.statePrefab);
                        if (null != ObjState)
                        {
                            PlayerInfoBar.Instance.InitAllyState(ObjState, FriendAllyInfo.style, FriendAllyInfo.sex, 0);
                            PlayerInfoBar.Instance.AddNpc(ObjState);
                            tNewAllyNpc.stateObj = ObjState;
                            //						ObjState.transform.parent = tNewAllyNpc.gameObject.transform;
                        }
                        else
                        {
                            LogManager.Log_Error("create a stateObj fail");
                        }

                        tNewAllyNpc.SetInitialAttribute(FriendAllyInfo);
                        tNewAllyNpc.SetAllyKind(AllyNpc.EAllyType.eFriend, false);
                        CS_SceneInfo.Instance.AllyNpcList.Add(tNewAllyNpc);
                    }
                }
            }
        }
		
		public void On_GetFriendListResult(vectorFriendCharInfo friendVec)
		{
			_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.Dismiss();
			_UI_CS_Summon.Instance.FriendInfoList.Clear();
			foreach (SFriendCharInfo info in friendVec){
				_UI_CS_Summon.Instance.FriendInfoList.Add(info);
			}
			_UI_CS_Summon.Instance.ShowFriendInfo();
			_UI_CS_Summon.Instance.ShowModel();
			
		}

		
		public void On_SelectFriendAllyResult(int charID, EServerErrorType result)
		{
			if(EServerErrorType.eSuccess == result.Get()){
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				_UI_CS_Summon.Instance.HideModel();
				MoneyBadgeInfo.Instance.Hide(false);
				_UI_CS_Summon.Instance.SummonPanel.Dismiss();
			}else{
				_UI_CS_PopupBoxCtrl.PopUpError(result);
			}
		}
		
		public void On_PlayerReviveNumNTF(int crystalReviveNum, int karmaReviveNum)
        {
//			_UI_CS_Revival.Instance.RevivalCount = reviveNum;
#if NGUI
			PlayerDataManager.Instance.SetRevivalCount(karmaReviveNum);
#else
			RevivePanel.Instance.SetRevivalCount(karmaReviveNum);
#endif
        }

		// Param : curTime	current time
		public void On_SyncCurTime(uint curTime)
		{
			CS_SceneInfo.Instance.On_SyncCurTime((uint)curTime);
		}
	}
}
