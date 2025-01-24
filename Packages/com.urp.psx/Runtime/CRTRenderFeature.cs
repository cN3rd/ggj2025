using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;
using Debug = System.Diagnostics.Debug;

namespace PSX
{
    public sealed class CRTRendererFeature : ScriptableRendererFeature
    {
        [SerializeField] Shader fogShader;
        Material _material;
        CRTRenderPass _pass;

        public override void Create()
        {
#if UNITY_EDITOR
            if (fogShader == null)
                fogShader = AssetDatabase.LoadAssetAtPath<Shader>("Packages/com.urp.psx/Runtime/Shaders/CRT.shader");
#endif

            _material = CoreUtils.CreateEngineMaterial(fogShader);
            _pass = new CRTRenderPass(name, _material);
        }


        public override void AddRenderPasses(ScriptableRenderer renderer,
            ref RenderingData renderingData)
        {
            if (_material == null || _pass == null)
                return;

            if (renderingData.cameraData.cameraType is CameraType.Preview or CameraType.Reflection)
                return;
            
            _pass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            _pass.ConfigureInput(ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth);

            renderer.EnqueuePass(_pass);
        }
    }

    public class CRTRenderPass : ScriptableRenderPass
    {
        static readonly int BlitTexturePropertyId = Shader.PropertyToID("_BlitTexture");

        static readonly int BlitScaleBiasPropertyId = Shader.PropertyToID("_BlitScaleBias");

        static readonly int ScanLinesWeight = Shader.PropertyToID("_ScanlinesWeight");
        static readonly int NoiseWeight = Shader.PropertyToID("_NoiseWeight");

        static readonly int ScreenBendX = Shader.PropertyToID("_ScreenBendX");
        static readonly int ScreenBendY = Shader.PropertyToID("_ScreenBendY");
        static readonly int VignetteAmount = Shader.PropertyToID("_VignetteAmount");
        static readonly int VignetteSize = Shader.PropertyToID("_VignetteSize");
        static readonly int VignetteRounding = Shader.PropertyToID("_VignetteRounding");
        static readonly int VignetteSmoothing = Shader.PropertyToID("_VignetteSmoothing");

        static readonly int ScanLinesDensity = Shader.PropertyToID("_ScanLinesDensity");
        static readonly int ScanLinesSpeed = Shader.PropertyToID("_ScanLinesSpeed");
        static readonly int NoiseAmount = Shader.PropertyToID("_NoiseAmount");

        static readonly int ChromaticRed = Shader.PropertyToID("_ChromaticRed");
        static readonly int ChromaticGreen = Shader.PropertyToID("_ChromaticGreen");
        static readonly int ChromaticBlue = Shader.PropertyToID("_ChromaticBlue");

        static readonly int GrilleOpacity = Shader.PropertyToID("_GrilleOpacity");
        static readonly int GrilleCounterOpacity = Shader.PropertyToID("_GrilleCounterOpacity");
        static readonly int GrilleResolution = Shader.PropertyToID("_GrilleResolution");
        static readonly int GrilleCounterResolution = Shader.PropertyToID("_GrilleCounterResolution");
        static readonly int GrilleBrightness = Shader.PropertyToID("_GrilleBrightness");
        static readonly int GrilleUvRotation = Shader.PropertyToID("_GrilleUvRotation");
        static readonly int GrilleUvMidPoint = Shader.PropertyToID("_GrilleUvMidPoint");
        static readonly int GrilleShift = Shader.PropertyToID("_GrilleShift");
        
        
        readonly Material _material;
        readonly MaterialPropertyBlock _propertyBlock = new();

        public CRTRenderPass(string passName, Material material)
        {
            profilingSampler = new ProfilingSampler(passName);
            _material = material;

            requiresIntermediateTexture = true;
        }

        void ExecuteMainPass(CRTPassData data, RasterGraphContext context)
        {
            RTHandle sourceTexture = data.inputTexture.IsValid() ? data.inputTexture : null;
            _propertyBlock.Clear();

            if (sourceTexture != null)
                _propertyBlock.SetTexture(BlitTexturePropertyId, sourceTexture);

            _propertyBlock.SetVector(BlitScaleBiasPropertyId, new Vector4(1, 1, 0, 0));

            var crt = VolumeManager.instance.stack?.GetComponent<CRTVolumeComponent>();
            Debug.Assert(crt != null, nameof(crt) + " != null");

            _propertyBlock.SetFloat(ScanLinesWeight, crt.scanlinesWeight.value);
            _propertyBlock.SetFloat(NoiseWeight, crt.noiseWeight.value);

            _propertyBlock.SetFloat(ScreenBendX, crt.screenBendX.value);
            _propertyBlock.SetFloat(ScreenBendY, crt.screenBendY.value);
            _propertyBlock.SetFloat(VignetteAmount, crt.vignetteAmount.value);
            _propertyBlock.SetFloat(VignetteSize, crt.vignetteSize.value);
            _propertyBlock.SetFloat(VignetteRounding, crt.vignetteRounding.value);
            _propertyBlock.SetFloat(VignetteSmoothing, crt.vignetteSmoothing.value);

            _propertyBlock.SetFloat(ScanLinesDensity, crt.scanlinesDensity.value);
            _propertyBlock.SetFloat(ScanLinesSpeed, crt.scanlinesSpeed.value);
            _propertyBlock.SetFloat(NoiseAmount, crt.noiseAmount.value);

            _propertyBlock.SetVector(ChromaticRed, crt.chromaticRed.value);
            _propertyBlock.SetVector(ChromaticGreen, crt.chromaticGreen.value);
            _propertyBlock.SetVector(ChromaticBlue, crt.chromaticBlue.value);

            _propertyBlock.SetFloat(GrilleOpacity, crt.grilleOpacity.value);
            _propertyBlock.SetFloat(GrilleCounterOpacity, crt.grilleCounterOpacity.value);
            _propertyBlock.SetFloat(GrilleResolution, crt.grilleResolution.value);
            _propertyBlock.SetFloat(GrilleCounterResolution, crt.grilleCounterResolution.value);
            _propertyBlock.SetFloat(GrilleBrightness, crt.grilleBrightness.value);
            _propertyBlock.SetFloat(GrilleUvRotation, crt.grilleUvRotation.value);
            _propertyBlock.SetFloat(GrilleUvMidPoint, crt.grilleUvMidPoint.value);
            _propertyBlock.SetVector(GrilleShift, crt.grilleShift.value);
            
            context.cmd.DrawProcedural(Matrix4x4.identity, data.material, 0, MeshTopology.Triangles, 3,
                1, _propertyBlock);
        }


        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourcesData = frameData.Get<UniversalResourceData>();
            var cameraData = frameData.Get<UniversalCameraData>();
            
            var fog = VolumeManager.instance.stack?.GetComponent<CRTVolumeComponent>();

            var vols = VolumeManager.instance.GetVolumes(cameraData.volumeLayerMask);
            
            if (fog == null || !fog.IsActive())
                return;

            
            using var builder =
                renderGraph.AddRasterRenderPass<CRTPassData>(passName, out var passData,
                    profilingSampler);
            
            passData.material = _material;

            var cameraColorDesc = renderGraph.GetTextureDesc(resourcesData.cameraColor);
            cameraColorDesc.name = "_CameraColorCustomPostProcessing";
            cameraColorDesc.clearBuffer = false;

            var destination = renderGraph.CreateTexture(cameraColorDesc);
            passData.inputTexture = resourcesData.cameraColor;

            builder.UseTexture(passData.inputTexture);
            builder.SetRenderAttachment(destination, 0);

            builder.SetRenderFunc((CRTPassData data, RasterGraphContext context) =>
                ExecuteMainPass(data, context));

            resourcesData.cameraColor = destination;
        }


        class CRTPassData
        {
            public TextureHandle inputTexture;
            public Material material;
        }
    }
}
