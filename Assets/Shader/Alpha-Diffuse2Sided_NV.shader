Shader "Transparent/Diffuse2Sided_NV" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Opacity("_Opacity", Float) = 1
}

CGINCLUDE		

struct v2f 
{
	half4 pos : SV_POSITION;
	half2 uv : TEXCOORD0;
	fixed4 vLightColor : Color;
};
	
#include "UnityCG.cginc"

sampler2D _MainTex;
fixed4 _Color;
float _Opacity;
fixed4 _LightColor0;

ENDCG 

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	Cull Off
	ZWrite Off ColorMask RGB
	Blend SrcAlpha OneMinusSrcAlpha
	
	Pass {
		CGPROGRAM
		
		half4 _MainTex_ST;		
				
		v2f vert (appdata_full v) 
		{
			v2f o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			
			half3 lightDir = normalize(ObjSpaceLightDir(v.vertex));
			half vn = clamp(dot (v.normal, lightDir), 0, 1);
			o.vLightColor = UNITY_LIGHTMODEL_AMBIENT;
			o.vLightColor += half4(_LightColor0.rgb * vn,1.0);
			o.vLightColor *= _Color;
			
			return o; 
		}		
		
		fixed4 frag (v2f i) : COLOR0 
		{
			fixed4 tex = tex2D (_MainTex, i.uv) * i.vLightColor;
			return fixed4(tex.xyz, tex.w * _Opacity);		
		}	
		
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	
		ENDCG
	}
}

Fallback off
}
