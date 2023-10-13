using UnityEngine;
using System.Collections;

public class RunState : PlayerState {
	
	public override void Enter()
	{
		//print("Enter Run State!");
		PlayRunAnim();
	}
	
	public override void Execute()
	{
		player.SortEnemyPos();
		
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
		
		if(player.AttackTarget)
		{
			player.AttackEnemy(player.AttackTarget);
		}

        if (player.PickupItem)
        {
            player.PickItem(player.PickupItem);
        }
	}
	
	public override void Exit()
	{
		//print("Exit Run State!");
	}	
	
	public void PlayRunAnim()
	{
        WeaponBase.EWeaponType wt = player.EquipementMan.GetWeaponType();
		
		if(wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_NoneWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
            player.AnimationModel.animation.CrossFade("Aka_1H_Run");	
		else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
            player.AnimationModel.animation.CrossFade("Aka_2H_Run");	
		else
            player.AnimationModel.animation.CrossFade("Aka_2HNodachi_Run");			

	}
}
