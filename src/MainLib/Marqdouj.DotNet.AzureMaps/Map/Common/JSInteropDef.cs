using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Common
{
    public class JSInteropDef
    {
        /// <summary>
        /// Custom Id for the object (Optional).
        /// Default is an internally generated valid Css Id.
        /// </summary>
        public string? Id { get; set; } = MapExtensions.GetRandomCssId();

        /// <summary>
        /// Used for identifying objects added to the map via JsInterop when the Id does not have a value.
        /// </summary>
        [JsonInclude]
        public string InteropId { get; internal set; } = MapExtensions.GetRandomCssId();
    }
}
