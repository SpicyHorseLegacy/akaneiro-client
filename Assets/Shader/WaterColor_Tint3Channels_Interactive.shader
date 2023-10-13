Shader "WaterColor/Tint3Channels_Interactive"
{
	Properties 
	{
_Color("_MainColor", Color) = (1,1,1,1)
_TintColorR("_TintColorR", Color) = (1,1,1,1)
_TintColorG("_TintColorG", Color) = (1,1,1,1)
_TintColorB("_TintColorB", Color) = (1,1,1,1)
_MainTex("_MainTex", 2D) = "white" {}
_TintMaskTex("_TintMaskTex", 2D) = "white" {}
_EdgeSharpness("_EdgeSharpness", Float) = 3
_EdgeMaskTex("_EdgeMaskTex", 2D) = "white" {}
_EdgeMaskTexCoord("_EdgeMaskTexCoord", Float) = 3
_EdgeMaskBrightness("_EdgeMaskBrightness", Float) = 1
_EdgeWidth("_EdgeWidth", Float) = 2
_EmissiveColor("_EmissiveColor", Color) = (0,0,0,1)
_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
_FogColor("_FogColor",Color) = (1,1,1,1)
_FogStartDis("_FogStartDis",Float) = 0
_FogEndDis("_FogEndDis",Float) = 1
_FogEnable("_FogEnable",Float) = 0
	}
	
	SubShader 
	{
		Tags
		{
"Queue"="AlphaTest"
"IgnoreProjector"="True"
"RenderType"="TransparentCutout"

		}

		
Cull Off
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{Mode off}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert alphatest:_Cutoff finalcolor:mycolor  
#pragma target 2.0
#include "UnityCG.cginc"


float4 _Color;
float4 _TintColorB;
float4 _TintColorG;
float4 _TintColorR;
sampler2D _MainTex;
sampler2D _TintMaskTex;
float _EdgeSharpness;
sampler2D _EdgeMaskTex;
float _EdgeMaskTexCoord;
float _EdgeMaskBrightness;
float _EdgeWidth;
float4 _EmissiveColor;
float4 _FogColor;
float  _FogStartDis;
float  _FogEndDis;
float  _FogEnable;

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
				float2 uv_TintMaskTex;
                float3 viewDir;
                float2 uv_EdgeMaskTex;
                half vfog;
			};


			void vert (inout appdata_full v, out Input o) {

               //float4 hpos = mul (UNITY_MATRIX_MVP, v.vertex);
               ///o.vfog = min (1, dot (hpos.xy, hpos.xy) * 0.01);
               //o.vfog =  o.vfog * _FogDensity;
               
               float3	hpos  = mul(UNITY_MATRIX_MV,v.vertex);
		       float    dist  = length(hpos);
               
               o.vfog  = (dist - _FogStartDis)/(_FogEndDis - _FogStartDis);
               o.vfog = clamp(o.vfog,0,1) * _FogEnable;
              
                   
			}
			
			 void mycolor (Input IN, EditorSurfaceOutput o, inout fixed4 color)
             {
                 fixed3 fogColor = _FogColor.rgb;
                 #ifdef UNITY_PASS_FORWARDADD
                 fogColor = 0;
                 #endif
                 color.rgb = lerp (color.rgb, fogColor, IN.vfog);
             }

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss=0.0;
				o.Specular=0.0;
				
float4 Sampled2D0=tex2D(_MainTex,IN.uv_MainTex.xy);
float4 Multiply3=Sampled2D0 * _TintColorR;
float4 Sampled2D1=tex2D(_TintMaskTex,IN.uv_TintMaskTex.xy);
float  Splat0=Sampled2D1.x;
float4 Lerp1=lerp(Sampled2D0,Multiply3,Splat0);
float4 Multiply0=_TintColorG * Lerp1;
float  Splat2=Sampled2D1.y;
float4 Lerp2=lerp(Lerp1,Multiply0,Splat2);
float4 Multiply1=_TintColorB * Lerp2;
float  Splat1=Sampled2D1.z;
float4 Lerp3=lerp(Lerp2,Multiply1,Splat1);
//float4 Multiply3=_Color * Sampled2D0;
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=float4( 1.0 - dot( normalize( float4(IN.viewDir, 1.0).xyz), normalize( Fresnel0_1_NoInput.xyz ) ) );
float4 Multiply8=Fresnel0 * float4(_EdgeWidth);
float4 Pow0=pow(Multiply8,float4(_EdgeSharpness));
float4 Multiply5=(IN.uv_EdgeMaskTex.xyxy) * float4(_EdgeMaskTexCoord);
float4 Tex2D0=tex2D(_EdgeMaskTex,Multiply5.xy);
float4 Multiply7=Tex2D0 * float4(_EdgeMaskBrightness);
float4 Invert1= float4(1.0) - Pow0;
float4 Clamp1=clamp(Invert1,float4( 0),float4( 1));
float4 Lerp0=lerp(Multiply7,float4( 1),Clamp1);
float4 Multiply4=Pow0 * Lerp0;
float4 Invert0= float4(1.0) - Multiply4;
float4 Clamp0=clamp(Invert0,float4( 0),float4( 1));
float4 Multiply10=Lerp3 * Clamp0;
float4 Multiply2=Multiply4 * _EmissiveColor;

o.Albedo = Multiply10;
o.Emission = Multiply2;
o.Alpha = Sampled2D0.a;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Transparent/Cutout/VertexLit"
}