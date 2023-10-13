using UnityEngine;
using System.Collections;

public class CameraEffectManager : MonoBehaviour {
	
	public static CameraEffectManager Instance;

    [SerializeField]  Animation CameraShakeAnimation;

	GameCamera gameCamera;
	
	CameraBaseEffect curCameraEffect;
	
	bool isCameraEffect;
	
	// Use this for initialization
	void Awake () {
		Instance = this;
		gameCamera = transform.GetComponent<GameCamera>();
	}
	
	void Update()
	{
		//if(Input.GetKey(KeyCode.Alpha4))
			//PlayFOVZoomInEffect();
			//PlayShakingEffect("light");
            //PlayShakingEffect("subtle");
			//PlayBulletTimeEffect();
			//PlayMatrixEffect(Player.Instance.transform);
		
		//if(Input.GetKey(KeyCode.Alpha5))
		//	PlayShakingEffect("normal");
		//if(Input.GetKey(KeyCode.Alpha6))
		//	PlayShakingEffect("heavy");
	}
	
	public void Execute()
	{
		if(isCameraEffect && curCameraEffect)
			curCameraEffect.Execute();
	}
	
	public void ExitEffect()
	{
		curCameraEffect.Exit();
		isCameraEffect = false;
	}
	
	public void StartEffect()
	{
		curCameraEffect.Enter();
		isCameraEffect = true;
	}
	
	private void setCurCameraEffect(CameraBaseEffect cureffect)
	{
		if(curCameraEffect)
		{
			curCameraEffect.Exit();
		}
		curCameraEffect = cureffect;
		curCameraEffect.EffectManager = this;
	}
	
	public void PlayShakingEffect()
	{
		///Camera_Shaking shaking = gameObject.AddComponent<Camera_Shaking>();
		//setCurCameraEffect(shaking);
		//StartEffect();
		
		PlayShakingEffect("normal");
	}
	
	public void PlayShakingEffect(string level)
	{
        CameraShakeAnimation.Play("CameraShake_Explosion_" + level);
	}
	
	public void PlayMeteorShakingEffect()
	{
        CameraShakeAnimation.Play("CameraShake_MeteorRain");
	}
	
	public void PlayRotatingEffect()
	{
		PlayRotatingEffectWithPowerAndTime(0.5f, 0.1f);
	}
	
	public void PlayRotatingEffectWithPowerAndTime(float pw, float time)
	{
		Camera_Rotating rotating = gameObject.AddComponent<Camera_Rotating>();
		setCurCameraEffect(rotating);
		StartEffect();
		rotating.angle = pw;
		rotating.time = time;
	}
	
	public void PlayBulletTimeEffect()
	{
		PlayBulletTimeEffectWithTimeAndScale(0.3f, 0.25f, 0.1f);
	}
	
	public void PlayBulletTimeEffectWithTimeAndScale( float lastTime, float backTime, float scale )
	{
		return;
		Camera_BulletTime bullettime = transform.GetComponent<Camera_BulletTime>();
		if(!bullettime){
			bullettime = gameObject.AddComponent<Camera_BulletTime>();
			setCurCameraEffect(bullettime);
		}
		StartEffect();
		bullettime.time = lastTime;
		bullettime.resumetime = backTime;
		bullettime.resumetotaltime = backTime;
		Time.timeScale = scale;

        CameraShakeAnimation["CameraShake_Explosion_light"].speed = 1 / scale;
        CameraShakeAnimation["CameraShake_Explosion_normal"].speed = 1 / scale;
        CameraShakeAnimation["CameraShake_Explosion_heavy"].speed = 1 / scale;
	}
	
	public void PlayFOVZoomInEffect()
	{
		Camera_FOVZoomIn fovzoomin = gameObject.AddComponent<Camera_FOVZoomIn>();
		setCurCameraEffect(fovzoomin);
		StartEffect();
	}
	
	public void PlayMatrixEffect(Transform target)
	{
		if(!target)
			return;
		
		Camera_Matrix matrix = gameObject.AddComponent<Camera_Matrix>();
		setCurCameraEffect(matrix);
		matrix.Target = target;
		matrix.time = 5f;
		StartEffect();
		
	}
}
