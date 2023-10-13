using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IdleState : PlayerState{
	
	// if enter idle after ability, there should be 2 sec for attackIdle animation
	bool isAttackIdleCounting = false;
	float attackIdleCountingTimer = 0f;
	
	public override void Enter()
	{
		// player could active ability
		player.CanActiveAbility = true;
		
		// check if should play attack idle animation
		bool isPlayAttackIdle = false;
		if(player.AttackTarget != null){
			if(player.AttackTarget.GetComponent<BaseObject>() && player.AttackTarget.GetComponent<BaseObject>().ObjType != ObjectType.NPC)
				isPlayAttackIdle = true;
		}else
		{
			foreach(AbilityBaseState abilitystate in player.abilityManager.Abilities)
			{
				if(player.FSM.IsLastState(abilitystate))
				{
					isPlayAttackIdle = true;
					isAttackIdleCounting = true;
					attackIdleCountingTimer = 2f;
					break;
				}
			}
		}
		player.PlayIdleAnim(isPlayAttackIdle);
	}
	
	public override void Execute()
	{
        if (_UI_CS_ScreenCtrl.Instance && (int)_UI_CS_ScreenCtrl.Instance.currentScreenType != (int)_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL)
            return;

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
		
		if(Input.GetMouseButtonDown(0))
		{
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (!Player.Instance.IsClickOnUI())
                {
                    Player.Instance.FSM.ChangeState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID));
                    return;
                }
            }

            if (!player.IsClickOnUI())
            {
                player.AttackTarget = null;
                player.PickupItem = null;
                player.isFindMoveTarget();
                return;
            }
		}
		else if(Input.GetMouseButton(0))
		{
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				if(!Player.Instance.IsClickOnUI()){
					Player.Instance.FSM.ChangeState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID));
					return;
				}
			}
			
			if(!player.AttackTarget)
			{
				player.isFindMoveTarget(true);
			}
		}
		else if(!FindEnemyAndAttack())
		{
			if(isAttackIdleCounting)
			{
				attackIdleCountingTimer -= Time.deltaTime;
				if(attackIdleCountingTimer < 0)
					isAttackIdleCounting = false;
			}
			else
			{
				if(player.AttackTarget && player.AttackTarget.GetComponent<BaseObject>())
				{
					if(player.AttackTarget.GetComponent<BaseObject>().ObjType != ObjectType.NPC)	
					{
						player.PlayIdleAnim(true);
						return;
					}
				}
				player.PlayIdleAnim(false);
			}
		}
	}
	
	public override void Exit()
	{
		//print("Exit Idle");
		isAttackIdleCounting = false;
	}
	
	public void EnableAttackIdle()
	{
		isAttackIdleCounting = true;
		attackIdleCountingTimer = 2;
	}
	
	// automatically find enemy around player and attack
    private bool FindEnemyAndAttack()
	{
		if(!GameConfig.IsAutoAttack)
			return false;

        if (CS_SceneInfo.Instance.MonsterList.Values.Count > 0)
		{
			List<Transform> nearEnemies = new List<Transform>();
			
			foreach(Transform enemy in CS_SceneInfo.Instance.MonsterList.Values)
			{
                if (enemy && enemy.GetComponent<NpcBase>() && player.isInChaseRange(enemy))
				{
					nearEnemies.Add(enemy);
				}
			}
			
			if(nearEnemies.Count > 0)
			{
				int index = Random.Range(0, nearEnemies.Count);
				player.AttackTarget = nearEnemies[index];
				player.FSM.ChangeState(player.RS);
                nearEnemies.Clear();
				return true;
			}

            nearEnemies.Clear();
		}
		return false;
	}
}
