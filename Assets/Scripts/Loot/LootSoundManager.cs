using UnityEngine;
using System.Collections;

public class LootSoundManager : MonoBehaviour {

    public static LootSoundManager Instance = null;

    public Transform RollingSoundPrefab;

    public Transform One_HandDropSoundPrefab;
    public Transform Two_HandDropSoundPrefab;

    public Transform LightArmorDropSoundPrefab;
    public Transform HeavyArmorDropSoundPrefab;
    
    public Transform AccessoryDropSoundPrefab;
    public Transform CloakDropSoundPrefab;

    public Transform GemDropSoundPrefab;
    public Transform CoreDropSoundPrefab;
    public Transform MaterialDropSoundPrefab;

    public Transform CommonDropSoundPrefab;

    public float MinInterval = 0.2f;
    float _lastPlayRollingSoundTime;
    float _lastPlayDropSoundTime;

    void Awake()
    {
        Instance = this;
    }

    public static void PlayRollingSound(Transform _target)
    {
        if (Time.realtimeSinceStartup < Instance._lastPlayRollingSoundTime + Instance.MinInterval) return;

        SoundCue.PlayPrefabAndDestroy(Instance.RollingSoundPrefab, _target.position);
        Instance._lastPlayRollingSoundTime = Time.realtimeSinceStartup;
    }

    public static void PlayDropSound(Transform _target)
    {
        if (Time.realtimeSinceStartup < Instance._lastPlayDropSoundTime + Instance.MinInterval) return;

        Transform _temp = Instance.CommonDropSoundPrefab;

        if (_target.GetComponent<WeaponBase>())
        {
            switch(_target.GetComponent<WeaponBase>().WeaponType)
            {
                case WeaponBase.EWeaponType.WT_OneHandWeapon:
                case WeaponBase.EWeaponType.WT_NoneWeapon:
                case WeaponBase.EWeaponType.WT_DualWeapon:
                    _temp = Instance.One_HandDropSoundPrefab;
                    break;

                case WeaponBase.EWeaponType.WT_TwoHandWeaponAxe:
                case WeaponBase.EWeaponType.WT_TwoHandWeaponSword:
                    _temp = Instance.Two_HandDropSoundPrefab;
                    break;
            }
        }
        else if (_target.GetComponent<ArmorBase>())
        {
            if (_target.GetComponent<ArmorBase>().armortype == ArmorBase.Armortype.isCloak)
            {
                _temp = Instance.CloakDropSoundPrefab;
            }
            else
            {
                switch (_target.GetComponent<ArmorBase>().ArmorCaterogy)
                {
                    case EArmorCaterogy.light:
                    case EArmorCaterogy.medium:
                        _temp = Instance.LightArmorDropSoundPrefab;
                        break;

                    case EArmorCaterogy.heavy:
                        _temp = Instance.HeavyArmorDropSoundPrefab;
                        break;
                }
            }
        }
        else if (_target.GetComponent<Accessory>())
        {
            _temp = Instance.AccessoryDropSoundPrefab;
        }
        else if (_target.GetComponent<Item_Mateiral>())
        {
            switch (_target.GetComponent<Item_Mateiral>().MaterialType)
            {
                case Item_Mateiral.EnumLootMaterailType.Core:
                    _temp = Instance.CoreDropSoundPrefab;
                    break;
                case Item_Mateiral.EnumLootMaterailType.Gem:
                    _temp = Instance.GemDropSoundPrefab;
                    break;
                case Item_Mateiral.EnumLootMaterailType.Material:
                    _temp = Instance.MaterialDropSoundPrefab;
                    break;
            }
        }

        if (_temp)
        {
            SoundCue.PlayPrefabAndDestroy(_temp, _target.position);
            Instance._lastPlayDropSoundTime = Time.realtimeSinceStartup;
        }
    }
}
