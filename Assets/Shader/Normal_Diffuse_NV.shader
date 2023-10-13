Shader "Custom/Normal_Diffuse_NV" {
	Properties {
	   _Color("_MainColor", Color) = (1,1,1,1)
       _MainTex("_MainTex", 2D) = "white" {}
	}
	
	SubShader {
	     Tags
		 {
            "Queue"="Geometry"
            "IgnoreProjector"="False"
            "RenderType"="Opaque"
		 }
		Pass {
			ZTest LEqual Cull Back ZWrite On ColorMask RGBA
			
					
	    CGPROGRAM
	    
	    #pragma vertex vert
	    #pragma fragment frag
	    #pragma fragmentoption ARB_precision_hint_fastest
	   
	    #include "UnityCG.cginc"
	    
	    fixed4 _Color;
		sampler2D _MainTex;
        
        fixed4 _LightColor0;
        
	    struct appdata {
			half4 vertex : POSITION;
			half3 normal : NORMAL;
			half4 texcoord : TEXCOORD0;
        };
        
        struct v2f {
			half4 vertex : POSITION;
			half2 uv : TEXCOORD0;
			fixed4 vLightColor : Color;
        };
        
        v2f vert(appdata v)
        {
			v2f o;
			// object space view direction
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = v.texcoord.xy;

			half3 mLightColor = ShadeVertexLights(v.vertex,v.normal);
		    
		    o.vLightColor = half4(mLightColor.rgb,1.0) * _Color;
						
			return o;
        }
        
	    half4 frag (v2f IN) : COLOR
	    {
			half4 output = tex2D(_MainTex, IN.uv) * 2 * IN.vLightColor;
			return half4(output.xyz, 1);
	    }
	    ENDCG

	}
}

Fallback Off
}