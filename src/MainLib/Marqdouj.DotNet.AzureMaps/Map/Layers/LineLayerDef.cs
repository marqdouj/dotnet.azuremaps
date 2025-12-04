using Marqdouj.DotNet.AzureMaps.Map.Common;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Layers
{
    public class LineLayerDef : MapLayerDef, ICloneable
    {
        [JsonIgnore]
        public override MapLayerType LayerType => MapLayerType.Line;
        public LineLayerOptions? Options { get; set; } = new();

        public object Clone()
        {
            var clone = (LineLayerDef)MemberwiseClone();
            clone.Options = (LineLayerOptions?)Options?.Clone();
            clone.DataSource = (DataSourceDef)(clone.DataSource.Clone());

            return clone;
        }
    }

    public enum LineCap
    {
        Butt,
        Round,
        Square,
    }

    public enum LineJoin
    {
        Bevel,
        Round,
        Miter,
    }

    public class LineLayerOptions : SourceLayerOptions, ICloneable
    {
        /// <summary>
        /// The amount of blur to apply to the line in pixels.
        /// Default '0'.
        /// </summary>
        public double? Blur { get; set; }

        /// <summary>
        /// Specifies how the ends of the lines are rendered.
        /// "butt": A cap with a squared-off end which is drawn to the exact endpoint of the line.
        /// "round": A cap with a rounded end which is drawn beyond the endpoint of the line
        /// at a radius of one-half of the lines width and centered on the endpoint of the line.
        /// "square": A cap with a squared-off end which is drawn beyond the endpoint of the line
        /// at a distance of one-half of the line width.
        /// Default 'round'.
        /// </summary>
        [JsonIgnore] public LineCap? LineCap { get; set; }

        [JsonInclude]
        [JsonPropertyName("lineCap")]
        internal string? LineCapJs { get => LineCap.EnumToJsonN(); set => LineCap = value.JsonToEnumN<LineCap>(); }

        /// <summary>
        /// Specifies how the joints in the lines are rendered.
        /// "bevel": A join with a squared-off end which is drawn beyond the endpoint of the line
        /// at a distance of one-half of the lines width.
        /// "round": A join with a rounded end which is drawn beyond the endpoint of the line
        /// at a radius of one-half of the lines width and centered on the endpoint of the line.
        /// "miter": A join with a sharp, angled corner which is drawn with the outer sides
        /// beyond the endpoint of the path until they meet.
        /// Default 'round'.
        /// </summary>
        [JsonIgnore] public LineJoin? LineJoin { get; set; }

        [JsonInclude]
        [JsonPropertyName("lineJoin")]
        internal string? LineJoinJs { get => LineJoin.EnumToJsonN(); set => LineJoin = value.JsonToEnumN<LineJoin>(); }

        /// <summary>
        /// Specifies the color of the line.
        /// Default '#1E90FF'.
        /// </summary>
        public string? StrokeColor { get; set; }

        /// <summary>
        /// Specifies the lengths of the alternating dashes and gaps that form the dash pattern.
        /// Numbers must be equal or greater than 0. The lengths are scaled by the strokeWidth.
        /// To convert a dash length to pixels, multiply the length by the current stroke width.
        /// </summary>
        public List<double>? StrokeDashArray { get; set; }

        /// <summary>
        /// Defines a gradient with which to color the lines.
        /// Requires the DataSource lineMetrics option to be set to true.
        /// Disabled if strokeDashArray is set.
        /// </summary>
        public string? StrokeGradient { get; set; }

        /// <summary>
        /// The line's offset.
        /// A positive value offsets the line to the right, relative to the direction of the line.
        /// A negative value offsets to the left.
        /// Default '0'.
        /// </summary>
        public double? Offset { get; set; }

        /// <summary>
        /// A number between 0 and 1 that indicates the opacity at which the line will be drawn.
        /// Default '1'.
        /// </summary>
        public double? StrokeOpacity { get; set; }

        /// <summary>
        /// The amount of offset in pixels to render the line relative to where it would render normally.
        /// Negative values indicate left and up.
        /// Default: '[0,0]'
        /// </summary>
        public Pixel? Translate { get; set; }

        /// <summary>
        /// Specifies the frame of reference for 'translate'.
        /// "map": Lines are translated relative to the map.
        /// "viewport": Lines are translated relative to the viewport
        /// Default: 'map'
        /// </summary>
        [JsonIgnore]
        public TranslateAnchor? TranslateAnchor { get; set; }

        [JsonInclude]
        [JsonPropertyName("translateAnchor")]
        internal string? TranslateAnchorJs { get => TranslateAnchor.EnumToJsonN(); set => TranslateAnchor = value.JsonToEnumN<TranslateAnchor>(); }

        /// <summary>
        /// The width of the line in pixels. Must be a value greater or equal to 0.
        /// Default '2'.
        /// </summary>
        public double? StrokeWidth { get; set; }

        public object Clone()
        {
            var clone = (LineLayerOptions)MemberwiseClone();
            clone.Translate = (Pixel?)Translate?.Clone();

            return clone;
        }
    }
}
