Shader "ImageEffects/InkVignettingForCg_NV" {
	Properties {
	    _MainTex("_MainTex", 2D) = "white" {}
        _MaxMultiply("_MaxMultiply", Float) = 1
        _MinAdd("_MinAdd", Float) = 1
       _InkVignetteTex("_InkVignetteTex", 2D) = "white" {}

	}
	// Shader code pasted into all further CGPROGRAM blocks
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};
	
    float _MaxMultiply;
    float _MinAdd;
    sampler2D _MainTex;
    sampler2D _InkVignetteTex;
		
	v2f vert( appdata_img v ) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	} 
	
	half4 frag(v2f i) : COLOR {
		return tex2D(_MainTex, i.uv) * tex2D(_InkVignetteTex, i.uv) * _MaxMultiply + _MinAdd;
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

