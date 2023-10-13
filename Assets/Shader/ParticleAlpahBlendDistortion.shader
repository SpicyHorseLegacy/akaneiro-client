Shader "Particles/AlphaBlendDistortion"
{
	Properties 
	{
_TintColor("_TintColor", Color) = (1,1,1,1)
_MainTex("_MainTex", 2D) = "white" {}
_NoiseTex("_NoiseTex", 2D) = "white" {}
_NoiseTexCoord("_NoiseTexCoord", Float) = 0
_Speed("_Speed", Float) = 0
_DistortionValue("_DistortionValue", Float) = 0

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="Transparent"

		}

		
Cull Off
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  alpha decal:blend vertex:vert
#pragma target 2.0


float4 _TintColor;
sampler2D _MainTex;
sampler2D _NoiseTex;
float _NoiseTexCoord;
float _Speed;
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
				float4 color : COLOR;
float2 uv_MainTex;
float2 uv_NoiseTex;

			};


			void vert (inout appdata_full v, out Input o) {



			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				//o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss=0.0;
				o.Specular=0.0;
				
float4 Multiply6=_TintColor * float4( 2);
float4 Multiply8=(IN.uv_NoiseTex.xyxy) * float4(_NoiseTexCoord);
float4 Multiply0=_Time * float4(_Speed);
float4 UV_Pan0=float4(Multiply8.x + Multiply0.y,Multiply8.y + Multiply0.y,Multiply8.z,Multiply8.w);
float4 Tex2D0=tex2D(_NoiseTex,UV_Pan0.xy);
float4 Splat0=Tex2D0.x;
float4 Multiply1=Splat0 * float4(_DistortionValue);
float4 Add0=(IN.uv_MainTex.xyxy) + Multiply1;
float4 Tex2D1=tex2D(_MainTex,Add0.xy);
float4 Multiply4=Multiply6 * Tex2D1;
float4 Multiply2=IN.color * Multiply4;
float4 Splat1=IN.color.w;
float4 Splat2=Multiply6.w;
float4 Multiply5=Splat2 * float4( Tex2D1.a);
float4 Multiply3=Splat1 * Multiply5;

o.Emission = Multiply2;
o.Alpha = Multiply3;

				//o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}