using System;
using UnityEditor;
using UnityEngine;

class StateChanger : EditorWindow
{
	
	enum GameStateEnum
	{
		s1,
		s2,
		s3,
		s4,
	}
    static StateChanger window;
	
	GameStateEnum mOption;
	
	StaticMeshState[] AllGroups;
	
	LightTurnonoff[] LightGroups;
	
    FogAdaping[] FogGroups;
	
	GlobalGameState[] GameStateGroups;

	
	
    
    [MenuItem("Custom/State Change")]
    static void Execute()
    {
       if (window == null)
            window = (StateChanger)GetWindow(typeof (StateChanger));
		
        window.Show();
		
    }

    void OnGUI()
    {
        mOption = (GameStateEnum)EditorGUILayout.EnumPopup("Which State Active", mOption);
	
        if(GUILayout.Button("ToggleOn"))
		{  
		   AllGroups = Resources.FindObjectsOfTypeAll(typeof(StaticMeshState)) as StaticMeshState[];
			
		   LightGroups = Resources.FindObjectsOfTypeAll(typeof(LightTurnonoff)) as LightTurnonoff[];
			
		   FogGroups = Resources.FindObjectsOfTypeAll(typeof(FogAdaping)) as FogAdaping[];
			
			
		   GameStateGroups = Resources.FindObjectsOfTypeAll(typeof(GlobalGameState)) as GlobalGameState[];
		   string dest = CovertStateString(mOption);
			
			
		   GlobalGameState.state = dest;
		   /*
		   if(GameStateGroups!= null)
		   {
			  foreach(GlobalGameState single in GameStateGroups)
			  {
					single.preState = dest;
					single.bCanEdit = true;
			  }
		   }
		   */
			
		   if( AllGroups != null)
		   {
		      foreach(StaticMeshState single in AllGroups)
		      {
				  bool bmatch = false;
				
				  if( single.LevelStateList == null)
					 continue;
				
				  foreach(string sp in single.LevelStateList)
				  {
					 if( sp == dest)
					 {
					    bmatch = true;
					    break;
					 }
				  }
				  if(bmatch)
					 single.gameObject.SetActiveRecursively(true);
				  else 
					 single.gameObject.SetActiveRecursively(false);
		       }
			}
			
			if( FogGroups != null)
			{
			   foreach(FogAdaping single in FogGroups)
			   {
					if( mOption ==  StateChanger.GameStateEnum.s1)
					{
						RenderSettings.fog = single.S1FogEnable;
			            RenderSettings.fogColor = single.S1FogColor;
			            RenderSettings.fogMode = single.S1FogMode;
			            RenderSettings.fogDensity = single.S1FogDensity;
			            RenderSettings.fogStartDistance = single.S1FogLinearStart;
			            RenderSettings.fogEndDistance = single.S1FogLinearEnd;
			            RenderSettings.ambientLight = single.S1AmbientLight;
					}
					else if( mOption ==  StateChanger.GameStateEnum.s2 )
					{
						RenderSettings.fog = single.S2FogEnable;
			            RenderSettings.fogColor = single.S2FogColor;
			            RenderSettings.fogMode = single.S2FogMode;
			            RenderSettings.fogDensity = single.S2FogDensity;
			            RenderSettings.fogStartDistance = single.S2FogLinearStart;
			            RenderSettings.fogEndDistance = single.S2FogLinearEnd;
			            RenderSettings.ambientLight = single.S2AmbientLight;
					}
					else if( mOption ==  StateChanger.GameStateEnum.s3 )
					{
						RenderSettings.fog = single.S3FogEnable;
			            RenderSettings.fogColor = single.S3FogColor;
			            RenderSettings.fogMode = single.S3FogMode;
			            RenderSettings.fogDensity = single.S3FogDensity;
			            RenderSettings.fogStartDistance = single.S3FogLinearStart;
			            RenderSettings.fogEndDistance = single.S3FogLinearEnd;
				            RenderSettings.ambientLight = single.S3AmbientLight;
						
					}
					else if( mOption ==  StateChanger.GameStateEnum.s4 )
					{
						RenderSettings.fog = single.S4FogEnable;
			            RenderSettings.fogColor = single.S4FogColor;
			            RenderSettings.fogMode = single.S4FogMode;
			            RenderSettings.fogDensity = single.S4FogDensity;
			            RenderSettings.fogStartDistance = single.S4FogLinearStart;
			            RenderSettings.fogEndDistance = single.S4FogLinearEnd;
			            RenderSettings.ambientLight = single.S4AmbientLight;
					}
			   }
			}
			
			//window.Close();
         }
		
		 if(GUILayout.Button("ActiveAll"))
		 {
		    AllGroups = Resources.FindObjectsOfTypeAll(typeof(StaticMeshState)) as StaticMeshState[];
			
		    LightGroups = Resources.FindObjectsOfTypeAll(typeof(LightTurnonoff)) as LightTurnonoff[];
			
		    FogGroups = Resources.FindObjectsOfTypeAll(typeof(FogAdaping)) as FogAdaping[];
			
			if( AllGroups != null)
		    {
		      foreach(StaticMeshState single in AllGroups)
		      {
				 single.gameObject.SetActiveRecursively(true);
			  }
		    }
			/*
		    if( LightGroups != null)
			{ 
			   foreach(LightTurnonoff single in LightGroups)
			   {
				  single.gameObject.SetActiveRecursively(true);
			   }
			}
			*/
			
			
			//window.Close();
		 }
		
    }
	
	string CovertStateString(GameStateEnum op)
	{
		string rs ="";
		switch(op)
		{
		   case GameStateEnum.s1:
			 rs = "s1";
			 break;
		   case GameStateEnum.s2:
			 rs = "s2";
			 break;
		   case GameStateEnum.s3:
			 rs = "s3";
			 break;
		   case GameStateEnum.s4:
			 rs = "s4";
			 break;
		   default:
			  break;
		}
		return rs;
	}
	
}