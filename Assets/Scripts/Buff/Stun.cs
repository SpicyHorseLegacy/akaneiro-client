using UnityEngine;
using System.Collections;

public class Stun : BaseBuff {
	
	Transform vfx;

	public override void Enter()
	{
		base.Enter();
		
		if(Owner)
		{
			if(VFXPrefab && !vfx)
			{
				vfx = Instantiate(VFXPrefab, Owner.position + Vector3.up * 1.2f, VFXPrefab.rotation) as Transform;
				vfx.parent = Owner;
			}

			NpcBase npc = Owner.GetComponent<NpcBase>();
			if(npc && !npc.FSM.IsInState(npc.DS))
			{
				npc.FSM.ChangeState(npc.StunState);
			}

            Player player = Owner.GetComponent<Player>();
            if (player && !player.FSM.IsInState(player.DS))
            {
                player.FSM.ChangeState(player.SS); 
            }

            AllyNpc _ally = Owner.GetComponent<AllyNpc>();
            if (_ally && !_ally.FSM.IsInState(_ally.DS))
            {
                _ally.FSM.ChangeState(_ally.SS);
            }
		}
	}
	
	public override void Execute(){
		base.Execute();
	}
	
	public override void Exit()
	{
		if(vfx)
			vfx.GetComponent<DestructAfterTime>().DestructNow();

        NpcBase npc = Owner.GetComponent<NpcBase>();
        if (npc && !npc.FSM.IsInState(npc.DS))
		{
			npc.AnimationModel.animation.Stop();
			npc.PlayIdleAnim();
		}

        Player player = Owner.GetComponent<Player>();
        if (player && !player.FSM.IsInState(player.DS))
        {
            player.FSM.ChangeState(player.IS);
        }

        AllyNpc _ally = Owner.GetComponent<AllyNpc>();
        if (_ally && !_ally.FSM.IsInState(_ally.DS))
        {
            _ally.FSM.ChangeState(_ally.IS);
        }
		
		base.Exit();
	}
}
