using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/FadeInOut")]
public class FadeInOut : ImageEffectBase {
	
	public float ratio = 0;
	
	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		ratio = Mathf.Clamp(ratio,0f,1f);
		material.SetFloat("_Ratio", ratio);
		Graphics.Blit (source, destination, material);
	}
}
