using UnityEngine;
using System.Collections;

public class DamageState : PlayerState {

	float DamageTime = 0.2f;
	float curStateTime = 0f;
	
	public override void Enter()
	{
		player.PlayDamageAnim();
		curStateTime = 0.0f;
	}
	
	public override void Execute()
	{
		curStateTime += Time.deltaTime;
		
		if(!player.AnimationModel.animation.isPlaying || curStateTime > DamageTime || Input.GetMouseButtonDown(0))
		{
			player.FSM.ChangeState(player.IS);
		}
	}
}
