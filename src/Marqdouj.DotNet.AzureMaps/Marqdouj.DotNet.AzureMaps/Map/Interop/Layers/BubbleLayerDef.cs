using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop.Layers
{
    public class BubbleLayerDef : MapLayerDef, ICloneable
    {
        [JsonIgnore]
        public override MapLayerType Type => MapLayerType.Bubble;

        public BubbleLayerOptions? Options { get; set; }

        public object Clone()
        {
            var clone = (BubbleLayerDef)MemberwiseClone();
            clone.Options = (BubbleLayerOptions?)Options?.Clone();

            return clone;
        }
    }

    public enum BubbleLayerPitchAlignment
    {
        Viewport,
        Map,
    }

    public class BubbleLayerOptions : SourceLayerOptions, ICloneable
    {
        /// <summary>
        /// The color to fill the circle symbol with.
        /// Default "#1A73AA" (Dark Blue).
        /// </summary>
        public string? Color { get; set; } = "#1A73AA";

        /// <summary>
        /// The amount to blur the circles.
        /// A value of 1 blurs the circles such that only the center point if at full opacity.
        /// Default '0'.
        /// </summary>
        public double? Blur { get; set; } = 0;

        /// <summary>
        /// A number between 0 and 1 that indicates the opacity at which the circles will be drawn.
        /// Default '1'.
        /// </summary>
        public double? Opacity { get; set; } = 1;

        /// <summary>
        /// The color of the circles' outlines.
        /// Default '#FFFFFF'.
        /// </summary>
        public string? StrokeColor { get; set; } = "#FFFFFF";

        /// <summary>
        /// A number between 0 and 1 that indicates the opacity at which the circles' outlines will be drawn.
        /// Default '1'.
        /// </summary>
        public double? StrokeOpacity { get; set; } = 1;

        /// <summary>
        /// The width of the circles' outlines in pixels.
        /// Default '2'.
        /// </summary>
        public double? StrokeWidth { get; set; } = 2;

        /// <summary>
        /// Specifies the orientation of circle when map is pitched.
        /// "map": The circle is aligned to the plane of the map.
        /// "viewport": The circle is aligned to the plane of the viewport.
        /// Default 'viewport'
        /// </summary>
        [JsonIgnore]
        public BubbleLayerPitchAlignment? PitchAlignment { get; set; }

        [JsonInclude]
        [JsonPropertyName("pitchAlignment")]
        internal string? PitchAlignmentJs { get => PitchAlignment.EnumToJsonN(); set => PitchAlignment = value.JsonToEnumN<BubbleLayerPitchAlignment>(); }

        /// <summary>
        /// The radius of the circle symbols in pixels.
        /// Must be greater than or equal to 0.
        /// Default '8'.
        /// </summary>
        public double? Radius { get; set; } = 8;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
