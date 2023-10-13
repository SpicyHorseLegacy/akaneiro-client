using UnityEngine;
using System.Collections;

public class Camera_Rotating : CameraBaseEffect {

	public float time = 0.1f;
	
	bool shakingSwitch = true;
	public float angle = 0.5f;
	
	public override void Enter()
	{
		temp = transform.localEulerAngles;
		//GameCamera.Instance.IsPlayingCameraAnim = true;
	}
	
	// Update is called once per frame
	public override void Execute () {
		if(time > 0)
		{
			time -= Time.deltaTime;

			if(shakingSwitch)
				transform.localEulerAngles = temp + new Vector3(0,0,angle);
			else
				transform.localEulerAngles = temp - new Vector3(0,0,angle);	
			
			shakingSwitch = !shakingSwitch;
		}else{
			EffectManager.ExitEffect();
		}
	}
	
	public override void Exit () {
		transform.localEulerAngles = temp;
		//GameCamera.Instance.IsPlayingCameraAnim = false;
		Destroy(this);
	}
}
