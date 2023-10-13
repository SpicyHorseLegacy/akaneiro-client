using UnityEngine;
using System.Collections;

public class AllyAbilityBaseState : AbilityBaseState
{
    public AllyNpc Executer;

    public int ActiveChance = 5;
    public float CoolDown;
    public float LastUseAbilityTime;

    public AnimationClip CastAnimation;
    protected string castAnimationString;
    public Transform CastSoundPrefab;
    public Transform CastVFXPrefab;
    public Transform CastPosition;
    public Transform AbilityImpactSoundPrefab;
    public Transform AbilityImpactVFXPrefab;

    public override void Initial()
    {
        base.Initial();
        Executer = (AllyNpc)playerController;
    }

    public override void Enter()
    {
        base.Enter();

        LastUseAbilityTime = Time.time;

        ProcessCasting();
    }

    public override void Execute()
    {
        base.Execute();

        if (!Executer.AnimationModel.animation.isPlaying)
        {
            Executer.FSM.ChangeState(Executer.IS);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Owner.GetComponent<AllyMovement>().bStopMove = false;
    }

    public override bool CanUseAbility()
    {
        if (Time.time < LastUseAbilityTime + CoolDown) return false;
        if (Executer.AttrMan.Attrs[EAttributeType.ATTR_CurMP] < Info.ManaCost) return false;
        if (Vector3.Distance(Executer.AttackTarget.position, Owner.position) >= AbilityInfo.Instance.AbilityInfomation.GetAbilityDetailInfoByID(id).EndDistance/2.0f) return false;
        return true;
    }

    public override void UseAbilityOK(SUseSkillResult useSkillResult)
    {
        base.UseAbilityOK(useSkillResult);
//        Debug.LogError("ok : " + useSkillResult.skillID + "[" + Time.realtimeSinceStartup + "]");
        CS_SceneInfo.Instance.On_UpdateAttribution(Owner, this, useSkillResult.attributeChangeVec, false);
    }

    public override void UseAbilityResult(SUseSkillResult useSkillResult)
    {
        base.UseAbilityResult(useSkillResult);
        CS_SceneInfo.Instance.On_UpdateResult(this, useSkillResult);
        ProcessImpactAtPos(useSkillResult.pos);
//        Debug.LogError("result : " + useSkillResult.skillID + "[" + Time.realtimeSinceStartup + "]");
    }

    public virtual void ProcessCasting()
    {
		if(CastAnimation)
		{
	        if (CastAnimation)
	            castAnimationString = CastAnimation.name;
	
	        if (castAnimationString != "")
	            Executer.AnimationModel.animation.CrossFade(castAnimationString);
		}

        Transform _castPosition = CastPosition;
        if (!_castPosition)
            _castPosition = Owner;

        if (CastSoundPrefab)
            SoundCue.PlayPrefabAndDestroy(CastSoundPrefab, _castPosition.position);

        if (CastVFXPrefab)
            Instantiate(CastVFXPrefab, _castPosition.position, _castPosition.rotation);
    }

    public virtual void ProcessImpactAtPos(Vector3 _pos)
    {
        if (AbilityImpactVFXPrefab)
            Object.Instantiate(AbilityImpactVFXPrefab, _pos + Vector3.up * 0.3f, Owner.rotation);

        if (AbilityImpactSoundPrefab)
            SoundCue.PlayPrefabAndDestroy(AbilityImpactSoundPrefab, _pos);
    }

    protected void SendUseAbilityRequest(uint skill_id, uint target_id, Vector3 skill_pos)
    {
        Debug.LogError("[ALLY ABI] Cast : " + skill_id);
        //send use ability request to server
        if (CS_Main.Instance != null)
        {
            CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.UseAllySkill((uint)Executer.ObjID, skill_id, target_id, skill_pos, CS_SceneInfo.Instance.SyncTime));
        }
    }

    protected Vector3 CheckIfInRange(Vector3 _targetPos)
    {
        if (Vector3.Distance(_targetPos, Owner.position) > Info.EndDistance)
        {
            return Owner.position + (_targetPos - Owner.position).normalized * Info.EndDistance;
        }
        //if (Vector3.Distance(_targetPos, Owner.position) < Info.StartDistance)
        //{
        //    return Owner.position + (_targetPos - Owner.position).normalized * Info.StartDistance;
        //}
        return _targetPos;
    }
}
