using UnityEngine;
using System.Collections;

public class Buff_SkinStone : BaseBuff {
	
	public Transform Active_VFX_Prefab;
	Transform vfx;
	
	public Transform AbilityOnSoundPrefab;
	public Transform AbilityOffSoundPrefab;
	
	Transform AbilityOnSound;
	Transform AbilityOffSound;
	
	public override void Enter()
	{
		base.Enter();
		
		if(Owner)
		{
			if(Active_VFX_Prefab)
			{
				Transform activevfx = Instantiate(Active_VFX_Prefab, Owner.position, Active_VFX_Prefab.rotation) as Transform;
				activevfx.parent = Owner;
			}
			
			if(VFXPrefab && !vfx)
			{
				vfx = Instantiate(VFXPrefab, Owner.position + Vector3.up * 1.2f, VFXPrefab.rotation) as Transform;
				vfx.parent = Owner;
			}
			
			Player.Instance.SetSkinOfStoneMaterial(true);
		}
        if (AbilityOnSoundPrefab != null)
        {
            SoundCue.PlayPrefabAndDestroy(AbilityOnSoundPrefab, Owner.position);
        }
	}
	
	public override void Execute(){
		base.Execute();
	}
	
	public override void Exit()
	{
		Player.Instance.SetSkinOfStoneMaterial(false);
		if(vfx)
			vfx.GetComponent<DestructAfterTime>().DestructNow();

        if (AbilityOffSoundPrefab != null)
        {
            SoundCue.PlayPrefabAndDestroy(AbilityOffSoundPrefab, Owner.position);
        }
		
		base.Exit();
	}
}
