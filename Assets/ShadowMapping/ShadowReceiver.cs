// ------------------------------------------------------------------------------
// COPYRIGHT (C) 2012 NVIDIA CORPORATION. ALL RIGHTS RESERVED.
// ------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class ShadowReceiver : MonoBehaviour {
	
	public Camera ShadowCamera = null;
	
	void Start()
	{
		if(GameCamera.Instance && GameCamera.Instance.ShadowCamera)
		{
			ShadowCamera = GameCamera.Instance.ShadowCamera;
		}	
	}
	
	public void OnWillRenderObject() {
		/*
		Camera shadow_cam = null;
		for(int i = 0; i < Camera.allCameras.Length; i++)
		{
			if(Camera.allCameras[i].name == "ShadowCamera")
			{
				shadow_cam = (Camera)Camera.allCameras[i];
				break;
			}
		}
		*/
		if(ShadowCamera != null)
		{
			Matrix4x4 m = ShadowCamera.projectionMatrix * ShadowCamera.worldToCameraMatrix * renderer.localToWorldMatrix;
			renderer.material.SetMatrix ("_LocalToShadowMatrix", m);
		}
	}
	
}
