using UnityEngine;
using System.Collections;

public class CameraVars : MonoBehaviour {

	public float _VignetteIntensity = 1.4f;
	public float _MaxMultiply = 1.5f;
	public float _MinAdd = -0.04f;
	
	public bool _isShadowCameraOn = false;
	
	void Start()
	{
		if(GameCamera.Instance && GameCamera.Instance.gameCamera.GetComponent<InkVignette>())
		{
			InkVignette _effect = GameCamera.Instance.gameCamera.GetComponent<InkVignette>();
			_effect._VignetteIntensity = _VignetteIntensity;
			_effect._MaxMultiply = _MaxMultiply;
			_effect._MinAdd = _MinAdd;
		}
		
		if(GameCamera.Instance && GameCamera.Instance.ShadowCamera)
		{
			GameCamera.Instance.ShadowCamera.gameObject.SetActive(_isShadowCameraOn);
		}
	}
}
