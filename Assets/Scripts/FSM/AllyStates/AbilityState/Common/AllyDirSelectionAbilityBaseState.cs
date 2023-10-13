using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllyDirSelectionAbilityBaseState : AllyAbilityBaseState
{
    protected Vector3 tempDir;

    public override void Enter()
    {
        base.Enter();
        SendUseAbilityRequest((uint)id, 0, tempDir);
    }

    public override void Exit()
    {
        base.Exit();
        tempDir = Vector3.zero;
    }

    public override bool CanUseAbility()
    {
        tempDir = (Executer.transform.position - Owner.position).normalized;
        return base.CanUseAbility();
    }
}