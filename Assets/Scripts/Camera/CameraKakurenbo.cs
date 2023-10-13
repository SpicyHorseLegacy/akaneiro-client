using UnityEngine;
using System.Collections;

public class CameraKakurenbo : MonoBehaviour {
	
	static public void hideGameObject(Transform obj, float alpha, float time) {
		Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
		if(renderers.Length == 0)
			renderers = obj.parent.GetComponentsInChildren<Renderer>();

		foreach(Renderer renderer in renderers)
		{
			if(renderer.tag.Contains("NoMatChange"))
				continue;
			
			CameraKakurenbo camera_kakurenbo = renderer.gameObject.GetComponent<CameraKakurenbo>();
			if (!camera_kakurenbo) {
				camera_kakurenbo = renderer.gameObject.AddComponent<CameraKakurenbo>();
			}
			
			camera_kakurenbo.setAlpha(alpha);
			camera_kakurenbo.setTimeout(time);
		}
	}
	
	Shader _old;
	float _expire_time;
	
	// Use this for initialization
	void Start () {
		_old = gameObject.renderer.material.shader;
		gameObject.renderer.material.shader = Shader.Find("WaterColor/Transparent");
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > _expire_time) {
			gameObject.renderer.material.shader = _old;
			Destroy(this);
		}
	}
	
	void setAlpha(float alpha) {
		gameObject.renderer.material.SetFloat("_Opacity", alpha);
	}
	
	void setTimeout(float timeout) {
		_expire_time = Time.time + timeout;
	}
}
