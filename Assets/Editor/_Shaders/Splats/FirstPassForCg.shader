Shader "TerrainPaint/Lightmap-FirstPassForCg" {
	
Properties {
	_Control ("Control (RGBA)", 2D) = "red" {}
	_Splat0 ("Layer 0 (G)", 2D) = "white" {}
	_Splat1 ("Layer 1 (R)", 2D) = "white" {}
	_ShadowColor ("Shadow Color", Color) = (0,0,0,1)
	_Color ("Main Color", Color) = (1,1,1,1)
	_ShadowMap("Shadow Map", 2D) = "black" {}
	_ShadowDarkness("_ShadowDarkness", Range(0,1)) = 1
	
}

CGINCLUDE		

 struct appdata 
 {
    half4 vertex : POSITION;
    half3 normal : NORMAL;
	half4 texcoord : TEXCOORD0;
	half4 texcoord1 : TEXCOORD1;

 };
        
 struct v2f 
 {
    half4 vertex : POSITION;
    half4 uv_norm :  TEXCOORD0; 
    half4 uv_norm1 : TEXCOORD1;
    half2 uvBorder : TEXCOORD2;
   
    #ifdef LIGHTMAP_ON
    half2 uvLM     : TEXCOORD3;
	#endif	
    
	//fixed4 vLightColor : Color;
 };
	
#include "UnityCG.cginc"		

sampler2D _Control;
sampler2D _Splat0,_Splat1;

half _ShadowDarkness;

sampler2D _ShadowMap;

float4x4 _LocalToShadowMatrix;

float4 _Control_ST;
float4 _Splat0_ST;
float4 _Splat1_ST;

float4 unity_LightmapST;	
sampler2D unity_Lightmap;

//fixed4 _LightColor0;

fixed4 _ShadowColor;

fixed4 _Color;


// NVCHANGE_BEGIN_YY : ShadowMapping
fixed4 GetShadowMapValue(half2 uv,half2 border)
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

ENDCG 
		
SubShader {
	Tags  { 
	    "SplatCount" = "2"
		"Queue" = "Geometry+100"
		"RenderType" = "Opaque"
    }
			
	Pass {
		CGPROGRAM
		
		 half3 VertexLightsViewSpace (half3 VP, half3 VN)
		 {
			half3 lightColor = half3(0.0,0.0,0.0);
	
			// preface & optimization
			half3 toLight0 = unity_LightPosition[1].xyz - VP;
			half3 toLight1 = unity_LightPosition[2].xyz - VP;
			half2 lengthSq2 = half2(dot(toLight0, toLight0), dot(toLight1, toLight1));
	
			half2 atten2 = half2(1.0,1.0) + lengthSq2 * half2(unity_LightAtten[1].z, unity_LightAtten[2].z);
			atten2 = 1.0 / atten2;
						
			// light #0
			half diff = saturate (dot (VN, normalize(toLight0)));
			lightColor += unity_LightColor[1].rgb * (diff * atten2.x);
	
			// light #1
			diff = saturate (dot (VN, normalize(toLight1)));
			lightColor += unity_LightColor[2].rgb * (diff * atten2.y);
	
			return lightColor;
		}	
		
		
		v2f vert(appdata v)
        {
			v2f o;
			
			// object space view direction
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			
			o.uv_norm.xy = TRANSFORM_TEX(v.texcoord.xy,_Control);

	        o.uv_norm.zw = GetShadowUV(v.vertex);
	        
	        o.uvBorder =  o.uv_norm.zw - half2(0.5,0.5);
	        
	        o.uv_norm1.xy = TRANSFORM_TEX(v.texcoord.xy,_Splat0);
	        
	        o.uv_norm1.zw = TRANSFORM_TEX(v.texcoord.xy,_Splat1);
	       
	        #ifdef LIGHTMAP_ON
				o.uvLM = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
			#endif
	        
	        //half3 lightDir = normalize(ObjSpaceLightDir(v.vertex));
			//half vn = clamp(dot (v.normal, lightDir), 0, 1);
			//o.vLightColor = UNITY_LIGHTMODEL_AMBIENT;
		
			//o.vLightColor += half4(_LightColor0.rgb * vn,1.0);
			
			//Add point lights [BEGIN]
			//half3 viewNormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
			//half3 viewPos = mul(UNITY_MATRIX_MV, v.vertex).xyz;
			//o.vLightColor += float4( VertexLightsViewSpace(viewPos, viewNormal), 1);
		    // Add point lights [END]
			
	        
			return o;
        }
				
		fixed4 frag (v2f i) : COLOR0 
		{
			half2 splat_control = tex2D (_Control, i.uv_norm.xy).xy;
			
	        half4 col;
	        col = splat_control.x * tex2D (_Splat0, i.uv_norm1.xy);
	        col += splat_control.y * tex2D (_Splat1, i.uv_norm1.zw);
	      
			fixed4 shadow = GetShadowMapValue(i.uv_norm.zw,i.uvBorder);
			
		    #ifdef LIGHTMAP_ON
				fixed3 lm = ( DecodeLightmap (tex2D(unity_Lightmap, i.uvLM)));
				col.rgb *= lm;
			#endif	
			
			col.xyz = col.xyz * (1.0 - shadow.a) + shadow.xyz * shadow.a;
			
			col.xyz = col.xyz * _Color.rgb;
			
			//col.xyz = col.xyz * i.vLightColor.xyz *3;
			
			
			return col;
		}	
		
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
		#pragma fragmentoption ARB_precision_hint_fastest 
	
		ENDCG
	}
} 

FallBack off 
}
