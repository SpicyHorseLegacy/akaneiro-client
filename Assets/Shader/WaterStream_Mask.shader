Shader "FX/WaterStream_Mask"
{
	Properties 
	{
_Brightness("_Brightness", Float) = 0
_Color("_Color", Color) = (1,1,1,1)
_MainTex("_MainTex", 2D) = "white" {}
_MaskTex("_MaskTex", 2D) = "white" {}
_MainTexCoord("_MainTexCoord", Vector) = (1,1,0,0)
_SecTexCoord("_SecTexCoord", Vector) = (1,1,0,0)
_LerpValue("_LerpValue", Range(0,1) ) = 0.5
_MainTexXpan("_MainTexXpan", Float) = 0
_MainTexYpan("_MainTexYpan", Float) = 0
_SecTexXpan("_SecTexXpan", Float) = 0
_SecTexYpan("_SecTexYpan", Float) = 0
_MaskTexXpan("_MaskTexXpan", Float) = 0
_MaskTexYpan("_MaskTexYpan", Float) = 0
_Opacity("_Opacity", Float) = 0
//_InvFade ("_InvFade", Range (0.05, 5.0)) = 1.0

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="Transparent"

		}

Blend SrcAlpha OneMinusSrcAlpha	
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}




		CGPROGRAM
#pragma surface surf BlinnPhongEditor  alpha decal:blend vertex:vert
#pragma target 2.0
#include "UnityCG.cginc"




float _Brightness;
float4 _Color;
sampler2D _MainTex;
sampler2D _MaskTex;
float4 _MainTexCoord;
float4 _SecTexCoord;
float _LerpValue;
float _Opacity;
float _MainTexXpan;
float _MainTexYpan;
float _SecTexXpan;
float _SecTexYpan;
float _MaskTexXpan;
float _MaskTexYpan;
//float _InvFade;
//sampler2D _CameraDepthTexture;


			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot (s.Normal, lightDir));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff * atten * 2.0;
				res.w = spec * Luminance (_LightColor0.rgb);

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_MainTex;
				float2 uv_MaskTex;
				//float4 vertex : POSITION;
				//float4 projPos : TEXCOORD2;

			};


			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);
                       //o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);		
                       //o.projPos = ComputeScreenPos( o.vertex);
                       
                       //COMPUTE_EYEDEPTH(o.projPos.z);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss=0.0;
				o.Specular=0.0;
				
				//float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.projPos))));
	            //float partZ = IN.projPos.z;
	            //float fade2 =  saturate (_InvFade * (sceneZ-partZ));
	
float4 Multiply3=(IN.uv_MainTex.xyxy) * _MainTexCoord;
float4 Multiply7 = _Time * _MainTexXpan;float _InvFade;
float4 Multiply8 = _Time * _MainTexYpan;
float4 UV_Pan0=float4(Multiply3.x + Multiply7.y,Multiply3.y + Multiply8.y,Multiply3.z,Multiply3.w);
float4 Tex2D0=tex2D(_MainTex,UV_Pan0.xy);
float4 Multiply4=(IN.uv_MainTex.xyxy) * _SecTexCoord;
float4 Multiply9 = _Time * _SecTexXpan;
float4 Multiply10 = _Time * _SecTexYpan;
float4 UV_Pan1=float4(Multiply4.x + Multiply9.y,Multiply4.y + Multiply10.y,Multiply4.z,Multiply4.w);
float4 Tex2D1=tex2D(_MainTex,UV_Pan1.xy);
float4 Lerp0=lerp(Tex2D0,Tex2D1,_LerpValue.xxxx);
float4 Multiply2=_Color * Lerp0;
float4 Multiply0=float4(_Brightness) * Multiply2;
float4 Multiply11 = _Time * _MaskTexXpan;
float4 Multiply12 = _Time * _MaskTexYpan;
float4 UV_Pan2=float4((IN.uv_MaskTex.xyxy).x + Multiply11.y,(IN.uv_MaskTex.xyxy).y + Multiply12.y,(IN.uv_MaskTex.xyxy).z,(IN.uv_MaskTex.xyxy).w);
float4 Tex2D2=tex2D(_MaskTex,UV_Pan2.xy);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_2_NoInput = float4(0,0,0,0);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply0;
o.Alpha = float4(_Opacity) * Tex2D2;
o.Alpha = o.Alpha ; //* fade2;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}