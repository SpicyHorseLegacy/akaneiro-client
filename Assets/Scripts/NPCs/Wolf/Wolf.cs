using UnityEngine;
using System.Collections;

public class Wolf : NpcBase {
	
	// Use this for initialization
	public override void Start () 
	{
		base.Start();
		
		AvoidanceRadius=1.5f; 
		
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		base.Update();
	}
}
