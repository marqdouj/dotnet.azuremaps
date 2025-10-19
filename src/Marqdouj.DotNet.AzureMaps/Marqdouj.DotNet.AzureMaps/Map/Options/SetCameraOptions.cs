namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    public class SetCameraOptions
    {
        public SetCameraOptions() { }

        public SetCameraOptions(CameraOptions? camera, SetCameraBoundsOptions? cameraBounds = null, AnimationOptions? animation = null)
        {
            Camera = camera;
            CameraBounds = cameraBounds;
            Animation = animation;
        }

        public AnimationOptions? Animation { get; set; }
        public CameraOptions? Camera { get; set; }
        public SetCameraBoundsOptions? CameraBounds { get; set; }

    }
}
