Shader "FoliageDistortion"
{
	Properties 
	{
_MainColor("_MainColor", Color) = (1,1,1,1)
_MainTex("_MainTex", 2D) = "white" {}
_DistortionValueX("_DistortionValueX", Float) = 0
_DistortionValueY("_DistortionValueY", Float) = 0
_NoiseTex("_NoiseTex", 2D) = "white" {}
_DistortionCycle("_DistortionCycle", Float) = 0.5
_NoiseSpeed("_NoiseSpeed", Float) = 1
_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}
	
	SubShader 
	{
		Tags
		{
"Queue"="AlphaTest"
"IgnoreProjector"="Ture"
"RenderType"="TransparentCutout"

		}

		
Cull Off
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert alphatest:_Cutoff
#pragma target 3.0


sampler2D _MainTex;
float _DistortionValueX;
float _DistortionValueY;
sampler2D _NoiseTex;
float _DistortionCycle;
float _NoiseSpeed;
float4 _MainColor;

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
				
float4 Assemble1_2_NoInput = float4(0,0,0,0);
float4 Assemble1_3_NoInput = float4(0,0,0,0);
float4 Assemble1=float4(float4(_DistortionValueX).x, float4(_DistortionValueY).y, Assemble1_2_NoInput.z, Assemble1_3_NoInput.w);
float4 Multiply0=float4(_DistortionCycle) * _Time;
float4 Sin0=sin(Multiply0);
float4 Lerp1=lerp(float4( 0.1),float4( 0.175),Sin0);
float4 Multiply6=(IN.uv_NoiseTex.xyxy) * Lerp1;
float4 Multiply9=Multiply0 * float4(_NoiseSpeed);
float4 UV_Pan0=float4(Multiply6.x + Multiply9.x,Multiply6.y + Multiply9.x,Multiply6.z,Multiply6.w);
float4 Tex2D1=tex2D(_NoiseTex,UV_Pan0.xy);
float4 Splat0=Tex2D1.x;
float4 Multiply11=float4( 1.6) * Multiply0;
float4 Sin1=sin(Multiply11);
float4 Multiply12=Splat0 * Sin1;
float4 Assemble0_2_NoInput = float4(0,0,0,0);
float4 Assemble0_3_NoInput = float4(0,0,0,0);
float4 Assemble0=float4(Multiply12.x, Splat0.y, Assemble0_2_NoInput.z, Assemble0_3_NoInput.w);
float4 Multiply1=Assemble1 * Assemble0;
float4 Add0=(IN.uv_MainTex.xyxy) + Multiply1;
float4 Tex2D2=tex2D(_MainTex,Add0.xy);
float4 Multiply3=Tex2D2 * _MainColor;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_2_NoInput = float4(0,0,0,0);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply3;
o.Alpha = float4( Tex2D2.a);

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Transparent/Cutout/VertexLit"
}