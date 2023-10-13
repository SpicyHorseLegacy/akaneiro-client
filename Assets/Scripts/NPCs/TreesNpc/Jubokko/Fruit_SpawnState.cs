using UnityEngine;
using System.Collections;

public class Fruit_SpawnState : NPC_SpawnState 
{
	//NpcBase npc;
	bool bStartFull=false;
	bool bStartLand=false;
	Vector3 LandPos = Vector3.zero;
	
	public Fruit_SpawnState(Transform o)
	{
		Owner=o;
		npc = Owner.GetComponent<NpcBase>();
	}
	
	public override void Enter()
	{
		npc = Owner.GetComponent<NpcBase>();
		npc.PlaySpawnAnim();
		bStartFull=false;
		bStartLand=false;
		if(npc.AnimationModel != null)
		{
		   npc.AnimationModel.animation["JubkFruit02_Jump"].layer=-1;
		   npc.AnimationModel.animation["JubkFruit02_Jump"].wrapMode = WrapMode.Once;
		}
	}
	
	public override void Execute()
	{
		 Renderer[] NpcRenderers = Owner.GetComponentsInChildren<Renderer>();
		 foreach(Renderer NpcRenderer in NpcRenderers)
		 {
	        NpcRenderer.enabled = true;
	     }
		
		if(!npc.AnimationModel.animation.isPlaying)
		{
			if(bStartLand)
			{
				npc.FSM.ChangeState(npc.IS);
				Vector3 dir = (Player.Instance.transform.position - Owner.position).normalized;
				dir.y=0f;
				Owner.forward = dir;
				return;
			}
			
			if(!bStartFull)
			{
				RaycastHit hit;
				if(Physics.Raycast(Owner.position,Vector3.down,out hit,100f,1<<LayerMask.NameToLayer("Walkable")))
				{
					LandPos = hit.point;
					bStartFull = true;
				}
				else
				{
					//can not find land point
					npc.AnimationModel.animation.CrossFade("JubkFruit02_Jump");
					bStartLand=true;
				}
			}
			else
			{
				Vector3 pos = Owner.position;
				pos.y -= 9.8f * Time.deltaTime;
				if(pos.y < LandPos.y)
				{
					pos = LandPos;
					Owner.position = LandPos;
					npc.AnimationModel.animation.CrossFade("JubkFruit02_Jump");
					bStartLand=true;
				}
				else
				{
					Owner.position = pos;
				}
			}			
		}
	}
	
	public override void Exit()
	{
		base.Exit();
	}
}
