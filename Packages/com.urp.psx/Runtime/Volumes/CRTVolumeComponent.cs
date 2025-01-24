using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PSX
{
    [VolumeRequiresRendererFeatures(typeof(CRTRendererFeature))]
    [SupportedOnRenderer(typeof(UniversalRendererData))]
    public class CRTVolumeComponent : VolumeComponent, IPostProcessComponent
    {
        public FloatParameter scanlinesWeight = new(1f);
        public FloatParameter noiseWeight = new(1f);

        public FloatParameter screenBendX = new(1000.0f);
        public FloatParameter screenBendY = new(1000.0f);
        public FloatParameter vignetteAmount = new(0.0f);
        public FloatParameter vignetteSize = new(2.0f);
        public FloatParameter vignetteRounding = new(2.0f);
        public FloatParameter vignetteSmoothing = new(1.0f);

        public FloatParameter scanlinesDensity = new(200.0f);
        public FloatParameter scanlinesSpeed = new(-10.0f);
        public FloatParameter noiseAmount = new(250.0f);

        public Vector2Parameter chromaticRed = new(new Vector2());
        public Vector2Parameter chromaticGreen = new(new Vector2());
        public Vector2Parameter chromaticBlue = new(new Vector2());

        // Grille Effect is a modified version of the shader from here:
        // https://godotshaders.com/shader/vhs-and-crt-monitor-effect/
        public FloatParameter grilleOpacity = new(0.4f);
        public FloatParameter grilleCounterOpacity = new(0.2f);
        public FloatParameter grilleResolution = new(360.0f);
        public FloatParameter grilleCounterResolution = new(540.0f);
        public FloatParameter grilleUvRotation = new(90.0f);
        public FloatParameter grilleBrightness = new(15.0f);
        public FloatParameter grilleUvMidPoint = new(0.5f);
        public Vector3Parameter grilleShift = new(new Vector3(1.0f, 1.0f, 1.0f));
        public CRTVolumeComponent() => displayName = "CRT";

        public bool IsActive() => scanlinesWeight.overrideState ||
                                  noiseWeight.overrideState ||
                                  screenBendX.overrideState ||
                                  screenBendY.overrideState ||
                                  vignetteAmount.overrideState ||
                                  vignetteSize.overrideState ||
                                  vignetteRounding.overrideState ||
                                  vignetteSmoothing.overrideState ||
                                  scanlinesDensity.overrideState ||
                                  scanlinesSpeed.overrideState ||
                                  noiseAmount.overrideState ||
                                  chromaticRed.overrideState ||
                                  chromaticGreen.overrideState ||
                                  chromaticBlue.overrideState ||
                                  grilleOpacity.overrideState ||
                                  grilleCounterOpacity.overrideState ||
                                  grilleResolution.overrideState ||
                                  grilleCounterResolution.overrideState ||
                                  grilleUvRotation.overrideState ||
                                  grilleBrightness.overrideState ||
                                  grilleUvMidPoint.overrideState ||
                                  grilleShift.overrideState;
    }
}
