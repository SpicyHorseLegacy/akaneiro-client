using UnityEngine;
using System.Collections;

public class NPC_NormalMeleeAttackState : NPCAbilityBaseState {

	public override void Enter()
	{
        //if(AttackTarget == null)
        //    AttackTarget = Player.Instance.transform;
        //Debug.LogError("enter normal attack");
		if(Player.Instance.AllEnemys.IndexOf(Owner)== -1)
		{
			Player.Instance.AllEnemys.Add(Owner);
		}

        Vector3 dir = Vector3.zero;
        if (npcController.AttackTarget)
        {
            dir = Vector3.Normalize(npcController.AttackTarget.transform.position - Owner.position);
            Owner.rotation = Quaternion.LookRotation(dir);
        }

        if (npcController.IsBoss && npcController.BossStartSound == null)
            npcController.BossStartSound = npcController.PlaySound(npcController.GetComponent<NpcSoundEffect>().BossStartSoundPrefab, npcController.BossStartSound);
		
		//print("NPC Attack animation!");
        npcController.PlayAttackAnim();
#if NGUI
#else
        if (!_UI_CS_DebugInfo.Instance.AttackPlayerEnemies.Contains(npcController))
        {
            _UI_CS_DebugInfo.Instance.AttackPlayerEnemies.Add(npcController);
        }
#endif
	}
	
	public override void Execute()
	{
        if (npcController.AttackTarget)
		{
            if (npcController.AttackTarget.ObjID != npcController.TargetObjectID)
			{
				Vector3 dir = Vector3.zero;
                dir = Vector3.Normalize(npcController.AttackTarget.transform.position - Owner.position);
				Owner.rotation = Quaternion.LookRotation(dir);
			}
		}
		
        if (npcController.IsAttackAnimFinish())
		{
            if (npcController.DamageAnim != null && !npcController.AnimationModel.animation.IsPlaying(npcController.DamageAnim.name))
                npcController.PlayIdleAnim();
	        
	        if(Player.Instance.IsDead)
                npcController.FSM.ChangeState(npcController.IS);
		}
	}
	
	public override void Exit()
	{
		base.Exit();
#if NGUI
#else
        if (_UI_CS_DebugInfo.Instance.AttackPlayerEnemies.Contains(npcController))
        {
            _UI_CS_DebugInfo.Instance.AttackPlayerEnemies.Remove(npcController);
        }
#endif		
		//print("Attack state Exit!");
	}
	
	// if elemental impact, play elemental sound.
	// else if npc has impact sound, play it.
    public override bool PlayImpactSoundToWho(BaseHitableObject target, EStatusElementType _element)
    {
		// if it's an elemental damage, sound should be handled in [UseAbilityResult];
		if(_element.Get() != EStatusElementType.StatusElement_Invalid)
			return true;
		
		if (ImpactSoundPrefab)
        {
            SoundCue.PlayPrefabAndDestroy(ImpactSoundPrefab, target.transform.position);
            return true;
        }
		
        if (npcController.GetComponent<NpcSoundEffect>())
        {
			// npc sound effect would select weapon impact sound if he is holding a weapon. otherwise, play the default impact sound in npc sound effect script which is used for enemy who doesn't have a weapon.
            npcController.GetComponent<NpcSoundEffect>().PlayImpactSound(target);
            return true;
        }
        return false;
    }

    const int Enemy_Elemental_Frost_ID = 70050;
    const int Enemy_Elemental_Flame_ID = 70050;
    const int Enemy_Elemental_Blast_ID = 70050;
    const int Enemy_Elemental_Storm_ID = 70050;

    public override void UseAbilityResult(SUseSkillResult useSkillResult)
    {
        base.UseAbilityResult(useSkillResult);

        bool isb = false;
        foreach (SStatusEffect statusEffect in useSkillResult.statusEffectVec)
        {
            if (statusEffect.statustID == Enemy_Elemental_Frost_ID ||
                statusEffect.statustID == Enemy_Elemental_Flame_ID ||
                statusEffect.statustID == Enemy_Elemental_Blast_ID ||
                statusEffect.statustID == Enemy_Elemental_Storm_ID)
                isb = true;
        }

		foreach (SSkillEffect skillEffect in useSkillResult.skillEffectVec)
        {
            BaseObject targetObj = CS_SceneInfo.Instance.GetTargetByID(skillEffect.targetID);
            SoundCue.PlayPrefabAndDestroy(SoundEffectManager.Instance.PlayElementalSound(skillEffect.elementType, isb), Owner.transform.position);
        }
    }
}
