using UnityEngine;
using System.Collections;

public class PlayerChiMend : PlayerInstanceAbility
{
	public Transform SoundPrefab;
	Transform sound;
	
	public override void Enter()
	{
		base.Enter();

        if (!sound && SoundPrefab) sound = newSoundForAbility(SoundPrefab);
		
		// play soundAndVFX
		PlaySoundAndVFX();
	}
	
	public void PlaySoundAndVFX()
	{
		if(sound)	SoundCue.Play(sound.gameObject);
	}
}
