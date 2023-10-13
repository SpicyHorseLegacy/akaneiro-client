using UnityEngine;
using System.Collections;

public class PlayerSwathDestruction : PlayerAbilitySwath {
	
	public Transform VFX_TrailPrefab;
	public Transform VFX_AbilityCompletePrefab;
	
	Transform vfx_trail;
	
	public override void Exit ()
	{
		base.Exit ();
		
		if(vfx_trail)
		{
			//vfx_trail.gameObject.SetActiveRecursively(false);
			vfx_trail.parent = null;
			if(vfx_trail.GetComponent<DestructAfterTime>())
			{
				vfx_trail.GetComponent<DestructAfterTime>().DestructNow();
			}
		}
	}
	
	public override void EndStep (bool isplayBulletTimeEffect)
	{
		base.EndStep (isplayBulletTimeEffect);
		
		if(VFX_AbilityCompletePrefab)
		{
			Transform vfx = CS_Main.Instance.SpawnObject(VFX_AbilityCompletePrefab);
			vfx.position = Owner.position + Vector3.up *1f + Owner.GetComponent<PlayerMovement>().PlayerObj.forward.normalized * 0.8f;
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
			vfx_trail.gameObject.SetActiveRecursively(true);
			vfx_trail.GetComponent<DestructAfterTime>().time = 1;
		}
	}
	
}
