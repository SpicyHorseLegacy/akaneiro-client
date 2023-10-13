using UnityEngine;
using System.Collections;

public class Buff_DarkHunter : BaseBuff
{
    public Transform Active_VFX_Prefab;
    Transform vfx;

    public Transform AbilityOnSoundPrefab;
    public Transform AbilityOffSoundPrefab;

    public override void Enter()
    {
        base.Enter();

        if (Owner)
        {
            if (Active_VFX_Prefab)
            {
                Transform activevfx = Instantiate(Active_VFX_Prefab, Owner.position, Active_VFX_Prefab.rotation) as Transform;
                activevfx.parent = Owner;
            }

            if (VFXPrefab && !vfx)
            {
                vfx = Instantiate(VFXPrefab, Owner.position + Vector3.up * 0.1f, VFXPrefab.rotation) as Transform;
                vfx.parent = Owner;
            }
        }

        if (AbilityOnSoundPrefab != null)
        {
            SoundCue.PlayPrefabAndDestroy(AbilityOnSoundPrefab, Owner.position);
        }
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        if (vfx)
            vfx.GetComponent<DestructAfterTime>().DestructNow();

        if (AbilityOffSoundPrefab != null)
        {
            SoundCue.PlayPrefabAndDestroy(AbilityOffSoundPrefab, Owner.position);
        }

        base.Exit();
    }
}
