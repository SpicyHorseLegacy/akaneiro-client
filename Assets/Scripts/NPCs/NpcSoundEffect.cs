using UnityEngine;
using System.Collections;

public class NpcSoundEffect : MonoBehaviour {

	//sound
	public Transform[] AttackSoundPrefab = new Transform[0];
	public Transform AttackIdeSoundPrefab;
	public Transform DamageLightSoundPrefab;
	public Transform DamageHeavySoundPrefab;
	public Transform SleepSoundPrefab;
	public Transform SleepWakeUpSoundPrefab;
	public Transform SpawnFromAboveSoundPrefab;
    public Transform SpawnFromBelowSoundPrefab;
	public Transform EntrySoundPrefab;
	public Transform EntryIdleSoundPrefab;
    public Transform AlarmSoundPrefab;
	public Transform JumpSoundPrefab;
	public Transform KnockBackSoundPrefab;
	public Transform DeathSoundPrefab;
	
	public Transform StepSoundPrefab;
	
	
	public Transform PainSoundPrefab;
	public bool IsDefaultImpactSound;
	public Transform ImpactSoundPrefab;
	public Transform IdleSoundPrefab;
	public Transform BodyFallSoundPrefab;
	public Transform CommonKilledSoundPrefab;
	public Transform BossSpawnnSoundPrefab;
	public Transform BossStartSoundPrefab;
	public Transform[] MiscSoundPrefabs;
	
	Transform[] attackSound = new Transform[0];
	
	Transform StepSound;
	Transform[] ImpactSounds = new Transform[2];
	
	public void Awake()
	{
		attackSound = new Transform[AttackSoundPrefab.Length];
	}
	
	
	public virtual void PlayAttackSound(int attackIndex)
	{
		if(attackSound.Length > 0)
		{
			//print("Play Attack Sound: " + attackIndex);
			
			// 如果传入的是-1，则代表随机播放一个 // 如果传入的Index大于了Attacksound的队列，则随机播放一个
			if(attackIndex == -1 || attackIndex > attackSound.Length)
				attackIndex = Random.Range(0, attackSound.Length);
			
			if(attackIndex >= attackSound.Length)
				return;
			
			if(attackSound[attackIndex] == null && AttackSoundPrefab[attackIndex] != null)
			{
				attackSound[attackIndex] = Instantiate(AttackSoundPrefab[attackIndex], transform.position, transform.rotation) as Transform;
				attackSound[attackIndex].parent = transform;
			}
			
			if(attackSound[attackIndex])
				SoundCue.Play(attackSound[attackIndex].gameObject);
		}
	}
	
	public void PlayPainSound()
	{
		if(PainSoundPrefab)
			SoundCue.Play(PainSoundPrefab.gameObject);
	}

    public bool PlayImpactSound(BaseHitableObject targetObj)
    {
        return PlayImpactSound(0, targetObj);
    }

    // 0 : right hand
    // 1 : left hand
    public bool PlayImpactSound(int side, BaseHitableObject targetObj)
	{
		if(IsDefaultImpactSound)
		{
            // if this enemy should play default impact sound. find which impact sound should play, Instantiate one, and play.
			if(transform.GetComponent<NpcBase>().Weapons[side]){
				if(transform.GetComponent<NpcBase>().Weapons[side].GetComponent<WeaponBase>() && targetObj)
				{
                    Transform tempSoundPrefab = SoundEffectManager.Instance.GetImpactSoundPrefab(transform.GetComponent<NpcBase>().Weapons[side].GetComponent<WeaponBase>().WeaponCategory, targetObj.AudioArmorCategory);
                    if (tempSoundPrefab)
					{
                        if (!ImpactSounds[side])
                        {
                            ImpactSounds[side] = Instantiate(tempSoundPrefab, transform.GetComponent<NpcBase>().Weapons[side].position, transform.rotation) as Transform;
                            ImpactSounds[side].parent = transform.GetComponent<NpcBase>().Weapons[side];
                            ImpactSounds[side].name = tempSoundPrefab.name;
                            if (!transform.GetComponent<NpcBase>().IsBoss)
                                ImpactSounds[side].audio.volume *= 0.45f;
                        }

                        if (tempSoundPrefab.name == ImpactSounds[side].name)
                        {
							SoundCue.Play(ImpactSounds[side].gameObject);
                            return true;
						}
                        // if targetObj changed its armor type, change the impact sound aslo.
						else
						{
							ImpactSounds[side] = null;
							PlayImpactSound(side, targetObj);
						}
					}
				}
			}
		}else{
			
			if(ImpactSounds[0] == null && ImpactSoundPrefab != null)
			{
				ImpactSounds[0] = Instantiate(ImpactSoundPrefab, transform.position, transform.rotation) as Transform;
				ImpactSounds[0].parent = transform;
			}

            if (ImpactSounds[0])
            {
                SoundCue.Play(ImpactSounds[0].gameObject);
                return true;
            }
		}
        return false;
	}
	
	public void PlayWalkSound()
	{
		if(StepSound == null && StepSoundPrefab != null)
		{
			StepSound = Instantiate(StepSoundPrefab, transform.position, transform.rotation) as Transform;
			StepSound.parent = transform;
		}
		
		if(StepSound)
			SoundCue.PlayAtPosAndRotation(StepSound.gameObject, transform.position, transform.rotation);
	}
}
