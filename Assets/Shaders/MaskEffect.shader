Shader "MaskEffect"
{
    SubShader
    {
        Tags { "Queue" = "Transparent" }

        Pass
        {
            Fog { Mode Off }
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // 顶点输入结构
            struct appdata
            {
                float4 vertex : POSITION0;
                float4 color : COLOR;
            };

            // 顶点输出结构
            struct v2f
            {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
            };

            // vertex
            v2f vert( appdata IN )
            {
                v2f OUT;
                OUT.pos = mul( UNITY_MATRIX_MVP, IN.vertex );
                OUT.color = IN.color;

                return OUT;
            }

            // fragment
            half4 frag( v2f IN ) : COLOR
            {
                half4 c = half4( IN.color.r, IN.color.g, IN.color.b, IN.color.a );

                return c;
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}