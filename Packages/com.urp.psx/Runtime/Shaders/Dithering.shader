Shader "PostEffect/Dithering"
{
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
    #include "cginc/DitheringPatterns.cginc"

    CBUFFER_START(UnityPerMaterial)
        uint  _PatternIndex;
        float _DitherThreshold;
        float _DitherStrength;
        float _DitherScale;
    CBUFFER_END

    float PixelBrightness(float3 col)
    {
        return col.r + col.g + col.b / 3.;
    }

    float4 GetTexelSize(float width, float height)
    {
        return float4(1 / width, 1 / height, width, height);
    }

    float Get4x4TexValue(float2 uv, float brightness, float4x4 pattern)
    {
        uint x = uv.x % 4;
        uint y = uv.y % 4;

        if((brightness * _DitherThreshold) < pattern[x][y]) return 0;
        return 1;
    }

    float4 Frag(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = input.texcoord;
        float2 screenPos = input.texcoord;
        float4 color = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv, _BlitMipLevel);

        uint2 ditherCoordinate = screenPos * _ScreenParams.xy;
        ditherCoordinate /= _DitherScale;
        
        float brightness = PixelBrightness(color.rgb);
        float ditherPixel = Get4x4TexValue(ditherCoordinate, brightness, ditherPatterns[_PatternIndex]);
        
        return color * ditherPixel;
    }
    ENDHLSL

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
        }
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