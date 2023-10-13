Shader "FX/WaterStreamNoDepth"
{
	Properties 
	{
_Color("_Color", Color) = (1,1,1,1)
_WaterTex("_WaterTex", 2D) = "white" {}
_WaterTexCoord("_WaterTexCoord", Vector) = (1,1,0,0)
_WaterTexXPan("_WaterTexXPan", Float) = -0.015
_WaterTexYpan("_WaterTexYpan", Float) = 0.2
_MainTex("_MainTex", 2D) = "white" {}
_MainTexCoord("_MainTexCoord", Vector) = (1,1,0,0)
_MainTexXPan("_MainTexXPan", Float) = -0.015
_MianTexYpan("_MianTexYpan", Float) = 0.2
_SecTexCoord("_SecTexCoord", Vector) = (1,1,0,0)
_SecTexXPan("_SecTexXPan", Float) = -0.015
_SecTexYpan("_SecTexYpan", Float) = 0.2
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

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  alpha decal:blend vertex:vert
#pragma target 2.0
//#include "UnityCG.cginc"

float4 _Color;
sampler2D _WaterTex;
float4 _WaterTexCoord;
float _WaterTexXPan;
float _WaterTexYpan;
sampler2D _MainTex;
float4 _MainTexCoord;
float _SecTexXPan;
float _SecTexYpan;
float _MainTexXPan;
float _MianTexYpan;
float4 _SecTexCoord;
float _Opacity;
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
				float2 uv_WaterTex;
                float2 uv_MainTex;
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
				
float4 Multiply8=(IN.uv_WaterTex.xyxy) * _WaterTexCoord;
float4 Multiply9=_Time * float4(_WaterTexXPan);
float4 Multiply10=_Time * float4(_WaterTexYpan);
//float4 Assemble1_2_NoInput = float4(0,0,0,0);
//float4 Assemble1_3_NoInput = float4(0,0,0,0);
//float4 Assemble1=float4(Multiply9.x, Multiply10.y, Assemble1_2_NoInput.z, Assemble1_3_NoInput.w);
float4 UV_Pan2=float4(Multiply8.x + Multiply9.y,Multiply8.y + Multiply10.y,Multiply8.z,Multiply8.w);
float4 Tex2D2=tex2D(_WaterTex,UV_Pan2.xy);
float4 Multiply3=(IN.uv_MainTex.xyxy) * _MainTexCoord;
float4 Multiply1=_Time * float4(_SecTexXPan);
float4 Multiply5=_Time * float4(_SecTexYpan);
float4 Multiply6=_Time * float4(_MainTexXPan);
float4 Multiply7=_Time * float4(_MianTexYpan);
//float4 Assemble0=float4(Multiply1.x, Multiply5.y, Multiply6.z, Multiply7.w);
float4 UV_Pan0=float4(Multiply3.x + Multiply6.y,Multiply3.y + Multiply7.y,Multiply3.z,Multiply3.w);
float4 Tex2D0=tex2D(_MainTex,UV_Pan0.xy);
float4 Multiply4=(IN.uv_MainTex.xyxy) * _SecTexCoord;
float4 UV_Pan1=float4(Multiply4.x + Multiply1.y,Multiply4.y + Multiply5.y,Multiply4.z,Multiply4.w);
float4 Tex2D1=tex2D(_MainTex,UV_Pan1.xy);
float4 Lerp0=lerp(Tex2D0,Tex2D1,float4( Tex2D1.a));
float4 Add0=float4( Tex2D0.a) + float4( Tex2D1.a);
float4 Clamp0=clamp(Add0,float4( 0),float4( 1));
float4 Lerp1=lerp(Tex2D2,Lerp0,Clamp0);
float4 Multiply2=_Color * Lerp1;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_2_NoInput = float4(0,0,0,0);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply2;
o.Alpha = float4(_Opacity) ;//* fade2;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}