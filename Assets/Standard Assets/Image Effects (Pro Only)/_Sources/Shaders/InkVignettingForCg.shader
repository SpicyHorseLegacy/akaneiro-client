Shader "ImageEffects/InkVignettingForCg" {
	Properties {
	    _MainTex("_MainTex", 2D) = "white" {}
		_MinAdd("_MinAdd", Float) = 0
        _MaxMultiply("_MaxMultiply", Float) = 1
       _InkVignetteTex("_InkVignetteTex", 2D) = "white" {}
       _vignetteIntensity("_vignetteIntensity", Float) = 0

	}
	// Shader code pasted into all further CGPROGRAM blocks
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};
	
	
	
	float _MinAdd;
    float _MaxMultiply;
    sampler2D _MainTex;
    sampler2D _InkVignetteTex;
    float _vignetteIntensity;
	
		
	v2f vert( appdata_img v ) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	} 
	
	half4 frag(v2f i) : COLOR {
		
		half2 uv = i.uv;
		
		half4 Sampled2D0=tex2D(_MainTex,uv);
		
        half4 Multiply4= half4(_MaxMultiply) * Sampled2D0;
        
        half4 Add0= half4(_MinAdd) + Multiply4;
        
        half4 Sampled2D1=tex2D(_InkVignetteTex,uv);
        
        half4 Multiply0=Sampled2D1 * Add0;
        
        half4 Lerp0=lerp(Add0,Multiply0,_vignetteIntensity);
		
		return Lerp0;
		
		//return Add0;
	}

	ENDCG 
	
Subshader {
 Pass {
	  ZTest Always Cull Off ZWrite Off
	  Fog { Mode off }      

      CGPROGRAM
      #pragma fragmentoption ARB_precision_hint_fastest 
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
  }
}

Fallback off
}

