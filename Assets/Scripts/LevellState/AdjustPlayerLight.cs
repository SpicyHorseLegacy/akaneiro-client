using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdjustPlayerLight : MonoBehaviour {

	Light PointLightOfPlayer = null;
	
	public float LightRange = 0f;
	public Color LightColor = new Color(1.0f,1.0f,1.0f,1.0f);
    public float intensity = 0f;
	
	public TriggerBase[] pTriggers = null;
	
	public Trigger_Unified[] pNewTriggers = new Trigger_Unified[0];
	
	public List<int> pTrigggerIDs = new List<int>();
	
	void Start () {
		
		if(Player.Instance != null)
		{
		   Transform lightTransForm = Player.Instance.transform.FindChild("Point light");
			
		   if(lightTransForm != null)
			  PointLightOfPlayer = lightTransForm.GetComponent<Light>();
			
		}
		
		if(PointLightOfPlayer != null )
		{
			PointLightOfPlayer.range = LightRange;
			
			PointLightOfPlayer.color = LightColor;
			
			PointLightOfPlayer.intensity = intensity;
		}
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

	// Update is called once per frame
	void Update () {
		
	    if( bMatchTrigger(CS_SceneInfo.Instance.mTeleportData.triggerID) == true && 
		   CS_SceneInfo.Instance.mTeleportData.bStart == true &&  CS_SceneInfo.Instance.mTeleportData.bEnterScene == true )
		{
		    if(PointLightOfPlayer != null)
		    {
			    PointLightOfPlayer.range = LightRange;
			
			    PointLightOfPlayer.color = LightColor;
			
			    PointLightOfPlayer.intensity = intensity;
			}
	
		}
		  
	}
	
	
	



	
}
