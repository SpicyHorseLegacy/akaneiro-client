using UnityEngine;
using System.Collections;

public class Ally_StunState : Ally_State
{
    public override void Enter()
    {
        base.Enter();
        Executer.PlayStunAnim();
        Executer.GetComponent<AllyMovement>().Freeze();
    }

    public override void Exit()
    {
        base.Exit();
        Executer.GetComponent<AllyMovement>().ReleaseFreeze();
    }
}
