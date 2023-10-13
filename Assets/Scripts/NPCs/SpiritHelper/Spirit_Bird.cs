using UnityEngine;
using System.Collections;

public class Spirit_Bird : SpiritBase {
	
	public float[] DamageIncrease = { 0.05f, 0.1f,0.15f}; 
	
	public override void Start ()
	{
		base.Start ();
		
		animation["GAM_Yosuzume_Idle_1"].layer = -1;
		animation["GAM_Yosuzume_Idle_1"].wrapMode = WrapMode.Loop;
		
		animation["GAM_Yosuzume_Run_1"].layer = -1;
		animation["GAM_Yosuzume_Run_1"].wrapMode = WrapMode.Loop;
		
		animation["GAM_Yosuzume_Idle_2"].layer = -1;
		animation["GAM_Yosuzume_Idle_2"].wrapMode = WrapMode.Loop;
		
	}
	
	public override void PlayIdleAnim ()
	{
	   int RandomSeed = Random.Range(1,3);
	   string animString = "GAM_Yosuzume_Idle_" + RandomSeed.ToString();
	   animation.CrossFade(animString);	
	}
	
	public override void PlayRunAnim()
	{
	   animation.CrossFade("GAM_Yosuzume_Run_1");		
	}
	
	public override bool IsPlayIdleAnim()
	{
		if ( animation.IsPlaying("GAM_Yosuzume_Idle_1") ||
		     animation.IsPlaying("GAM_Yosuzume_Idle_2") )
			return true;
		return false;
	}
	
	public override bool IsPlayRunAnim()
	{
		if ( animation.IsPlaying("GAM_Yosuzume_Run_1") )
			return true;
		return false;
	}
	
	public override void CallOn ()
	{
		base.CallOn ();
		
	}

	public override void CallOff ()
	{
		base.CallOff ();
	
		Destroy(gameObject);
	}
}
