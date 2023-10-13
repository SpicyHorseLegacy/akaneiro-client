Shader "Hidden/Show UV Mapping" {
Category {
	Fog { Mode Off }
	
SubShader {
	Tags { "RenderType"="Opaque" }
	Pass {		
CGPROGRAM
#pragma exclude_renderers gles
#pragma vertex vert
#include "UnityCG.cginc"

struct v2f {
	float4 pos : POSITION;
	float4 uv : TEXCOORD0;
};
v2f vert( appdata_base v ) {
	v2f o;
	o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
	o.uv.xy = v.texcoord.xy * 4.0;
	o.uv.zw = float2(0,1);
	return o;
}
ENDCG
		SetTexture[_CheckerTex] { combine texture }
	}
}

SubShader {
	Tags { "RenderType"="Transparent" }
	Pass {
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
CGPROGRAM
#pragma exclude_renderers gles
#pragma vertex vert
#include "UnityCG.cginc"

struct v2f {
	float4 pos : POSITION;
	float4 uv : TEXCOORD0;
};
v2f vert( appdata_base v ) {
	v2f o;
	o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
	o.uv.xy = v.texcoord.xy * 4.0;
	o.uv.zw = float2(0,1);
	return o;
}
ENDCG
		SetTexture[_CheckerTex] { constantColor(0.5,0.5,0.5,0.5) combine texture, constant }
	}
}

}
}
