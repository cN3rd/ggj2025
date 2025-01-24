using UnityEngine.Rendering;

namespace PSX
{
    public class PixelationVolumeComponent : VolumeComponent, IPostProcessComponent
    {
        public PixelationVolumeComponent() => displayName = "Pixelation";

        public FloatParameter widthPixelation = new(512);
        public FloatParameter heightPixelation = new(512);
        public FloatParameter colorPrecision = new(32.0f);

        public bool IsActive() => widthPixelation.overrideState ||
                                  heightPixelation.overrideState ||
                                  colorPrecision.overrideState;
    }
}
