using UnityEngine;
using System.Collections;

public class FadeMTLInTime : MonoBehaviour {
	
	bool isStart = false;
	Color endColor;
	float fadeTime = 0.5f;
	float tempTime = 0;
	
	void Update()
	{
		if(isStart)
		{
			renderer.material.SetColor("_TintColor", Color.Lerp(renderer.material.GetColor("_TintColor"), endColor, Time.deltaTime / fadeTime));
			tempTime += Time.deltaTime;
			if(tempTime > fadeTime)
			{
				isStart = false;
				Destroy(this);
			}
		}
	}
	
	
	public void StartWithOpacityAndTime(float opacity, float _time)
	{
		endColor = renderer.material.GetColor("_TintColor");
		endColor.a = opacity;
		fadeTime = _time;
		tempTime = 0;
		isStart = true;
	}
	
}
