// ------------------------------------------------------------------------------
// COPYRIGHT (C) 2012 NVIDIA CORPORATION. ALL RIGHTS RESERVED.
// ------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class ShadowCamera : MonoBehaviour {
	
	Camera thisCamera = null;
	// Use this for initialization
	void Start () {
	   thisCamera = transform.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    void  OnPreRender() {
		if(thisCamera != null)
		{
			 Shader pShader = Shader.Find("Custom/RenderShadow");
			
			if( pShader == null &&  CS_SceneInfo.Instance != null)
				pShader = CS_SceneInfo.Instance.ShadowShader;
			   
			 if(pShader != null && CS_SceneInfo.Instance != null)
			 {
				CS_SceneInfo.Instance.pShadow = " Shadow";
				
			 }

			 thisCamera.SetReplacementShader(pShader, "RenderType");
		}
		
		//thisCamera.RenderWithShader(Shader.Find("Custom/RenderShadow"),"RenderType");
	}
}
