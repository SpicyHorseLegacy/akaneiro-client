Shader "WaterColor/Diffuse_NV_Simplified" {
	Properties {
	   _Color("_MainColor", Color) = (1,1,1,1)
       _MainTex("_MainTex", 2D) = "white" {}
       _EdgeMaskTex("_EdgeMaskTex", 2D) = "white" {}
       _EdgeMaskTexCoord("_EdgeMaskTexCoord", Float) = 1
       
       _EdgeWidthNV("_EdgeWidth", Float) = 0.0
       _EdgeSharpness("_EdgeSharpness", Float) = 1.5
       _EdgeMaskBrightness("_EdgeMaskBrightness", Float) = 0.5


		
	}
	
	SubShader {
	
	     Tags
		 {
            "Queue"="Geometry"
            "IgnoreProjector"="False"
            "RenderType"="Opaque"

		 }
		Pass {
		
		    Tags { "LightMode" = "Vertex" }
		     
			ZTest LEqual Cull Back ZWrite On ColorMask RGBA
					
	    CGPROGRAM
	    
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
        
        fixed4 _LightColor0;
        
	    struct appdata {
			half4 vertex : POSITION;
			half3 normal : NORMAL;
			half4 texcoord : TEXCOORD0;
        };
        
        struct v2f {
			half4 vertex : POSITION;
			half4 uv_norm : TEXCOORD0; //xy = uv, z = dot(normalize(vNormal), normalize(-viewpos))
			half2 uv_edge : TEXCOORD1;
			fixed4 vLightColor : Color;
        };
        
        v2f vert(appdata v)
        {
			v2f o;

			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			
			o.uv_norm.xy = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord.xy );
			o.uv_edge = o.uv_norm.xy * _EdgeMaskTexCoord;
			
			half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
			o.uv_norm.z = dot(normalize(v.normal), viewDir);
			o.uv_norm.z = 1.0 - clamp( ( o.uv_norm.z - _EdgeWidthNV ) * _EdgeSharpness, 0, 1.0 );
			
			half3 lightDir = normalize(ObjSpaceLightDir(v.vertex));
			half vn = clamp(dot (v.normal, lightDir), 0, 1.0);
			o.vLightColor = UNITY_LIGHTMODEL_AMBIENT;
			o.vLightColor += half4(_LightColor0.rgb * vn, 1.0);
			
			o.vLightColor *= _Color;
			
			return o;
        }
        
	    half4 frag (v2f IN) : COLOR
	    {
			half4 output = tex2D(_MainTex, IN.uv_norm.xy) * 2 * IN.vLightColor;
			
			half EdgeMask = tex2D(_EdgeMaskTex, IN.uv_edge) * _EdgeMaskBrightness;
           
          	half EdgeMaskFacor = IN.uv_norm.z;
			output = output * clamp((1.0 - EdgeMaskFacor * EdgeMaskFacor * EdgeMask), 0 , 1.0);
			
			return half4(output.xyz, 1.0);
	    }
	    ENDCG

	}
}

Fallback Off
}