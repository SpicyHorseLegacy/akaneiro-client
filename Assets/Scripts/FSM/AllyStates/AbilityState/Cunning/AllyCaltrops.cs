using UnityEngine;
using System.Collections;

public class AllyCaltrops : AllyTargetPosAbilityBaseState
{
    public Transform ThrowCaltropsVFXPrefab;

    public Transform TrapPrefab;
    public Transform AbilityRangePrefab;

    public override void Enter()
    {
        base.Enter();
        Owner.GetComponent<AllyMovement>().StopMove(false);
    }

    public override void ProcessCasting()
    {
        base.ProcessCasting();
        //Play VFX 
        if (ThrowCaltropsVFXPrefab != null)
        {
            Vector3 pos = Owner.position + Owner.forward * 0.8f + Vector3.up * 0.3f;
            Quaternion rot = Owner.rotation;
            Transform obj = Object.Instantiate(ThrowCaltropsVFXPrefab, pos, rot) as Transform;
            Vector3 angle = obj.eulerAngles;
            angle.y += 270f;
            obj.eulerAngles = angle;
            obj.GetComponent<DestructAfterTime>().time = 2f;
        }
    }

    public override AbilityObject On_SkillObjectEnter(SSkillObjectEnter skillObjectInfo)
    {
        if (TrapPrefab)
        {
            Transform trap = Instantiate(TrapPrefab, skillObjectInfo.pos + Vector3.up * 0.2f, TrapPrefab.rotation) as Transform;
            trap.GetComponent<AbilityObject>().ObjID = skillObjectInfo.objectID;
            trap.GetComponent<AbilityObject>().TypeID = skillObjectInfo.objectTypeID;
            trap.GetComponent<AbilityObject>().DestAbility = this;
            trap.GetComponent<AbilityObject>().SkillObjectInfo = skillObjectInfo;
            trap.GetComponent<AbilityObject>().Init();
            return trap.GetComponent<AbilityObject>();
        }
        return null;
    }
}
