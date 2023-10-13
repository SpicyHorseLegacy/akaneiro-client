using UnityEngine;
using System.Collections;

public class PlayerSwathFlame : PlayerAbilitySwath {
	
	public Transform VFX_TrailPrefab;
	public Transform VFX_FlamewayPrefab;
	
	Transform vfx_trail;
	
	bool isFlameWay = false;
	Vector3 lastFlameObjPos;
	
	public override void Enter ()
	{
		base.Enter ();
		
		isFlameWay = false;
	}
	
	public override void Execute ()
	{
		base.Execute ();
		
		if(isFlameWay)
		{
			if(Vector3.Distance(Owner.position, lastFlameObjPos) > 0.5f)
			{
				createAFlameWay();
			}
		}
	}
	
	public override void Exit ()
	{
		base.Exit ();
		
		if(vfx_trail)
		{
			vfx_trail.parent = null;
			TrailRenderer[] trails = vfx_trail.GetComponentsInChildren<TrailRenderer>();
			foreach(TrailRenderer trail in trails)
			{
				trail.autodestruct = true;
				trail.transform.parent = null;
				trail.gameObject.AddComponent<DestroyAfterFadeOut>();
				trail.GetComponent<DestroyAfterFadeOut>().GoToHell();
			}
			if(vfx_trail.GetComponent<DestructAfterTime>())
			{
				vfx_trail.GetComponent<DestructAfterTime>().DestructNow();
			}
		}
		
		isFlameWay = false;
	}
	
	public override void EndStep (bool isplayBulletTimeEffect)
	{
		base.EndStep (isplayBulletTimeEffect);

        Debug.LogError("Endstep");

		if(vfx_trail)
		{
			vfx_trail.parent = null;
			TrailRenderer[] trails = vfx_trail.GetComponentsInChildren<TrailRenderer>();
			foreach(TrailRenderer trail in trails)
			{
				trail.autodestruct = true;
				trail.transform.parent = null;
				trail.gameObject.AddComponent<DestroyAfterFadeOut>();
				trail.GetComponent<DestroyAfterFadeOut>().GoToHell();
			}
			if(vfx_trail.GetComponent<DestructAfterTime>())
			{
				vfx_trail.GetComponent<DestructAfterTime>().DestructNow();
			}
		}
	}
	
	public override void playSoundAndVFX(){
		base.playSoundAndVFX();
		
		if(!vfx_trail)
		{
			vfx_trail = CS_Main.Instance.SpawnObject(VFX_TrailPrefab);
			vfx_trail.parent = Owner;
			vfx_trail.position = Owner.position + Vector3.up * 1.2f;
			vfx_trail.rotation = Owner.GetComponent<PlayerMovement>().PlayerObj.rotation;
			vfx_trail.GetComponent<DestructAfterTime>().time = 1;
		}
		
		createAFlameWay();
		isFlameWay = true;
	}
	
	
	private void createAFlameWay()
	{
		if(VFX_FlamewayPrefab)
		{
			Transform flameway = CS_Main.Instance.SpawnObject(VFX_FlamewayPrefab);
			flameway.position = Owner.position;
			flameway.rotation = Player.Instance.GetComponent<PlayerMovement>().PlayerObj.rotation;
			lastFlameObjPos = flameway.position;
		}
	}
}
