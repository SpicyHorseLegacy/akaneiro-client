using UnityEngine;
using System.Collections;

public class AllyCurPosAbilityBaseState : AllyAbilityBaseState
{
    public override void Enter()
    {
        base.Enter();
        SendUseAbilityRequest((uint)id, 0, Owner.position);
    }
	
}
