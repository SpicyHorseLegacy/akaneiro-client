
// ------------------------------------------------------------------------------
// COPYRIGHT (C) 2012 NVIDIA CORPORATION. ALL RIGHTS RESERVED.
// ------------------------------------------------------------------------------

Shader "NVIDIA/Terrain3LayersWithShadowsCTB" {
	
Properties {
	_Control ("Control", 2D) = "white" {}
	_Splat0 ("Splat0", 2D) = "white" {}
	_Splat1 ("Splat1", 2D) = "white" {}
	_Splat2 ("Splat2", 2D) = "white" {}
	_Color ("Color", Color) = (1, 1, 1, 1)
	// NVCHANGE_BEGIN_YY : ShadowMapping	
	_ShadowMap("Shadow Map", 2D) = "black" {}
	_ShadowDarkness("_ShadowDarkness", Range(0,1)) = 1
	_ShadowColor ("ShadowColor", Color) = (0, 0, 0, 1)
	// NVCHANGE_END_YY : ShadowMapping
}


CGINCLUDE		

struct v2f 
{
	half4 pos : SV_POSITION;
	// NVCHANGE_BEGIN_YY : ShadowMapping		
	half4 uv : TEXCOORD0;
	half4 uvSplat : TEXCOORD1;
	half4 uvSplat2 : TEXCOORD4;
	half2 uvBorder : TEXCOORD2;
	// NVCHANGE_END_YY : ShadowMapping
	#ifdef LIGHTMAP_ON
		half2 uvLM : TEXCOORD3;
	#endif
};
	
#include "UnityCG.cginc"

sampler2D _Control;
sampler2D _Splat0;
sampler2D _Splat1;
sampler2D _Splat2;
fixed4 _Color;

// NVCHANGE_BEGIN_YY : ShadowMapping
sampler2D _ShadowMap;
half _ShadowDarkness;
fixed4 _ShadowColor;
float4x4 _LocalToShadowMatrix;

fixed4 GetShadowMapValue(half2 uv, half2 border)
{
	fixed4 output = tex2D(_ShadowMap, uv);
	border = abs(border);
	return fixed4(_ShadowColor.xyz, output.r * (max(border.x, border.y) > 0.5 ? 0.0 : 1.0) * _ShadowDarkness);
}

half2 GetShadowUV(half4 vert)
{
	half4 screen_point = mul(_LocalToShadowMatrix, vert);
	return screen_point.xy/screen_point.w * half2(0.5, 0.5) + half2(0.5, 0.5);
}
// NVCHANGE_END_YY : ShadowMapping		
					
ENDCG 

SubShader {
	Tags { "RenderType"="Opaque"
				"Queue"="Transparent-1" }
	
	Pass {
		CGPROGRAM
		
		half4 unity_LightmapST;
		sampler2D unity_Lightmap;
		half4 _Control_ST;		
		half4 _Splat0_ST;	
		half4 _Splat1_ST;
		half4 _Splat2_ST;	
				
		v2f vert (appdata_full v) 
		{
			v2f o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			o.uv.xy = TRANSFORM_TEX(v.texcoord, _Control);
			// NVCHANGE_BEGIN_YY : ShadowMapping
			o.uv.zw = GetShadowUV(v.vertex);
			o.uvBorder = o.uv.zw - half2(0.5, 0.5);
			// NVCHANGE_END_YY : ShadowMapping
			o.uvSplat.xy = TRANSFORM_TEX(v.texcoord, _Splat0);
			o.uvSplat.zw = TRANSFORM_TEX(v.texcoord, _Splat1);
			o.uvSplat2.xy = TRANSFORM_TEX(v.texcoord, _Splat2);
			#ifdef LIGHTMAP_ON
				o.uvLM = v.texcoord1 * unity_LightmapST.xy + unity_LightmapST.zw;
			#endif
			return o; 
		}		
		
		fixed4 frag (v2f i) : COLOR0 
		{
			fixed4 splat_control = tex2D (_Control, i.uv.xy).xyzw;	
			fixed4 tex = splat_control.x * tex2D (_Splat0, i.uvSplat.xy) + splat_control.y * tex2D (_Splat1, i.uvSplat.zw);	
			tex += splat_control.z * tex2D (_Splat2, i.uvSplat2.xy);
			
			// NVCHANGE_BEGIN_YY : ShadowMapping
			fixed4 shadow = GetShadowMapValue(i.uv.zw, i.uvBorder);
			// NVCHANGE_END_YY : ShadowMapping

			#ifdef LIGHTMAP_ON
				fixed3 lm = DecodeLightmap (tex2D(unity_Lightmap, i.uvLM));
				tex.rgb *= lm;
			#else
				tex.rgb *= 2.0 * _Color;	
			#endif	

			// NVCHANGE_BEGIN_YY : ShadowMapping
			tex.xyz = tex.xyz * (1.0 - shadow.a) + shadow.xyz * shadow.a;
			// NVCHANGE_END_YY : ShadowMapping

			return tex;		
		}	
		
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	
		ENDCG
	}
} 

FallBack Off
}

