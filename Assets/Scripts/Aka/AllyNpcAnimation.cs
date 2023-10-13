using UnityEngine;
using System.Collections;

public class AllyNpcAnimation : MonoBehaviour {

	Transform movement_sound;
	
	public AllyNpc Executer;
	
	public void AttackNotify()
	{
		Executer.AttackNotify();
	}
	
	public void PlayMovementSound()
	{
		if(movement_sound == null && Executer.GetComponent<AllyMovement>().MovementSound != null)
		{
			movement_sound = Instantiate(Executer.GetComponent<AllyMovement>().MovementSound) as Transform;
		}
		
		if(movement_sound != null)
		{
			movement_sound.transform.position = transform.position;
			movement_sound.transform.rotation = transform.rotation;
			SoundCue.Play(movement_sound.gameObject);
		}
	}

    public void BackToIdleState()
    {
        if (!Executer.FSM.IsInState(Executer.DS))
        {
            Executer.FSM.ChangeState(Executer.IS);
        }
    }
	
	public void PlayBowDrawSound()
	{
	}
	
	public void PlayWeaponAttackSound()
	{
        if (Executer && Executer.GetComponent<AllySoundHandler>())
            Executer.GetComponent<AllySoundHandler>().PlayWeaponAttackSound();
	}
	
	public void PlayBodyFallSound()
	{
		Executer.PlayBodyFallSound();
	}
	
	public void PlayShockWaveEffect()
	{
		return;
		/*
		if(Executer.FSM.IsInState(Executer.abilityManager.GetAbilityByID((uint)AbilityIDs.Shockwave_ID)))
		{
			Shockwave ability =	(Shockwave)(Executer.abilityManager.GetAbilityByID((uint)AbilityIDs.Shockwave_ID));
			ability.PlayShockWaveEffect();
		}*/
	}

    public void PlayIceBarricadeAnimation()
    {
        //return;
    }
	
	public void SetUpTrap(){
		
	}
	
	public void ThrowCaltrops()
	{
	}
	
	public void ShowMeleeTrail()
	{
        Ally_NormalAttackState meleestate = (Ally_NormalAttackState)Executer.abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID);
		if(meleestate.hand == 0)
		{
            if (Executer.EquipementMan.RightHandWeapon && Executer.EquipementMan.RightHandWeapon.GetComponent<MeleeWeaponTrail>())
			{
                Executer.EquipementMan.RightHandWeapon.GetComponent<MeleeWeaponTrail>().Emit = true;
			}
		}
		
		if(meleestate.hand == 1)
		{
            if (Executer.EquipementMan.LeftHandWeapon && Executer.EquipementMan.LeftHandWeapon.GetComponent<MeleeWeaponTrail>())
			{
                Executer.EquipementMan.LeftHandWeapon.GetComponent<MeleeWeaponTrail>().Emit = true;
			}
		}
			
			
		
		/*
		MeleeWeaponTrail[] meleeTrails = Player.Instance.GetComponentsInChildren<MeleeWeaponTrail>();
		if(meleeTrails.Length > 0)
		{
			foreach(MeleeWeaponTrail meleeTrail in meleeTrails)
			{
				meleeTrail.Emit = true;
			}
		}
		*/
	}
	
	public void HideMeleeTrail()
	{
        MeleeWeaponTrail[] meleeTrails = Executer.GetComponentsInChildren<MeleeWeaponTrail>();
		if(meleeTrails.Length > 0)
		{
			foreach(MeleeWeaponTrail meleeTrail in meleeTrails)
			{
				meleeTrail.Emit = false;
			}
		}
	}
}
