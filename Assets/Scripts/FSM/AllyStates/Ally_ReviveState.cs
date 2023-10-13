using UnityEngine;
using System.Collections;

public class Ally_ReviveState : State {

	AllyNpc Executer;
	Transform Aka_Model;
	
	public void SetAlly(AllyNpc o)
	{
		Executer = o;
		Owner = Executer.transform;
	}
	
	public override void Enter()
	{
		Executer.PlayReviveAnim();
		Executer.IsRevive =true;
	    
		Aka_Model = Owner.FindChild("Aka_Model");

	}
	
	public override void Execute()
	{
		if( !Aka_Model.animation.isPlaying)
		    Executer.FSM.ChangeState(Executer.IS); 
	}
	
	public override void Exit()
	{
		Executer.IsDead = false;
	}
}
