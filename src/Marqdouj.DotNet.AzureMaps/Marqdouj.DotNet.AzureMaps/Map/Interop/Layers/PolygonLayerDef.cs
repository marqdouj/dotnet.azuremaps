using Marqdouj.DotNet.AzureMaps.Map.Options;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop.Layers
{
    public class PolygonLayerDef : MapLayerDef, ICloneable
    {
        [JsonIgnore]
        public override MapLayerType Type => MapLayerType.Polygon;
        public PolygonLayerOptions? Options { get; set; } = new();

        public object Clone()
        {
            var clone = (PolygonLayerDef)MemberwiseClone();
            clone.Options = (PolygonLayerOptions?)Options?.Clone();
            clone.SourceOptions = (DataSourceOptions?)(clone.SourceOptions?.Clone());

            return clone;
        }
    }

    public class PolygonLayerOptions : SourceLayerOptions, ICloneable
    {
        /// <summary>
        /// The color to fill the polygons with.
        /// Default '#1E90FF'.
        /// </summary>
        public string? FillColor { get; set; }

        /// <summary>
        /// A number between 0 and 1 that indicates the opacity at which the fill will be drawn.
        /// Default '0.5'.
        /// </summary>
        public double? FillOpacity { get; set; }

        /// <summary>
        /// Name of image in sprite to use for drawing image fills.
        /// For seamless patterns, image width must be a factor of two (2, 4, 8, ..., 512).
        /// </summary>
        public string? FillPattern { get; set; }

        /// <summary>
        /// Whether or not the fill should be antialiased.
        /// Default 'true'.
        /// </summary>
        public bool? FillAntialias { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
