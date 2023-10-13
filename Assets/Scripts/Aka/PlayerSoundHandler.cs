using UnityEngine;
using System.Collections;

public class PlayerSoundHandler : MonoBehaviour {

    //sound 
    public Transform SpawnOutSound;
    public Transform FatalBlowSound;
    public Transform BodyFallSound;
    public Transform PickItem_Sound;

    public Transform FistAttackSoundPrefab;

    public void PlayWeaponAttackSound()
    {
        MeleeAttackState normalAttackState = (MeleeAttackState)Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID);
        int hand = normalAttackState.hand;
        Transform weapon = null;
        if (hand == 0)
        {
            weapon = Player.Instance.EquipementMan.RightHandWeapon;
        }
        else
        {
            weapon = Player.Instance.EquipementMan.LeftHandWeapon;
        }

        if (weapon && weapon.GetComponent<WeaponBase>())
            weapon.GetComponent<WeaponBase>().PlayAttackSound();
        else
            SoundCue.PlayPrefabAndDestroy(FistAttackSoundPrefab, Player.Instance.transform.position);
         
    }

    public bool PlayWeaponImpactSoundWithHand(int hand, BaseHitableObject target)
    {
        Transform weapon = null;
        if (hand == 0)
        {
            weapon = Player.Instance.EquipementMan.RightHandWeapon;
        }
        else
        {
            weapon = Player.Instance.EquipementMan.LeftHandWeapon;
        }

        if (weapon && weapon.GetComponent<WeaponBase>())
            return weapon.GetComponent<WeaponBase>().PlayImpactSound(target);
        else
        {
            SoundCue.PlayPrefabAndDestroy(SoundEffectManager.Instance.GetImpactSoundPrefab(EWeaponCategory.None, target.AudioArmorCategory), target.transform.position);
        }
        return false;
    }

    public bool PlayWeaponImpactSound(BaseHitableObject target)
    {
        MeleeAttackState normalAttackState = (MeleeAttackState)Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID);
        int hand = normalAttackState.hand;

        return PlayWeaponImpactSoundWithHand(hand, target);
    }

    public void PlaySpawnOutSound()
    {
        SoundCue.PlayPrefabAndDestroy(SpawnOutSound);
    }

    public void PlayDeathSound()
    {
        SoundCue.PlayPrefabAndDestroy(FatalBlowSound);
    }

    public void PlayBodyFallSound()
    {
        SoundCue.PlayPrefabAndDestroy(BodyFallSound);
    }

    public void PlayPickuPItemSound(Transform Item)
    {
        SoundCue.PlayPrefabAndDestroy(PickItem_Sound);
    }
}
