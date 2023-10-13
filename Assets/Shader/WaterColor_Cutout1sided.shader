Shader "WaterColor/Cutout1sided"
{
	Properties 
	{

_Color("_MainColor", Color) = (1,1,1,1)
_MainTex("_MainTex", 2D) = "white" {}
_EdgeMaskTex("_EdgeMaskTex", 2D) = "white" {}
_EdgeMaskTexCoord("_EdgeMaskTexCoord", Float) = 3
_EdgeSharpness("_EdgeSharpness", Float) = 3
_EdgeWidth("_EdgeWidth", Float) = 2
_EdgeMaskBrightness("_EdgeMaskBrightness", Float) = 1
_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
_GrayScaleEff("_GrayScaleFlag",Float) = 0
	}
	
	SubShader 
	{
		Tags
		{
"Queue"="AlphaTest"
"IgnoreProjector"="True"
"RenderType"="TransparentCutout"

		}
LOD 200
		
Cull Back




		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert alphatest:_Cutoff  
#pragma target 2.0


float4 _EdgeColor;
float4 _Color;
sampler2D _MainTex;
sampler2D _EdgeMaskTex;
float _EdgeMaskTexCoord;
float _EdgeSharpness;
float _EdgeWidth;
float _EdgeMaskBrightness;
float _GrayScaleEff;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
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
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_MainTex;
float3 viewDir;
float2 uv_EdgeMaskTex;

			};

			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				//o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Sampled2D0=tex2D(_MainTex,IN.uv_MainTex.xy);
float4 Multiply3=_Color * Sampled2D0;
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx;
float4 Multiply8=Fresnel0 * float4(_EdgeWidth);
float4 Pow0=pow(Multiply8,float4(_EdgeSharpness));
float4 Multiply5=(IN.uv_EdgeMaskTex.xyxy) * _EdgeMaskTexCoord.xxxx;
float4 Tex2D0=tex2D(_EdgeMaskTex,Multiply5.xy);
float4 Multiply7=Tex2D0 * _EdgeMaskBrightness.xxxx;
float4 Invert1= float4(1.0, 1.0, 1.0, 1.0) - Pow0;
float4 Clamp1=clamp(Invert1,float4( 0,0,0,0 ),float4( 1,1,1,1 ));
float4 Lerp0=lerp(Multiply7,float4( 1,1,1,1 ),Clamp1);
float4 Multiply4=Pow0 * Lerp0;
float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - Multiply4;
float4 Clamp0=clamp(Invert0,float4( 0,0,0,0 ),float4( 1,1,1,1 ));
float4 Multiply10=Multiply3 * Clamp0;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_2_NoInput = float4(0,0,0,0);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
if(_GrayScaleEff>0)
   Multiply10 = (Multiply10.r + Multiply10.g + Multiply10.b)/3.0;

o.Albedo = Multiply10;
o.Alpha = Sampled2D0.a;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Transparent/Cutout/VertexLit"
}