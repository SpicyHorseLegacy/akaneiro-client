using UnityEngine;
using System.Collections;

public class BaseAttackableObject : BaseHitableObject {
    public AbilityManager abilityManager;                                   // every baseObject has an ability manager, which controls all ability this object has. Normal melee attack is also an ability

    [HideInInspector]
    public StateMachine FSM;                              // FSM controls what's the state of object, eg. IdleState, RunState etc.
	
}
