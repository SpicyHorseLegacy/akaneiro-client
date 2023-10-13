using UnityEngine;
using System.Collections;

public class KnockBackState : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        player.PlayStunAnim();
        player.GetComponent<PlayerMovement>().Freeze();
    }

    public override void Exit()
    {
        base.Exit();
        player.GetComponent<PlayerMovement>().ReleaseFreeze();
    }
}
