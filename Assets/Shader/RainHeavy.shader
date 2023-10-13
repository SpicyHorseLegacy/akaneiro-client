Shader "FX/RainHeavy"
{
	Properties 
	{
_Color("_Color", Color) = (1,1,1,1)
_RainTex("_RainTex", 2D) = "white" {}
_FirstTexCoord("_FirstTexCoord", Vector) = (1,1,0,0)
_FadeTex("_FadeTex", 2D) = "white" {}
_SecTexCoord("_SecTexCoord", Vector) = (1,1,0,0)
_MainTexXpan("_MainTexXpan", Float) = 0
_MainTexYpan("_MainTexYpan", Float) = 0
_SecTexXpan("_SecTexXpan", Float) = 0
_SecTexYpan("_SecTexYpan", Float) = 0
_Opacity("_Opacity", Float) = 0

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


float4 _Color;
sampler2D _RainTex;
float4 _FirstTexCoord;
sampler2D _FadeTex;
float4 _SecTexCoord;
float _Opacity;
float _MainTexXpan;
float _MainTexYpan;
float _SecTexXpan;
float _SecTexYpan;
float _LerpValue;

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
				float2 uv_RainTex;
float2 uv_FadeTex;

			};


			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				//o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss=0.0;
				o.Specular=0.0;
				
float4 Multiply3=(IN.uv_RainTex.xyxy) * _FirstTexCoord;
float4 Multiply6 = _Time * _MainTexXpan;
Multiply6=frac(Multiply6);
float4 Multiply7 = _Time * _MainTexYpan;
Multiply7=frac(Multiply7);
float4 UV_Pan0=float4(Multiply3.x + Multiply6.y,Multiply3.y + Multiply7.y,Multiply3.z,Multiply3.w);
UV_Pan0 = frac(UV_Pan0);
float4 Tex2D0=tex2D(_RainTex,UV_Pan0.xy);
float4 Split0=Tex2D0;
float4 Sampled2D1=tex2D(_FadeTex,IN.uv_FadeTex.xy);
float4 Multiply4=(IN.uv_RainTex.xyxy) * _SecTexCoord;
float4 Multiply8 = _Time * _SecTexXpan;
Multiply8 = frac(Multiply8);
float4 Multiply9 = _Time * _SecTexYpan;
Multiply9 = frac(Multiply9);
float4 UV_Pan1=float4(Multiply4.x + Multiply8.y,Multiply4.y + Multiply9.y,Multiply4.z,Multiply4.w);
UV_Pan1 = frac(UV_Pan1);
float4 Tex2D1=tex2D(_RainTex,UV_Pan1.xy);
float4 Split1=Tex2D1;
float4 Add0=float4( Split0.xxxx) + float4( Split1.xxxx);
float4 Multiply0=Sampled2D1 * Add0;
float4 Multiply5=Multiply0 * float4(_Opacity);

o.Emission = _Color;
o.Alpha = Multiply5;

				//o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}