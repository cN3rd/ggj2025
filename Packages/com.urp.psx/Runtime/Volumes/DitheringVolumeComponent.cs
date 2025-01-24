using UnityEngine.Rendering;

namespace PSX
{
    public class DitheringVolumeComponent : VolumeComponent, IPostProcessComponent
    {
        public DitheringVolumeComponent() => displayName = "Dithering";

        //public TextureParameter ditherTexture;
        public IntParameter patternIndex = new(0);
        public FloatParameter ditherThreshold = new(512);
        public FloatParameter ditherStrength = new(1);
        public FloatParameter ditherScale = new(2);
        
        public bool IsActive() => patternIndex.overrideState ||
            ditherThreshold.overrideState ||
            ditherStrength.overrideState ||
            ditherScale.overrideState;
    }
}
