using UnityEngine;
using System.Collections;

public class WerewolfRanged : NpcBase 
{
	public override void Start () 
	{
		base.Start();
		
		//FindHands();
		
		//AttackRadius = 2f;
		AvoidanceRadius=1.5f; 
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		base.Update();
	}
	
    //public override void PlayAttackAnim ()
    //{
    //    float dis = (Player.Instance.transform.position - transform.position).magnitude;
    //    if(dis < AttackState.AttackArray[CurAttackPropertyIndex].AttackRadiusOffset * 0.5f)
    //        AttackAnimIndex = 1;
    //    else
    //        AttackAnimIndex = 0;
		
    //    if(AttackAnims.Length > AttackAnimIndex)
    //    {
    //        AnimationModel.animation.CrossFade(AttackAnims[AttackAnimIndex].name,GeneralAnimBlendTime);
    //    }
    //}
	
	public void Throw()
	{
		//StopChargeEffect();
		
		//Debug.Log("Throw weapon!");
		
		//temp
		// AttackPlayer();
		
		/*
		if (m_ChargeParticleInstance != null)
		{
			StopChargeEffect();
		}
		
		if(projectile)
		{
			if(m_LeftHand)
			{
				Transform obj = Object.Instantiate(projectile,m_LeftHand.position,m_LeftHand.rotation) as Transform;
				obj.GetComponent<WereWolfThrowObj>().werewolf = this;
				obj.GetComponent<WereWolfThrowObj>().speed = ProjectileSpeed;
				obj.GetComponent<WereWolfThrowObj>().damage = ProjectileDamage;
			}
		}
		*/
	}
}
