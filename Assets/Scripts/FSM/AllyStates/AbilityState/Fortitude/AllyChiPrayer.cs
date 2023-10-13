using UnityEngine;
using System.Collections;

public class AllyChiPrayer : AllyAbilityBaseState {

    public Transform SoundPrefab;
    Transform sound;

    public override void Enter()
    {
        base.Enter();

        // Send message to server
        SendUseAbilityRequest((uint)id, 0, Owner.position);

        if (sound == null && SoundPrefab) sound = newSoundForAbility(SoundPrefab);

        PlayChiPrayerAnimation();
        PlayActiveVFXAndSound();

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

    public void PlayChiPrayerAnimation()
    {
        // hide weapons
        Executer.SetPlayerWeaponVisible(false);

        // play animation
        //AnimationModel.animation[playerController.abilityManager.AbiAniManager.ChiPrayerPrepare.name].time = 0;
        //AnimationModel.animation[playerController.abilityManager.AbiAniManager.ChiPrayerPrepare.name].wrapMode = WrapMode.Once;
        //AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.ChiPrayerPrepare.name);

        // if the backtoidle evnent is not at the end of animation and player keep using the same ability, the next animation request would come before the first animation finished.
        // if don't restore the animation, the 2nd animation won't play, but just crossfade the idle animation, so player enters ability state and keep playing the 1st animation which has actived the event
        // so player couldn't back to normal state again.
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.ChiPrayerActive.name].time = 0;
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.ChiPrayerActive.name].wrapMode = WrapMode.ClampForever;
        AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.ChiPrayerActive.name);
    }

    void PlayActiveVFXAndSound()
    {
        if (sound) SoundCue.Play(sound.gameObject);
    }
}
