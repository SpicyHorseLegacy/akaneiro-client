
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveDiffState : MonoBehaviour {

	// Use this for initialization
	bool mbActive = false;
	public bool  mFogEnable;
	public Color mFogColor;
	public FogMode mFogMode = FogMode.Linear;
    public float mFogDensity;
    public float mFogLinearStart;
    public float mFogLinearEnd;
	public Color mAmbientLight;
	public bool mbEnterCave = false;
	
	public Light[] ActualLightOn;
	public Light[] ActualLightOff;
	
	public Transform[] FadingInSounds = new Transform[0];
	public Transform[] FadingOutSounds = new Transform[0];
	
    public float FadeInDuration = 1f;
	public float FadeOutDuration = 1f;
	
	public bool bTrigger = true;
	//public int TriggerID = -1;
	
	public TriggerBase[] pTriggers = null;
	
	public Trigger_Unified[] pNewTriggers = new Trigger_Unified[0];
	
	public List<int> pTrigggerIDs = new List<int>();
	
	public Color GlowFlickerColor;
	
	List<float> FadeInVolumes = new List<float>();
	
	Transform PlayerGlowFicker = null;
	
	void Awake(){
		
		
		
	}
	
	void Start () {
		
		foreach(Transform it in FadingInSounds)
		{
			if(it == null)
				continue;
			FadeInVolumes.Add(it.audio.volume);
		}
		
		PlayerGlowFicker = Player.Instance.transform.FindChild("TabletComponent");
		if( PlayerGlowFicker != null)
		{
			PlayerGlowFicker = PlayerGlowFicker.FindChild("GlowFlicker");
		}
			
		
	    if(!bTrigger &&  CS_SceneInfo.Instance.mTeleportData.bStart == false)
		{
		    ChangeGlowFicker();
		}
		
		
	}
	
	void ChangeGlowFicker()
	{
		if(PlayerGlowFicker != null)
	    {
		   if( PlayerGlowFicker.renderer != null &&  PlayerGlowFicker.renderer.material!= null)
		   {
		       Material mtl = PlayerGlowFicker.renderer.material;
					
			   if(mtl.HasProperty("_TintColor"))
			   {
				  mtl.SetColor("_TintColor",GlowFlickerColor);
			   }
					
		   }
		}
	}
	
	// Update is called once per frame
	void  Active(){
		 mbActive = true;	
	}
	
	bool bMatchTrigger(int id)
	{
	
		foreach(int it in pTrigggerIDs)
		{
			if(it == null)
				continue;
			
			if( CS_SceneInfo.Instance.mTeleportData.triggerID  == it)
				return true;
		}
		return false;
	}
	
	void Update () {
		
		if(  bMatchTrigger(CS_SceneInfo.Instance.mTeleportData.triggerID) == true && 
		   CS_SceneInfo.Instance.mTeleportData.bStart == true &&  CS_SceneInfo.Instance.mTeleportData.bEnterScene == true )
		{ 
	
			RenderSettings.fog = mFogEnable;
            RenderSettings.fogColor = mFogColor;
	        RenderSettings.fogMode = mFogMode;
	        RenderSettings.fogDensity = mFogDensity;
	        RenderSettings.fogStartDistance = mFogLinearStart;
	        RenderSettings.fogEndDistance = mFogLinearEnd;
	        RenderSettings.ambientLight = mAmbientLight;
			
			if(ActualLightOn != null )
		    {
			   foreach(Light iter in ActualLightOn)
				{
				  if( iter == null)
					 continue;
			      iter.enabled = true;
				}
		    }
		    if( ActualLightOff != null)
		    {
			    foreach(Light iter in ActualLightOff)
				{
				   if( iter == null)
					 continue;
				  iter.enabled = false;
				}
		    }
			
			if(bTrigger)
			{
				ChangeGlowFicker();
			}
		}
	
		if(  bMatchTrigger(CS_SceneInfo.Instance.mTeleportData.triggerID) == true )
		{
		     foreach(Transform it in FadingOutSounds)
		     {
				if(it){
					if(it.audio.volume <= 0.0f)
						continue;
				   
				     it.audio.volume -= Time.deltaTime/FadeOutDuration;
				     if (it.audio.volume <= 0.0f)
				         it.audio.volume = 0.0f; 
				     it.audio.Play();
				}
		     }
			 
			 for(int i = 0; i < FadingInSounds.Length;i++)
			 {
				if(FadingInSounds[i])
				{
					if (FadingInSounds[i].audio.volume >= FadeInVolumes[i])
						   continue;
					
					FadingInSounds[i].audio.volume += Time.deltaTime/FadeInDuration;
				    if (FadingInSounds[i].audio.volume >= FadeInVolumes[i])
					    FadingInSounds[i].audio.volume = FadeInVolumes[i];
				    FadingInSounds[i].audio.Play();
				}
				
			 }	
			
		
		}
	}
	
}
