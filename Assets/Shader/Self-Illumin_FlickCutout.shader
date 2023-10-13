Shader "Self-Illumin/FlickCutout"
{
	Properties 
	{
_Sampled2D("_Sampled2D", 2D) = "white" {}
_MainColor("_MainColor", Color) = (1,1,1,1)
_EmissiveColor("_EmissiveColor", Color) = (1,1,1,1)
_NoiseTex("_NoiseTex", 2D) = "white" {}
_FlickSpeed("_FlickSpeed", Float) = 1
_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}
	
	SubShader 
	{
		Tags
		{
"Queue"="AlphaTest"
"IgnoreProjector"="False"
"RenderType"="TransparentCutout"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert alphatest:_Cutoff
#pragma target 2.0


sampler2D _Sampled2D;
float4 _MainColor;
float4 _EmissiveColor;
sampler2D _NoiseTex;
float _FlickSpeed;

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
				float2 uv_Sampled2D;
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
				
float4 Sampled2D0=tex2D(_Sampled2D,IN.uv_Sampled2D.xy);
float4 Multiply0=Sampled2D0 * _MainColor;
float4 Multiply3=Sampled2D0 * _EmissiveColor;
float4 Multiply1=float4(_FlickSpeed) * _Time;
float4 UV_Pan0=float4((IN.uv_NoiseTex.xyxy).x + Multiply1.x,(IN.uv_NoiseTex.xyxy).y + Multiply1.x,(IN.uv_NoiseTex.xyxy).z,(IN.uv_NoiseTex.xyxy).w);
float4 Tex2D0=tex2D(_NoiseTex,UV_Pan0.xy);
float4 Multiply2=Multiply3 * Tex2D0;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply0;
o.Emission = Multiply2;
o.Alpha = float4( Sampled2D0.a);

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Transparent/Cutout/VertexLit"
}