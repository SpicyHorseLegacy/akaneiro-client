using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Lightning")]
public class Lightning : ImageEffectBase {
	public Texture _Mask;
	public float _speed;
	public Color _color;
	public Vector4 texCoord;
	public float _Brightness;

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetTexture("_LightningMask",_Mask);
		material.SetFloat("_Speed",_speed);
		material.SetColor("_Color",_color);
		material.SetVector("_SecTexCoord",texCoord);
		material.SetFloat("_Brightness",_Brightness);
		Graphics.Blit(source, destination, material);
	}
}
