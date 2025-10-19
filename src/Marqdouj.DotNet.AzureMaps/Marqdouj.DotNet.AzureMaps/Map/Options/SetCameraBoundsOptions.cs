using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    /// <summary>
    /// Typescript definition: CameraBoundsOptions &amp; { pitch?: number, bearing?: number }
    /// </summary>
    public class SetCameraBoundsOptions : CameraBoundsOptions
    {
        /// <summary>
        /// <see cref="CameraOptions.Bearing"/>
        /// </summary>
        public double? Bearing { get; set; }

        /// <summary>
        /// <see cref="CameraOptions.Pitch"/>
        /// </summary>
        public double? Pitch { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
