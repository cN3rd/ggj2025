using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;
using Debug = System.Diagnostics.Debug;

namespace PSX
{
    public sealed class FogRendererFeature : ScriptableRendererFeature
    {
        [SerializeField] Shader fogShader;
        Material _material;
        FogRenderPass _pass;

        public override void Create()
        {
#if UNITY_EDITOR
            if (fogShader == null)
                fogShader = AssetDatabase.LoadAssetAtPath<Shader>("Packages/com.urp.psx/Runtime/Shaders/Fog.shader");
#endif

            _material = CoreUtils.CreateEngineMaterial(fogShader);
            _pass = new FogRenderPass(name, _material);
        }


        public override void AddRenderPasses(ScriptableRenderer renderer,
            ref RenderingData renderingData)
        {
            if (_material == null || _pass == null)
                return;

            if (renderingData.cameraData.cameraType is CameraType.Preview or CameraType.Reflection)
                return;

            var fog = VolumeManager.instance.stack?.GetComponent<FogVolumeComponent>();

            if (fog == null || !fog.IsActive())
                return;

            _pass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            _pass.ConfigureInput(ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth);

            renderer.EnqueuePass(_pass);
        }
    }

    public class FogRenderPass : ScriptableRenderPass
    {
        static readonly int BlitTexturePropertyId = Shader.PropertyToID("_BlitTexture");

        static readonly int BlitScaleBiasPropertyId = Shader.PropertyToID("_BlitScaleBias");

        static readonly int FogDensity = Shader.PropertyToID("_FogDensity");
        static readonly int FogDistance = Shader.PropertyToID("_FogDistance");
        static readonly int FogColor = Shader.PropertyToID("_FogColor");
        static readonly int AmbientColor = Shader.PropertyToID("_AmbientColor");
        static readonly int FogNear = Shader.PropertyToID("_FogNear");
        static readonly int FogFar = Shader.PropertyToID("_FogFar");
        static readonly int FogAltScale = Shader.PropertyToID("_FogAltScale");
        static readonly int FogThinning = Shader.PropertyToID("_FogThinning");
        static readonly int NoiseScale = Shader.PropertyToID("_NoiseScale");
        static readonly int NoiseStrength = Shader.PropertyToID("_NoiseStrength");

        readonly Material _material;
        readonly MaterialPropertyBlock _propertyBlock = new();

        public FogRenderPass(string passName, Material material)
        {
            profilingSampler = new ProfilingSampler(passName);
            _material = material;

            requiresIntermediateTexture = true;
        }

        void ExecuteMainPass(FogPassData data, RasterGraphContext context)
        {
            RTHandle sourceTexture = data.inputTexture.IsValid() ? data.inputTexture : null;
            _propertyBlock.Clear();

            if (sourceTexture != null)
                _propertyBlock.SetTexture(BlitTexturePropertyId, sourceTexture);

            _propertyBlock.SetVector(BlitScaleBiasPropertyId, new Vector4(1, 1, 0, 0));

            var fog = VolumeManager.instance.stack?.GetComponent<FogVolumeComponent>();
            Debug.Assert(fog != null, nameof(fog) + " != null");

            _propertyBlock.SetFloat(FogDensity, fog.fogDensity.value);
            _propertyBlock.SetFloat(FogDistance, fog.fogDistance.value);
            _propertyBlock.SetColor(FogColor, fog.fogColor.value);
            _propertyBlock.SetColor(AmbientColor, fog.ambientColor.value);
            _propertyBlock.SetFloat(FogNear, fog.fogNear.value);
            _propertyBlock.SetFloat(FogFar, fog.fogFar.value);
            _propertyBlock.SetFloat(FogAltScale, fog.fogAltScale.value);
            _propertyBlock.SetFloat(FogThinning, fog.fogThinning.value);
            _propertyBlock.SetFloat(NoiseScale, fog.noiseScale.value);
            _propertyBlock.SetFloat(NoiseStrength, fog.noiseStrength.value);

            context.cmd.DrawProcedural(Matrix4x4.identity, data.material, 0, MeshTopology.Triangles, 3,
                1, _propertyBlock);
        }


        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourcesData = frameData.Get<UniversalResourceData>();
            using var builder =
                renderGraph.AddRasterRenderPass<FogPassData>(passName, out var passData,
                    profilingSampler);
            
            passData.material = _material;

            var cameraColorDesc = renderGraph.GetTextureDesc(resourcesData.cameraColor);
            cameraColorDesc.name = "_CameraColorCustomPostProcessing";
            cameraColorDesc.clearBuffer = false;

            var destination = renderGraph.CreateTexture(cameraColorDesc);
            passData.inputTexture = resourcesData.cameraColor;

            builder.UseTexture(passData.inputTexture);
            builder.SetRenderAttachment(destination, 0);

            builder.SetRenderFunc((FogPassData data, RasterGraphContext context) =>
                ExecuteMainPass(data, context));

            resourcesData.cameraColor = destination;
        }


        class FogPassData
        {
            public TextureHandle inputTexture;
            public Material material;
        }
    }
}