using UnityEngine;
using System.Collections;

public class BaseHitableObject : BaseObject { 

    public EArmorCaterogy AudioArmorCategory = EArmorCaterogy.light;        // this works for impact sounds. when an enemy did damage to this, play the impact sound depends on the weapon category and this armor category.
    public float AvoidanceRadius = 0.5f;                                    // the virtual size of this object. Player and enemies have attack radius. If the distance is less than AvoidanceRadius + AttackRadius, they attack

    [HideInInspector] public AttributionManager AttrMan;
    [HideInInspector] public BuffManager BuffMan;                           // BuffManager controls getting and removing buffs from server commands. it will add to the object automatically by awake function
   	[HideInInspector] public EquipementManager EquipementMan;

    public Transform BeHitImpactSound = null;
    public Transform BeHitImpactVFX = null;

    protected override void Awake()
    {
        base.Awake();

        GameObject attrManObj = new GameObject();
        attrManObj.name = "Attribution Manager";
        attrManObj.transform.parent = transform;
        attrManObj.transform.position = transform.position;
        AttrMan = attrManObj.AddComponent<AttributionManager>();
        AttrMan.Owner = transform;
        EquipementMan = attrManObj.AddComponent<EquipementManager>();
        EquipementMan.Owner = transform;

        GameObject buffManObj = new GameObject();
        buffManObj.name = "Buff Manager";
        buffManObj.transform.parent = transform;
        buffManObj.transform.position = transform.position;
        BuffMan = buffManObj.AddComponent<BuffManager>();
        BuffMan.Initial(transform);
    }

    public virtual void TakeDamage(int damage)
    {
        AttrMan.Attrs[EAttributeType.ATTR_CurHP] += damage;

        if (AttrMan.Attrs[EAttributeType.ATTR_CurHP] > AttrMan.Attrs[EAttributeType.ATTR_MaxHP])
            AttrMan.Attrs[EAttributeType.ATTR_CurHP] = AttrMan.Attrs[EAttributeType.ATTR_MaxHP];

        if (damage < 0 && AttrMan.Attrs[EAttributeType.ATTR_CurHP] < 0)
        {
            GoToHell();
        }
    }

    public virtual void TakeDamage(int damage, DamageSource source)
    {
        TakeDamage(damage);
    }
    public virtual void TakeDamage(int damage, DamageSource source, bool isCrit, EStatusElementType elementType)
    {
        TakeDamage(damage, source);
        PlayDamageImpactFrom(damage, source, elementType);

        if (CameraEffectManager.Instance)
        {
            if(isCrit)
                CameraEffectManager.Instance.PlayShakingEffect();
        }
    }
    public virtual void TakeDamage(int damage, DamageSource source, bool isCrit, EStatusElementType elementType, bool isShowDamageText)
    {
        TakeDamage(damage, source, isCrit, elementType);

#if NGUI
        if(isShowDamageText && InGameScreenCombatHudCtrl.Instance != null)
            InGameScreenCombatHudCtrl.Instance.ShowDamageAtPos(damage, transform, isCrit, elementType);
#else
        if (isShowDamageText && DamageTextManager.Instance)
            DamageTextManager.Instance.ShowDamageText(damage, transform, isCrit, elementType);
#endif
    }

    public virtual Transform GetAnimationModel() { return null; }

    public virtual BaseBuff AddBuff(int id, Transform _sourceTransform)
    {
        return BuffMan.AddBuffByID(id, _sourceTransform);
    }

    /// <summary>
    /// Play Impact VFX and SFX
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="source"></param>
    /// <param name="elementType"></param>
    public virtual void PlayDamageImpactFrom(int damage, DamageSource source, EStatusElementType elementType)
    {
        //Debug.LogError("Ele : " + elementType.GetString());

        // if source is self, ignore.
        if (source && source.Owner && source.Owner == transform) return;

        if (damage >= 0) return;
        #region VFX
        // VFX part
        // if damage is nagative, could play impact vfx
        // if it's elemental damage, play elemental vfx and ignore any special vfx which source or be hit obj has.
        // if source has a special vfx, play it and ignore which be hit obj has.
        // if this object has special vfx, play it.
        // if all is not, play common vfx.
        bool bPlayedVFX = false;

        if (elementType.Get() != EStatusElementType.StatusElement_Invalid)
        {
            VFXManager.Instance.PlayImpactEffectAtTransform(elementType, transform);
            bPlayedVFX = true;
        }

        if (!bPlayedVFX && (ObjType == ObjectType.Enermy || ObjType == ObjectType.Player || ObjType == ObjectType.Ally || ObjType == ObjectType.NPC))
        {
            if (source && source.IsPlayImpactVFX && source.ImpactVFXPrefab)
            {
                Vector3 pos = transform.position + Vector3.up * 1.2f;
                Vector3 dir = Vector3.zero;
                if (source && source.Owner)
                    dir = (source.Owner.position - transform.position).normalized;
                Quaternion rot = Quaternion.identity;
                if (dir != Vector3.zero)
                {
                    rot = Quaternion.LookRotation(dir);
                }
                Instantiate(source.ImpactVFXPrefab, pos, rot);
                bPlayedVFX = true;
            }
        }

        if (!bPlayedVFX)
        {
            if (BeHitImpactVFX)
            {
                Vector3 pos = transform.position + Vector3.up * 1.2f;
                Vector3 dir = Vector3.zero;
                if (source && source.Owner)
                    dir = (source.Owner.position - transform.position).normalized;
                Instantiate(BeHitImpactVFX, pos, Quaternion.LookRotation(dir));
            }
            else
            {
                if (ObjType != ObjectType.BreakableObj && ObjType != ObjectType.IteractiveObj)
                {
                    VFXManager.Instance.PlayImpactEffectAtTransform(elementType, transform);
                }
            }
        }
        #endregion
        #region SFX
        // SFX part
        // if object has a behitimpactsound, that means source can't play impact sound.
        // play impact sound
        bool bPlayedSFX = false;

        if (ObjType == ObjectType.BreakableObj || ObjType == ObjectType.IteractiveObj)
        {
            if (BeHitImpactSound)
            {
                SoundCue.PlayPrefabAndDestroy(BeHitImpactSound, transform.position);
                return;
            }
        }

        if (source && source.Owner && source.IsPlayImpactSound)
        {
			// for npc which could make elemental damage, if it's an elemental damage, play a null sound and override play sound in npc attack state.
            bPlayedSFX = source.PlayImpactSoundToWho(this, elementType);
        }

        if (!bPlayedSFX)
        {
            if (BeHitImpactSound)
            {
                SoundCue.PlayPrefabAndDestroy(BeHitImpactSound, transform.position);
                return;
            }
        }
        #endregion
    }

    public virtual void GoToHell() { }
}
