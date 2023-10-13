Shader "WaterColor/Tint_2Channels"
{
	Properties 
	{
_Color("_MainColor", Color) = (1,1,1,1)
_MainTex("_MainTex", 2D) = "white" {}
_IsColor ("_IsColor", Range(0,1)) = 1
_TintColorR("_TintColorR", Color) = (1,1,1,1)
_TintMaskTex("_TintMaskTex", 2D) = "white" {}
_TintColorG("_TintColorG", Color) = (1,1,1,1)
_EdgeWidth("_EdgeWidth", Float) = 2
_EdgeSharpness("_EdgeSharpness", Float) = 3
_EdgeMaskTex("_EdgeMaskTex", 2D) = "white" {}
_EdgeMaskTexCoord("_EdgeMaskTexCoord", Float) = 1
_EdgeMaskBrightness("_EdgeMaskBrightness", Float) = 1
_EmissiveColor("_EmissiveColor", Color) = (0,0,0,1)
_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert alphatest:_Cutoff
#pragma target 2.0


sampler2D _MainTex;
float _IsColor;
float4 _TintColorR;
sampler2D _TintMaskTex;
float4 _TintColorG;
float _EdgeWidth;
float _EdgeSharpness;
sampler2D _EdgeMaskTex;
float _EdgeMaskTexCoord;
float _EdgeMaskBrightness;
float4 _EmissiveColor;

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
				
float4 Sampled2D0=tex2D(_MainTex,IN.uv_MainTex.xy);
float4 Multiply3=Sampled2D0 * _TintColorR;
float4 Sampled2D1=tex2D(_TintMaskTex,IN.uv_TintMaskTex.xy);
float4 Splat0=Sampled2D1.x;
float4 Lerp1=lerp(Sampled2D0,Multiply3,Splat0);
float4 Multiply0=Lerp1 * _TintColorG;
float4 Splat2=Sampled2D1.y;
float4 Lerp2=lerp(Lerp1,Multiply0,Splat2);
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
float4 Multiply10=Lerp2 * Clamp0;

// ºÚ°×±äÉ«
if(_IsColor == 0){
float3 combined = float3(2.0f * Multiply10.rgb);
Multiply10.rgb = dot(combined, float3(0.3f, 0.59f, 0.11f));//lerp(float3(x,x,x), combined.rgb, 0f); //Use the lerp version for more control

}

float4 Multiply2=Multiply4 * _EmissiveColor;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply10;
o.Emission = Multiply2;
o.Alpha = float4( Sampled2D0.a);

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Transparent/Cutout/VertexLit"
}