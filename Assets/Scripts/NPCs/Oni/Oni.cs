using UnityEngine;
using System.Collections;

public class Oni : NpcBase {

	// Use this for initialization
	Transform OniwhooshSound;
	
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
	
	public override void PlayTurnLeftAnim ()
	{
		animation["Oni_Walk_Turn_Lft"].layer=-1;
		animation["Oni_Walk_Turn_Lft"].wrapMode = WrapMode.Loop;
		animation.CrossFade("Oni_Walk_Turn_Lft",GeneralAnimBlendTime);
	}
	
	public override void PlayTurnRightAnim ()
	{
		animation["Oni_Walk_Turn_Rgt"].layer=-1;
		animation["Oni_Walk_Turn_Rgt"].wrapMode = WrapMode.Loop;
		animation.CrossFade("Oni_Walk_Turn_Rgt",GeneralAnimBlendTime);
	}
	
	public void PlayWhooshSound()
	{
		Transform[] Sounds = GetComponent<NpcSoundEffect>().MiscSoundPrefabs;
		if( Sounds != null && Sounds.Length > 0)                              
		   OniwhooshSound = PlaySound(GetComponent<NpcSoundEffect>().MiscSoundPrefabs[0],OniwhooshSound);
		
	}
}
