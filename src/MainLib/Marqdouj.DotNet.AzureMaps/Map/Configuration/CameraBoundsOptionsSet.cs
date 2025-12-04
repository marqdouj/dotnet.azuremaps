using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Configuration
{
    /// <summary>
    /// Used when setting camera bounds.
    /// Typescript definition: CameraBoundsOptions &amp; { pitch?: number, bearing?: number }
    /// </summary>
    public class CameraBoundsOptionsSet : CameraBoundsOptions
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
