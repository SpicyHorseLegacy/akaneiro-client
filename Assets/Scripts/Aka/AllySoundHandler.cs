using UnityEngine;
using System.Collections;

public class AllySoundHandler : MonoBehaviour {

    public AllyNpc executer;

    public void PlayWeaponAttackSound()
    {
        print("PlayAttackSound");
        Ally_NormalAttackState normalAttackState = (Ally_NormalAttackState)executer.abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID);
        int hand = normalAttackState.hand;
        Transform weapon = null;
        if (hand == 0)
        {
            weapon = executer.EquipementMan.RightHandWeapon;
        }
        else
        {
            weapon = executer.EquipementMan.LeftHandWeapon;
        }

        if (weapon && weapon.GetComponent<WeaponBase>())
            weapon.GetComponent<WeaponBase>().PlayAttackSound();
        else
            SoundCue.PlayPrefabAndDestroy(Player.Instance.GetComponent<PlayerSoundHandler>().FistAttackSoundPrefab, Player.Instance.transform.position);

    }

    public bool PlayWeaponImpactSoundWithHand(int hand, BaseHitableObject target)
    {
        Transform weapon = null;
        if (hand == 0)
        {
            weapon = executer.EquipementMan.RightHandWeapon;
        }
        else
        {
            weapon = executer.EquipementMan.LeftHandWeapon;
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
        //Debug.Log("play impact sound");

        Ally_NormalAttackState normalAttackState = (Ally_NormalAttackState)executer.abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID);
        int hand = normalAttackState.hand;

        return PlayWeaponImpactSoundWithHand(hand, target);
    }
}
