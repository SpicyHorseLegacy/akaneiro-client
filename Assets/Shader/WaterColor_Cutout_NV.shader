Shader "WaterColor/Cutout_NV" {
	Properties {
	   _Color("_MainColor", Color) = (1,1,1,1)
       _MainTex("_MainTex", 2D) = "white" {}
       _EdgeMaskTex("_EdgeMaskTex", 2D) = "white" {}
       _EdgeMaskTexCoord("_EdgeMaskTexCoord", Float) = 1
       
       _EdgeWidthNV("_EdgeWidth", Float) = 0.0
       _EdgeSharpness("_EdgeSharpness", Float) = 1.5
       _EdgeMaskBrightness("_EdgeMaskBrightness", Float) = 0.5
	   _Cutoff ("_Cutoff", Range(0,1)) = 0.5

		
	}
	
	SubShader {
	     Tags
		 {
           "Queue"="AlphaTest"
"IgnoreProjector"="True"
"RenderType"="TransparentCutout"

		 }
		Pass {
		
		    Tags { "LightMode" = "ForwardBase" }
		    
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

	    
	    fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _EdgeMaskTex;
        fixed _EdgeMaskTexCoord;
       
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
        
        struct appdata_lm {
			half4 vertex : POSITION;
			half3 normal : NORMAL;
			half4 texcoord : TEXCOORD0;
			float2 texcoord1 : TEXCOORD1;
        };
        
        struct v2f {
			half4 vertex : POSITION;
			half4 uv_norm : TEXCOORD0; //xy = uv, z = dot(normalize(vNormal), normalize(-viewpos))
			fixed4 vLightColor : Color;
        };
        
        struct v2f_lm {
			half4 vertex : POSITION;
			half4 uv_norm : TEXCOORD0; //xy = uv, z = dot(normalize(vNormal), normalize(-viewpos))
			float2 uv_lm: TEXCOORD1;
			fixed4 vLightColor : Color;
        };
        
	  // These are prepopulated by Unity
	  sampler2D unity_Lightmap;
	  float4 unity_LightmapST;
		float4 _MainTex_ST; 
        
        v2f vert(appdata v)
        {
			v2f o;
			// object space view direction
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			
			o.uv_norm.xy = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord.xy );

			///half3 vNormal = mul((float3x3)UNITY_MATRIX_MV, v.normal);
			half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
			o.uv_norm.z = dot(normalize(v.normal), viewDir);
			
			o.vLightColor.xyz = ShadeSH9(float4(mul((float3x3)_Object2World, v.normal), 1.0));
			return o;
        }
		
        v2f_lm vert_lm(appdata_lm v)
        {
			v2f_lm o;
			// object space view direction
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			
			o.uv_norm.xy = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord.xy );

			///half3 vNormal = mul((float3x3)UNITY_MATRIX_MV, v.normal);
			half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
			o.uv_norm.z = dot(normalize(v.normal), viewDir);
			
			half3 lightDir = normalize(ObjSpaceLightDir(v.vertex));
			half vn = clamp(dot (v.normal, lightDir), 0, 1);
			o.vLightColor = UNITY_LIGHTMODEL_AMBIENT;
			o.vLightColor += half4(_LightColor0.rgb * vn,1.0);
			o.vLightColor *= _Color;
			
			// Use `unity_LightmapST` NOT `unity_Lightmap_ST`
			o.uv_lm = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;

			return o;
        }
        
	    half4 frag (v2f IN) : COLOR
	    {
			half4 TexClr = tex2D(_MainTex, IN.uv_norm.xy);
			clip(TexClr.w - _Cutoff);
			
			half3 output = TexClr.xyz * IN.vLightColor * _Color; 
			
			half2 EdgeMaskUV = IN.uv_norm.xy * _EdgeMaskTexCoord;
			half EdgeMask = tex2D(_EdgeMaskTex,EdgeMaskUV) * _EdgeMaskBrightness;
           
			half OutLine = IN.uv_norm.z;// dot(normalize(IN.Norm), normalize(IN.ViewDir));
			OutLine = clamp( ( OutLine - _EdgeWidthNV ) * _EdgeSharpness, 0, 1 );

          	/// output = output * ( OutLine + (1 - OutLine) * EdgeMask );
			half EdgeMaskFacor = 1 - OutLine;
			output = output * clamp((1 - EdgeMaskFacor * EdgeMaskFacor * EdgeMask),0,1);    
			       
			return half4(output.xyz, TexClr.w);
	    }
		
	    half4 frag_lm (v2f_lm IN) : COLOR
	    {
			half4 TexClr = tex2D(_MainTex, IN.uv_norm.xy);
			clip(TexClr.w - _Cutoff);
			
			half3 output = TexClr.xyz ;//* IN.vLightColor;
			
			output.xyz *= DecodeLightmap(tex2D(unity_Lightmap, IN.uv_lm));
			
			half2 EdgeMaskUV = IN.uv_norm.xy * _EdgeMaskTexCoord;
			half EdgeMask = tex2D(_EdgeMaskTex,EdgeMaskUV) * _EdgeMaskBrightness;
           
			half OutLine = IN.uv_norm.z;// dot(normalize(IN.Norm), normalize(IN.ViewDir));
			OutLine = clamp( ( OutLine - _EdgeWidthNV ) * _EdgeSharpness, 0, 1 );

          	/// output = output * ( OutLine + (1 - OutLine) * EdgeMask );
			half EdgeMaskFacor = 1 - OutLine;
			output = output * clamp((1 - EdgeMaskFacor * EdgeMaskFacor * EdgeMask),0,1);    
			       
			
				   
			return half4(output.xyz, TexClr.w);
	    }
		
	    ENDCG
     }
 
}

Fallback Off
}