using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllyMeteorRain : AllyTargetPosAbilityBaseState
{
    public Transform MeteorRainPrefab;

    public override void Enter()
    {
        base.Enter();

        // ally stop movement
        Owner.GetComponent<AllyMovement>().StopMove(false);

        // hide weapons
        Executer.SetPlayerWeaponVisible(false);

        // play animation
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.MeteorActive.name].time = 0;
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.MeteorActive.name].wrapMode = WrapMode.ClampForever;
        AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.MeteorActive.name);

        // play cast sound
        SoundCue.PlayPrefabAndDestroy(CastSoundPrefab, tempTargetPos);

        SendUseAbilityRequest((uint)id, 0, tempTargetPos);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();

        // show weapons
        Executer.SetPlayerWeaponVisible(true);
        // switch pathfinding enabled
        Owner.GetComponent<AllyMovement>().bStopMove = false;
    }
    
    public override AbilityObject On_SkillObjectEnter(SSkillObjectEnter skillObjectInfo)
    {
        if (MeteorRainPrefab)
        {
            Transform meteorRain = CS_Main.Instance.SpawnObject(MeteorRainPrefab, skillObjectInfo.pos + Vector3.one * 0.2f, Quaternion.identity);
            MeteorRainObj mrobj = meteorRain.GetComponent<MeteorRainObj>();
            mrobj.ObjID = skillObjectInfo.objectID;
            mrobj.DestAbility = this;
            mrobj.SkillObjectInfo = skillObjectInfo;
            return mrobj;
        }

        return null;
    }
}
