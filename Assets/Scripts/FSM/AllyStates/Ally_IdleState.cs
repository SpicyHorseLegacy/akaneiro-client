using UnityEngine;
using System.Collections;

public class Ally_IdleState : State {

	AllyNpc Executer;
	
	[HideInInspector]
	public bool IsAttackingSpot=false;

    bool isAttackIdleCounting = false;
    float attackIdleCountingTimer = 0f;
	
	public Ally_IdleState(AllyNpc o)
	{
		Executer = o;
		Owner = Executer.transform;
	}
	
	public void SetAlly(AllyNpc o)
	{
		Executer = o;
		Owner = Executer.transform;
	}
	
	public override void Enter()
	{
		Owner = Executer.transform;
		
		IsAttackingSpot=false;
		
		Executer.PlayIdleAnim(Executer.AttackTarget != null);
		
	}
	
	public override void Execute()
	{
//		if(Executer.IsAttackTargetLocked) 
//			return;
		
	    if(Executer.AttrMan.Attrs[EAttributeType.ATTR_CurHP] <= 0)
			return;
		
		if( (Owner.position - Player.Instance.transform.position).magnitude  > Executer.MaxAllyDitance)
		{
			Executer.ForceToFindPath();
		}
		else
		{
			if(Executer.ThinkAI())
		       Executer.FindMoveTarget();
		}
	
	}
	
	public override void Exit()
	{
		IsAttackingSpot=false;
	}

    public void EnableAttackIdle()
    {
        isAttackIdleCounting = true;
        attackIdleCountingTimer = 2;
    }
}
