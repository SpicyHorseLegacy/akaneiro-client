using UnityEngine;
using System.Collections;

public class Knockback : BaseBuff {

	public override void Enter()
	{
		base.Enter();
		
		if(Owner)
		{
			NpcBase npc = Owner.GetComponent<NpcBase>();
			if(npc && !npc.FSM.IsInState(npc.DS))
			{
				npc.FSM.ChangeState(npc.KnockbackState);
			}

            // player's solution is in function "AddBuff" of "Player" script
		}
	}
	
	public override void Execute(){
		base.Execute();
	}
	
	public override void Exit()
	{
        NpcBase npc = Owner.GetComponent<NpcBase>();
		if(npc && !npc.FSM.IsInState(npc.DS))
		{
            npc.PlayIdleAnim();
            if (npc.FSM.IsInState(npc.KnockbackState))
                npc.KnockbackState.ExitState();
		}

        Player _player = Owner.GetComponent<Player>();
        if (_player && !_player.FSM.IsInState(_player.DS))
        {
            _player.FSM.ChangeState(_player.IS);
        }
		
		base.Exit();
	}
}
