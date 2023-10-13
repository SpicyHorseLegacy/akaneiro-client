using UnityEngine;
using System.Collections;

public class DestroyAfterFadeOut : MonoBehaviour {
	
	bool isGo = false;
	
	float time = 0.5f;
	Color endColor;
	
	// Update is called once per frame
	void Update () {
		
		if(isGo)
		{
			renderer.material.SetColor("_TintColor", Color.Lerp(renderer.material.GetColor("_TintColor"), endColor, Time.deltaTime / time));
			if(renderer.material.GetColor("_TintColor").a < 0.01f){
				renderer.material.SetColor("_TintColor", endColor);
				Destroy(gameObject);
			}
		}
	}

    public void GoToHell()
	{
        isGo = true;
		if(renderer.material.HasProperty("_TintColor"))
			endColor = renderer.material.GetColor("_TintColor");
		endColor.a = 0;
        transform.parent = null;
	}
}
