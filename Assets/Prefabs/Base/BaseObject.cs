using UnityEngine;
using System.Collections;

public class BaseObject : BaseExportNode
{
    /* --------------------------------------------------------------------------------------------------------------------------------------------------------
     * Player
     * Ally
     * NPC
     * Enemy
     * IntercativeObject
     * Item
     * AbilityObject
     * are extended from BaseObject
     * --------------------------------------------------------------------------------------------------------------------------------------------------------
    */
    public int ObjID;                                   // Sync from server. Player is always "1"

    [HideInInspector]
    public ObjectType ObjType;

    public Transform AnimationModel;

    protected virtual void Awake() { }
}

public enum ObjectType
{
	Player,
	Ally,
	Enermy,
	NPC,
	IteractiveObj,
	BreakableObj,
	Item,
	NpcSpawner,
	Tigger,
}
