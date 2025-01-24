Shader "PostEffect/Pixelation"
{
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
    
    CBUFFER_START(UnityPerMaterial)
        float _WidthPixelation;
        float _HeightPixelation;
        float _ColorPrecision;
    CBUFFER_END

    float4 Frag(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        float2 uv = input.texcoord;

        // pixelation
        uv.x = floor(uv.x * _WidthPixelation) / _WidthPixelation;
        uv.y = floor(uv.y * _HeightPixelation) / _HeightPixelation;
            
        float4 color = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv, _BlitMipLevel);
        // color precision
        color = floor(color * _ColorPrecision)/_ColorPrecision;
        return color;
    }
    ENDHLSL

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "FogPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            ENDHLSL
        }
    }
}