using UnityEngine;
using System.Collections;

public class AllyRainOfBlow : AllyDirSelectionAbilityBaseState
{
    //Sound
    public Transform SoundPrefab;

    //VFX
    public Transform UseAbilityVFXPrefab;

    [HideInInspector]
    public Transform sound;

    public AnimationClip RainBlow_0H;
    public AnimationClip RainBlow_1H;
    public AnimationClip RainBlow_2H;
    public AnimationClip RainBlow_2HNodachi;

    public override void Enter()
    {
        base.Enter();
        //init sound and vfx
        if (sound == null && SoundPrefab != null) sound = newSoundForAbility(SoundPrefab);

        Owner.GetComponent<AllyMovement>().StopMove(false);

        PlayRainOfBlowAnim();
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        Owner.GetComponent<AllyMovement>().bStopMove = false;
    }


    public void PlayRainOfBlowAnim()
    {
        WeaponBase.EWeaponType wt = Player.Instance.EquipementMan.GetWeaponType();

        if (wt == WeaponBase.EWeaponType.WT_NoneWeapon)
            playerController.abilityManager.AbiAniManager.RainBlowActive = RainBlow_0H;
        else if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
            playerController.abilityManager.AbiAniManager.RainBlowActive = RainBlow_1H;
        else if (wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
            playerController.abilityManager.AbiAniManager.RainBlowActive = RainBlow_2H;
        else
            playerController.abilityManager.AbiAniManager.RainBlowActive = RainBlow_2HNodachi;

        // play animation
        // if the backtoidle evnent is not at the end of animation and player keep using the same ability, the next animation request would come before the first animation finished.
        // if don't restore the animation, the 2nd animation won't play, but just crossfade the idle animation, so player enters ability state and keep playing the 1st animation which has actived the event
        // so player couldn't back to normal state again.
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.RainBlowActive.name].time = 0;
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.RainBlowActive.name].speed = 2;
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.RainBlowActive.name].wrapMode = WrapMode.ClampForever;
        AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.RainBlowActive.name, 0.1f);

        PlaySound();
    }

    public void PlayVFX()
    {

        if (UseAbilityVFXPrefab)
        {
            Vector3 pos = Player.Instance.transform.position + Vector3.up + Player.Instance.transform.GetComponent<PlayerMovement>().PlayerObj.forward * 1.5f;
            Quaternion rotation = Player.Instance.transform.rotation;
            Transform particle = CS_Main.Instance.SpawnObject(UseAbilityVFXPrefab, pos, rotation);
        }
    }

    void PlaySound()
    {
        //play sound
        if (sound)
        {
            SoundCue.Play(sound.gameObject);
        }
    }
}
