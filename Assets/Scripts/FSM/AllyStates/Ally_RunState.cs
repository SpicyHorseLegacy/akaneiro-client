using UnityEngine;
using System.Collections;

public class Ally_RunState : Ally_State {

	public override void Enter()
	{
        base.Enter();
		PlayRunAnim();
        Debug.Log("ally run enter");
	}

	public override void Execute()
	{
        base.Execute();

		if(Executer == null)
			return;
		
		if( (Owner.position - Player.Instance.transform.position).magnitude  > Executer.MaxAllyDitance)
		{
			Executer.ForceToFindPath();
			return;
		}

        if (Executer.AttackTarget)
        {
            //if(Vector3.Distance(Executer.AttackTarget.position, Owner.position) < Executer.AttackRange)
            //{
            //    Executer.UseSkill();
            //}
            Executer.AttackEnemy(Executer.AttackTarget);
        }
        else
        {
            if (Executer.ThinkAI())
                Executer.FindMoveTarget();
        }
	}
	
	public override void Exit()
	{
		//print("Exit Ally Run State!");
	    AllyMovement pm = GetComponent<AllyMovement>();

        if (pm != null)
        {
            pm.IsMoving = false;
        }
		
	}	
	
	public void PlayRunAnim()
	{
        //Debug.Log("ally play run animation");

        WeaponBase.EWeaponType wt = Executer.EquipementMan.GetWeaponType();
		
		if(wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_NoneWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
            Executer.AnimationModel.animation.CrossFade("Aka_1H_Run");	
		else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
            Executer.AnimationModel.animation.CrossFade("Aka_2H_Run");	
		else
            Executer.AnimationModel.animation.CrossFade("Aka_2HNodachi_Run");			

	}
}
