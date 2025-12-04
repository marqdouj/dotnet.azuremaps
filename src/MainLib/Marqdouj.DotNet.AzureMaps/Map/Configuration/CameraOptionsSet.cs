using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Configuration
{
    /// <summary>
    /// Used to set the map control's camera options.
    /// </summary>
    public class CameraOptionsSet
    {
        [JsonConstructor]
        public CameraOptionsSet() { }

        public CameraOptionsSet(CameraOptions? camera, CameraBoundsOptionsSet? cameraBounds = null, AnimationOptions? animation = null)
        {
            Camera = camera;
            CameraBounds = cameraBounds;
            Animation = animation;
        }

        public AnimationOptions? Animation { get; set; }
        public CameraOptions? Camera { get; set; }
        public CameraBoundsOptionsSet? CameraBounds { get; set; }
    }
}
