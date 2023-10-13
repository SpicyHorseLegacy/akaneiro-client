using UnityEngine;
using System.Collections;

public class PlayerAbilityHungryCleave : PlayerAbilityBaseState {
	
	public Transform SoundPrefab;
	public Transform VFXPrefab;
	
	Transform sound;
	
	
	public AnimationClip HungryCleaveAni_0H;
	public AnimationClip HungryCleaveAni_1H;
	public AnimationClip HungryCleaveAni_2H;
	public AnimationClip HungryCleaveAni_2HNodachi;
	
	public override void Enter()
	{
		base.Enter();
		
		if(sound == null && SoundPrefab)	sound = newSoundForAbility(SoundPrefab);
		
		step = PrepareStep.WaitForAnimationFinish;
		
		Owner.GetComponent<PlayerMovement>().StopMove(false);
	}
	
	public override void Execute()
	{
		base.Execute();
		
		if(step == PrepareStep.WaitForAnimationFinish)
		{
			if(!AnimationModel.animation.isPlaying){
				Player.Instance.FSM.ChangeState(Player.Instance.IS);
			}
		}
		
		if(step == PrepareStep.WaitForMouseDown || step == PrepareStep.WaitForMouseUp)
		{
			Player.Instance.GetComponent<PlayerMovement>().LookAtMousePoint(false);
#if NGUI
			if(Input.GetMouseButtonUp(0))
			{
				AcitveAbilityWithMousePos(Input.mousePosition);
			}
#else
			
			if(Input.GetMouseButtonUp(0) && !_UI_CS_Ctrl.Instance.m_UI_Manager.GetComponent<UIManager>().DidAnyPointerHitUI())
			{
				AcitveAbilityWithMousePos(Input.mousePosition);
			}
#endif
		}
	}
	
	public override void Exit()
	{
		base.Exit();
		Owner.GetComponent<PlayerMovement>().bStopMove = false;
		Player.Instance.CanActiveAbility = true;
	}
	
	public override void PrepareForAbilityWithoutKeyboardInput ()
	{
		base.PrepareForAbilityWithoutKeyboardInput ();

        if (Player.Instance.AttackTarget)
        {
            Player.Instance.GetComponent<PlayerMovement>().LookAtPosition(Player.Instance.AttackTarget.position);
            PlayHungryCleaveAnimation();
            Vector3 dir = Player.Instance.AttackTarget.position - Player.Instance.transform.position;
            dir.y = 0;
            SendUseAbilityRequest((uint)id, 0, dir);
        }
        else
        {
            Player.Instance.PlayIdleAnim(true);
            step = PrepareStep.WaitForMouseDown;
        }
	}
	
	public override void AcitveAbilityWithMousePos (Vector3 mousePos)
	{
		base.AcitveAbilityWithMousePos (mousePos);

        Owner.GetComponent<PlayerMovement>().LookAtMousePoint(mousePos, false);
		
		// play animation
		PlayHungryCleaveAnimation();
		// Send message to server
        SendUseAbilityRequest((uint)id, 0, Owner.GetComponent<PlayerMovement>().PlayerObj.forward);
		Player.Instance.CanActiveAbility = false;
	}
	
	public override void UseAbilityResult (SUseSkillResult useSkillResult)
	{
		base.UseAbilityResult (useSkillResult);
		
		PlaySoundAndVFX();
	}

	public void PlayHungryCleaveAnimation()
	{
		PlaySound();

        WeaponBase.EWeaponType wt = Player.Instance.EquipementMan.GetWeaponType();
		
		if(wt == WeaponBase.EWeaponType.WT_NoneWeapon)
			playerController.abilityManager.AbiAniManager.HungryCleaveActive = HungryCleaveAni_0H;
        else if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
			playerController.abilityManager.AbiAniManager.HungryCleaveActive = HungryCleaveAni_1H;
		else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
			playerController.abilityManager.AbiAniManager.HungryCleaveActive = HungryCleaveAni_2H;
		else
			playerController.abilityManager.AbiAniManager.HungryCleaveActive = HungryCleaveAni_2HNodachi;
		
		// if the backtoidle evnent is not at the end of animation and player keep using the same ability, the next animation request would come before the first animation finished.
		// if don't restore the animation, the 2nd animation won't play, but just crossfade the idle animation, so player enters ability state and keep playing the 1st animation which has actived the event
		// so player couldn't back to normal state again.
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.HungryCleaveActive.name].time = 0;
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.HungryCleaveActive.name].wrapMode = WrapMode.ClampForever;
		AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.HungryCleaveActive.name,0.1f);
	}
	
	void PlaySound()
	{
		if(sound)	SoundCue.Play(sound.gameObject);
	}
	
	public void PlaySoundAndVFX()
	{
		if(VFXPrefab)
		{
			Vector3 pos = Owner.position + Owner.GetComponent<PlayerMovement>().PlayerObj.forward*0.5f + Vector3.up*0.3f;
			Object.Instantiate(VFXPrefab, pos, Owner.GetComponent<PlayerMovement>().PlayerObj.rotation);
		}
	}
}
