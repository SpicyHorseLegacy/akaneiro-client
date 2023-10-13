using UnityEngine;
using System.Collections;

public class AllyTargetPosAbilityBaseState : AllyAbilityBaseState
{
    protected Vector3 tempTargetPos;

    public override void Enter()
    {
        base.Enter();
        SendUseAbilityRequest((uint)id, 0, tempTargetPos);
    }

    public override void Exit()
    {
        base.Exit();
        tempTargetPos = Vector3.zero;
    }

    public override bool CanUseAbility()
    {
        tempTargetPos = CheckIfInRange(Executer.AttackTarget.position);
        return base.CanUseAbility();
    }
}
