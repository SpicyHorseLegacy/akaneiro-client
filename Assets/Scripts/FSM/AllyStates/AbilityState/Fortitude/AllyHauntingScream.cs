using UnityEngine;
using System.Collections;

public class AllyHauntingScream : AllyAbilityBaseState
{

    public Transform SoundPrefab;
    public Transform VFXPrefab;

    Transform sound;

    public override void Enter()
    {
        base.Enter();

        if (sound == null && SoundPrefab) sound = newSoundForAbility(SoundPrefab);

        // Send message to server
        SendUseAbilityRequest((uint)id, 0, Owner.position);

        // play animation
        PlayHauntingScreamAnimation();

        // play sound and vfx
        PlaySoundAndVFX();

        Owner.GetComponent<AllyMovement>().StopMove(false);
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
        Owner.GetComponent<AllyMovement>().bStopMove = false;
    }

    public override void UseAbilityResult(SUseSkillResult useSkillResult)
    {
        base.UseAbilityResult(useSkillResult);
    }

    /// <summary>
    /// Play animation decided by what kind of weapon cha holds
    /// </summary>
    public void PlayHauntingScreamAnimation()
    {
        // show weapons
        Executer.SetPlayerWeaponVisible(false);

        // if the backtoidle evnent is not at the end of animation and player keep using the same ability, the next animation request would come before the first animation finished.
        // if don't restore the animation, the 2nd animation won't play, but just crossfade the idle animation, so player enters ability state and keep playing the 1st animation which has actived the event
        // so player couldn't back to normal state again.
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.HauntingScreamActive.name].time = 0;
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.HauntingScreamActive.name].wrapMode = WrapMode.ClampForever;
        AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.HauntingScreamActive.name);
    }

    /// <summary>
    /// Play Sound and VFX
    /// </summary>
    public void PlaySoundAndVFX()
    {
        if (sound) SoundCue.Play(sound.gameObject);
        if (VFXPrefab)
        {
            Vector3 pos = Owner.position + Vector3.up * 0.8f;
            Object.Instantiate(VFXPrefab, pos, VFXPrefab.rotation);
        }
    }
}
