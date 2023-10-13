using UnityEngine;
using System.Collections;

public class PlayerCalTrops : PlayerAbilityTrap {
	//VFX
	public Transform ThrowCaltropsVFXPrefab;
	
	public override void PrepareForAbilityWithoutKeyboardInput ()
	{
		base.PrepareForAbilityWithoutKeyboardInput ();
		
		AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.CaltropPrepare.name);
	}
	
	public override void AcitveAbilityWithMousePos (Vector3 mousePos)
	{
		base.AcitveAbilityWithMousePos (mousePos);
		
		// play animation
		// if the backtoidle evnent is not at the end of animation and player keep using the same ability, the next animation request would come before the first animation finished.
		// if don't restore the animation, the 2nd animation won't play, but just crossfade the idle animation, so player enters ability state and keep playing the 1st animation which has actived the event
		// so player couldn't back to normal state again.
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.CaltropActive.name].time = 0;
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.CaltropActive.name].wrapMode = WrapMode.ClampForever;
		AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.CaltropActive.name);
	}

    public override AbilityObject On_SkillObjectEnter(SSkillObjectEnter skillObjectInfo)
    {
        return base.On_SkillObjectEnter(skillObjectInfo);
    }

    public override void PlaySoundAndVFX()
	{
        base.PlaySoundAndVFX();
		
		//Play VFX 
		if(ThrowCaltropsVFXPrefab !=  null)
		{
			Vector3 pos = Owner.position + Owner.GetComponent<PlayerMovement>().PlayerObj.forward * 0.8f + Vector3.up * 0.3f;
			Quaternion rot = Owner.GetComponent<PlayerMovement>().PlayerObj.rotation;
			Transform obj = Object.Instantiate(ThrowCaltropsVFXPrefab, pos, rot) as Transform;
			Vector3 angle =  obj.eulerAngles;
			angle.y +=270f;
			obj.eulerAngles=angle;
			obj.GetComponent<DestructAfterTime>().time = 2f;
		}
	}
}
