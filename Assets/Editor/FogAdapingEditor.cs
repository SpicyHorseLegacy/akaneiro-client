using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FogAdaping))] 
public class FogAdapingEditor : Editor
{
	private FogAdaping theScript;
	
	private bool m_s1Foldout = true;
	private bool m_s2Foldout = true;
	private bool m_s3Foldout = true;
	private bool m_s4Foldout = true;
	private bool m_s5Foldout = true;
	private bool m_s6Foldout = true;
	
	//private bool m_PreviewFoldout = true;
	
	void OnEnable() 
	{
        Debug.Log("editorscript was enabled");
		theScript = (FogAdaping)target;
		/*
		if (!Application.isPlaying)
		{
			if (!theScript.IsPreviewing)
			{
				Sample();
			}
			theScript.CanUpdateEditorControlled = true;
		}
		*/
		
	}
	
	void OnDisable()
	{
		Debug.Log("script was disabled");
		//theScript.CanUpdateEditorControlled = false;
	}
	
	void OnDestroy()
	{
		Debug.Log("script was destroyed");
		//theScript.CanUpdateEditorControlled = false;
	}
	
	public override void OnInspectorGUI()
	{
		//DrawDefaultInspector ();
		EditorGUIUtility.LookLikeInspector();
		
		EditorGUILayout.Space();
		m_s1Foldout = EditorGUILayout.Foldout(m_s1Foldout, "S1 RenderSettings:");
		
		if (m_s1Foldout)
		{
			EditorGUI.indentLevel = 1;
			theScript.S1Uptodate = EditorGUILayout.Toggle("UpdateToDate", theScript.S1Uptodate);
			theScript.S1FogEnable = EditorGUILayout.Toggle("Enable Fog", theScript.S1FogEnable);
			theScript.S1FogColor = EditorGUILayout.ColorField("Fog Colour", theScript.S1FogColor);
		    theScript.S1FogMode = (FogMode)EditorGUILayout.EnumPopup("Fog Mode", theScript.S1FogMode);
		    theScript.S1FogDensity = Mathf.Clamp(EditorGUILayout.FloatField("Fog Density", theScript.S1FogDensity), 0f, 1f);
		    theScript.S1FogLinearStart = EditorGUILayout.FloatField("Linear Fog Start", theScript.S1FogLinearStart);
		    theScript.S1FogLinearEnd = EditorGUILayout.FloatField("Linear Fog End", theScript.S1FogLinearEnd);
			theScript.S1AmbientLight = EditorGUILayout.ColorField("Ambient Light", theScript.S1AmbientLight);
		}
		EditorGUI.indentLevel = 0;
		
		EditorGUILayout.Space();
		m_s2Foldout = EditorGUILayout.Foldout(m_s2Foldout, "S2 RenderSettings:");
		
		if (m_s2Foldout)
		{
			EditorGUI.indentLevel = 1;
			theScript.S2Uptodate = EditorGUILayout.Toggle("UpdateToDate", theScript.S2Uptodate);
			theScript.S2FogEnable = EditorGUILayout.Toggle("Enable Fog", theScript.S2FogEnable);
			theScript.S2FogColor = EditorGUILayout.ColorField("Fog Colour", theScript.S2FogColor);
			theScript.S2FogMode = (FogMode)EditorGUILayout.EnumPopup("Fog Mode", theScript.S2FogMode);
		    theScript.S2FogDensity = Mathf.Clamp(EditorGUILayout.FloatField("Fog Density", theScript.S2FogDensity), 0f, 1f);
		    theScript.S2FogLinearStart = EditorGUILayout.FloatField("Linear Fog Start", theScript.S2FogLinearStart);
		    theScript.S2FogLinearEnd = EditorGUILayout.FloatField("Linear Fog End", theScript.S2FogLinearEnd);
			theScript.S2AmbientLight = EditorGUILayout.ColorField("Ambient Light", theScript.S2AmbientLight);
		}
		EditorGUI.indentLevel = 0;
		
		EditorGUILayout.Space();
		m_s3Foldout = EditorGUILayout.Foldout(m_s3Foldout, "S3 RenderSettings:");
		
		if (m_s3Foldout)
		{
			EditorGUI.indentLevel = 1;
			theScript.S3Uptodate = EditorGUILayout.Toggle("UpdateToDate", theScript.S3Uptodate);
			theScript.S3FogEnable = EditorGUILayout.Toggle("Enable Fog", theScript.S3FogEnable);
			theScript.S3FogColor = EditorGUILayout.ColorField("Fog Colour", theScript.S3FogColor);
			theScript.S3FogMode = (FogMode)EditorGUILayout.EnumPopup("Fog Mode", theScript.S3FogMode);
		    theScript.S3FogDensity = Mathf.Clamp(EditorGUILayout.FloatField("Fog Density", theScript.S3FogDensity), 0f, 1f);
		    theScript.S3FogLinearStart = EditorGUILayout.FloatField("Linear Fog Start", theScript.S3FogLinearStart);
		    theScript.S3FogLinearEnd = EditorGUILayout.FloatField("Linear Fog End", theScript.S3FogLinearEnd);
			theScript.S3AmbientLight = EditorGUILayout.ColorField("Ambient Light", theScript.S3AmbientLight);
		}
		EditorGUI.indentLevel = 0;
		
		EditorGUILayout.Space();
		m_s4Foldout = EditorGUILayout.Foldout(m_s4Foldout, "S4 RenderSettings:");
		
		if (m_s4Foldout)
		{
			EditorGUI.indentLevel = 1;
			theScript.S4Uptodate = EditorGUILayout.Toggle("UpdateToDate", theScript.S4Uptodate);
			theScript.S4FogEnable = EditorGUILayout.Toggle("Enable Fog", theScript.S4FogEnable);
			theScript.S4FogColor = EditorGUILayout.ColorField("Fog Colour", theScript.S4FogColor);
			theScript.S4FogMode = (FogMode)EditorGUILayout.EnumPopup("Fog Mode", theScript.S4FogMode);
		    theScript.S4FogDensity = Mathf.Clamp(EditorGUILayout.FloatField("Fog Density", theScript.S4FogDensity), 0f, 1f);
		    theScript.S4FogLinearStart = EditorGUILayout.FloatField("Linear Fog Start", theScript.S4FogLinearStart);
		    theScript.S4FogLinearEnd = EditorGUILayout.FloatField("Linear Fog End", theScript.S4FogLinearEnd);
			theScript.S4AmbientLight = EditorGUILayout.ColorField("Ambient Light", theScript.S4AmbientLight);
		}
		EditorGUI.indentLevel = 0;
		
		EditorGUILayout.Space();
		m_s5Foldout = EditorGUILayout.Foldout(m_s5Foldout, "S5 RenderSettings:");
		
		if (m_s5Foldout)
		{
			EditorGUI.indentLevel = 1;
			theScript.S5Uptodate = EditorGUILayout.Toggle("UpdateToDate", theScript.S5Uptodate);
			theScript.S5FogEnable = EditorGUILayout.Toggle("Enable Fog", theScript.S5FogEnable);
			theScript.S5FogColor = EditorGUILayout.ColorField("Fog Colour", theScript.S5FogColor);
			theScript.S5FogMode = (FogMode)EditorGUILayout.EnumPopup("Fog Mode", theScript.S5FogMode);
		    theScript.S5FogDensity = Mathf.Clamp(EditorGUILayout.FloatField("Fog Density", theScript.S5FogDensity), 0f, 1f);
		    theScript.S5FogLinearStart = EditorGUILayout.FloatField("Linear Fog Start", theScript.S5FogLinearStart);
		    theScript.S5FogLinearEnd = EditorGUILayout.FloatField("Linear Fog End", theScript.S5FogLinearEnd);
			theScript.S5AmbientLight = EditorGUILayout.ColorField("Ambient Light", theScript.S5AmbientLight);
		}
		EditorGUI.indentLevel = 0;
		
		EditorGUILayout.Space();
		m_s6Foldout = EditorGUILayout.Foldout(m_s6Foldout, "S6 RenderSettings:");
		
		if (m_s6Foldout)
		{
			EditorGUI.indentLevel = 1;
			theScript.S6Uptodate = EditorGUILayout.Toggle("UpdateToDate", theScript.S6Uptodate);
			theScript.S6FogEnable = EditorGUILayout.Toggle("Enable Fog", theScript.S6FogEnable);
			theScript.S6FogColor = EditorGUILayout.ColorField("Fog Colour", theScript.S6FogColor);
			theScript.S6FogMode = (FogMode)EditorGUILayout.EnumPopup("Fog Mode", theScript.S6FogMode);
		    theScript.S6FogDensity = Mathf.Clamp(EditorGUILayout.FloatField("Fog Density", theScript.S6FogDensity), 0f, 1f);
		    theScript.S6FogLinearStart = EditorGUILayout.FloatField("Linear Fog Start", theScript.S6FogLinearStart);
		    theScript.S6FogLinearEnd = EditorGUILayout.FloatField("Linear Fog End", theScript.S6FogLinearEnd);
			theScript.S6AmbientLight = EditorGUILayout.ColorField("Ambient Light", theScript.S6AmbientLight);
		}
		EditorGUI.indentLevel = 0;
		
		
		/*
		EditorGUILayout.Space();
		m_PreviewFoldout = EditorGUILayout.Foldout(m_PreviewFoldout, "Realtime Preview PS RenderSettings:");
		
		if (m_PreviewFoldout)
		{
			EditorGUI.indentLevel = 1;
			theScript.m_PreviewPSState = EditorGUILayout.Toggle("Preview", theScript.m_PreviewPSState);
		}
		
		EditorGUILayout.Space();
		theScript.m_FadeTime = EditorGUILayout.FloatField("Transition Time", theScript.m_FadeTime);
		
		/* Obsolete because of the onEnabled function
		EditorGUILayout.Space();
		EditorGUILayout.BeginVertical();
		if(GUILayout.Button("Sample Default RenderSettings"))
		{
			 Debug.Log("Button was clicked");
			Sample();
		}
		EditorGUILayout.EndVertical();
		*/
		EditorUtility.SetDirty (target);
	}
	
	private void Sample()
	{
		/*
		Debug.Log("Sampling settings");
		theScript.m_DefaultFogEnable = RenderSettings.fog;
		theScript.m_DefaultFogColor = RenderSettings.fogColor;
		theScript.m_DefaultFogMode = RenderSettings.fogMode;
	    theScript.m_DefaultFogDensity = RenderSettings.fogDensity;
	    theScript.m_DefaultFogLinearStart = RenderSettings.fogStartDistance;
	    theScript.m_DefaultFogLinearEnd = RenderSettings.fogEndDistance;
		theScript.m_DefaultAmbientLight = RenderSettings.ambientLight;
		
		EditorUtility.SetDirty (target);
		*/
	}
}
