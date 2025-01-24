Shader "PostEffect/Fog"
{
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
    #include "./cginc/voronoi.cginc"
    
    CBUFFER_START(UnityPerMaterial)
        float _FogDensity;
        float _FogDistance;
        float4 _FogColor;
        float4 _AmbientColor;

        float _FogNear;
        float _FogFar;
        float _FogAltScale;
        float _FogThinning;
        float _NoiseScale;
        float _NoiseStrength;
    CBUFFER_END

    float ComputeDistance(float depth)
    {
        float dist = depth * _ProjectionParams.z;
        dist -= _ProjectionParams.y * _FogDistance;
        return dist;
    }

    half ComputeFog(float z, float density)
    {
        half fog = exp2(density * z);
        return saturate(fog);
    }

    float4 Frag(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 screenPos = input.texcoord; // UVs are actually correct here
        float2 screenParam = _ScreenParams.xy;
        float2 uv = input.texcoord;

        float4 color = SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearRepeat, uv, _BlitMipLevel);
        float4 ambientColor = _AmbientColor;
        
        float depth = SampleSceneDepth(uv);
        float dist = ComputeDistance(depth);
        float fog = 1.0 - ComputeFog(dist, _FogDensity);

        float screenNoise = cnoise(screenPos * screenParam / _NoiseScale);

        return lerp(color, _FogColor * ambientColor, saturate(fog + (screenNoise * _NoiseStrength)));
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