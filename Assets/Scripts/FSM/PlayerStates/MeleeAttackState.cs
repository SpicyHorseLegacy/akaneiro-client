 using UnityEngine;
using System.Collections;

public class MeleeAttackState : PlayerAbilityBaseState
{
    string[] WeaponType_0h_AnimName_R = { "Aka_0H_Attack_2" };
    string[] WeaponType_0h_AnimName_L = { "Aka_0H_Attack_1" };
    string[] WeaponType_1h_AnimName_R = { "Aka_1H_Attack_1", "Aka_1H_Attack_3" };
    string[] WeaponType_1h_AnimName_L = { "Aka_1H_Attack_2" };
    string[] WeaponType_2h_AnimName = { "Aka_2H_Attack_1", "Aka_2H_Attack_2", "Aka_2H_Attack_3", "Aka_2H_Attack_4" };
    string[] WeaponType_2hNodachi_AnimName = { "Aka_2HNodachi_Attack_1", "Aka_2HNodachi_Attack_2", "Aka_2HNodachi_Attack_3", "Aka_2HNodachi_Attack_4" };
	
	public float attackAnimationLength;
	
	Transform lastTarget;
	
	public int NextAbilityID;
	public _UI_CS_UseAbilities NextAbilityButton;
	public Vector3 NextAbilityPos;
	bool IsKeyboardInput;
	
	public int hand = 1;				// 1为右手，0为左手
    float lastAttack = -1;
	
	public NextAction NextActionAfterAnimation = NextAction.backtoidle;
	
	public enum NextAction
	{
		backtoidle,
		continueAttacking,                      // if contineueAttacking after attack animation, that means attack if the target is still alive.
        continueAttackingFromNullTarget,        // no matter there is still a target, attack after previous attack animation
		activeAbility,
		move,
		playAttackIdleAnimation
	}
	
	Vector3 tempMousePosition;
	
	public override void Enter()
	{
		//print("Enter Melee State!");
		
		base.Enter();

        Owner.GetComponent<PlayerMovement>().StopMove(false);
		
		if(Player.Instance.AttackTarget)
		{
			if(Player.Instance.AttackTarget.GetComponent<NpcBase>() || Player.Instance.AttackTarget.GetComponent<InteractiveObj>())
			{
				Owner.GetComponent<PlayerMovement>().LookAtPosition(Player.Instance.AttackTarget.position);
			}
		}else{
			Owner.GetComponent<PlayerMovement>().LookAtMousePoint(true);
		}
		
		NextActionAfterAnimation = NextAction.backtoidle;
		
		attack();
	}
	
	public override void Execute()
	{
		// click RMB to active the ability which is put in the default UI
		if(Input.GetMouseButtonDown(1) && !GameCamera.Instance.IsFreeCameraMode)
		{
#if NGUI
			if(UI_Hud_AbilitySlot_Manager.Instance)
			{
				int _abiid = UI_Hud_AbilitySlot_Manager.Instance.GetSlotAbiIDBySlotID(0);
				if(_abiid > 0)
				{
					PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                    _abiManager.UseAbility(_abiid, true);
					return;
				}
			}
#else
			_UI_CS_UseAbilities abilitiesObject  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(0);
			if(null != abilitiesObject)
			{
				if(abilitiesObject.m_isCoolDownFinish)
				{
                    PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                    _abiManager.UseAbility(abilitiesObject, true);
					return;
				}
			}
#endif
		}

        if (Input.GetMouseButtonUp(1))
        {
#if NGUI
			if(UI_Hud_AbilitySlot_Manager.Instance)
			{
				int _abiid = UI_Hud_AbilitySlot_Manager.Instance.GetSlotAbiIDBySlotID(0);
				if(_abiid > 0)
				{
					PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                    _abiManager.KeyboardKeyUp(_abiid);
					return;
				}
			}
#else
            _UI_CS_UseAbilities abilitiesObject  = _AbiMenuCtrl.Instance.GetUseAbilitiesID(0);
            if (null != abilitiesObject)
            {
                if (abilitiesObject.m_isCoolDownFinish)
                {
                    PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                    _abiManager.KeyboardKeyUp(abilitiesObject);
                    return;
                }
            }
#endif
        }

        if (Input.GetMouseButtonDown(0) && NextActionAfterAnimation != NextAction.activeAbility)
        {
            Transform _target = Player.Instance.HoverTarget;
            if (_target && _target.GetComponent<BaseHitableObject>())
            {
                Player.Instance.AttackTarget = _target;
                NextActionAfterAnimation = NextAction.continueAttacking;
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    NextActionAfterAnimation = NextAction.move;
                }
                tempMousePosition = Input.mousePosition;
            }
        }
		
		// because if player killed a target and holding the LMB, player would play attackidle animation. if playing attackidle animation, 'AnimationModel.animation.isPlaying' would be wrong
		attackAnimationLength -= Time.deltaTime;
		if(attackAnimationLength < 0.0f)
		{
			//Debug.Log("id : " + NextActionAfterAnimation);
			if(NextActionAfterAnimation == NextAction.activeAbility && NextAbilityButton)
			{
                PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
                _abiManager.UseAbility(NextAbilityButton, NextAbilityPos, IsKeyboardInput);
				NextAbilityButton = null;
				NextAbilityPos = Vector3.zero;

                if (Input.GetMouseButton(0)) Player.Instance.PGS.IsHoldingLMB = true;

				return;
			}

            if (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))){
                    NextActionAfterAnimation = NextAction.continueAttackingFromNullTarget;
            }

			if(NextActionAfterAnimation == NextAction.move)
			{
				//print(tempMousePosition);
				//Ray ray = Camera.main.ScreenPointToRay(tempMousePosition);
                Ray ray = GameCamera.Instance.gameCamera.ScreenPointToRay(tempMousePosition);
				RaycastHit hit;

                int layer = 1 << LayerMask.NameToLayer("Walkable");
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, layer) && Time.timeScale > 0)
                {
                    if (Vector3.Distance(Owner.position, hit.point) > 1)
                    {
                        Owner.GetComponent<PlayerMovement>().bStopMove = false;
                        Owner.GetComponent<PlayerMovement>().MoveTarget = hit.point;
                        Player.Instance.PathFindingState = Player.EPathFindingState.PathFinding_Processing;
                        Owner.GetComponent<Seeker>().StartPath(Owner.position, hit.point);
                        Player.Instance.AttackTarget = null;
                        Player.Instance.FSM.ChangeState(Player.Instance.RS);
                        return;
                    }
                }
			}
			
			if(Player.Instance.AttackTarget)
			{
				if((GameConfig.IsAutoAttack || Input.GetMouseButton(0)))
				{
					NextActionAfterAnimation = NextAction.continueAttacking;
				}
			}

            if (NextActionAfterAnimation == NextAction.continueAttacking)
            {
                if (Player.Instance.AttackTarget && Player.Instance.AttackTarget.GetComponent<BaseHitableObject>())
                {
                    if (Player.Instance.AttackTarget.GetComponent<BaseHitableObject>().AttrMan.Attrs[EAttributeType.ATTR_CurHP] > 0)
                    {
                        if (!Player.Instance.isInAttackRange(Player.Instance.AttackTarget))
                        {
                            Player.Instance.GetComponent<PlayerMovement>().MoveTarget = Player.Instance.AttackTarget.position;
                            Player.Instance.PathFindingState = Player.EPathFindingState.PathFinding_Processing;
                            Player.Instance.GetComponent<Seeker>().StartPath(Player.Instance.transform.position, Player.Instance.AttackTarget.position);
                            Player.Instance.FSM.ChangeState(Player.Instance.RS);
                        }
                        else
                        {
                            Owner.GetComponent<PlayerMovement>().LookAtPosition(Player.Instance.AttackTarget.position);
                            attack();
                        }
                        return;
                    }
                }
            }

            if (NextActionAfterAnimation == NextAction.continueAttackingFromNullTarget)
            {
                Owner.GetComponent<PlayerMovement>().LookAtMousePoint(true);
                attack();
                return;
            }

            if (Input.GetMouseButton(0) && !Player.Instance.AttackTarget)
                NextActionAfterAnimation = NextAction.playAttackIdleAnimation;

            if (NextActionAfterAnimation == NextAction.playAttackIdleAnimation)
            {
                Player.Instance.PlayIdleAnim(true);
                return;
            }
			
			Player.Instance.FSM.ChangeState(Player.Instance.IS);
		}
	}
	
	public override void Exit()
	{
		base.Exit();

        Owner.GetComponent<PlayerMovement>().bStopMove = false;
		NextActionAfterAnimation = NextAction.backtoidle;
        Player.Instance.AnimationModel.GetComponent<PlayerAnimation>().HideMeleeTrail();
	}

    public override void UseAbilityResult(SUseSkillResult useSkillResult)
    {
        base.UseAbilityResult(useSkillResult);
        CameraEffectManager.Instance.PlayShakingEffect("subtle");
    }

    //public override void CalculateResult()
    //{
    //    base.CalculateResult();
    //    CameraEffectManager.Instance.PlayShakingEffect("subtle");
    //}

	private void playAttackAnimation()
	{
        WeaponBase.EWeaponType wt = Player.Instance.EquipementMan.GetWeaponType();
		
		// hand == 1 左手攻击   hand == 0 右手攻击
		hand ++;
		hand = hand % 2;

        if (Time.time > lastAttack + 3)
            hand = 0;

		string aniString = "";
		if(wt == WeaponBase.EWeaponType.WT_NoneWeapon){
			if(hand == 1)
				aniString = WeaponType_0h_AnimName_L[Random.Range(0,WeaponType_0h_AnimName_L.Length)];
			else
				aniString = WeaponType_0h_AnimName_R[Random.Range(0,WeaponType_0h_AnimName_R.Length)];
		}
        else if (wt == WeaponBase.EWeaponType.WT_DualWeapon)
		{
			if(hand == 1)
				aniString = WeaponType_1h_AnimName_L[Random.Range(0,WeaponType_1h_AnimName_L.Length)];
			else
				aniString = WeaponType_1h_AnimName_R[Random.Range(0,WeaponType_1h_AnimName_R.Length)];
		}
        else if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon)
        {
            aniString = WeaponType_1h_AnimName_R[Random.Range(0, WeaponType_1h_AnimName_R.Length)];
            hand = 0;
        }
		else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
		{
			aniString = WeaponType_2h_AnimName[Random.Range(0,WeaponType_2h_AnimName.Length)];
			hand = 0;
		}
		else
		{
			aniString = WeaponType_2hNodachi_AnimName[Random.Range(0,WeaponType_2hNodachi_AnimName.Length)];
			hand = 0;
		}
		
        Transform weaponObj = null;
        if (hand == 0)
            weaponObj = Player.Instance.EquipementMan.RightHandWeapon;
        else
            weaponObj = Player.Instance.EquipementMan.LeftHandWeapon;

        float attackSpeedFactor = 1;
        if (weaponObj)
        {
            WeaponBase weapon = weaponObj.GetComponent<WeaponBase>();
            attackSpeedFactor = 1.0f / weapon.AttackSpeedFactor;
		}
        attackSpeedFactor *= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_AttackSpeed] / 100f;
        AnimationModel.animation[aniString].speed = attackSpeedFactor;

        AnimationModel.animation[aniString].time = 0;
		AnimationModel.animation.CrossFade(aniString,0.1f);
        attackAnimationLength = AnimationModel.animation[aniString].length / AnimationModel.animation[aniString].speed;

        lastAttack = Time.time;
	}
	
	private void attack()
	{
		uint targetId = 0;

		// if there is target, attack it at once
		if(Player.Instance.AttackTarget && Player.Instance.isInAttackRange(Player.Instance.AttackTarget))
		{
			targetId = (uint)Player.Instance.AttackTarget.GetComponent<BaseObject>().ObjID;
		}
		else
		{
		// if no target, find one
			Player.Instance.AttackTarget = null;
			Transform target = null;
			float minDot = 60;
			float minDis = 999;
			
			int layer = 1<<LayerMask.NameToLayer("NPC") | 1 << LayerMask.NameToLayer("Breakable") |  1 << LayerMask.NameToLayer("InteractiveOBJ");
			Collider[] attackableObjs = Physics.OverlapSphere(Owner.position + Vector3.up * 0.5f, 5f, layer);
			
			foreach(Collider obj in attackableObjs)
			{
				Vector3 objTempPos = new Vector3(obj.transform.position.x, Owner.position.y, obj.transform.position.z);
				Vector3 dir = objTempPos - Owner.position;
				float tempDot = Vector3.Angle(dir, Owner.GetComponent<PlayerMovement>().PlayerObj.forward);
				if(tempDot < minDot)
				{
					if(Vector3.Distance(obj.transform.position, Owner.position) < minDis)
					{
						if(Player.Instance.isInAttackRange(obj.transform))
						{
							target = obj.transform;
							minDot = tempDot;
							minDis = Vector3.Distance(obj.transform.position, Owner.position);
						}
					}
				}
			}
			
			if(target)
			{
				targetId = (uint)target.GetComponent<BaseObject>().ObjID;
				Player.Instance.AttackTarget = target;
			}
		}
		
		// Look At Target
		if(Player.Instance.AttackTarget)
			Player.Instance.GetComponent<PlayerMovement>().LookAtPosition(Player.Instance.AttackTarget.position);
		
        if (Player.Instance.AttackTarget && Player.Instance.AttackTarget.GetComponent<InteractiveHandler>() && !Player.Instance.AttackTarget.GetComponent<InteractiveHandler>().NeedsAttackAnimationToUSE)
        {
            Player.Instance.FSM.ChangeState(Player.Instance.IS);
        }
        else
        {
            playAttackAnimation();
        }

        if (CS_Main.Instance != null)
        {
            //print("Send Message! Normal Attack");
            EWeaponType wt = new EWeaponType(hand);
            CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.UseNormalAttack(wt, targetId, CS_SceneInfo.Instance.SyncTime));
            //NewAbilityInQueue();
        }

        NextActionAfterAnimation = NextAction.backtoidle;
	}
	
	public bool CanUseNewAbility(int _nextID, bool isKeyboardInput)
	{
		if(attackAnimationLength < 0.1f){
			return true;
		}else{
            // these abilities could active instantly, so enter this state instantly as well.
            if (_nextID == (int)AbilityIDs.ChiMend_I_ID ||
                _nextID == (int)AbilityIDs.ChiMend_II_ID ||
               	_nextID == (int)AbilityIDs.ChiMend_III_ID ||
                _nextID == (int)AbilityIDs.ChiMend_IV_ID ||
                _nextID == (int)AbilityIDs.ChiMend_V_ID ||
                _nextID == (int)AbilityIDs.ChiMend_VI_ID ||

                _nextID == (int)AbilityIDs.SeeingRed_I_ID ||
                _nextID == (int)AbilityIDs.SeeingRed_II_ID ||
                _nextID == (int)AbilityIDs.SeeingRed_III_ID ||
                _nextID == (int)AbilityIDs.SeeingRed_IV_ID ||
                _nextID == (int)AbilityIDs.SeeingRed_V_ID ||
                _nextID == (int)AbilityIDs.SeeingRed_VI_ID ||

                _nextID == (int)AbilityIDs.SkinOfStone_I_ID ||
                _nextID == (int)AbilityIDs.SkinOfStone_II_ID ||
                _nextID == (int)AbilityIDs.SkinOfStone_III_ID ||
                _nextID == (int)AbilityIDs.SkinOfStone_IV_ID ||
                _nextID == (int)AbilityIDs.SkinOfStone_V_ID ||
                _nextID == (int)AbilityIDs.SkinOfStone_VI_ID ||

                _nextID == (int)AbilityIDs.DarkHunter_I_ID ||
                _nextID == (int)AbilityIDs.DarkHunter_II_ID ||
                _nextID == (int)AbilityIDs.DarkHunter_III_ID ||
                _nextID == (int)AbilityIDs.DarkHunter_IV_ID ||
                _nextID == (int)AbilityIDs.DarkHunter_V_ID ||
                _nextID == (int)AbilityIDs.DarkHunter_VI_ID ||

                _nextID == (int)AbilityIDs.IronThorns_I_ID ||
                _nextID == (int)AbilityIDs.IronThorns_II_ID ||
                _nextID == (int)AbilityIDs.IronThorns_III_ID ||
                _nextID == (int)AbilityIDs.IronThorns_IV_ID ||
                _nextID == (int)AbilityIDs.IronThorns_V_ID ||
                _nextID == (int)AbilityIDs.IronThorns_VI_ID)
            {
                PlayerAbilityBaseState abi = (PlayerAbilityBaseState)Player.Instance.abilityManager.GetAbilityByID((uint)_nextID);
                if (abi.IsManaOK())
                {
                    abi.Enter();
                    return false;
                }
            }

            if (_nextID == (int)AbilityIDs.SwathOfDestruction_ID ||
                _nextID == (int)AbilityIDs.SwathOfFlame_ID ||
                _nextID == (int)AbilityIDs.SwathOfDestructionIII_ID ||
                _nextID == (int)AbilityIDs.SwathOfDestructionIV_ID ||
                _nextID == (int)AbilityIDs.SwathOfDestructionV_ID ||
                _nextID == (int)AbilityIDs.SwathOfDestructionVI_ID ||

                _nextID == (int)AbilityIDs.MeteorOfRain_I_ID ||
                _nextID == (int)AbilityIDs.MeteorOfRain_II_ID ||
                _nextID == (int)AbilityIDs.MeteorOfRain_III_ID ||
                _nextID == (int)AbilityIDs.MeteorOfRain_IV_ID ||
                _nextID == (int)AbilityIDs.MeteorOfRain_V_ID ||
                _nextID == (int)AbilityIDs.MeteorOfRain_VI_ID ||

                _nextID == (int)AbilityIDs.HauntingScream_I_ID ||
                _nextID == (int)AbilityIDs.HauntingScream_II_ID ||
                _nextID == (int)AbilityIDs.HauntingScream_III_ID ||
                _nextID == (int)AbilityIDs.HauntingScream_IV_ID ||
                _nextID == (int)AbilityIDs.HauntingScream_V_ID ||
                _nextID == (int)AbilityIDs.HauntingScream_VI_ID ||

                _nextID == (int)AbilityIDs.NinjaEscape_I_ID ||
                _nextID == (int)AbilityIDs.NinjaEscape_II_ID ||
                _nextID == (int)AbilityIDs.NinjaEscape_III_ID ||
                _nextID == (int)AbilityIDs.NinjaEscape_IV_ID ||
                _nextID == (int)AbilityIDs.NinjaEscape_V_ID ||
                _nextID == (int)AbilityIDs.NinjaEscape_VI_ID)
            {
                PlayerAbilityBaseState abi = (PlayerAbilityBaseState)Player.Instance.abilityManager.GetAbilityByID((uint)_nextID);
                if (abi.IsManaOK())
                    return true;
            }

			//print("cant use ability");
			NextAbilityID = _nextID;
			IsKeyboardInput = isKeyboardInput;
			if(IsKeyboardInput)
				NextAbilityPos = Input.mousePosition;
			NextActionAfterAnimation = NextAction.activeAbility; // 2 = active ability
			return false;
		}
	}
	
	public bool CanUseNewAbility(_UI_CS_UseAbilities next, bool isKeyboardInput)
	{
		if(attackAnimationLength < 0.1f){
			return true;
		}else{
            // these abilities could active instantly, so enter this state instantly as well.
            if (next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.ChiMend_I_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.ChiMend_II_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.ChiMend_III_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.ChiMend_IV_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.ChiMend_V_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.ChiMend_VI_ID ||

                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SeeingRed_I_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SeeingRed_II_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SeeingRed_III_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SeeingRed_IV_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SeeingRed_V_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SeeingRed_VI_ID ||

                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SkinOfStone_I_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SkinOfStone_II_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SkinOfStone_III_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SkinOfStone_IV_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SkinOfStone_V_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SkinOfStone_VI_ID ||

                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.DarkHunter_I_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.DarkHunter_II_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.DarkHunter_III_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.DarkHunter_IV_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.DarkHunter_V_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.DarkHunter_VI_ID ||

                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.IronThorns_I_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.IronThorns_II_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.IronThorns_III_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.IronThorns_IV_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.IronThorns_V_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.IronThorns_VI_ID)
            {
                //Player.Instance.FSM.ChangeState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.ChiMend_ID));
                PlayerAbilityBaseState abi = (PlayerAbilityBaseState)Player.Instance.abilityManager.GetAbilityByID((uint)next.m_abilitiesInfo.m_AbilitieID);
                if (abi.IsManaOK())
                {
                    abi.Enter();
                    return false;
                }
            }

            if (next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SwathOfDestruction_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SwathOfFlame_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SwathOfDestructionIII_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SwathOfDestructionIV_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SwathOfDestructionV_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.SwathOfDestructionVI_ID ||

                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.MeteorOfRain_I_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.MeteorOfRain_II_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.MeteorOfRain_III_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.MeteorOfRain_IV_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.MeteorOfRain_V_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.MeteorOfRain_VI_ID ||

                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.HauntingScream_I_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.HauntingScream_II_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.HauntingScream_III_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.HauntingScream_IV_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.HauntingScream_V_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.HauntingScream_VI_ID ||

                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.NinjaEscape_I_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.NinjaEscape_II_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.NinjaEscape_III_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.NinjaEscape_IV_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.NinjaEscape_V_ID ||
                next.m_abilitiesInfo.m_AbilitieID == (int)AbilityIDs.NinjaEscape_VI_ID)
            {
                PlayerAbilityBaseState abi = (PlayerAbilityBaseState)Player.Instance.abilityManager.GetAbilityByID((uint)next.m_abilitiesInfo.m_AbilitieID);
                if (abi.IsManaOK())
                    return true;
            }

			//print("cant use ability");
			NextAbilityButton = next;
			IsKeyboardInput = isKeyboardInput;
			if(IsKeyboardInput)
				NextAbilityPos = Input.mousePosition;
			NextActionAfterAnimation = NextAction.activeAbility; // 2 = active ability
			return false;
		}
	}

    public override bool PlayImpactSoundToWho(BaseHitableObject target, EStatusElementType _element)
    {
		if(_element.Get() != EStatusElementType.StatusElement_Invalid)
		{
			SoundCue.PlayPrefabAndDestroy(SoundEffectManager.Instance.PlayElementalSound(_element, false), Owner.transform.position);
		}
		
        return Owner.GetComponent<PlayerSoundHandler>().PlayWeaponImpactSound(target);
    }
}
