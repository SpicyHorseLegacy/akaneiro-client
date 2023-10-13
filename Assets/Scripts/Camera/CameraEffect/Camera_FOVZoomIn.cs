using UnityEngine;
using System.Collections;

public class Camera_FOVZoomIn : CameraBaseEffect {

	public override void Enter()
	{
		camera.fieldOfView = 40;
		EffectManager.ExitEffect();
	}
}
