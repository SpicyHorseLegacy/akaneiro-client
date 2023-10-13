using UnityEngine;
using System.Collections;

public class PlayerInstanceAbility : PlayerAbilityBaseState
{
    public override void Enter()
    {
        base.Enter();
        SendUseAbilityRequest((uint)id, 0, Owner.position);
    }

    public override void Execute()
    {
        base.Execute();
        Player.Instance.FSM.RevertToPreviousState();
    }
}
