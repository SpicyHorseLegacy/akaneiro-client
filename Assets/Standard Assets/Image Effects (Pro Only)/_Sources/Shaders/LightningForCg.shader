Shader "ImageEffects/LightningForCg" {
	Properties {
	    _MainTex("_MainTex", 2D) = "white" {}
        _LightningMask("_LightningMask", 2D) = "white" {}
        _SecTexCoord("_SecTexCoord", Vector) = (1,1,0,0)
        _Speed("_Speed", Float) = 1
        _Color("_Color", Color) = (1,1,1,1)
        _Brightness("_Brightness", Float) = 1

	}
	// Shader code pasted into all further CGPROGRAM blocks
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};
	
	
	
    sampler2D _MainTex;
    sampler2D _LightningMask;
    float4 _SecTexCoord;
    float _Speed;
    float4 _Color;
    float _Brightness;
	
		
	v2f vert( appdata_img v ) {
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	} 
	
	half4 frag(v2f i) : COLOR {
	
		half4 Sampled2D0=tex2D(_MainTex,i.uv);
		half2 Multiply1 = i.uv * _SecTexCoord.xy;
		half2 Multiply2 = _Time * half2(_Speed);
		half2 UV_Pan0 = half2(Multiply1.x + Multiply2.y,Multiply1.y + Multiply2.y);
	    UV_Pan0 = frac(UV_Pan0);
		half4 Tex2D0 = tex2D(_LightningMask,UV_Pan0);
		half4 Multiply5 = _Color * half4(_Brightness);
		Multiply5 = Multiply5 * Tex2D0;
		Multiply5 = Multiply5 * Sampled2D0;
		Multiply5 = Multiply5 + Sampled2D0;
		
		return Multiply5;	
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

