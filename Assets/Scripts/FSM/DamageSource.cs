using UnityEngine;
using System.Collections;

public class DamageSource : State {

    [HideInInspector]
    public Transform SourceObj = null;

    public bool IsPlayImpactSound = false;          // control if play impact sound
    public Transform ImpactSoundPrefab;             // When calculating damage, if needs to play impact sound, play this Impact sound

    public bool IsPlayImpactVFX = false;            // control if show vfx
    public Transform ImpactVFXPrefab;               // When calculating damage, if needs to play impact vfx, play this Impact VFX prfab

    public virtual bool PlayImpactSoundToWho(BaseHitableObject target, EStatusElementType _element)
    {
        if (ImpactSoundPrefab)
        {
            SoundCue.PlayPrefabAndDestroy(ImpactSoundPrefab, target.transform.position);
            return true;
        }
		
		if(_element.Get() != EStatusElementType.StatusElement_Invalid)
		{
			SoundCue.PlayPrefabAndDestroy(SoundEffectManager.Instance.PlayElementalSound(_element, false), Owner.transform.position);
			return true;
		}
		
        return false;
    }

}
