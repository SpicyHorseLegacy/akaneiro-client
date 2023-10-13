Shader "FX/FR_Waterfall_Depth"
{
	Properties 
	{
_WaterfallTex("_WaterfallTex", 2D) = "white" {}
_WaterfallTexCoord("_WaterfallTexCoord", Vector) = (0,0,0,0)
_WaterfallTexSpeed("_WaterfallTexSpeed", Float) = 0
_TopColor("_TopColor", Color) = (1,1,1,1)
_Brightness("_Brightness", Float) = 1
_DetailTex("_DetailTex", 2D) = "white" {}
_UniformTexCoord("_UniformTexCoord", Float) = 0
_DetailTexSpeed("_DetailTexSpeed", Float) = 0
_Opacity("_Opacity", Float) = 1
_InvFade ("_InvFade", Range (0.05, 5.0)) = 1.0
	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="Transparent"

		}

		
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

sampler2D _WaterfallTex;
float4 _WaterfallTexCoord;
float _WaterfallTexSpeed;
float4 _TopColor;
float _Brightness;
sampler2D _DetailTex;
float _UniformTexCoord;
float _DetailTexSpeed;
float _Opacity;
float _InvFade;
sampler2D _CameraDepthTexture;

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
				float2 uv_WaterfallTex;
                float2 uv_DetailTex;
                float4 vertex : POSITION;
				float4 projPos : TEXCOORD2;

			};


			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);
                       o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);		
                       o.projPos = ComputeScreenPos( o.vertex);
                       
                       COMPUTE_EYEDEPTH(o.projPos.z);

			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss=0.0;
				o.Specular=0.0;
				
				float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.projPos))));
	            float partZ = IN.projPos.z;
	            float fade2 =  saturate (_InvFade * (sceneZ-partZ));
				
float4 Multiply8=(IN.uv_WaterfallTex.xyxy) * _WaterfallTexCoord;
float4 Multiply0=_Time * float4(_WaterfallTexSpeed);
float4 UV_Pan0=float4(Multiply8.x,Multiply8.y + Multiply0.y,Multiply8.z,Multiply8.w);
float4 Tex2D0=tex2D(_WaterfallTex,UV_Pan0.xy);
float4 Multiply1=Tex2D0 * _TopColor;
float4 Multiply5=Multiply1 * float4(_Brightness);
float4 Multiply9=(IN.uv_DetailTex.xyxy) * float4(_UniformTexCoord);
float4 Multiply10=_Time * float4(_DetailTexSpeed);
float4 UV_Pan1=float4(Multiply9.x,Multiply9.y + Multiply10.y,Multiply9.z,Multiply9.w);
float4 Tex2D2=tex2D(_DetailTex,UV_Pan1.xy);
float4 Splat3=Tex2D2.x;
float4 UV_Pan2=float4((IN.uv_WaterfallTex.xyxy).x,(IN.uv_WaterfallTex.xyxy).y + Multiply0.y,(IN.uv_WaterfallTex.xyxy).z,(IN.uv_WaterfallTex.xyxy).w);
float4 Tex2D1=tex2D(_WaterfallTex,UV_Pan2.xy);
float4 Multiply11=Splat3 * float4( Tex2D1.a);
float4 Lerp0=lerp(Multiply11,float4( Tex2D1.a),float4( Tex2D1.a));
float4 Tex2D3=tex2D(_DetailTex,(IN.uv_DetailTex.xyxy).xy);
float4 Splat4=Tex2D3.y;
float4 Multiply2=Lerp0 * Splat4;
float4 Multiply4=Multiply2 * float4(_Opacity);
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Emission = Multiply5;
o.Alpha = Multiply4 * fade2;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}