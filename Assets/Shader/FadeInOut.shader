Shader "FadeInOut" {
	Properties {
	    _MainTex ("Base (RGB)", 2D) = "white" {}
		_Ratio("_Ratio", Float) = 0
	}
	
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
	    CGPROGRAM
	    #pragma vertex vert_img
	    #pragma fragment frag
	    #pragma fragmentoption ARB_precision_hint_fastest 
	    #include "UnityCG.cginc"
	
		uniform sampler2D _MainTex;
	    uniform float _Ratio;
	     
	    float4 frag (v2f_img i) : COLOR
	    {
		   float4 output = tex2D(_MainTex, i.uv);
		   output.rgb = output.rgb * _Ratio;
		   return output;
	    }
	    ENDCG

	}
}

Fallback off
}
