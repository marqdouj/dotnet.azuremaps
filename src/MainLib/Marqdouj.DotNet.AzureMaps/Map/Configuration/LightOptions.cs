using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Configuration
{
    /// <summary>
    /// The options for the map's lighting.
    /// </summary>
    public class LightOptions : ICloneable
    {
        /// <summary>
        /// Specifies whether extruded geometries are lit relative to the map or viewport.
        /// Supported values:
        /// "map": The position of the light source is aligned to the rotation of the map.
        /// "viewport": The position fo the light source is aligned to the rotation of the viewport.
        /// Default: 'map'.
        /// </summary>
        [JsonIgnore]
        public LightAnchorType? Anchor { get; set; }

        [JsonInclude]
        [JsonPropertyName("anchor")]
        internal string? AnchorJs { get => Anchor.EnumToJsonN(); set => Anchor = value.JsonToEnumN<LightAnchorType>(); }

        /// <summary>
        /// Color tint for lighting extruded geometries.
        /// Default: '#FFFFFF'.
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// Intensity of lighting (on a scale from 0 to 1).
        /// Higher numbers will present as more extreme contrast.
        /// Default '0.5'.
        /// </summary>
        public double? Intensity { get; set; }

        /// <summary>
        /// <see cref="LightPosition"/>
        /// </summary>
        public LightPosition? Position { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public object Clone()
        {
            var clone = (LightOptions)MemberwiseClone();
            clone.Position = (LightPosition?)Position?.Clone();

            return clone;
        }
    }
}
