using UnityEngine;
using System.Collections;

public class Camera_Shaking : CameraBaseEffect {
	
	public float time = 0.1f;
	public float offset = 0.5f;
	
	public override void Enter()
	{
		temp = GameCamera.Instance.gameCamera.transform.localPosition;
		//GameCamera.Instance. = true;
	}
	
	// Update is called once per frame
	public override void Execute () {
		if(time > 0)
		{
			time -= Time.deltaTime;
			
			float x = Random.Range(-offset, offset);
			float y = Random.Range(-offset, offset);
			
			GameCamera.Instance.gameCamera.transform.localPosition = new Vector3(x, y, 0);
		}else{
			EffectManager.ExitEffect();
		}
	}
	
	public override void Exit () {
		GameCamera.Instance.gameCamera.transform.localPosition = temp;
		//GameCamera.Instance.IsPlayingCameraAnim = false;
		Destroy(this);
	}
}
