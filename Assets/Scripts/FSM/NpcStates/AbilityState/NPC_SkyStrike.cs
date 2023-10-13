using UnityEngine;
using System.Collections;

public class NPC_SkyStrike : NPCAbilityBaseState
{
	public AnimationClip ShootAnimation;
	public Transform ShootVFXPrefab;
	public Transform ShootSoundPrefab;
	
	public AnimationClip LoopAnimation;
	public Transform LoopVFXPrefab;
	public Transform LoopSoundPrefab;
	Transform loopVFXInstance;
	Transform loopSoundInstance;
	
	public AnimationClip EndAnimation;
	
	public Transform ProtectVFXPrefab;
	
	public Transform ProjectileVFXPrefab;
	
	public string LaurncherBoneName;
	Transform LaurncherTransform;
	

	enum SkyStrikeSteps
	{
		None,
		PrepareAnim,
		LoopAnim,
		EndAnim,
	}
	SkyStrikeSteps animStep = SkyStrikeSteps.PrepareAnim;
	float animTime = 0;
	
	public override void Enter ()
	{
		CastAnimation.wrapMode = WrapMode.Once;
		
		base.Enter ();
		
		if(ProtectVFXPrefab)
		{
			Instantiate(ProtectVFXPrefab, Owner.position, Quaternion.identity);
		}
		
		animStep = SkyStrikeSteps.PrepareAnim;
		LaurncherTransform = AnimationModel.Find(LaurncherBoneName);
	}
	
	public override void Execute ()
	{
		base.Execute ();
		
		if(animStep == SkyStrikeSteps.PrepareAnim)
		{
			animTime -= Time.deltaTime;
			if(animTime < 0)
			{
				LoopAnimation.wrapMode = WrapMode.Loop;
				AnimationModel.animation.CrossFade(LoopAnimation.name);
				
				animStep = SkyStrikeSteps.LoopAnim;
				animTime = Info.AnimationDuration - CastAnimation.length - EndAnimation.length;
				
				if(LaurncherTransform)
				{
					if(LoopVFXPrefab && !loopVFXInstance)
					{
						loopVFXInstance = Instantiate(LoopVFXPrefab, LaurncherTransform.position, LaurncherTransform.rotation) as Transform;
						loopVFXInstance.parent = LaurncherTransform;
					}
					if(LoopSoundPrefab && !loopSoundInstance)
					{
						loopSoundInstance = Instantiate(LoopSoundPrefab, LaurncherTransform.position, LaurncherTransform.rotation) as Transform;
						loopSoundInstance.parent = LaurncherTransform;
						SoundCue.Play(loopSoundInstance);
					}
				}else
				{
					Debug.LogWarning("[ABI] " + Owner.name + " : [Sky Strike] Can not find the laurncher transform.");
				}
			}
		}
		if(animStep == SkyStrikeSteps.LoopAnim)
		{
			animTime -= Time.deltaTime;
			if(animTime < 0)
			{
				AnimationModel.animation.CrossFade(EndAnimation.name);
				animStep = SkyStrikeSteps.EndAnim;
				animTime = EndAnimation.length- 0.5f;
				
				if(loopVFXInstance)
					DestructAfterTime.DestructGameObjectNow(loopVFXInstance);
				if(loopSoundInstance)
					DestructAfterTime.DestructGameObjectNow(loopSoundInstance);
				
				return;
			}
		}
		if(animStep == SkyStrikeSteps.EndAnim)
		{
			animTime -= Time.deltaTime;
			if(animTime < 0)
			{
				npcController.PlayIdleAnim();
				animStep = SkyStrikeSteps.None;
				return;
			}
		}
	}
	
	public override void Exit ()
	{
		base.Exit ();
		
		if(loopVFXInstance)
			DestructAfterTime.DestructGameObjectNow(loopVFXInstance);
		if(loopSoundInstance)
			DestructAfterTime.DestructGameObjectNow(loopSoundInstance);
	}
	
	public override AbilityObject On_SkillObjectEnter (SSkillObjectEnter skillObjectInfo)
	{
		ShootAnimation.wrapMode = WrapMode.Once;
		AnimationModel.animation.CrossFade(ShootAnimation.name);
		
		Instantiate(ShootVFXPrefab, LaurncherTransform.position, Quaternion.identity);
		SoundCue.PlayPrefabAndDestroy(ShootSoundPrefab, LaurncherTransform.position);

		if (ProjectileVFXPrefab)
        {
            Transform _temProjectile = Instantiate(ProjectileVFXPrefab, skillObjectInfo.pos + Vector3.up * 0.1f, ProjectileVFXPrefab.rotation) as Transform;
            _temProjectile.GetComponent<AbilityObject>().ObjID = skillObjectInfo.objectID;
            _temProjectile.GetComponent<AbilityObject>().TypeID = skillObjectInfo.objectTypeID;
            _temProjectile.GetComponent<AbilityObject>().DestAbility = this;
            _temProjectile.GetComponent<AbilityObject>().SkillObjectInfo = skillObjectInfo;
            _temProjectile.GetComponent<AbilityObject>().Init();
            return _temProjectile.GetComponent<AbilityObject>();
        }
        return null;;
	}
	
	public override void On_SkillObjectActive (int objID)
	{
		base.On_SkillObjectActive (objID);
	}
	
	public override void PlayCastAnimaitonAndSound ()
	{
		base.PlayCastAnimaitonAndSound ();
		animStep = SkyStrikeSteps.PrepareAnim;
		animTime = CastAnimation.length - 0.5f;
	}
}