Shader "FX/SimpleBackground+900" {
	 Properties {
	   _Color("_MainColor", Color) = (1,1,1,1)
	}
	
	SubShader {
	     Tags
		 {
            "Queue"="Geometry+900"
            "IgnoreProjector"="False"
            "RenderType"="Opaque"
		 }
		Pass {
			ZTest LEqual Cull Back ZWrite On  ColorMask RGBA
			
					
	    CGPROGRAM
	    
	    #pragma vertex vert
	    #pragma fragment frag
	    #pragma fragmentoption ARB_precision_hint_fastest
	   
	    #include "UnityCG.cginc"
	    
	    fixed4 _Color;
		
        
	    struct appdata {
			half4 vertex : POSITION;
			
        };
        
        struct v2f {
			half4 vertex : POSITION;
			
        };
        
        v2f vert(appdata v)
        {
			v2f o;
			
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			
						
			return o;
        }
        
	    half4 frag (v2f IN) : COLOR
	    {
			
			return _Color;
	    }
	    ENDCG
	  }

	}
	
	Fallback Off

}
