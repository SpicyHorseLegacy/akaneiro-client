using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class FogAdaping : MonoBehaviour {
	
	public bool  S1Uptodate = false;
    public bool  S1FogEnable;
	public Color S1FogColor;
	public FogMode S1FogMode = FogMode.Linear;
    public float S1FogDensity;
    public float S1FogLinearStart;
    public float S1FogLinearEnd;
	public Color S1AmbientLight;   
		
	public bool  S2Uptodate = false;
	public bool  S2FogEnable;
	public Color S2FogColor;
	public FogMode S2FogMode = FogMode.Linear;
    public float S2FogDensity;
    public float S2FogLinearStart;
    public float S2FogLinearEnd;
	public Color S2AmbientLight;   
	
	public bool  S3Uptodate = false;
	public bool  S3FogEnable;
	public Color S3FogColor;
	public FogMode S3FogMode = FogMode.Linear;
    public float S3FogDensity;
    public float S3FogLinearStart;
    public float S3FogLinearEnd;
	public Color S3AmbientLight;   
	
	public bool  S4Uptodate = false;
	public bool  S4FogEnable;
	public Color S4FogColor;
	public FogMode S4FogMode = FogMode.Linear;
    public float S4FogDensity;
    public float S4FogLinearStart;
    public float S4FogLinearEnd;
	public Color S4AmbientLight;  
	
	
	public bool  S5Uptodate = false;
	public bool  S5FogEnable;
	public Color S5FogColor;
	public FogMode S5FogMode = FogMode.Linear;
    public float S5FogDensity;
    public float S5FogLinearStart;
    public float S5FogLinearEnd;
	public Color S5AmbientLight;   
	
	public bool  S6Uptodate = false;
	public bool  S6FogEnable;
	public Color S6FogColor;
	public FogMode S6FogMode = FogMode.Linear;
    public float S6FogDensity;
    public float S6FogLinearStart;
    public float S6FogLinearEnd;
	public Color S6AmbientLight;   
	
	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!Application.isPlaying )
		{
			if(S1Uptodate)
			{
				S1Uptodate = false;
				S1FogEnable = RenderSettings.fog;
			    S1FogColor = RenderSettings.fogColor;
			    S1FogMode = RenderSettings.fogMode;
			    S1FogDensity = RenderSettings.fogDensity;
			    S1FogLinearStart = RenderSettings.fogStartDistance;
			    S1FogLinearEnd = RenderSettings.fogEndDistance;
			    S1AmbientLight = RenderSettings.ambientLight;
			}
			
			if(S2Uptodate)
			{
				S2Uptodate = false;
				S2FogEnable = RenderSettings.fog;
			    S2FogColor = RenderSettings.fogColor;
			    S2FogMode = RenderSettings.fogMode;
			    S2FogDensity = RenderSettings.fogDensity;
			    S2FogLinearStart = RenderSettings.fogStartDistance;
			    S2FogLinearEnd = RenderSettings.fogEndDistance;
			    S2AmbientLight = RenderSettings.ambientLight;
			}
			if(S3Uptodate)
			{
				S3Uptodate = false;
				S3FogEnable = RenderSettings.fog;
			    S3FogColor = RenderSettings.fogColor;
			    S3FogMode = RenderSettings.fogMode;
			    S3FogDensity = RenderSettings.fogDensity;
			    S3FogLinearStart = RenderSettings.fogStartDistance;
			    S3FogLinearEnd = RenderSettings.fogEndDistance;
			    S3AmbientLight = RenderSettings.ambientLight;
			}
			if(S4Uptodate)
			{
				S4Uptodate = false;
				S4FogEnable = RenderSettings.fog;
			    S4FogColor = RenderSettings.fogColor;
			    S4FogMode = RenderSettings.fogMode;
			    S4FogDensity = RenderSettings.fogDensity;
			    S4FogLinearStart = RenderSettings.fogStartDistance;
			    S4FogLinearEnd = RenderSettings.fogEndDistance;
			    S4AmbientLight = RenderSettings.ambientLight;
			}
			if(S5Uptodate)
			{
				S5Uptodate = false;
				S5FogEnable = RenderSettings.fog;
			    S5FogColor = RenderSettings.fogColor;
			    S5FogMode = RenderSettings.fogMode;
			    S5FogDensity = RenderSettings.fogDensity;
			    S5FogLinearStart = RenderSettings.fogStartDistance;
			    S5FogLinearEnd = RenderSettings.fogEndDistance;
			    S5AmbientLight = RenderSettings.ambientLight;
			}
			if(S6Uptodate)
			{
				S6Uptodate = false;
				S6FogEnable = RenderSettings.fog;
			    S6FogColor = RenderSettings.fogColor;
			    S6FogMode = RenderSettings.fogMode;
			    S6FogDensity = RenderSettings.fogDensity;
			    S6FogLinearStart = RenderSettings.fogStartDistance;
			    S6FogLinearEnd = RenderSettings.fogEndDistance;
			    S6AmbientLight = RenderSettings.ambientLight;
			}
		}
	    
		else
		{
		   if(GlobalGameState.bInCave)
				return;
			
		   if(GlobalGameState.state  == "s1")
		   {
			   RenderSettings.fog = S1FogEnable;
			   RenderSettings.fogColor = S1FogColor;
			   RenderSettings.fogMode = S1FogMode;
			   RenderSettings.fogDensity = S1FogDensity;
			   RenderSettings.fogStartDistance = S1FogLinearStart;
			   RenderSettings.fogEndDistance = S1FogLinearEnd;
			   RenderSettings.ambientLight = S1AmbientLight;
		   }
	       else if(GlobalGameState.state  == "s2")
		   {
			   RenderSettings.fog = S2FogEnable;
			   RenderSettings.fogColor = S2FogColor;
			   RenderSettings.fogMode = S2FogMode;
			   RenderSettings.fogDensity = S2FogDensity;
			   RenderSettings.fogStartDistance = S2FogLinearStart;
			   RenderSettings.fogEndDistance = S2FogLinearEnd;
			   RenderSettings.ambientLight = S2AmbientLight;
		   }
		   else if(GlobalGameState.state  == "s3")
		   {
			   RenderSettings.fog = S3FogEnable;
			   RenderSettings.fogColor = S3FogColor;
			   RenderSettings.fogMode = S3FogMode;
			   RenderSettings.fogDensity = S3FogDensity;
			   RenderSettings.fogStartDistance = S3FogLinearStart;
			   RenderSettings.fogEndDistance = S3FogLinearEnd;
			   RenderSettings.ambientLight = S3AmbientLight;
		   }
		   else if(GlobalGameState.state  == "s4")
		   {
			   RenderSettings.fog = S4FogEnable;
			   RenderSettings.fogColor = S4FogColor;
			   RenderSettings.fogMode = S4FogMode;
			   RenderSettings.fogDensity = S4FogDensity;
			   RenderSettings.fogStartDistance = S4FogLinearStart;
			   RenderSettings.fogEndDistance = S4FogLinearEnd;
			   RenderSettings.ambientLight = S4AmbientLight;
		   }
		   else if(GlobalGameState.state  == "s5")
		   {
			   RenderSettings.fog = S5FogEnable;
			   RenderSettings.fogColor = S5FogColor;
			   RenderSettings.fogMode = S5FogMode;
			   RenderSettings.fogDensity = S5FogDensity;
			   RenderSettings.fogStartDistance = S5FogLinearStart;
			   RenderSettings.fogEndDistance = S5FogLinearEnd;
			   RenderSettings.ambientLight = S5AmbientLight;
		   }
		  else if(GlobalGameState.state  == "s6")
		   {
			   RenderSettings.fog = S6FogEnable;
			   RenderSettings.fogColor = S6FogColor;
			   RenderSettings.fogMode = S6FogMode;
			   RenderSettings.fogDensity = S6FogDensity;
			   RenderSettings.fogStartDistance = S6FogLinearStart;
			   RenderSettings.fogEndDistance = S6FogLinearEnd;
			   RenderSettings.ambientLight = S6AmbientLight;
		   }
			
		}
		
	} 
}
