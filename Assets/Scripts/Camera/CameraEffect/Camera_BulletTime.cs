using UnityEngine;
using System.Collections;

public class Camera_BulletTime : CameraBaseEffect {
	
	public float time;
	float tempTS;
	
	public float resumetime;
	public float resumetotaltime;
	
	public override void Enter()
	{
		tempTS = Time.timeScale;
	}
	
	// Update is called once per frame
	public override void Execute () {
		if(time > 0)
		{
			time -= Time.deltaTime / Time.timeScale;

			
		}else{
			if(resumetime > 0){
				resumetime -= Time.deltaTime / Time.timeScale;
				Time.timeScale = Mathf.Clamp(Mathf.Lerp(Time.timeScale, tempTS, (Time.deltaTime / Time.timeScale) / resumetotaltime), 0.01f, 100);
			}else
				EffectManager.ExitEffect();
		}
	}
	
	public override void Exit () {
		Time.timeScale = 1;
		GameCamera.Instance.gameCamera.animation["CameraShake_Explosion_light"].speed = 1;
		GameCamera.Instance.gameCamera.animation["CameraShake_Explosion_normal"].speed = 1;
		GameCamera.Instance.gameCamera.animation["CameraShake_Explosion_heavy"].speed = 1;
		Destroy(this);
	}
	
}
