Shader "Particles/Additive_SwiftPan"
{
	Properties 
	{
_TintColor("Tint Color", Color) = (1,1,1,1)
_SecTex("SecTex", 2D) = "white" {}
_SecSpeed("SecSpeed", Float) = 0
_MainTex("MainTex", 2D) = "white" {}
_Speed("Speed", Float) = 0

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
#pragma surface surf BlinnPhongEditor  alpha decal:add vertex:vert
#pragma target 2.0


float4 _TintColor;
sampler2D _SecTex;
float _SecSpeed;
sampler2D _MainTex;
float _Speed;

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
float2 uv_SecTex;
float2 uv_MainTex;

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
				
float4 Multiply6=_TintColor * float4( 2);
float4 Multiply10=float4(_SecSpeed) * _Time;
float4 UV_Pan2=float4((IN.uv_SecTex.xyxy).x + Multiply10.x,(IN.uv_SecTex.xyxy).y,(IN.uv_SecTex.xyxy).z,(IN.uv_SecTex.xyxy).w);
float4 Tex2D2=tex2D(_SecTex,UV_Pan2.xy);
float4 Multiply9=_Time * float4(_Speed);
float4 UV_Pan1=float4((IN.uv_MainTex.xyxy).x + Multiply9.x,(IN.uv_MainTex.xyxy).y,(IN.uv_MainTex.xyxy).z,(IN.uv_MainTex.xyxy).w);
float4 Tex2D1=tex2D(_MainTex,UV_Pan1.xy);
float4 Multiply0=Tex2D2 * Tex2D1;
float4 Splat3=Tex2D1.x;
float4 Lerp0=lerp(float4( 0),Multiply0,Splat3);
float4 Multiply4=Multiply6 * Lerp0;
float4 Multiply2=IN.color * Multiply4;
float4 Splat1=IN.color.w;
float4 Splat2=Multiply6.w;
float4 Splat0=Lerp0.w;
float4 Multiply5=Splat2 * Splat0;
float4 Multiply3=Splat1 * Multiply5;
float4 Multiply7=Multiply2 * Multiply3;
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Emission = Multiply7;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}