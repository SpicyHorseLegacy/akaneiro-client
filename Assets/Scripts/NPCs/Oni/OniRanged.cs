using UnityEngine;
using System.Collections;

public class OniRanged : NpcBase {
	
	public Transform projectile;
	public Transform ReadyParticle;
	public float ProjectileSpeed = 20f;
	[HideInInspector]
	public int ProjectileDamage = 5;
	
	[HideInInspector]
	public Transform ready_particle; 
	
	bool IsWeaponReady = false;
	Transform RightHand=null;
	
	protected override void Awake ()
	{
        base.Awake();

		Component[] all = transform.GetComponentsInChildren<Component>();
		foreach(Component T in all)
		{
			if(T.name == "Bip001 Prop1")
			{
				RightHand = T.transform;
				break;
			}
		}			

		if(ReadyParticle && !ready_particle)
		{
			ready_particle = Object.Instantiate(ReadyParticle,RightHand.position,RightHand.rotation) as Transform;
			ready_particle.gameObject.SetActiveRecursively(false);
			ready_particle.parent = RightHand;
		}
		
		IsRangedNpc = true;
	}
	
	// Use this for initialization
	public override void Start () 
	{
		base.Start();
		
		//AttackRadius = 7f;
		//AvoidanceRadius=1.5f; 

	}
	
	// Update is called once per frame
	public override void Update () 
	{
		base.Update();
	}
	
	public override void PlayIdleAnim ()
	{
		base.PlayIdleAnim ();
		
		/*
		if(!RightHand) return;
		
		if(projectile && !IsWeaponReady)
		{
			Transform obj = Object.Instantiate(projectile) as Transform;
			obj.position = RightHand.position;
			obj.rotation = RightHand.rotation;
			obj.parent = RightHand;
			obj.name = "projectile_obj";
			obj.GetComponent<OniThrowObj>().speed = ProjectileSpeed;
			//obj.GetComponent<OniThrowObj>().damage = ProjectileDamage;
			IsWeaponReady = true;
		}
		*/
		
		
		if(ready_particle)
		{
			ready_particle.gameObject.SetActiveRecursively(true);
			ready_particle.parent = RightHand;
		}
	}
	
	public void Throw()
	{
		//Debug.Log("Throw weapon!");
		//if(!RightHand) return;
		 
		//temp
		// AttackPlayer();
		
		
		/*
		
		Transform obj = RightHand.FindChild("projectile_obj");
		
		if(obj)
		{
			obj.parent = null;
			obj.name = "";
			obj.position = RightHand.position;
			obj.rotation = RightHand.rotation;
			obj.GetComponent<OniThrowObj>().dir = (Player.Instance.transform.position + Vector3.up * 0.8f - obj.position).normalized;
			obj.GetComponent<OniThrowObj>().oni = this;
			
			IsWeaponReady = false;
				
			if(ReadyParticle)
			{
				ready_particle.gameObject.SetActiveRecursively(false);
			}
		}
		*/
	}
}
