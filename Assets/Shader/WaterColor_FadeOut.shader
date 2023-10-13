Shader "WaterColor/FadeOut"
{
	Properties 
	{
_Color("_MainColor", Color) = (1,1,1,1)
_MainTex("_MainTex", 2D) = "white" {}
_EdgeSharpness("_EdgeSharpness", Float) = 3
_EdgeMaskTex("_EdgeMaskTex", 2D) = "white" {}
_EdgeMaskTexCoord("_EdgeMaskTexCoord", Float) = 3
_EdgeMaskBrightness("_EdgeMaskBrightness", Float) = 1
_EdgeWidth("_EdgeWidth", Float) = 2
_EmissiveColor("_EmissiveColor", Color) = (0,0,0,1)
_FadeOutMask("_FadeOutMask", 2D) = "white" {}
_FadeValue("_FadeValue", Range(0,1) ) = 0
_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="AlphaTest+51"
"IgnoreProjector"="True"
"RenderType"="TransparentCutout"

		}

		
Cull Back



CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff


float4 _Color;
sampler2D _MainTex;
float _EdgeSharpness;
sampler2D _EdgeMaskTex;
float _EdgeMaskTexCoord;
float _EdgeMaskBrightness;
float _EdgeWidth;
float4 _EmissiveColor;
sampler2D _FadeOutMask;
float _FadeValue;

			
			
			

			
			
			struct Input {
				float2 uv_MainTex;
                float3 viewDir;
                float2 uv_EdgeMaskTex;
                float2 uv_FadeOutMask;

			};


			
			

			void surf (Input IN, inout SurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss=0.0;
				o.Specular=0.0;
				
float4 Sampled2D0=tex2D(_MainTex,IN.uv_MainTex.xy);
float4 Multiply3=_Color * Sampled2D0;
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=float4( 1.0 - dot( normalize( float4(IN.viewDir, 1.0).xyz), normalize( Fresnel0_1_NoInput.xyz ) ) );
float4 Multiply8=Fresnel0 * float4(_EdgeWidth);
float4 Pow0=pow(Multiply8,float4(_EdgeSharpness));
float4 Multiply5=(IN.uv_EdgeMaskTex.xyxy) * float4(_EdgeMaskTexCoord);
float4 Tex2D0=tex2D(_EdgeMaskTex,Multiply5.xy);
float4 Multiply7=Tex2D0 * float4(_EdgeMaskBrightness);
float4 Invert1= float4(1.0) - Pow0;
float4 Clamp1=clamp(Invert1,float4( 0),float4( 1));
float4 Lerp0=lerp(Multiply7,float4( 1),Clamp1);
float4 Multiply4=Pow0 * Lerp0;
float4 Invert0= float4(1.0) - Multiply4;
float4 Clamp0=clamp(Invert0,float4( 0),float4( 1));
float4 Multiply10=Multiply3 * Clamp0;
float4 Multiply0=Multiply4 * _EmissiveColor;
float4 Sampled2D1=tex2D(_FadeOutMask,IN.uv_FadeOutMask.xy);
float4 Invert2= float4(1.0) - _FadeValue.xxxx;
float4 Add0=Sampled2D1 + Invert2;
float4 Floor0=floor(Add0);
float4 Multiply1=float4( Sampled2D0.a) * Floor0;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply10;
o.Emission = Multiply0;
o.Alpha = Multiply1;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Transparent/Cutout/VertexLit"
}