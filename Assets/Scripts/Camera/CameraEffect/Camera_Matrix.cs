using UnityEngine;
using System.Collections;

public class Camera_Matrix : CameraBaseEffect {
	
	public Transform Target;
	
	public float time;
	
	Vector3 originalPosition;
	
	Vector3 targetPosition;
	
	public float smoothEnterTime;
	float smoothEnterTotalTime;
	public float lastTime;
	float lastTotalTime;
	public float smoothExitTime;
	float smoothExitTotalTime;
	
	float lastFrameTime;
	
	enum camera_matrix_step
	{
		entering,
		rotating,
		exiting
	}
	
	camera_matrix_step step;
	
	float angle;
	
	public override void Enter()
	{
		if(Target)
		{
			originalPosition = transform.position;
			
			targetPosition = Target.position + Target.forward * 5f + Target.up * 5f;
		}
		
		smoothEnterTime = time / 10f;
		smoothEnterTotalTime = smoothEnterTime;
		smoothExitTime = time / 10f;
		smoothExitTotalTime = smoothExitTime;
		lastTime = time - smoothEnterTime - smoothExitTime;
		lastTotalTime = lastTime;
		
		step = Camera_Matrix.camera_matrix_step.entering;
		
		Time.timeScale = 0.00f;
		lastFrameTime = Time.realtimeSinceStartup;
		
		GameCamera.Instance.IsPlayingCameraAnim = true;
	}
	
	// Update is called once per frame
	public override void Execute () {
		
		float dt = Time.realtimeSinceStartup - lastFrameTime;
		lastFrameTime = Time.realtimeSinceStartup;
		
		if(step == Camera_Matrix.camera_matrix_step.entering)
		{
			if(smoothEnterTime > 0)
			{
				smoothEnterTime -= dt;
				transform.position = Vector3.Lerp(targetPosition, originalPosition, smoothEnterTime / smoothEnterTotalTime );
			}else{
				transform.position = targetPosition;
				step = Camera_Matrix.camera_matrix_step.rotating;
				
				Vector3 temp = transform.position;
				temp.y = Target.position.y;
				temp = temp - Target.position;
				angle = Vector3.Angle(temp, Vector3.right);
				if(temp.z < 0)
					angle = -angle;
			}
		}
		
		if(step == Camera_Matrix.camera_matrix_step.rotating)
		{
			if(lastTime > 0)
			{
				lastTime -= dt;	
				
				angle += 360 / lastTotalTime * dt;
				
				float x = Target.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * 5f;
				float z = Target.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * 5f;
				
				transform.position = new Vector3(x, targetPosition.y, z);
				
			}else{
				step = Camera_Matrix.camera_matrix_step.exiting;
			}
		}
		
		if(step == Camera_Matrix.camera_matrix_step.exiting)
		{
			if(smoothExitTime > 0)
			{
				smoothExitTime -= dt;
				
				transform.position = Vector3.Lerp(originalPosition, targetPosition, smoothExitTime / smoothExitTotalTime);
			}else{
				EffectManager.ExitEffect();
			}
		}
			transform.LookAt(Target);
	}
	
	public override void Exit () {
		Time.timeScale = 1;
		GameCamera.Instance.IsPlayingCameraAnim = false;
		Destroy(this);
	}
}
