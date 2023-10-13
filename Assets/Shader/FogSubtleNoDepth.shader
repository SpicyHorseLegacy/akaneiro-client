Shader "FX/FogSubtleNoDepth"
{
	Properties 
	{
_Color("_Color", Color) = (1,1,1,1)
_CloudTex("_CloudTex", 2D) = "white" {}
_CloudTexCoord("_CloudTexCoord", Vector) = (1,1,0,0)
_TexDistritionValue("_TexDistritionValue", Float) = 0
_FadeTex("_FadeTex", 2D) = "white" {}
_MaskTex("_MaskTex", 2D) = "white" {}
_MaskTexCoord("_MaskTexCoord", Vector) = (1,1,0,0)
_CloudTexXpan("_CloudTexXpan", Float) = 0
_CloudTexYpan("_CloudTexYpan", Float) = 0
_MaskTexXpan("_MaskTexXpan", Float) = 0
_MaskTexYpan("_MaskTexYpan", Float) = 0
_Opacity("_Opacity", Float) = 0
//_InvFade ("_InvFade", Range (0.05, 5.0)) = 1.0
	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent+1"
"IgnoreProjector"="False"
"RenderType"="Transparent"

		}

Blend SrcAlpha OneMinusSrcAlpha		
Cull Off
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  alpha decal:blend vertex:vert
//#pragma target 2.0
//#include "UnityCG.cginc"


float4 _Color;
sampler2D _CloudTex;
float4 _CloudTexCoord;
float _TexDistritionValue;
sampler2D _FadeTex;
sampler2D _MaskTex;
float4 _MaskTexCoord;
float _Opacity;
float _CloudTexXpan;
float _CloudTexYpan;
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
				float2 uv_CloudTex;
                //float2 uv_FadeTex;
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
				
float4 Multiply3=(IN.uv_CloudTex.xyxy) * _CloudTexCoord;
float4 Sampled2D0=tex2D(_CloudTex,IN.uv_CloudTex.xy);
float4 Multiply1=Sampled2D0 * float4(_TexDistritionValue);
float4 Add0=Multiply3 + Multiply1;
float4 Multiply7 = _Time * _CloudTexXpan;
float4 Multiply8 = _Time * _CloudTexYpan;
float4 UV_Pan0=float4(Add0.x + Multiply7.y,Add0.y + Multiply8.y,Add0.z,Add0.w);
float4 Tex2D0=tex2D(_CloudTex,UV_Pan0.xy);
float4 Multiply2=_Color * Tex2D0 ;//* float(1.5)+ float(0.2);
float4 Sampled2D1=tex2D(_FadeTex,IN.uv_MaskTex.xy);
float4 Multiply4=(IN.uv_MaskTex.xyxy) * _MaskTexCoord;
float4 Multiply9 = _Time * _MaskTexXpan;
float4 Multiply10 = _Time * _MaskTexYpan;
float4 UV_Pan1=float4(Multiply4.x + Multiply9.y,Multiply4.y + Multiply10.y,Multiply4.z,Multiply4.w);
float4 Tex2D1=tex2D(_MaskTex,UV_Pan1.xy);
float4 Multiply0=Sampled2D1 * Tex2D1;
float4 Multiply5=Multiply0 * float4(_Opacity);
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Emission = Multiply2;
o.Alpha = Multiply5 ;//* fade2;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Transparent/VertexLit"
}