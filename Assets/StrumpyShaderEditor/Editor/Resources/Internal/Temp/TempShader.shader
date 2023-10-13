Shader "ShaderEditor/EditorShaderCache"
{
	Properties 
	{
_Color("_Color", Color) = (1,1,1,1)
_Sampler2D("_Sampler2D", 2D) = "white" {}
_Float4("_Float4", Vector) = (0,0,0,0)
_Float("_Float", Float) = 0
_EditorTime("_EditorTime",Vector) = (0.0,0.0,0.0,0.0)

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


float4 _Color;
sampler2D _Sampler2D;
float4 _Float4;
float _Float;
float4 _EditorTime;

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
				float2 uv_Sampler2D;

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
				
float4 Multiply3=(IN.uv_Sampler2D.xyxy) * _Float4;
float4 Multiply1=_EditorTime * float4(_Float);
float4 UV_Pan0=float4(Multiply3.x + Multiply1.y,Multiply3.y + Multiply1.y,Multiply3.z,Multiply3.w);
float4 Tex2D0=tex2D(_Sampler2D,UV_Pan0.xy);
float4 Multiply4=(IN.uv_Sampler2D.xyxy) * _Float4;
float4 Multiply5=_EditorTime * float4(_Float);
float4 UV_Pan1=float4(Multiply4.x + Multiply5.y,Multiply4.y + Multiply5.y,Multiply4.z,Multiply4.w);
float4 Tex2D1=tex2D(_Sampler2D,UV_Pan1.xy);
float4 Lerp0=lerp(Tex2D0,Tex2D1,float4(_Float));
float4 Multiply2=_Color * Lerp0;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_2_NoInput = float4(0,0,0,0);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply2;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}