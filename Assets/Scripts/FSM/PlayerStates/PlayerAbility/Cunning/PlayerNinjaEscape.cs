using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerNinjaEscape : PlayerMovementAbilityBaseState {
	
	public Transform SoundPrefab;
	public Transform DisappearPrefab;
	public Transform AppearPrefab;
	
	Transform sound;
	
	public override void Enter()
	{
		base.Enter();
		
		step = PrepareStep.WaitForMouseDown;
		
		Owner.GetComponent<PlayerMovement>().StopMove(false);
		
		playPrepareAniamtion();
	}
	
	public override void EnterInRunState()
	{
		base.EnterInRunState();
		
		step = PrepareStep.WaitForMouseDown;
		
		Player.Instance.PGS.IsSkillOn = true;
		
		Player.Instance.CanActiveAbility = false;
	}
	
	public override void Execute()
	{
		base.Execute();
		
		if(step == PrepareStep.WaitForAnimationFinish)
		{
			if(!AnimationModel.animation.isPlaying)
				Player.Instance.FSM.ChangeState(Player.Instance.IS);
		}
		
		if(step == PrepareStep.WaitForMouseDown || step == PrepareStep.WaitForMouseUp)
		{
			if(!Player.Instance.GetComponent<PlayerMovement>().IsMoving)
				Player.Instance.GetComponent<PlayerMovement>().LookAtMousePoint(true);
			
#if NGUI
			if(Input.GetMouseButtonDown(0) && step == PrepareStep.WaitForMouseDown)
			{
				step = PrepareStep.WaitForMouseUp;
			}
			if(Input.GetMouseButtonUp(0) && step == PrepareStep.WaitForMouseUp)
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
		
		// show weapons
		Player.Instance.SetPlayerWeaponVisible(true);
		Owner.GetComponent<PlayerMovement>().bStopMove = false;
		Player.Instance.CanActiveAbility = true;
		
		Player.Instance.PGS.IsSkillOn = false;
		
		if(throughableWalls.Count > 0)
		{
			for(int i = throughableWalls.Count - 1; i >= 0; i--)
			{
				throughableWalls[i].isTrigger = false;
				throughableWalls.RemoveAt(i);
			}
		}
	}
	
	public override void PlayerAnimationAfterRun()
	{
		Player.Instance.SetPlayerWeaponVisible(false);
		
		playPrepareAniamtion();
	}
	
	public override void PrepareForAbilityWithoutKeyboardInput ()
	{
		base.PrepareForAbilityWithoutKeyboardInput ();
		
		step = PrepareStep.WaitForMouseDown;
	}
	
	public override void AcitveAbilityWithMousePos (Vector3 mousePos)
	{
		base.AcitveAbilityWithMousePos (mousePos);
		
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		RaycastHit hit;
		int layer = 1<<LayerMask.NameToLayer("Walkable");
		if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer))
		{
			Owner.GetComponent<PlayerMovement>().LookAtPosition(hit.point);
			Vector3 targetPos = hit.point;
			float dis = Vector3.Distance(targetPos,Owner.position);
            if (dis > Info.EndDistance)
                dis = Info.EndDistance;

            targetPos = checkIfWall(targetPos);
            targetPos = checkIfThroughWall(targetPos);

			// Send message to server
			SendUseAbilityRequest((uint)id, 0, targetPos);
            moveAkaToTargetPos(targetPos);
			
			//step = PrepareStep.WaitForServerCallback;
			Player.Instance.CanActiveAbility = false;

			step = PrepareStep.WaitForServerCallback;
		}
	}
	
	public override void UseAbilityResult (SUseSkillResult useSkillResult)
	{
        //Debug.Log("Result from NinjaEscape");
		base.UseAbilityResult (useSkillResult);
	}
	
	private void playPrepareAniamtion()
	{
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.NinjaEscapePrepare.name].time = 0;
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.NinjaEscapePrepare.name].wrapMode = WrapMode.Loop;
		AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.NinjaEscapePrepare.name);
		
		// hide weapons
		Player.Instance.SetPlayerWeaponVisible(false);
	}
	
	private void moveAkaToTargetPos(Vector3 tar)
	{
		Vector3 disappearPos = Owner.position;
		
		// if the backtoidle evnent is not at the end of animation and player keep using the same ability, the next animation request would come before the first animation finished.
		// if don't restore the animation, the 2nd animation won't play, but just crossfade the idle animation, so player enters ability state and keep playing the 1st animation which has actived the event
		// so player couldn't back to normal state again.
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.NinjaEscapeActive.name].time = 0;
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.NinjaEscapeActive.name].wrapMode = WrapMode.ClampForever;
		AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.NinjaEscapeActive.name);
		
		Vector3 destPosition = Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(tar);

        if (destPosition != Owner.position)
        {
            // check if there is an audio trigger
            RaycastHit[] hits = Physics.RaycastAll(Owner.transform.position + Vector3.up * 1f,
                                                    Player.Instance.GetComponent<PlayerMovement>().PlayerObj.forward,
                                                    Vector3.Distance(Owner.transform.position + Vector3.up * 1f, destPosition + Vector3.up * 1f));
            if (hits.Length > 0)
            {
                foreach (RaycastHit _hit in hits)
                {
                    if (_hit.transform.GetComponent<AudioTrigger>())
                    {
                        _hit.transform.GetComponent<AudioTrigger>().ActiveTrigger();
                    }
                }
            }

            Owner.position = destPosition;
        }
		//Debug.DrawRay(Owner.position+Vector3.up * 5f, Vector3.down * Vector3.Distance(hit.point, Owner.position + Vector3.up * 5f), Color.red, 5f);
        Player.Instance.GetComponent<PlayerMovement>().SetTriggersAroundPlayer();
		
		// play sound an vfx
		playSoundAndVFXAt(disappearPos, Owner.position);
		
		step = PrepareStep.WaitForAnimationFinish;
	}
	
	public void playSoundAndVFXAt( Vector3 disappearPos,  Vector3 appearPos )
	{
		//Particle
		if(DisappearPrefab)
			Object.Instantiate(DisappearPrefab, disappearPos, DisappearPrefab.rotation);
		if(AppearPrefab)
			Object.Instantiate(AppearPrefab, appearPos, AppearPrefab.rotation);
		// set sound
		if(sound == null && SoundPrefab != null)	sound = newSoundForAbility(SoundPrefab);
		if(sound)	SoundCue.Play(sound.gameObject);
		
	}
}