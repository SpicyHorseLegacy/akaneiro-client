using UnityEngine;
using System.Collections;

public class PlayerState : State {

    protected Player player;

    public void Initial()
    {
        Owner = Player.Instance.transform;
        player = Owner.GetComponent<Player>();
    }
}
