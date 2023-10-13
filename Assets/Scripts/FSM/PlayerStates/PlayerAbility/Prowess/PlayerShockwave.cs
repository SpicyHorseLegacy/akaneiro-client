using UnityEngine;
using System.Collections;

public class PlayerShockwave : PlayerAbilityBaseState {
	
	//Sound 
	public Transform SoundPrefab;
	
	//VFX
	public Transform ShockwaveVFXPrefab;
    public Transform ShockwaveImpactSoundPrefab;
	
	Transform sound;
	
	public AnimationClip ShockWaveAni_0H;
	public AnimationClip ShockWaveAni_1H;
	public AnimationClip ShockWaveAni_2H;
	public AnimationClip ShockWaveAni_2HNodachi;

	public override void Enter()
	{
		base.Enter();

		step = PrepareStep.WaitForServerCallback;
		
		PlayShockWaveAnim();
		
		// Aka stop movement
		Owner.GetComponent<PlayerMovement>().StopMove(false);
		Player.Instance.CanActiveAbility = false;
		
		// Send Message to Server
        SendUseAbilityRequest((uint)id, 0, Owner.position);
	}
	
	public override void Execute()
	{
        base.Execute();
	}
	
	public override void Exit()
	{
		base.Exit();
		Player.Instance.GetComponent<PlayerMovement>().bStopMove = false;
	}
	
	public override void UseAbilityResult(SUseSkillResult useSkillResult)
	{
		base.UseAbilityResult(useSkillResult);

		PlayShockWaveEffectAtPos( useSkillResult.pos );
		
		step = PrepareStep.WaitForAnimationFinish;
	}
	
	public void PlayShockWaveAnim()
	{
		WeaponBase.EWeaponType wt = Player.Instance.EquipementMan.GetWeaponType();

		if(wt == WeaponBase.EWeaponType.WT_NoneWeapon)
			playerController.abilityManager.AbiAniManager.ShockWaveActive = ShockWaveAni_0H;
        else if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
			playerController.abilityManager.AbiAniManager.ShockWaveActive = ShockWaveAni_1H;
		else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
			playerController.abilityManager.AbiAniManager.ShockWaveActive = ShockWaveAni_2H;
		else
			playerController.abilityManager.AbiAniManager.ShockWaveActive = ShockWaveAni_2HNodachi;

        AnimationModel.animation[playerController.abilityManager.AbiAniManager.ShockWaveActive.name].time = 0;
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.ShockWaveActive.name].wrapMode = WrapMode.ClampForever;
		AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.ShockWaveActive.name);
        PlaySound();
	}

    public void PlayShockWaveEffectAtPos( Vector3 pos )
	{
		if(CameraEffectManager.Instance)
		{
			CameraEffectManager.Instance.PlayShakingEffect("heavy");
		}
		
		//Particle
		if(ShockwaveVFXPrefab)
		{
            Object.Instantiate(ShockwaveVFXPrefab, pos + Vector3.up * 0.3f, Owner.GetComponent<PlayerMovement>().PlayerObj.rotation);
		}
        if (ShockwaveImpactSoundPrefab)
            SoundCue.PlayPrefabAndDestroy(ShockwaveImpactSoundPrefab, pos);
	}

    void PlaySound()
    {
        //set sound
        if (sound == null && SoundPrefab != null) sound = newSoundForAbility(SoundPrefab);
        if (sound) SoundCue.Play(sound.gameObject);
    }
}
