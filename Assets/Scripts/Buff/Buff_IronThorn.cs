using UnityEngine;
using System.Collections;

public class Buff_IronThorn : BaseBuff {

	Transform vfx;
	
	public Transform activeVFXPrefab;
	
	public Transform AbilityOnSoundPrefab;
	public Transform AbilityOffSoundPrefab;
	
	Transform AbilityOnSound;
	Transform AbilityOffSound;
	
	public override void Enter()
	{
		base.Enter();
		
		if(Owner)
		{
			Transform activeVFX =  Instantiate(activeVFXPrefab, Owner.position + Vector3.up * 1.2f, VFXPrefab.rotation) as Transform;
			activeVFX.parent = Owner;
			
			if(VFXPrefab && !vfx)
			{
				vfx = Instantiate(VFXPrefab, Owner.position + Vector3.up * 0.1f, VFXPrefab.rotation) as Transform;
				vfx.parent = Owner;
			}
			
			if(AbilityOnSound == null && AbilityOnSoundPrefab != null)		AbilityOnSound = Object.Instantiate(AbilityOnSoundPrefab) as Transform;
			if(AbilityOffSound == null && AbilityOffSoundPrefab != null)	AbilityOffSound = Object.Instantiate(AbilityOffSoundPrefab) as Transform;
			
			PlayAbilityOnSound();
		}
	}
	
	public override void Execute(){
		base.Execute();
	}
	
	public override void Exit()
	{
		if(vfx)
			vfx.GetComponent<DestructAfterTime>().DestructNow();
		
		PlayAbilityOffSound();
		
		base.Exit();
	}
	
	private void PlayAbilityOnSound()
	{	
		if(AbilityOnSound)
			SoundCue.Play(AbilityOnSound.gameObject);
	}
	
	private void PlayAbilityOffSound()
	{
		if(AbilityOffSound)
			SoundCue.Play(AbilityOffSound.gameObject);
	}
}
