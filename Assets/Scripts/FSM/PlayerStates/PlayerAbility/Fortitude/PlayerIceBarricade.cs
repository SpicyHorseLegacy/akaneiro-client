using UnityEngine;
using System.Collections;

public class PlayerIceBarricade : PlayerAbilityBaseState {
	
	public Transform SoundPrefab;
    public Transform VFXImpactSoundPrefab;
	public Transform IceMeshPrefab;
	public Transform VFXPrefab;
    
	Transform sound;
	Transform iceMesh;
	
	public override void Enter()
	{
		base.Enter();
		
		if(sound == null && SoundPrefab)	sound = newSoundForAbility(SoundPrefab);
        if (!iceMesh && IceMeshPrefab)
        {
            iceMesh = CS_Main.Instance.SpawnObject(IceMeshPrefab);
            iceMesh.position = Vector3.one * 1000.0f;
        }
		
		Owner.GetComponent<PlayerMovement>().StopMove(false);

        PlayAnimation();
        PlaySound();

        // Send message to server
        SendUseAbilityRequest((uint)id, 0, Owner.position);
        Player.Instance.CanActiveAbility = false;
        step = PrepareStep.WaitForServerCallback;
	}
	
	public override void Execute()
	{
		if(step == PrepareStep.WaitForAnimationFinish)
		{
			if(!AnimationModel.animation.isPlaying){
				Player.Instance.FSM.ChangeState(Player.Instance.IS);
			}
		}
		
        /*
		if(step == PrepareStep.WaitForMouseDown || step == PrepareStep.WaitForMouseUp)
		{
			Player.Instance.GetComponent<PlayerMovement>().LookAtMousePoint(true);
			if(Input.GetMouseButtonUp(0) && !_UI_CS_Ctrl.Instance.m_UI_Manager.GetComponent<UIManager>().DidAnyPointerHitUI())
			{
                AcitveAbilityWithMousePos(Input.mousePosition);
			}
		}
		*/
		base.Execute();
	}
	
	public override void Exit()
	{
		base.Exit();
		// show weapons
		Player.Instance.SetPlayerWeaponVisible(true);

		Owner.GetComponent<PlayerMovement>().bStopMove = false;

		Player.Instance.CanActiveAbility = true;
	}
	
	public override void PrepareForAbilityWithoutKeyboardInput ()
	{
		base.PrepareForAbilityWithoutKeyboardInput ();
		
		//Player.Instance.PlayIdleAnim(true);
		
		//step = PrepareStep.WaitForMouseDown;
	}
	
	public override void AcitveAbilityWithMousePos (Vector3 mousePos)
	{
		base.AcitveAbilityWithMousePos (mousePos);
		/*
		Vector3 dir = Vector3.zero;
		if(Player.Instance.AttackTarget){
			dir = Player.Instance.AttackTarget.position - Owner.position;
			dir.y = 0;
		}else{
			Owner.GetComponent<PlayerMovement>().LookAtMousePoint(mousePos, true);

			Ray ray = Camera.main.ScreenPointToRay(mousePos);
			RaycastHit hit;
			int layer = 1<<LayerMask.NameToLayer("Walkable");
			if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer) && Time.timeScale > 0){
				dir = hit.point - Owner.position;
				dir.y = 0;
			}
		}
		
		PlayAnimation();
		PlaySound();
		
		// Send message to server
		SendUseAbilityRequest((uint)id, 0, dir);
		Player.Instance.CanActiveAbility = false;
		step = PrepareStep.WaitForServerCallback;
         */
	}
	
	public override void UseAbilityResult (SUseSkillResult useSkillResult)
	{
		base.UseAbilityResult (useSkillResult);
		
		PlaySoundAndVFXAtPos( useSkillResult.pos );
	}

    public void PlayAnimation()
    {
        // show weapons
        Player.Instance.SetPlayerWeaponVisible(false);

        // if the backtoidle evnent is not at the end of animation and player keep using the same ability, the next animation request would come before the first animation finished.
        // if don't restore the animation, the 2nd animation won't play, but just crossfade the idle animation, so player enters ability state and keep playing the 1st animation which has actived the event
        // so player couldn't back to normal state again.
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.IceBarricadeActive.name].time = 0;
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.IceBarricadeActive.name].wrapMode = WrapMode.ClampForever;
        AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.IceBarricadeActive.name);
    }
	
	public void PlaySound()
	{
		if(sound)	SoundCue.Play(sound.gameObject);
	}

    public void PlaySoundAndVFXAtPos( Vector3 pos)
    {
        //Debug.Log("ICE POS  " + pos);

		if(CameraEffectManager.Instance)
		{
			CameraEffectManager.Instance.PlayShakingEffect();
		}
		
		if(VFXPrefab)
		{
            Vector3 vfxpos = pos + Vector3.up * 0.3f;
			Object.Instantiate(VFXPrefab, vfxpos, Owner.GetComponent<PlayerMovement>().PlayerObj.rotation);
		}
		if(iceMesh)
		{
            Vector3 meshpos = pos - Vector3.up * 0.1f;
			iceMesh.position = meshpos;
			iceMesh.rotation = Owner.GetComponent<PlayerMovement>().PlayerObj.rotation;
			iceMesh.GetComponent<IceObj>().Go();
		}

        if (VFXImpactSoundPrefab)
            SoundCue.PlayPrefabAndDestroy(VFXImpactSoundPrefab, transform.position);
	}
}
