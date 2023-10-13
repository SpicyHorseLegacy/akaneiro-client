using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityBaseState : DamageSource {
	 
	public int id;
    public AbilityDetailInfo Info;
	public Texture2D icon;
	//public string name;
	public int Level{get {return level;}}
	[SerializeField] protected int level;

	[HideInInspector] public BaseAttackableObject playerController;							// Owner的控制器，Player的话由Player控制，敌人由NPCBase控制，等等，但是他们都归属于BaseObject
	
	// 用于播放动画的物体，Player就是Aka_model
	public Transform AnimationModel
	{
		get
		{
			if(playerController != null)
				return playerController.AnimationModel;
			
			return null;
		}
	}

	public virtual void SetOwner(BaseAttackableObject _attackObj)
	{
		Owner = _attackObj.transform;
        SourceObj = _attackObj.transform;
		playerController = _attackObj;
	}
	
	public virtual void Initial(){}
	
	public override void Enter()
	{
		base.Enter();
	}
	
	/// <summary>
	/// 如果是“可以在跑动中释放的技能”，在ChangeState的时候不进入正常的Enter()，而这里进行代替()
	/// </summary>
	public virtual void EnterInRunState(){}
	
	public override void Execute ()
	{
		base.Execute();
	}
	
	public override void Exit ()
	{
		base.Exit ();
	}

    public virtual bool CanUseAbility()
    {
        return true;
    }
	
	/// <summary>
	/// Begins the ability.
	/// </summary>
	/// <param name='useSkillResult'>
	/// Use skill result.
	/// </param>
	public virtual void UseAbilityOK(SUseSkillResult useSkillResult){
#if NGUI
		if(InGameScreenCtrl.Instance.HudCtrl != null)
		{
			InGameScreenCtrl.Instance.HudCtrl.StartCoolDown(Info.ID, Info.CoolDown);
		}
#else
#endif
	}
	
	/// <summary>
	/// Uses the ability result.
	/// </summary>
	/// <param name='useSkillResult'>
	/// Use skill result.
	/// </param>
	public virtual void UseAbilityResult(SUseSkillResult useSkillResult){}
	
	/// <summary>
	/// Uses the ability failed.
	/// </summary>
	/// <param name='skillID'>
	/// Skill I.
	/// </param>
	/// <param name='reason'>
	/// Reason.
	/// </param>
    public virtual void UseAbilityFailed(uint skillID, EServerErrorType reason) { }
	
	/// <summary>
	/// 有物体进来，根据各个技能会各自进行处理 
	/// </summary>
	/// <param name="skillObjectInfo">
	/// A <see cref="SSkillObjectEnter"/>
	/// </param>
    public virtual AbilityObject On_SkillObjectEnter(SSkillObjectEnter skillObjectInfo)
    {
        return null;
    }

    public virtual void On_SkillObjectActive(int objID) { }

	/// <summary>
	/// 实例化一个声音给场景，并且把位置捆在玩家身上，用于各种3D效果音（例如回音等等）
	/// </summary>
	/// <returns>
	/// The sound for ability.
	/// </returns>
	/// <param name='soundPrefab'>
	/// Sound prefab.
	/// </param>
	public Transform newSoundForAbility(Transform soundPrefab)
	{
		Transform tempSound = Instantiate(soundPrefab) as Transform;
		tempSound.parent = transform;
		tempSound.position = Owner.position;
		
		return tempSound;
	}
}
