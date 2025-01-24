using UnityEngine;
using UnityEngine.Rendering;

namespace PSX
{
    public class FogVolumeComponent : VolumeComponent, IPostProcessComponent
    {
        public FogVolumeComponent() => displayName = "Fog";

        [Range(0, 10)] public FloatParameter fogDensity = new(1.0f);

        [Range(0, 100)] public FloatParameter fogDistance = new(10.0f);

        public ColorParameter fogColor = new(Color.white);
        public ColorParameter ambientColor = new(new Color(0.1f, 0.1f, 0.1f, 0.1f));

        [Range(0, 100)] public FloatParameter fogNear = new(1.0f);

        [Range(0, 100)] public FloatParameter fogFar = new(100.0f);

        [Range(0, 100)] public FloatParameter fogAltScale = new(10.0f);

        [Range(0, 1000)] public FloatParameter fogThinning = new(100.0f);

        [Range(0, 1000)] public FloatParameter noiseScale = new(100.0f);

        [Range(0, 1)] public FloatParameter noiseStrength = new(0.05f);

        public bool IsActive() =>  fogDensity.overrideState || 
                                   fogDistance.overrideState || 
                                   fogColor.overrideState || 
                                   ambientColor.overrideState || 
                                   fogNear.overrideState || 
                                   fogFar.overrideState || 
                                   fogAltScale.overrideState || 
                                   fogThinning.overrideState || 
                                   noiseScale.overrideState || 
                                   noiseStrength.overrideState;
    }
}
