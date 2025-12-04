using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Layers
{
    public class DataSourceDef() : ICloneable
    {
        /// <summary>
        /// Datasource Id (If applicable).
        /// Not all layer types require a data source (Image, Tile, and WebGL).
        /// </summary>
        public string Id { get; set; } = MapExtensions.GetRandomCssId();

        /// <summary>
        /// URL to fetch the data from (If applicable).
        /// </summary>
        public string? Url { get; set; }

        public DataSourceOptions? Options { get; set; } = new();

        public object Clone()
        {
            var clone = (DataSourceDef)MemberwiseClone();
            clone.Options = (DataSourceOptions?)Options?.Clone();
            return clone;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    /// <summary>
    /// The options for adding a datasource to the map.
    /// </summary>
    public class DataSourceOptions : ICloneable
    {
        /// <summary>
        /// The size of the buffer around each tile.
        /// A buffer value of 0 will provide better performance but will be more likely to generate artifacts when rendering.
        /// Larger buffers will produce left artifacts but will result in slower performance.
        /// Default '128'.
        /// </summary>
        public int? Buffer { get; set; }

        /// <summary>
        /// Maximum zoom level at which to create vector tiles (higher means greater detail at high zoom levels).
        /// Default '18'.
        /// </summary>
        public int? MaxZoom { get; set; }

        /// <summary>
        /// A boolean indicating if Point features in the source should be clustered or not.
        /// If set to true, points will be clustered together into groups by radius.
        /// Default 'false'.
        /// </summary>
        public bool? Cluster { get; set; }

        /// <summary>
        /// The radius of each cluster in pixels.
        /// Default '50'.
        /// </summary>
        public double? ClusterRadius { get; set; }

        /// <summary>
        /// The maximum zoom level in which to cluster points.
        /// Defaults to one zoom less than `maxZoom` so that last zoom features are not clustered.
        /// </summary>
        public int? ClusterMaxZoom { get; set; }

        /// <summary>
        /// Specifies whether to calculate line distance metrics.
        /// This is required for line layers that specify `lineGradient` values.
        /// Default 'false'.
        /// </summary>
        public bool? LineMetrics { get; set; }

        /// <summary>
        /// The Douglas-Peucker simplification tolerance that is applied to the data 
        /// when rendering (higher means simpler geometries and faster performance).
        /// Default '0.375'.
        /// </summary>
        public double? Tolerance { get; set; }

        /// <summary>
        /// Minimum number of points necessary to form a cluster if clustering is enabled.
        /// Default '2'.
        /// </summary>
        public double? ClusterMinPoints { get; set; }

        /// <summary>
        /// Whether to generate ids for the GeoJson features. When enabled, the feature.id property will be auto assigned 
        /// based on its index in the features array, over-writing any previous values.
        /// </summary>
        public bool? GenerateId { get; set; }

        /// <summary>
        /// A specified property name to be used as a feature ID (for feature state).
        /// </summary>
        public string? PromoteId { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
