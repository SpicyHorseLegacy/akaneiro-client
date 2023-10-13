using UnityEngine;
using System.Collections;

public class Ally_NormalAttackState : AllyAbilityBaseState {

    string[] WeaponType_0h_AnimName_R = { "Aka_0H_Attack_2" };
    string[] WeaponType_0h_AnimName_L = { "Aka_0H_Attack_1" };
    string[] WeaponType_1h_AnimName_R = { "Aka_1H_Attack_1", "Aka_1H_Attack_3" };
    string[] WeaponType_1h_AnimName_L = { "Aka_1H_Attack_2" };
    string[] WeaponType_2h_AnimName = { "Aka_2H_Attack_1", "Aka_2H_Attack_2", "Aka_2H_Attack_3", "Aka_2H_Attack_4" };
    string[] WeaponType_2hNodachi_AnimName = { "Aka_2HNodachi_Attack_1", "Aka_2HNodachi_Attack_2", "Aka_2HNodachi_Attack_3", "Aka_2HNodachi_Attack_4" };
	
	float attackAnimationLength;
	
    public int hand = 1;				// 1为右手，0为左手
	
	float htime = 0f;
	
	string CurrentAniString = "";
	
	public override void SetOwner(BaseAttackableObject _attackObj)
	{
		base.SetOwner(_attackObj);
		
		if(_attackObj.GetType() == typeof(AllyNpc))
			Executer = (AllyNpc) _attackObj;
		else
			Debug.LogWarning("[Warning] Ally prefab doesn't have the correct script.");
	}
	
	public override void Enter()
	{
		base.Enter();

        Owner.GetComponent<AllyMovement>().IsMoving = false;
		Owner.GetComponent<AllyMovement>().curPathPointIndex=0;
		Owner.GetComponent<AllyMovement>().bStopMove = true;
		
		if(Executer.AttackTarget != null)
		   FaceToTarget(Executer.AttackTarget);

        if (Executer.AttackTarget && Executer.AttackTarget.GetComponent<NpcBase>() != null && Executer.AttackTarget.GetComponent<NpcBase>().AttrMan.Attrs[EAttributeType.ATTR_CurHP] > 0)
		    attack();
		else
		    Executer.FSM.ChangeState(Executer.IS); 
	}
	
	public override void Execute()
	{
		if((Owner.position - Player.Instance.transform.position).magnitude  > Executer.MaxAllyDitance)
		{
			Executer.FSM.ChangeState(Executer.IS);
			return;
		}
		
		float TimeInterval = 0.8f;
		
		if( Executer.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_DualWeapon ||
			Executer.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_OneHandWeapon)
			TimeInterval = 0.57f;
		
		if(CurrentAniString.Length != 0 && !AnimationModel.animation.IsPlaying(CurrentAniString) && Time.time -  htime >= TimeInterval)
		{
			if(Executer.AttackTarget)
			{
				if(Executer.AttackTarget.GetComponent<NpcBase>())
				{
					NpcBase attackTarget = Executer.AttackTarget.GetComponent<NpcBase>();
                    if (attackTarget.AttrMan.Attrs[EAttributeType.ATTR_CurHP] > 0 || !attackTarget.bNotifyDead)
					{
						attack();
						return;
					}
					else 
					{
					    Executer.AttackTarget = null;
					}
				}
			}
			
			Executer.FSM.ChangeState(Executer.IS);
		}
		else if(CurrentAniString.Length != 0 && !AnimationModel.animation.IsPlaying(CurrentAniString))
		{
			Executer.PlayIdleAnim(true);
		}
	}
	
	public override void Exit()
	{
		Owner.GetComponent<AllyMovement>().bStopMove = false;
	}
	
    //public override void CalculateResult()
    //{
    //    base.CalculateResult();
    //    CameraEffectManager.Instance.PlayShakingEffect("subtle");
    //}

    public override void UseAbilityResult(SUseSkillResult useSkillResult)
    {
        base.UseAbilityResult(useSkillResult);
    }

	private void playAttackAnimation()
	{
        WeaponBase.EWeaponType wt = Executer.EquipementMan.GetWeaponType();

        // hand == 1 左手攻击   hand == 0 右手攻击
        hand++;
        hand = hand % 2;

        string aniString = "";
        if (wt == WeaponBase.EWeaponType.WT_NoneWeapon)
        {
            if (hand == 1)
                aniString = WeaponType_0h_AnimName_L[Random.Range(0, WeaponType_0h_AnimName_L.Length)];
            else
                aniString = WeaponType_0h_AnimName_R[Random.Range(0, WeaponType_0h_AnimName_R.Length)];
        }
        else if (wt == WeaponBase.EWeaponType.WT_DualWeapon)
        {
            if (hand == 1)
                aniString = WeaponType_1h_AnimName_L[Random.Range(0, WeaponType_1h_AnimName_L.Length)];
            else
                aniString = WeaponType_1h_AnimName_R[Random.Range(0, WeaponType_1h_AnimName_R.Length)];
        }
        else if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon)
        {
            aniString = WeaponType_1h_AnimName_R[Random.Range(0, WeaponType_1h_AnimName_R.Length)];
            hand = 0;
        }
        else if (wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
        {
            aniString = WeaponType_2h_AnimName[Random.Range(0, WeaponType_2h_AnimName.Length)];
            hand = 0;
        }
        else
        {
            aniString = WeaponType_2hNodachi_AnimName[Random.Range(0, WeaponType_2hNodachi_AnimName.Length)];
            hand = 0;
        }

        AnimationModel.animation[aniString].speed = 1;

        Transform weaponObj = null;
        if (hand == 0)
            weaponObj = Player.Instance.EquipementMan.RightHandWeapon;
        else
            weaponObj = Player.Instance.EquipementMan.LeftHandWeapon;
        if (weaponObj)
        {
            WeaponBase weapon = weaponObj.GetComponent<WeaponBase>();
            float attackSpeedFactor = Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_AttackSpeed] / 100f;
            AnimationModel.animation[aniString].speed = (1.0f / weapon.AttackSpeedFactor) * attackSpeedFactor;
        }
		CurrentAniString = aniString;
        AnimationModel.animation[aniString].time = 0;
        AnimationModel.animation.CrossFade(aniString, 0.1f);
        attackAnimationLength = AnimationModel.animation[aniString].length / AnimationModel.animation[aniString].speed;
	}
	
	private void attack()
	{
		htime = Time.time;
		
		if(Executer.AttackTarget)
		{
		    float dis = Vector3.Distance(Executer.AttackTarget.position, Owner.position);
			
			if(dis >= Executer.AttackRange)
			{
				Executer.AttackTarget = null;
				return;
			}
			
			FaceToTarget(Executer.AttackTarget);
			
			playAttackAnimation();
			
			uint targetId = (uint) Executer.AttackTarget.GetComponent<NpcBase>().ObjID;
			CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.UseAllyNormalAttack(new EWeaponType(hand), targetId, CS_SceneInfo.Instance.SyncTime));
			//NewAbilityInQueue();
        }
		

	}
	
	void FaceToTarget(Transform target)
	{
		 Vector3 dir = target.position - Owner.position;
		 dir.y = 0;
		 Owner.forward = dir;
	}

    public override bool PlayImpactSoundToWho(BaseHitableObject target, EStatusElementType _element)
    {
		if(_element.Get() != EStatusElementType.StatusElement_Invalid)
		{
			SoundCue.PlayPrefabAndDestroy(SoundEffectManager.Instance.PlayElementalSound(_element, false), Owner.transform.position);
			return true;
		}
		
        return Owner.GetComponent<AllySoundHandler>().PlayWeaponImpactSound(target);
    }
}
