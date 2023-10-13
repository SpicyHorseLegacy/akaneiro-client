using UnityEngine;
using System.Collections;

public class CameraAnimTrigger : TriggerBase {
	
	public AnimationClip CameraAnim;
	
	bool IsPlayingCameraAnim = false;
	
	// Use this for initialization
	void Start () {
	
		if(CameraAnim)
		{
			Camera.main.animation.AddClip(CameraAnim,CameraAnim.name);
			IsPlayingCameraAnim = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if(IsPlayingCameraAnim)
		{
			IsPlayingCameraAnim = Camera.main.animation.isPlaying;
			GameCamera.Instance.IsPlayingCameraAnim = IsPlayingCameraAnim;
		}
	}
	
	public virtual void OnTriggerEnter (Collider other)
	{
		if(CameraAnim)
		{
			IsPlayingCameraAnim = true;
			GameCamera.Instance.IsPlayingCameraAnim = IsPlayingCameraAnim;
			Camera.main.animation.CrossFade(CameraAnim.name);
		}
	}
}
