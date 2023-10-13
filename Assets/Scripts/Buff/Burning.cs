using UnityEngine;
using System.Collections;

public class Burning : BaseBuff {
	Transform vfx;
	
	public override void Enter()
	{
		base.Enter();
		
		if(Owner)
		{
			if(VFXPrefab)
			{
				vfx = Instantiate(VFXPrefab, Owner.position + Vector3.up * 0.5f, VFXPrefab.rotation) as Transform;
				vfx.parent = Owner;
			}
		}
	}
	
	public override void Execute(){
		base.Execute();
	}
	
	public override void Exit()
	{
		if(vfx)
			vfx.GetComponent<DestructAfterTime>().DestructNow();
		
		base.Exit();
	}
}
