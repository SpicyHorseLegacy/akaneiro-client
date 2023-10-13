Shader "WaterColor/Tint_Diffuse_NV" {
	Properties {
	   _TintColor("_TintColor", Color) = (1,1,1,1)
       _MainTex("_MainTex", 2D) = "white" {}
       _EdgeMaskTex("_EdgeMaskTex", 2D) = "white" {}
       _EdgeMaskTexCoord("_EdgeMaskTexCoord", Float) = 1
       
       _EdgeWidthNV("_EdgeWidth", Float) = 0.0
       _EdgeSharpness("_EdgeSharpness", Float) = 1.5
       _EdgeMaskBrightness("_EdgeMaskBrightness", Float) = 0.5
	   _Cutoff ("_Cutoff", Range(0,1)) = 0.5
	   
	   _TintBrightness("_TintBrightness", Float) = 1
       _TintMaskTex("_TintMaskTex", 2D) = "white" {}

		
	}
	
	SubShader {
	     Tags
		 {
           "Queue"="AlphaTest"
"IgnoreProjector"="True"
"RenderType"="TransparentCutout"

		 }
		Pass {
		
		    Tags { "LightMode" = "Vertex" }
		    
			ZTest LEqual Cull Off ZWrite On ColorMask RGBA
			
					
	    CGPROGRAM
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members vNormal)
#pragma exclude_renderers xbox360
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members vNormal)
//#pragma exclude_renderers xbox360
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members vNormal)
//#pragma exclude_renderers xbox360

        //#pragma target 2.0 
	    
	    #pragma vertex vert
	    #pragma fragment frag
	   
	    #include "UnityCG.cginc"
	    
	    fixed4 _TintColor;
		sampler2D _MainTex;
		sampler2D _EdgeMaskTex;
        fixed _EdgeMaskTexCoord;
        
        half _TintBrightness;
        sampler2D _TintMaskTex;
       
        half _EdgeWidthNV;
        half _EdgeSharpness;
        half _EdgeMaskBrightness;
        fixed _Cutoff;
        
        fixed4 _LightColor0;
        
	    struct appdata {
			half4 vertex : POSITION;
			half3 normal : NORMAL;
			half4 texcoord : TEXCOORD0;
        };
        
        struct v2f {
			half4 vertex : POSITION;
			half4 uv_norm : TEXCOORD0; //xy = uv, z = dot(normalize(vNormal), normalize(-viewpos))
			fixed4 vLightColor : Color;
        };
        
        half3 VertexLightsViewSpace (half3 VP, half3 VN)
		{
			half3 lightColor = half3(0.0,0.0,0.0);
	
			// preface & optimization
			half3 toLight0 = unity_LightPosition[1].xyz - VP;
			half3 toLight1 = unity_LightPosition[2].xyz - VP;
			half2 lengthSq2 = half2(dot(toLight0, toLight0), dot(toLight1, toLight1));
	
			half2 atten2 = half2(1.0,1.0) + lengthSq2 * half2(unity_LightAtten[1].z, unity_LightAtten[2].z);
			atten2 = 1.0 / atten2;
						
			// light #0
			half diff = saturate (dot (VN, normalize(toLight0)));
			lightColor += unity_LightColor[1].rgb * (diff * atten2.x);
	
			// light #1
			diff = saturate (dot (VN, normalize(toLight1)));
			lightColor += unity_LightColor[2].rgb * (diff * atten2.y);
	
			return lightColor;
		}	
			 
        
        v2f vert(appdata v)
        {
			v2f o;
			
			// object space view direction
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv_norm.xy = v.texcoord;
			
			half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
			o.uv_norm.z = dot(normalize(v.normal), viewDir);
			
			half3 lightDir = normalize(ObjSpaceLightDir(v.vertex));
			half vn = clamp(dot (v.normal, lightDir), 0, 1);
			o.vLightColor = UNITY_LIGHTMODEL_AMBIENT;
			o.vLightColor += half4(_LightColor0.rgb * vn,1.0);
			// Add point lights [BEGIN]
			half3 viewNormal = mul((float3x3)UNITY_MATRIX_MV, v.normal);
			half3 viewPos = mul(UNITY_MATRIX_MV, v.vertex).xyz;
			o.vLightColor += float4( VertexLightsViewSpace(viewPos, viewNormal), 1);
		    // Add point lights [END]

			return o;
        }
        
	    half4 frag (v2f IN) : COLOR
	    {
			half4 TexClr = tex2D(_MainTex, IN.uv_norm.xy);
			clip(TexClr.w - _Cutoff);
			
			//half3 output =  2 * TexClr.xyz * IN.vLightColor;
			
			half3 output =  TexClr.xyz * _TintColor;
			
			output = output * _TintBrightness;
			
			half4 Sampled2D1=tex2D(_TintMaskTex,IN.uv_norm.xy);
			
            half3 Splat0 = Sampled2D1.x;
            
            output= lerp(TexClr.xyz,output,Splat0);
			
			
			half2 EdgeMaskUV = IN.uv_norm.xy * _EdgeMaskTexCoord;
			
			half EdgeMask = tex2D(_EdgeMaskTex,EdgeMaskUV) * _EdgeMaskBrightness;
           
			half OutLine = IN.uv_norm.z;// dot(normalize(IN.Norm), normalize(IN.ViewDir));
			OutLine = clamp( ( OutLine - _EdgeWidthNV ) * _EdgeSharpness, 0, 1 );

          	/// output = output * ( OutLine + (1 - OutLine) * EdgeMask );
			half EdgeMaskFacor = 1 - OutLine;
			output = output * clamp((1 - EdgeMaskFacor * EdgeMaskFacor * EdgeMask),0,1); 
			
			output = output * IN.vLightColor * 2;
			
			//output.xyz = half3(1,1,1);
			       
			return half4(output.xyz, TexClr.w);
			
			
	    }
	    ENDCG
     }
 
}

Fallback Off
}
