using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;
using Debug = System.Diagnostics.Debug;

namespace PSX
{
    public sealed class PixelationRendererFeature : ScriptableRendererFeature
    {
        [SerializeField] Shader pixelationShader;
        Material _material;
        PixelationRenderPass _pass;

        public override void Create()
        {
#if UNITY_EDITOR
            if (pixelationShader == null)
                pixelationShader = AssetDatabase.LoadAssetAtPath<Shader>("Packages/com.urp.psx/Runtime/Shaders/Pixelation.shader");
#endif

            _material = CoreUtils.CreateEngineMaterial(pixelationShader);
            _pass = new PixelationRenderPass(name, _material);
        }


        public override void AddRenderPasses(ScriptableRenderer renderer,
            ref RenderingData renderingData)
        {
            if (_material == null || _pass == null)
                return;

            if (renderingData.cameraData.cameraType is CameraType.Preview or CameraType.Reflection)
                return;

            var pixelation = VolumeManager.instance.stack?.GetComponent<PixelationVolumeComponent>();

            if (pixelation == null || !pixelation.IsActive())
                return;

            _pass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            _pass.ConfigureInput(ScriptableRenderPassInput.Color);

            renderer.EnqueuePass(_pass);
        }
    }

    public class PixelationRenderPass : ScriptableRenderPass
    {
        static readonly int BlitTexturePropertyId = Shader.PropertyToID("_BlitTexture");
        static readonly int BlitScaleBiasPropertyId = Shader.PropertyToID("_BlitScaleBias");
        
        static readonly int WidthPixelation = Shader.PropertyToID("_WidthPixelation");
        static readonly int HeightPixelation = Shader.PropertyToID("_HeightPixelation");
        static readonly int ColorPrecision = Shader.PropertyToID("_ColorPrecision");

        readonly Material _material;
        readonly MaterialPropertyBlock _propertyBlock = new();

        public PixelationRenderPass(string passName, Material material)
        {
            profilingSampler = new ProfilingSampler(passName);
            _material = material;

            requiresIntermediateTexture = true;
        }

        void ExecuteMainPass(PixelationPassData data, RasterGraphContext context)
        {
            RTHandle sourceTexture = data.inputTexture.IsValid() ? data.inputTexture : null;
            _propertyBlock.Clear();

            if (sourceTexture != null)
                _propertyBlock.SetTexture(BlitTexturePropertyId, sourceTexture);

            _propertyBlock.SetVector(BlitScaleBiasPropertyId, new Vector4(1, 1, 0, 0));

            var pixelation = VolumeManager.instance.stack?.GetComponent<PixelationVolumeComponent>();
            Debug.Assert(pixelation != null, nameof(pixelation) + " != null");
            
            _propertyBlock.SetFloat(WidthPixelation, pixelation.widthPixelation.value);
            _propertyBlock.SetFloat(HeightPixelation, pixelation.heightPixelation.value);
            _propertyBlock.SetFloat(ColorPrecision, pixelation.colorPrecision.value);

            context.cmd.DrawProcedural(Matrix4x4.identity, data.material, 0, MeshTopology.Triangles, 3,
                1, _propertyBlock);
        }


        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourcesData = frameData.Get<UniversalResourceData>();
            using var builder =
                renderGraph.AddRasterRenderPass<PixelationPassData>(passName, out var passData,
                    profilingSampler);
            
            passData.material = _material;

            var cameraColorDesc = renderGraph.GetTextureDesc(resourcesData.cameraColor);
            cameraColorDesc.name = "_CameraColorCustomPostProcessing";
            cameraColorDesc.clearBuffer = false;

            var destination = renderGraph.CreateTexture(cameraColorDesc);
            passData.inputTexture = resourcesData.cameraColor;

            builder.UseTexture(passData.inputTexture);
            builder.SetRenderAttachment(destination, 0);

            builder.SetRenderFunc((PixelationPassData data, RasterGraphContext context) =>
                ExecuteMainPass(data, context));

            resourcesData.cameraColor = destination;
        }


        class PixelationPassData
        {
            public TextureHandle inputTexture;
            public Material material;
        }
    }
}