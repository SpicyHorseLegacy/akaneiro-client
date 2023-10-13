using UnityEngine;
using System.Collections;

public class AllyIceBarricade : AllyDirSelectionAbilityBaseState {

    public Transform SoundPrefab;
    public Transform VFXImpactSoundPrefab;
    public Transform IceMeshPrefab;
    public Transform VFXPrefab;

    Transform sound;
    Transform iceMesh;

    public override void Enter()
    {
        base.Enter();

        if (sound == null && SoundPrefab) sound = newSoundForAbility(SoundPrefab);
        if (!iceMesh && IceMeshPrefab) iceMesh = CS_Main.Instance.SpawnObject(IceMeshPrefab);

        Owner.GetComponent<AllyMovement>().bStopMove = true;

        PlayAnimation();
        PlaySound();
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

        PlaySoundAndVFXAtPos(useSkillResult.pos);
    }

    public void PlayAnimation()
    {
        // show weapons
        Executer.SetPlayerWeaponVisible(false);

        // if the backtoidle evnent is not at the end of animation and player keep using the same ability, the next animation request would come before the first animation finished.
        // if don't restore the animation, the 2nd animation won't play, but just crossfade the idle animation, so player enters ability state and keep playing the 1st animation which has actived the event
        // so player couldn't back to normal state again.
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.IceBarricadeActive.name].time = 0;
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.IceBarricadeActive.name].wrapMode = WrapMode.ClampForever;
        AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.IceBarricadeActive.name);
    }

    public void PlaySound()
    {
        if (sound) SoundCue.Play(sound.gameObject);
    }

    public void PlaySoundAndVFXAtPos(Vector3 pos)
    {
        if (CameraEffectManager.Instance)
        {
            CameraEffectManager.Instance.PlayShakingEffect();
        }

        if (VFXPrefab)
        {
            Vector3 vfxpos = Owner.position + Owner.GetComponent<PlayerMovement>().PlayerObj.forward * 1.5f + Vector3.up * 0.3f;
            Object.Instantiate(VFXPrefab, vfxpos, Owner.GetComponent<PlayerMovement>().PlayerObj.rotation);
        }
        if (iceMesh)
        {
            Vector3 meshpos = Owner.position + Owner.GetComponent<PlayerMovement>().PlayerObj.forward * 1.5f - Vector3.up * 0.1f;
            iceMesh.position = meshpos;
            iceMesh.rotation = Owner.GetComponent<PlayerMovement>().PlayerObj.rotation;
            iceMesh.GetComponent<IceObj>().Go();
        }

        if (VFXImpactSoundPrefab)
            SoundCue.PlayPrefabAndDestroy(VFXImpactSoundPrefab, transform.position);
    }
}
