using UnityEngine;
using System.Collections;

public class Ally_State : State {

    protected AllyNpc Executer;

    public virtual void SetAlly(AllyNpc o)
    {
        Executer = o;
        Owner = Executer.transform;
    }
}
