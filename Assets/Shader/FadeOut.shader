Shader "Transparent/Cutout/FadeOut" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_FadeOutMask("_FadeOutMask", 2D) = "white" {}
	_FadeValue("_FadeValue", Range(0,1) ) = 0
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 200
	
CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff
#pragma target 3.0

sampler2D _MainTex;
fixed4 _Color;
sampler2D _FadeOutMask;
float _FadeValue;

struct Input {
	float2 uv_MainTex;
	float2 uv_FadeOutMask;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	float4 Sampled2D1=tex2D(_FadeOutMask,IN.uv_FadeOutMask.xy);
    float4 Invert2= float4(1.0) - _FadeValue.xxxx;
    float4 Add0=Sampled2D1 + Invert2;
    float4 Floor0=floor(Add0);
    float4 Multiply1=float4( c.a) * Floor0;
	o.Albedo = c.rgb;
	o.Alpha = Multiply1;
}
ENDCG
}

Fallback "Transparent/Cutout/VertexLit"
}
