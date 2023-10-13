Shader "FX/WaterLeak"
{
	Properties 
	{
_Color("_Color", Color) = (1,1,1,1)
_MainTex("_MainTex", 2D) = "white" {}
_MianTexXPan("_MianTexXPan", Float) = 0
_SecTex("_SecTex", 2D) = "white" {}
_SecTexSpeed("_SecTexSpeed", Float) = 0
_Opacity("_Opacity", Float) = 1

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
sampler2D _MainTex;
float _MianTexXPan;
sampler2D _SecTex;
float _SecTexSpeed;
float _Opacity;

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
float2 uv_SecTex;

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
				
float4 Multiply1=_Time * float4(_MianTexXPan);
Multiply1 = frac(Multiply1);
float4 UV_Pan0=float4((IN.uv_MainTex.xyxy).x,(IN.uv_MainTex.xyxy).y + Multiply1.y,(IN.uv_MainTex.xyxy).z,(IN.uv_MainTex.xyxy).w);
float4 Frac1=frac(UV_Pan0);
float4 Tex2D0=tex2D(_MainTex,Frac1.xy);
float4 Multiply5=_Time * float4(_SecTexSpeed);
Multiply5=frac(Multiply5);
float4 UV_Pan1=float4((IN.uv_SecTex.xyxy).x,(IN.uv_SecTex.xyxy).y + Multiply5.y,(IN.uv_SecTex.xyxy).z,(IN.uv_SecTex.xyxy).w);
float4 Frac3=frac(UV_Pan1);
float4 Tex2D1=tex2D(_SecTex,Frac3.xy);
float4 Multiply6=Tex2D0 * Tex2D1;
float4 Tex2D2=tex2D(_SecTex,(IN.uv_SecTex.xyxy).xy);
float4 Multiply0=Multiply6 * float4( Tex2D2.a);
float4 Multiply2=Multiply0 * float4(_Opacity);

o.Emission = _Color;
o.Alpha = Multiply2;

				//o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}