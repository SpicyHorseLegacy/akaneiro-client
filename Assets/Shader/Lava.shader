Shader "FX/Lava"
{
	Properties 
	{
_EmissiveColor("_EmissiveColor", Color) = (1,1,1,1)
_EmissiveBrightness("_EmissiveBrightness", Float) = 1
_MainTex("_MainTex", 2D) = "white" {}
_MainTexSpeed("_MainTexSpeed", Float) = 0
_NoiseTex("_NoiseTex", 2D) = "white" {}
_NoiseTexCoord("_NoiseTexCoord", Float) = 0
_NoiseSpeed("_NoiseSpeed", Float) = 0
_DistortionValue("_DistortionValue", Float) = 0

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


float4 _EmissiveColor;
float _EmissiveBrightness;
sampler2D _MainTex;
float _MainTexSpeed;
sampler2D _NoiseTex;
float _NoiseTexCoord;
float _NoiseSpeed;
float _DistortionValue;

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
float2 uv_NoiseTex;

			};


			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss=0.0;
				o.Specular=0.0;
				
float4 Multiply6=_EmissiveColor * float4(_EmissiveBrightness);
float4 Multiply7=_Time * float4(_MainTexSpeed);
float4 UV_Pan1=float4((IN.uv_MainTex.xyxy).x + Multiply7.y,(IN.uv_MainTex.xyxy).y + Multiply7.y,(IN.uv_MainTex.xyxy).z,(IN.uv_MainTex.xyxy).w);
float4 Multiply8=(IN.uv_NoiseTex.xyxy) * float4(_NoiseTexCoord);
float4 Multiply0=_Time * float4(_NoiseSpeed);
float4 UV_Pan0=float4(Multiply8.x + Multiply0.y,Multiply8.y + Multiply0.y,Multiply8.z,Multiply8.w);
float4 Tex2D0=tex2D(_NoiseTex,UV_Pan0.xy);
float4 Splat0=Tex2D0.x;
float4 Multiply1=Splat0 * float4(_DistortionValue);
float4 Add0=UV_Pan1 + Multiply1;
float4 Tex2D1=tex2D(_MainTex,Add0.xy);
float4 Multiply4=Multiply6 * Tex2D1;
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Emission = Multiply4;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}