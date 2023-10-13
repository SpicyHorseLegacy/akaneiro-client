using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCAbilityBaseState : AbilityBaseState
{
    #region Define variables
    public enum AbilityPositionType
    {
        Target = 0,
        CurPosition = 1,
        PlayerPosition = 2,
        PlayerDir = 3,
    }

    [System.Serializable]
    public class AbilityCondition
    {
        public enum AbiConditionEnum
        {
            HealthAbove = 0,
            HealthBelow,
            DistanceAbove,
            DistanceBelow,
            PlayerHealthAbove,
            PlayerHealthBelow,
            PlayerCantMove,
            ABICONDITIONMAX,
        }

        public AbiConditionEnum AbiCondition;
        public float Num;                           // if Num == -1, that means this condition is useless.
    }

    [HideInInspector] public NpcBase npcController;

    public AbilityPositionType PosType = AbilityPositionType.CurPosition;
    public Vector2 PositionOffset;
    public List<AbilityCondition> AbilityConditions = new List<AbilityCondition>();
    public int ActiveTime = -1;
    public float AbilityCoolDown;
    public float AbilityCoolDownDif = 2;

    public AnimationClip CastAnimation;
    public Transform CastSoundPrefab;

    public Transform AbilityImpactVFXPrefab;
    public Transform AbilityImpactPosition;
    public Transform AbilityImpactSoundPrefab;
    #endregion

    #region State Functions
    public override void Initial ()
	{
		npcController = (NpcBase) playerController;
        Owner = npcController.transform;
        if (AbilityInfo.Instance != null)
            Info = AbilityInfo.Instance.AbilityInfomation.GetAbilityDetailInfoByID(id);
		base.Initial ();
	}
	
	public override void Enter ()
	{
		base.Enter ();

        PlayCastAnimaitonAndSound();
	}
	
	public override void Execute ()
	{
		base.Execute ();
	}
	
	public override void Exit ()
	{
		base.Exit ();
	}
    #endregion

    #region Callback
    public override void UseAbilityOK (SUseSkillResult useSkillResult)
	{
		base.UseAbilityOK(useSkillResult);

        //Debug.LogError("NPC SKILL : ID : " + useSkillResult.skillID + "");

        if (useSkillResult.skillID != 10001)
        {
            //Debug.Log("[ABI] " + Owner.transform.name + " use ability : [" + useSkillResult.skillID + "] " + name + " || Dest Position : " + useSkillResult.pos);
            //Debug.DrawLine(CS_SceneInfo.pointOnTheGround(useSkillResult.pos), CS_SceneInfo.pointOnTheGround(useSkillResult.pos) + Vector3.up * 10, Color.yellow, 5f);
        }

		if(!npcController.FSM.IsInState(npcController.abilityManager.GetAbilityByID((uint)useSkillResult.skillID)))
			npcController.FSM.ChangeState(npcController.abilityManager.GetAbilityByID((uint)useSkillResult.skillID));
		else
			npcController.FSM.GetCurrentState().Enter();
	}
	
	public override void UseAbilityResult(SUseSkillResult useSkillResult)
	{
		base.UseAbilityResult(useSkillResult);

        CS_SceneInfo.Instance.On_UpdateResult(this, useSkillResult);
        PlayImpactVFXAndSound();
	}
    #endregion

    #region General Functions
    /// <summary>
    /// Play cast animation and sound when owner start casting.
    /// </summary>
    public virtual void PlayCastAnimaitonAndSound()
    {
        AnimationModel.animation.AddClip(CastAnimation, CastAnimation.name);
        AnimationModel.animation[CastAnimation.name].time = 0;
        AnimationModel.animation[CastAnimation.name].wrapMode = WrapMode.Once;
        AnimationModel.animation.CrossFade(CastAnimation.name, 0.2f, PlayMode.StopAll);
        AnimationModel.animation.CrossFadeQueued(npcController.AttackIdleAnim.name, 0.2f);

        if (CastSoundPrefab)
        {
            SoundCue.PlayPrefabAndDestroy(CastSoundPrefab, Owner.position);
        }
    }

    /// <summary>
    /// Play impact vfx and sound when caculating damage.
    /// </summary>
    public virtual void PlayImpactVFXAndSound()
    {
        Transform pos = AbilityImpactPosition;
        if (!pos) pos = Owner;

        if (AbilityImpactVFXPrefab)
            Object.Instantiate(AbilityImpactVFXPrefab, pos.position, pos.rotation);

        if (AbilityImpactSoundPrefab)
            SoundCue.PlayPrefabAndDestroy(AbilityImpactSoundPrefab, pos.position);
    }

    /// <summary>
    /// Froce to make owner face to player.
    /// </summary>
    protected void FaceToPlayer()
    {
        Vector3 dir = Player.Instance.transform.position - npcController.transform.position;
        dir.y = 0;
        npcController.transform.forward = dir;
    }
    #endregion
}
