// ------------------------------------------------------------------------------
// COPYRIGHT (C) 2012 NVIDIA CORPORATION. ALL RIGHTS RESERVED.
// ------------------------------------------------------------------------------

Shader "Custom/RenderShadow" {
	SubShader {
		Tags { "RenderType"="Opaque" }
	
       Pass { 
        	Fog { Mode Off }
        	Color (1,0,0,0) 
        }
   }
  
  	SubShader {
		Tags { "RenderType"="TransparentCutout" }
	
        Pass { 
            Fog { Mode off }
            
            CGPROGRAM
            
            #pragma vertex vert
	        #pragma fragment frag
	   
	        #include "UnityCG.cginc"
	        
	        struct appdata {
			   half4 vertex : POSITION;
			   half3 normal : NORMAL;
			   half4 texcoord : TEXCOORD0;
             };
        
            struct v2f {
			   half4 vertex : POSITION;
			   half4 uv_norm : TEXCOORD0;
            };
	    
            fixed _Cutoff;
            sampler2D _MainTex;
            
            v2f vert(appdata v)
            {
			  v2f o;
			  o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			  o.uv_norm.xy = v.texcoord;
			  return o;
            }
            
            fixed4 frag (v2f IN) : COLOR
	        {
			  half4 TexClr = tex2D(_MainTex, IN.uv_norm.xy);
			  clip(TexClr.w - _Cutoff);
			  return fixed4(1,0,0,0);
			}

            ENDCG
        }
    }
    
    Fallback Off
}
