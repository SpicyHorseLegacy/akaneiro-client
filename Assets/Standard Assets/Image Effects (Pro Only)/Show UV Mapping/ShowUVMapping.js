@script ExecuteInEditMode
@script RequireComponent(Camera)

var shader : Shader;

private var shaderCamera : GameObject;
private var checker : Texture2D;

function OnPostRender()
{
	if (!enabled || !gameObject.active || !shader)
		return;
	if (!shaderCamera) {
		shaderCamera = new GameObject("ShaderCamera", Camera);
		shaderCamera.camera.enabled = false;
		shaderCamera.hideFlags = HideFlags.HideAndDontSave;
	}
	if (!checker) {
		checker = new Texture2D (2,2,TextureFormat.ARGB32,false);
		checker.hideFlags = HideFlags.HideAndDontSave;
		var color1 = Color(0.5,0.5,0.5,1);
		var color2 = Color(1.0,1.0,1.0,1);
		checker.SetPixel (0,0,color1);
		checker.SetPixel (1,0,color2);
		checker.SetPixel (0,1,color2);
		checker.SetPixel (1,1,color1);
		checker.Apply ();
		checker.filterMode = FilterMode.Point;
		Shader.SetGlobalTexture ("_CheckerTex", checker);
	}
	var cam = shaderCamera.camera;
	cam.CopyFrom (camera);
	cam.backgroundColor = Color(0,0,0,0);
	cam.clearFlags = CameraClearFlags.SolidColor;
	cam.RenderWithShader (shader, "RenderType");
}

function OnDisable() {
	DestroyImmediate(shaderCamera);
	DestroyImmediate(checker);
}
