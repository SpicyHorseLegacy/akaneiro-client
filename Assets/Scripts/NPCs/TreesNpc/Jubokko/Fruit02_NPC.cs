using UnityEngine;
using System.Collections;

public class Fruit02_NPC : NpcBase 
{
	public override void InitialState()
	{
		base.InitialState();
		
		GameObject states = transform.FindChild("STATES").gameObject;
	
		if(states.GetComponent<NPC_SpawnState>())
		   Object.Destroy(states.GetComponent<NPC_SpawnState>());
		
		SpawnState = states.AddComponent<Fruit_SpawnState>();
		
		SpawnState.SetNPC(this);
		
	}
	
	public override void Start () 
	{
		base.Start();
		
		AvoidanceRadius=0.5f; 
		if(AnimationModel != null)
		{
		   AnimationModel.animation["JubkFruit02_Land"].layer=-1;
		   AnimationModel.animation["JubkFruit02_Land"].wrapMode = WrapMode.Once;
			
		   AnimationModel.animation["JubkFruit02_Jump"].layer=-1;
		   AnimationModel.animation["JubkFruit02_Jump"].wrapMode = WrapMode.Once;
		}
		
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		base.Update();
	}

}
