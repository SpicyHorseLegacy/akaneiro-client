using UnityEngine;
using System.Collections;

public class Buff_HPRegenBonus : BaseBuff {
	
	public Transform ActiveVFXPrefab;
	public Transform TickVFXPrefab;
	
	Transform vfx;
	
	public override void Enter()
	{
		base.Enter();
		
		if(Owner)
		{
			if(ActiveVFXPrefab)
			{
				Transform activeVFX = Instantiate(ActiveVFXPrefab, Owner.position + Vector3.up * 0.3f, Owner.rotation) as Transform;
				activeVFX.parent = Owner;
			}
			
			if(VFXPrefab && !vfx)
			{
				vfx = Instantiate(VFXPrefab, Owner.position + Vector3.up * 0.3f, VFXPrefab.rotation) as Transform;
				vfx.parent = Owner;
			}
		}
	}
	
	public override void Execute()
	{
		base.Execute();
	}
	
	public override void Exit()
	{
		if(vfx)
			vfx.GetComponent<DestructAfterTime>().DestructNow();
		
		base.Exit();
	}
	
	public override void TickExecute()
	{
		if(TickVFXPrefab && Owner)
		{
			Transform tickVFX = Instantiate(TickVFXPrefab, Owner.position, Owner.rotation) as Transform;
			tickVFX.parent = Owner;
		}
	}
}
