using UnityEngine;
using System.Collections;

public class Player_Revival : PlayerShockwave {

    public override void Exit()
    {
        base.Exit();

        Player.Instance.ReactivePlayer();
    }
}
