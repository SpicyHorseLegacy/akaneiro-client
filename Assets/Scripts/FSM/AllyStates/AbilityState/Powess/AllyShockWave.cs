using UnityEngine;
using System.Collections;

public class AllyShockWave : AllyCurPosAbilityBaseState {

	public override void Enter()
	{
		base.Enter();
        Owner.GetComponent<AllyMovement>().StopMove(false);
	}

    public override void ProcessCasting()
	{
        WeaponBase.EWeaponType wt = Executer.EquipementMan.GetWeaponType();

		if(wt == WeaponBase.EWeaponType.WT_NoneWeapon)
            castAnimationString = "Aka_Shockwave_0H";
		else if(wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
            castAnimationString = "Aka_Shockwave_1H";
		else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
            castAnimationString = "Aka_Shockwave_2H";	
		else
            castAnimationString = "Aka_Shockwave_2HNodachi";
		
		Executer.AnimationModel.animation.CrossFade(castAnimationString);
        
        base.ProcessCasting();
	}

    public override void ProcessImpactAtPos(Vector3 _pos)
    {
        base.ProcessImpactAtPos(_pos);
        if (CameraEffectManager.Instance)
            CameraEffectManager.Instance.PlayShakingEffect("heavy");
    }
}
