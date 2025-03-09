Shader "Custom/DepthMaskMR"
{
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Opaque" }
        ZWrite On
        ColorMask 0 // Don't write color, only depth
        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
            };
            
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }
            
            half4 frag(Varyings IN) : SV_Target
            {
                discard; // Make it invisible but keep depth
                return half4(0, 0, 0, 0);
            }
            
            ENDHLSL
        }
    }
}
