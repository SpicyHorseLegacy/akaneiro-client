using UnityEngine;
using System.Collections;

public class NPC_WhirlWind : NPCAbilityBaseState {

    bool _isFirstGetAbilityResult = false;

    public override void Enter()
    {
        base.Enter();

        _isFirstGetAbilityResult = true;
    }

    public override void Exit()
    {
        base.Exit();
        StopVFX();
    }

    public override void UseAbilityResult(SUseSkillResult useSkillResult)
    {
        CS_SceneInfo.Instance.On_UpdateResult(this, useSkillResult);

        if (_isFirstGetAbilityResult)
        {
            _isFirstGetAbilityResult = false;
            PlayImpactVFXAndSound();
        }
    }

    [HideInInspector]
    public Transform vfxControl = null;
    public override void PlayImpactVFXAndSound()
    {
        Transform pos = AbilityImpactPosition;
        if (!pos) pos = Owner;

        if (AbilityImpactVFXPrefab)
        {
            vfxControl = Object.Instantiate(AbilityImpactVFXPrefab, pos.position, pos.rotation) as Transform;
            vfxControl.GetComponent<WhirlWindControl>().Go();
        }

        if (AbilityImpactSoundPrefab)
            SoundCue.PlayPrefabAndDestroy(AbilityImpactSoundPrefab, pos.position);
    }

    public void StopVFX()
    {
        if (vfxControl)
        {
            vfxControl.GetComponent<WhirlWindControl>().GoToHell();
            return;
        }
    }
}
