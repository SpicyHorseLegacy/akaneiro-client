using UnityEngine;
using System.Collections;

public class NPC_Toss : NPC_ProjectileBase {
	//VFX
    public enum ChangingHand
    {
        eLeftHand,
        eRightHand,
    }

    public ChangingHand ChargingHand = ChangingHand.eRightHand;
	private Transform m_LeftHand = null;
    private Transform m_RightHand = null;
    
	public override void Enter()
	{
		base.Enter();

        FindHands();
	}
	
	public override AbilityObject On_SkillObjectEnter (SSkillObjectEnter skillObjectInfo)
	{
        StopChargeEffect();

        if (ProjectilePrefab)
        {
            Vector3 _tempPos = skillObjectInfo.pos;
            _tempPos.y = Owner.position.y + 1;
            Transform SkillObjectInstance = Instantiate(ProjectilePrefab, _tempPos, Quaternion.identity) as Transform;
            SkillObjectInstance.eulerAngles = Vector3.up * Random.Range(0, 360);
            SkillObjectInstance.gameObject.SetActive(true);
            OniThrowObj throwObjcontroller = SkillObjectInstance.GetComponent<OniThrowObj>();
            throwObjcontroller.ObjID = skillObjectInfo.objectID;
            throwObjcontroller.dir = skillObjectInfo.direction;
            throwObjcontroller.DestAbility = this;
            throwObjcontroller.SkillObjectInfo = skillObjectInfo;

            if (ThrowSoundPrefab)
                SoundCue.PlayPrefabAndDestroy(ThrowSoundPrefab, Owner.position);

            return throwObjcontroller;
        }

        return null;
	}

    public override void StartChargeEffect()
    {
        if (ChargingHand == ChangingHand.eRightHand && m_RightHand && ChargingVFXPrefab != null)
        {
            m_ChargeParticleInstance = Object.Instantiate(ChargingVFXPrefab, m_RightHand.position, m_RightHand.rotation) as Transform;
            m_ChargeParticleInstance.parent = m_RightHand;
            m_ChargeParticleInstance.gameObject.AddComponent<DestructAfterTime>();
            m_ChargeParticleInstance.GetComponent<DestructAfterTime>().time = 3;
        }

        if (ChargingHand == ChangingHand.eLeftHand && m_LeftHand && ChargingVFXPrefab != null)
        {
            m_ChargeParticleInstance = Object.Instantiate(ChargingVFXPrefab, m_LeftHand.position, m_LeftHand.rotation) as Transform;
            m_ChargeParticleInstance.parent = m_LeftHand;
            m_ChargeParticleInstance.gameObject.AddComponent<DestructAfterTime>();
            m_ChargeParticleInstance.GetComponent<DestructAfterTime>().time = 3;
        }
    }

    //Sets m_RightHand
    private void FindHands()
    {
        bool foundOne = false;
        Component[] all = Owner.GetComponentsInChildren<Component>();
        foreach (Component T in all)
        {
            if (T.name == "Bip001 Prop1")
            {
                m_RightHand = T.transform;
                if (foundOne == true)
                {
                    break;
                }
                foundOne = true;
            }

            if (T.name == "Bip001 Prop2")
            {
                m_LeftHand = T.transform;
                if (foundOne == true)
                {
                    break;
                }
                foundOne = true;
            }
        }
    }
}