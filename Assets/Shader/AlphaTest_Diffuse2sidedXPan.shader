Shader "Transparent/Cutout/Diffuse_2SidedXPan" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	_Speed("Speed", Float) = 0
}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 200

	Cull Off	
CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff

sampler2D _MainTex;
fixed4 _Color;
float _Speed;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
    float4 Multiply9=_Time * float4(_Speed);
    float4 UV_Pan1=float4((IN.uv_MainTex.xyxy).x + Multiply9.x,(IN.uv_MainTex.xyxy).y,(IN.uv_MainTex.xyxy).z,(IN.uv_MainTex.xyxy).w);
    //float4 Tex2D1=tex2D(_MainTex,UV_Pan1.xy);
	fixed4 c = tex2D(_MainTex, UV_Pan1.xy) * _Color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Transparent/Cutout/VertexLit"
}
