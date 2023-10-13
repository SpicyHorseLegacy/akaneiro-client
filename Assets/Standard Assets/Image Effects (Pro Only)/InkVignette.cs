using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/InkVignette")]
public class InkVignette : ImageEffectBase {
	public Texture _InkVignetteTex;
	//public float _InkVignetteTexCoord;
	public float _VignetteIntensity;
	public float _MaxMultiply;
	public float _MinAdd;
	
	//public Texture _Sampler2D;

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
			
		material.SetTexture("_InkVignetteTex",_InkVignetteTex);
		//material.SetFloat("_InkVignetteTexCoord",_InkVignetteTexCoord);
		material.SetFloat("_vignetteIntensity",_VignetteIntensity);
		material.SetFloat("_MaxMultiply",_MaxMultiply);
		material.SetFloat("_MinAdd",_MinAdd);
		//material.SetTexture("_Sampler2D",_Sampler2D);
		
		Graphics.Blit(source, destination, material);
	}
}
