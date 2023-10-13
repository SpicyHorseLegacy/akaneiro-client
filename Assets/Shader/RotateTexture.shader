Shader "FX/RotateTexture" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white"
        //_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Pass {
            AlphaTest Greater 0.5
            SetTexture [_MainTex] {
                Matrix [_Matrix]
            }
        }
    }
}