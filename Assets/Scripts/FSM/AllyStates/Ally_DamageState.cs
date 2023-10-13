using UnityEngine;
using System.Collections;

public class Ally_DamageState : State {

	AllyNpc Executer;
	Transform Aka_Model;
	
	float DamageTime = 0.2f;
	float curStateTime = 0f;
	
	public void SetAlly(AllyNpc o)
	{
		Executer = o;
		Owner = Executer.transform;
	}
	
	public override void Enter()
	{
		Owner = Executer.transform;
		

		Aka_Model = Owner.FindChild("Aka_Model");
		
		Executer.PlayDamageAnim();
		curStateTime = 0.0f;
	}
	
	public override void Execute()
	{
		curStateTime += Time.deltaTime;
		
		if(!Aka_Model.animation.isPlaying || curStateTime > DamageTime )
		{
			Executer.FSM.ChangeState(Executer.IS);
		}
	}
	
	public override void Exit()
	{
		
	}
}
