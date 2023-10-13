using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {
	
	Transform movement_sound;
	
	public void AttackNotify()
	{
		Player.Instance.AttackNotify();
	}
	
	public void PlayMovementSound()
	{
		if(movement_sound == null && Player.Instance.GetComponent<PlayerMovement>().MovementSound != null)
		{
			movement_sound = Instantiate(Player.Instance.GetComponent<PlayerMovement>().MovementSound) as Transform;
		}
		
		if(movement_sound != null)
		{
			movement_sound.transform.position = transform.position;
			movement_sound.transform.rotation = transform.rotation;
			SoundCue.Play(movement_sound.gameObject);
		}
	}
	
	public void PlayBowDrawSound()
	{
	}
	
	public void PlayWeaponAttackSound()
	{
        if (Player.Instance && Player.Instance.GetComponent<PlayerSoundHandler>())
            Player.Instance.SoundHandler.PlayWeaponAttackSound();
	}
	
	public void PlayBodyFallSound()
	{
		Player.Instance.SoundHandler.PlayBodyFallSound();
	}
	
	public void PlayHungryCleaveEffect()
	{
		return;
		if(Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.HungryCleave_I_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.HungryCleave_II_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.HungryCleave_III_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.HungryCleave_IV_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.HungryCleave_V_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.HungryCleave_VI_ID)))
		{
			//HungryCleave ability =	(HungryCleave)(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.HungryCleave_ID));
			//ability.PlaySoundAndVFX();
		}
	}
	
	public void PlayShockWaveEffect()
	{
		return;
        if (Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Shockwave_I_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Shockwave_II_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Shockwave_III_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Shockwave_IV_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Shockwave_V_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Shockwave_VI_ID)))
		{
			//Shockwave ability =	(Shockwave)(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Shockwave_ID));
			//ability.PlayShockWaveEffect();
		}
	}
	
	public void SetUpTrap(){
        return;
	}
	
	public void ThrowCaltrops()
	{
        if (Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Caltrops_I_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Caltrops_II_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Caltrops_III_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Caltrops_IV_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Caltrops_V_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.Caltrops_VI_ID)))
        {
            PlayerCalTrops caltropsState = (PlayerCalTrops)(Player.Instance.FSM.GetCurrentState());
            caltropsState.PlaySoundAndVFX();
        }
	}
	
	public void PlayIceBarricadeAnimation()
	{
		return;
        //if (Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.IceBarricade_I_ID)) ||
        //    Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.IceBarricade_II_ID)) ||
        //    Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.IceBarricade_III_ID)))
        //{
        //    PlayerIceBarricade iceState = (PlayerIceBarricade)(Player.Instance.FSM.GetCurrentState());
        //    iceState.PlaySoundAndVFX();
        //}
	}
	
	public void BackToIdleState()
	{
		//Debug.LogError("BACK TO IDLE STATE FROM ANIMATION EVENT");
        if (Player.Instance.FSM.IsInState(Player.Instance.DS)) return;
        if (Player.Instance.PGS.IsHoldingLMB && Player.Instance.AttackTarget)
        {
            Player.Instance.AttackEnemy();
        }
        else
            Player.Instance.FSM.ChangeState(Player.Instance.IS);
	}
	
	public void RainBlowAttackNotify()
	{
		//ability vfx
        if (Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.RainOfBlows_I_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.RainOfBlows_II_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.RainOfBlows_III_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.RainOfBlows_IV_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.RainOfBlows_V_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.RainOfBlows_VI_ID)))
		{
			PlayerRainBlow rb = (PlayerRainBlow)Player.Instance.FSM.GetCurrentState();
		   	rb.PlayVFX();
			//rb.CalculateRainBlow();
		}
	}
	
	public void ShootNotify()
	{
		//print("Shoot");
        if (Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SteadyShot_I_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SteadyShot_II_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SteadyShot_III_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SteadyShot_IV_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SteadyShot_V_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SteadyShot_VI_ID)))
        {
            PlayerAbilityShot shoot = (PlayerAbilityShot)Player.Instance.FSM.GetCurrentState();
            shoot.MountFinished();
        }
        if (Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.StingingShot_I_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.StingingShot_II_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.StingingShot_III_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.StingingShot_IV_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.StingingShot_V_ID)) ||
            Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.StingingShot_VI_ID)))
        {
            PlayerAbilityShot shoot = (PlayerAbilityShot)Player.Instance.FSM.GetCurrentState();
            shoot.MountFinished();
        }
	}
	
	public void ShowMeleeTrail()
	{
        if (!Player.Instance) return;

		MeleeAttackState meleestate = (MeleeAttackState)Player.Instance.abilityManager.GetAbilityByID((uint) AbilityIDs.NormalAttack_1H_ID);
		if(meleestate.hand == 0)
		{
            if (Player.Instance.EquipementMan.RightHandWeapon && Player.Instance.EquipementMan.RightHandWeapon.GetComponent<MeleeWeaponTrail>())
			{
                Player.Instance.EquipementMan.RightHandWeapon.GetComponent<MeleeWeaponTrail>().Emit = true;
			}
		}
		
		if(meleestate.hand == 1)
		{
            if (Player.Instance.EquipementMan.LeftHandWeapon && Player.Instance.EquipementMan.LeftHandWeapon.GetComponent<MeleeWeaponTrail>())
			{
                Player.Instance.EquipementMan.LeftHandWeapon.GetComponent<MeleeWeaponTrail>().Emit = true;
			}
		}
	}
	
	public void HideMeleeTrail()
	{
        if (!Player.Instance) return;

		MeleeWeaponTrail[] meleeTrails = Player.Instance.GetComponentsInChildren<MeleeWeaponTrail>();
		if(meleeTrails.Length > 0)
		{
			foreach(MeleeWeaponTrail meleeTrail in meleeTrails)
			{
				meleeTrail.Emit = false;
			}
		}
	}
}