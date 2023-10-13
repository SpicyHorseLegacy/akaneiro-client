using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arrow : AbilityObject {

	public Transform Owner;
	bool fire = false;
	float flyingTime = 0f;
	
	public float Speed = 50f;
	public Transform VFX_Impact;

	// Update is called once per frame
	void Update () {
	
		if(fire)
		{
			flyingTime += Time.deltaTime;
			if(flyingTime > 1f)
				hideArrow(null);
		}
	}
	
	void OnTriggerEnter( Collider other )
	{
		//print(other.name);
		if(fire)
			hideArrow(other.transform);
	}
	
	public void Shoot(Vector3 flyDir)
	{
		transform.forward = new Vector3(flyDir.x,0,flyDir.z);
		fire = true;
		flyingTime = 0;
		rigidbody.velocity = transform.forward * Speed;
		
		TrailRenderer trail = transform.GetComponentInChildren<TrailRenderer>();
		trail.enabled = true;
	}
	
	
	public void hideArrow( Transform hit ){
		
		if(VFX_Impact)
		{
			Transform vfx = Instantiate(VFX_Impact) as Transform;
			vfx.position = transform.position + transform.forward;
			vfx.rotation = transform.rotation;
			
			if(hit && hit.GetComponent<BaseObject>() && ( hit.GetComponent<BaseObject>().ObjType == ObjectType.NPC || hit.GetComponent<BaseObject>().ObjType == ObjectType.Enermy ))
			{
				Vector3 pos = hit.position;
				pos.y = transform.position.y;
				vfx.position = pos;
				
				Vector3 dir = transform.position - Owner.position;
				vfx.forward = dir;
			}
		}
		
		TrailRenderer trail = transform.GetComponentInChildren<TrailRenderer>();
		trail.transform.parent = null;
		trail.autodestruct = true;
		
		gameObject.SetActiveRecursively(false);
		resetArrow();
	}
	
	public void resetArrow()
	{
		fire = false;
		flyingTime=0;
		rigidbody.velocity = Vector3.zero;
	}
	
	public void DestroySelf()
	{
		hideArrow(null);
		Destroy(gameObject);
	}
}
