using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAbilitySwath : PlayerMovementAbilityBaseState {
	
	Vector3 TargetPoint = Vector3.zero;
	
	public Transform SoundPrefab;
	
	Transform sound;
	
	public AnimationClip SwathPrepareAni_0H;
	public AnimationClip SwathPrepareAni_1H;
	public AnimationClip SwathPrepareAni_2H;
	public AnimationClip SwathPrepareAni_2HNodachi;
	
	public AnimationClip SwathDashAni_0H;
	public AnimationClip SwathDashAni_1H;
	public AnimationClip SwathDashAni_2H;
	public AnimationClip SwathDashAni_2HNodachi;
	
	public AnimationClip SwathFinishAni_0H;
	public AnimationClip SwathFinishAni_1H;
	public AnimationClip SwathFinishAni_2H;
	public AnimationClip SwathFinishAni_2HNodachi;
	
	
	public Transform MoveMarkerPrefab;
	
	public override void Enter()
	{
		base.Enter();
		
		step = PrepareStep.WaitForMouseDown;
		
		// Aka stop movement
		Owner.GetComponent<PlayerMovement>().StopMove(false);
		
		playAnimation("B");
		
		Player.Instance.CanActiveAbility = false;
		
		//Dash();
	}
	
	public override void EnterInRunState()
	{
		base.EnterInRunState();
		
		step = PrepareStep.WaitForMouseDown;
		
		Owner.GetComponent<PlayerMovement>().bStopMove=true;
		
		Player.Instance.PGS.IsSkillOn = true;
		
		Player.Instance.CanActiveAbility = false;
		//Dash();
	}
	
	public override void PlayerAnimationAfterRun()
	{
		if(step != PrepareStep.WaitForAnimationFinish)
			playAnimation("B");
	}
	
	public override void Execute()
	{
		base.Execute();
		
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
				DashToMousePos(Input.mousePosition);
			}
#else
			
			if(Input.GetMouseButtonUp(0) && !_UI_CS_Ctrl.Instance.m_UI_Manager.GetComponent<UIManager>().DidAnyPointerHitUI())
			{
				DashToMousePos(Input.mousePosition);
			}
#endif
		}
	}
	
	public override void Exit()
	{
		Owner.GetComponent<PlayerMovement>().bStopMove=false;
		Player.Instance.PGS.IsSkillOn = false;
		Player.Instance.CanActiveAbility = true;
		
		// reset all walls which character through this time to Collider
		if(throughableWalls.Count > 0)
		{
			for(int i = throughableWalls.Count - 1; i >= 0; i--)
			{
				throughableWalls[i].isTrigger = false;
				throughableWalls.RemoveAt(i);
			}
		}
		
		base.Exit();
	}
	
	public override void AcitveAbilityWithMousePos (Vector3 mousePos)
	{
		base.AcitveAbilityWithMousePos (mousePos);
		
		DashToMousePos(mousePos);
	}
	
	public override void UseAbilityResult (SUseSkillResult useSkillResult)
	{
		base.UseAbilityResult (useSkillResult);
        //startMoveToTarget(useSkillResult.pos);
	}
	
	public override void UseAbilityFailed(uint skillID, EServerErrorType reason)
	{
		if(step != PrepareStep.WaitForMouseDown)
		{
			Owner.GetComponent<PlayerMovement>().IsMoving = false;
			Owner.GetComponent<PlayerMovement>().curPathPointIndex=0;
			Owner.GetComponent<PlayerMovement>().bStopMove=false;
			Owner.GetComponent<PlayerMovement>().IsDash = false;
		}
		
		base.UseAbilityFailed(skillID, reason);
	}
	
	public virtual void EndStep(bool isplayBulletTimeEffect)
	{
		step = PrepareStep.WaitForAnimationFinish;
		
		if(isplayBulletTimeEffect)
		{
			if(CameraEffectManager.Instance)
				CameraEffectManager.Instance.PlayBulletTimeEffect();
		}
		
		playAnimation("C");
	}

	private void playAnimation(string step)
	{
		playAnimationIsImmediatlly(step,false);
	}
	
	public virtual void playSoundAndVFX(){
		
		if(sound == null && SoundPrefab) sound = newSoundForAbility(SoundPrefab);
		if(sound)	SoundCue.PlayAtPosAndRotation(sound.gameObject, Owner.position, Owner.rotation);
		
		if(CameraEffectManager.Instance)
		{
			CameraEffectManager.Instance.PlayShakingEffect("normal");
		}
	}
	
	private void playAnimationIsImmediatlly(string step, bool playimm)
	{
        WeaponBase.EWeaponType wt = Player.Instance.EquipementMan.GetWeaponType();
		
		if(wt == WeaponBase.EWeaponType.WT_NoneWeapon){
			playerController.abilityManager.AbiAniManager.SwathPrepare = SwathPrepareAni_0H;
			playerController.abilityManager.AbiAniManager.SwathDash = SwathDashAni_0H;
			playerController.abilityManager.AbiAniManager.SwathFinish = SwathFinishAni_0H;
        }else if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon){
			playerController.abilityManager.AbiAniManager.SwathPrepare = SwathPrepareAni_1H;
			playerController.abilityManager.AbiAniManager.SwathDash = SwathDashAni_1H;
			playerController.abilityManager.AbiAniManager.SwathFinish = SwathFinishAni_1H;
		}else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe){
			playerController.abilityManager.AbiAniManager.SwathPrepare = SwathPrepareAni_2H;
			playerController.abilityManager.AbiAniManager.SwathDash = SwathDashAni_2H;
			playerController.abilityManager.AbiAniManager.SwathFinish = SwathFinishAni_2H;
		}else{
			playerController.abilityManager.AbiAniManager.SwathPrepare = SwathPrepareAni_2HNodachi;
			playerController.abilityManager.AbiAniManager.SwathDash = SwathDashAni_2HNodachi;
			playerController.abilityManager.AbiAniManager.SwathFinish = SwathFinishAni_2HNodachi;
		}
		
		string AniString = "";
		switch(step)
		{
		case "A":
			AniString = playerController.abilityManager.AbiAniManager.SwathPrepare.name;
			break;
			
		case "B":
			AniString = playerController.abilityManager.AbiAniManager.SwathDash.name;
			break;
			
		case "C":
			AniString = playerController.abilityManager.AbiAniManager.SwathFinish.name;
			break;
		}
		
		if(step == "C" || playimm)
		{
			// if the backtoidle evnent is not at the end of animation and player keep using the same ability, the next animation request would come before the first animation finished.
			// if don't restore the animation, the 2nd animation won't play, but just crossfade the idle animation, so player enters ability state and keep playing the 1st animation which has actived the event
			// so player couldn't back to normal state again.
			AnimationModel.animation[AniString].time = 0;
			AnimationModel.animation[AniString].wrapMode = WrapMode.ClampForever;
			AnimationModel.animation.Play(AniString);
		}else{
			AnimationModel.animation.CrossFade(AniString, 0.1f);
		}
	}
	
	private void startMoveToTarget( Vector3 tar )
	{
		// play sounds and vfx
		playSoundAndVFX();

        //Debug.LogError("move to target");
		Owner.GetComponent<PlayerMovement>().MoveTarget = tar;
		Owner.GetComponent<PlayerMovement>().IsDash = true;
		Player.Instance.PathFindingState = Player.EPathFindingState.PathFinding_Processing;
		Owner.GetComponent<Seeker>().StartPath(Owner.position, tar);
		
		Owner.GetComponent<PlayerMovement>().dashHitEnemy=false;
		
		step = PrepareStep.WaitForMoveDone;
		playAnimationIsImmediatlly("B",true);
		
		Debug.DrawRay(tar + Vector3.up * 7, Vector3.down * 7, Color.green, 5f);
		
		//draw move marker
		if(MoveMarkerPrefab != null)
		{
			Object.Instantiate(MoveMarkerPrefab, tar + Vector3.up*0.2f, Quaternion.identity);
		}
	}
	
	private void DashToMousePos( Vector3 mousePos )
	{
		//if(step == PrepareStep.WaitForMouseDown)
		{
			Ray ray = Camera.main.ScreenPointToRay(mousePos);
			RaycastHit hit;
			int layer = 1<<LayerMask.NameToLayer("Walkable");
			
			if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer))
			{
				//TurnToPoint(hit.point);
				Debug.DrawLine(ray.origin, hit.point, Color.yellow, 5f);
				Debug.DrawRay(hit.point + Vector3.up *5, Vector3.down * 5, Color.red, 5f);

				// Check if there is collison at the front of Character
				Vector3 targetPos = hit.point;
				targetPos = new Vector3(targetPos.x, Owner.position.y, targetPos.z);
	
				Vector3 dir = targetPos - Owner.position;
				dir = dir.normalized;
				Player.Instance.GetComponent<PlayerMovement>().PlayerObj.forward = dir;
						
                targetPos = checkIfWall(targetPos);
                targetPos = checkIfThroughWall(targetPos);
                //targetPos = AstarPath.GetNeasetPos(targetPos);

                //float dis = Vector3.Distance(targetPos, Owner.position);
                //if (dis > Info.EndDistance)
                //    dis = Info.EndDistance;
                //if (dis < Info.StartDistance)
                //    dis = Info.StartDistance;
				//targetPos = Owner.position + dir * dis;
				//targetPos = Player.Instance.transform.GetComponent<PlayerMovement>().pointOnTheGround( targetPos );
						
				// Send message to server
				SendUseAbilityRequest((uint)id, 0, targetPos);
                startMoveToTarget(targetPos);
                Debug.DrawLine(targetPos, targetPos + Vector3.up * 10, Color.blue, 10);
				//step = PrepareStep.WaitForServerCallback;

                Debug.DrawRay(targetPos + Vector3.up * 5, Vector3.down * 5, Color.red, 5f);
			}
		}
	}	
}
