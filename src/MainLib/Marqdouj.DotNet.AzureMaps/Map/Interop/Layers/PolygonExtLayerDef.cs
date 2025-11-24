using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.Options;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop.Layers
{
    public class PolygonExtLayerDef : MapLayerDef, ICloneable
    {
        [JsonIgnore]
        public override MapLayerType Type => MapLayerType.PolygonExtrusion;
        public PolygonExtLayerOptions? Options { get; set; } = new();

        public object Clone()
        {
            var clone = (PolygonExtLayerDef)MemberwiseClone();
            clone.Options = (PolygonExtLayerOptions?)Options?.Clone();
            clone.SourceOptions = (DataSourceOptions?)(clone.SourceOptions?.Clone());

            return clone;
        }
    }

    public class PolygonExtLayerOptions : SourceLayerOptions, ICloneable
    {
        /// <summary>
        /// The height in meters to extrude the base of this layer.
        /// This height is relative to the ground.
        /// Must be greater or equal to 0 and less than or equal to 'height'.
        /// Default '0'.
        /// </summary>
        public double? Base { get; set; }

        /// <summary>
        /// The color to fill the polygons with.
        /// Ignored if 'fillPattern' is set.
        /// Default '#1E90FF'.
        /// </summary>
        public string? FillColor { get; set; }

        /// <summary>
        /// The height in meters to extrude this layer.
        /// This height is relative to the ground.
        /// Must be a number greater or equal to 0.
        /// Default '0'.
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// A number between 0 and 1 that indicates the opacity at which the fill will be drawn.
        /// Default '1'.
        /// </summary>
        public double? FillOpacity { get; set; }

        /// <summary>
        /// Name of image in sprite to use for drawing image fills.
        /// For seamless patterns, image width must be a factor of two (2, 4, 8, ..., 512).
        /// </summary>
        public string? FillPattern { get; set; }

        /// <summary>
        /// The amount of offset in pixels to render the line relative to where it would render normally.
        /// Negative values indicate left and up.
        /// Default '[0,0]'.
        /// </summary>
        public Pixel? Translate { get; set; }

        /// <summary>
        /// Specifies the frame of reference for 'translate'.
        /// "map": Lines are translated relative to the map.
        /// "viewport": Lines are translated relative to the viewport
        /// Default 'map'.
        /// </summary>
        [JsonIgnore]
        public TranslateAnchor? TranslateAnchor { get; set; }

        [JsonInclude]
        [JsonPropertyName("translateAnchor")]
        internal string? TranslateAnchorJs { get => TranslateAnchor.EnumToJsonN(); set => TranslateAnchor = value.JsonToEnumN<TranslateAnchor>(); }

        /// <summary>
        /// Specifies if the polygon should have a vertical gradient on the sides of the extrusion.
        /// Default 'true'.
        /// </summary>
        public bool? VerticalGradient { get; set; }

        public object Clone()
        {
            var clone = (PolygonExtLayerOptions)MemberwiseClone();
            clone.Translate = (Pixel?)Translate?.Clone();

            return clone;
        }
    }
}
