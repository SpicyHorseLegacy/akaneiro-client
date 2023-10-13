using UnityEngine;
using System.Collections;

public class AllyInstanceAbility : AllyAbilityBaseState
{
    public override void Enter()
    {
        base.Enter();
        SendUseAbilityRequest((uint)id, 0, Owner.position);
    }

    public override void Execute()
    {
        base.Execute();
        Executer.FSM.RevertToPreviousState();
    }
}
