using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;
using Debug = System.Diagnostics.Debug;

namespace PSX
{
    public sealed class DitheringRendererFeature : ScriptableRendererFeature
    {
        [SerializeField] Shader ditheringShader;
        Material _material;
        DitheringRenderPass _pass;

        public override void Create()
        {
#if UNITY_EDITOR
            if (ditheringShader == null)
                ditheringShader = AssetDatabase.LoadAssetAtPath<Shader>("Packages/com.urp.psx/Runtime/Shaders/Dithering.shader");
#endif

            _material = CoreUtils.CreateEngineMaterial(ditheringShader);
            _pass = new DitheringRenderPass(name, _material);
        }


        public override void AddRenderPasses(ScriptableRenderer renderer,
            ref RenderingData renderingData)
        {
            if (_material == null || _pass == null)
                return;

            if (renderingData.cameraData.cameraType is CameraType.Preview or CameraType.Reflection)
                return;

            var dithering = VolumeManager.instance.stack?.GetComponent<DitheringVolumeComponent>();

            if (dithering == null || !dithering.IsActive())
                return;

            _pass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            _pass.ConfigureInput(ScriptableRenderPassInput.Color);

            renderer.EnqueuePass(_pass);
        }
    }

    public class DitheringRenderPass : ScriptableRenderPass
    {
        static readonly int BlitTexturePropertyId = Shader.PropertyToID("_BlitTexture");

        static readonly int BlitScaleBiasPropertyId = Shader.PropertyToID("_BlitScaleBias");

        static readonly int PatternIndex = Shader.PropertyToID("_PatternIndex");
        static readonly int DitherThreshold = Shader.PropertyToID("_DitherThreshold");
        static readonly int DitherStrength = Shader.PropertyToID("_DitherStrength");
        static readonly int DitherScale = Shader.PropertyToID("_DitherScale");
        
        readonly Material _material;
        readonly MaterialPropertyBlock _propertyBlock = new();

        public DitheringRenderPass(string passName, Material material)
        {
            profilingSampler = new ProfilingSampler(passName);
            _material = material;

            requiresIntermediateTexture = true;
        }

        void ExecuteMainPass(DitheringPassData data, RasterGraphContext context)
        {
            RTHandle sourceTexture = data.inputTexture.IsValid() ? data.inputTexture : null;
            _propertyBlock.Clear();

            if (sourceTexture != null)
                _propertyBlock.SetTexture(BlitTexturePropertyId, sourceTexture);

            _propertyBlock.SetVector(BlitScaleBiasPropertyId, new Vector4(1, 1, 0, 0));

            var dithering = VolumeManager.instance.stack?.GetComponent<DitheringVolumeComponent>();
            Debug.Assert(dithering != null, nameof(dithering) + " != null");

            _propertyBlock.SetInt(PatternIndex, dithering.patternIndex.value);
            _propertyBlock.SetFloat(DitherThreshold, dithering.ditherThreshold.value);
            _propertyBlock.SetFloat(DitherStrength, dithering.ditherStrength.value);
            _propertyBlock.SetFloat(DitherScale, dithering.ditherScale.value);

            context.cmd.DrawProcedural(Matrix4x4.identity, data.material, 0, MeshTopology.Triangles, 3,
                1, _propertyBlock);
        }


        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourcesData = frameData.Get<UniversalResourceData>();
            using var builder =
                renderGraph.AddRasterRenderPass<DitheringPassData>(passName, out var passData,
                    profilingSampler);
            
            passData.material = _material;

            var cameraColorDesc = renderGraph.GetTextureDesc(resourcesData.cameraColor);
            cameraColorDesc.name = "_CameraColorCustomPostProcessing";
            cameraColorDesc.clearBuffer = false;

            var destination = renderGraph.CreateTexture(cameraColorDesc);
            passData.inputTexture = resourcesData.cameraColor;

            builder.UseTexture(passData.inputTexture);
            builder.SetRenderAttachment(destination, 0);

            builder.SetRenderFunc((DitheringPassData data, RasterGraphContext context) =>
                ExecuteMainPass(data, context));

            resourcesData.cameraColor = destination;
        }


        class DitheringPassData
        {
            public TextureHandle inputTexture;
            public Material material;
        }
    }
}