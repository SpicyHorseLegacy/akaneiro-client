using UnityEngine;
using System.Collections;

public class NPC_ProjectileBase : NPCAbilityBaseState {

	public Transform ChargingVFXPrefab;
	protected Transform m_ChargeParticleInstance = null;
	
	public Transform ThrowSoundPrefab;
	public Transform ProjectilePrefab;
	
	public override void Enter()
	{
		base.Enter();

		FaceToPlayer();
		StartChargeEffect();
		
		if(!CastSoundPrefab)
            Owner.GetComponent<NpcSoundEffect>().PlayAttackSound(-1);
#if NGUI
#else
		if (!_UI_CS_DebugInfo.Instance.AttackPlayerEnemies.Contains(npcController))
            _UI_CS_DebugInfo.Instance.AttackPlayerEnemies.Add(npcController);
#endif
	}
	
	public override void Exit()
	{
		base.Exit();

        StopChargeEffect();
#if NGUI
#else		
		if (_UI_CS_DebugInfo.Instance.AttackPlayerEnemies.Contains(npcController))
            _UI_CS_DebugInfo.Instance.AttackPlayerEnemies.Remove(npcController);
#endif
	}
	
	public override AbilityObject On_SkillObjectEnter (SSkillObjectEnter skillObjectInfo)
	{
        StopChargeEffect();
		
		if (ProjectilePrefab)
        {
            Vector3 _tempPos = skillObjectInfo.pos;
            _tempPos.y = Owner.position.y + 0.1f;
            Transform SkillObjectInstance = Instantiate(ProjectilePrefab, _tempPos, ProjectilePrefab.rotation) as Transform;
            SkillObjectInstance.gameObject.SetActive(true);
			
			AbilityObject _abiObj = SkillObjectInstance.GetComponent<AbilityObject>();
			_abiObj.ObjID = skillObjectInfo.objectID;
			_abiObj.DestAbility = this;
			_abiObj.SkillObjectInfo = skillObjectInfo;

            return _abiObj;
        }

        return null;
	}
	
	public virtual void StartChargeEffect()
    {
		if(ChargingVFXPrefab)
		{
			m_ChargeParticleInstance = Object.Instantiate(ChargingVFXPrefab, transform.position, transform.rotation) as Transform;
	        m_ChargeParticleInstance.parent = transform;
			DestructAfterTime.DestructGameObjectAfterTime(m_ChargeParticleInstance, 3);
		}
	}
	
    public void StopChargeEffect()
    {
        if (m_ChargeParticleInstance != null)
        {
            //Destroy(m_ChargeParticleInstance.gameObject);
            m_ChargeParticleInstance.gameObject.AddComponent<DestructAfterTime>();
            m_ChargeParticleInstance.GetComponent<DestructAfterTime>().DestructNow();
            m_ChargeParticleInstance = null;
        }
    }
}
