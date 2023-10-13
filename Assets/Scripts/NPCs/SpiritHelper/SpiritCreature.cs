using UnityEngine;
using System.Collections;

public class SpiritCreature : SpiritBase {
	
	public AnimationClip[] IdleAnimations;
	public AnimationClip[] RunAnimations;
	
	int[] IdleFixTimes  = {2,1,1};
	int[] IdlePlayTimes = {2,1,1};
	
	int RandomSeed = 0;

	public override void Start ()
	{
		foreach( AnimationClip it in IdleAnimations)
		{
			AnimationModel.animation[it.name].layer = -1;
            AnimationModel.animation[it.name].wrapMode = WrapMode.Once;
		}
		foreach( AnimationClip it in RunAnimations)
		{
            AnimationModel.animation[it.name].layer = -1;
            AnimationModel.animation[it.name].wrapMode = WrapMode.Loop;
		}
		
		base.Start ();
	}
	
	public override void PlayIdleAnim ()
	{
	   int iCount = IdleAnimations.Length;
		
	   if( iCount == 0)
			return;
		
	   int  tempSeed = RandomSeed;
			
	   if(IdlePlayTimes[tempSeed] <= 0)
	   {
	      IdlePlayTimes[tempSeed] = IdleFixTimes[tempSeed];
			
		  if(RandomSeed == 0)
		  {
			 if(IdleAnimations.Length > 2)
			   RandomSeed += Random.Range(1,IdleAnimations.Length);	
			 else
			   RandomSeed = 1;
		  }
		  else
		  {
		     RandomSeed = 0;
		  }
		  
		  tempSeed = RandomSeed;
		  IdlePlayTimes[tempSeed] -= 1;
	   }
	   else
	   {  
		  IdlePlayTimes[tempSeed] -= 1;
	   }  
		
	   string animString = IdleAnimations[RandomSeed].name;

       AnimationModel.animation.CrossFade(animString);
	}
	
	public override void PlayRunAnim()
	{
	   int iCount = RunAnimations.Length;
		
	   if( iCount == 0)
			return;
		
	   int RandomSeed = Random.Range(0,iCount);
		
	   if( RunAnimations[RandomSeed] != null)
	   {
	      string animString = RunAnimations[RandomSeed].name;
          AnimationModel.animation.CrossFade(animString);		
	    }
	}
	
	public override bool IsPlayIdleAnim()
	{
		foreach( AnimationClip it in IdleAnimations)
		{
            if (AnimationModel.animation.IsPlaying(it.name))
				return true;
		}
		
		return false;
	}
	
	public override bool IsPlayRunAnim()
	{
		foreach( AnimationClip it in RunAnimations)
		{
			if(it == null)
			   continue;
            if (AnimationModel.animation.IsPlaying(it.name))
				return true;
		}
		
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
