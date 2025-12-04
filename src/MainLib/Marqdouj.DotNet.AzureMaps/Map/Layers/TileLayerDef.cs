using Marqdouj.DotNet.AzureMaps.Map.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Layers
{
    public class TileLayerDef : MapLayerDef, ICloneable
    {
        [JsonIgnore]
        public override MapLayerType LayerType => MapLayerType.Tile;
        public TileLayerOptions? Options { get; set; } = new();

        public object Clone()
        {
            var clone = (TileLayerDef)MemberwiseClone();
            clone.Options = (TileLayerOptions?)Options?.Clone();
            clone.DataSource = (DataSourceDef)(clone.DataSource.Clone());

            return clone;
        }
    }

    public class TileLayerOptions : MediaLayerOptions, ICloneable
    {
        /// <summary>
        /// A bounding box that specifies where tiles are available.
        /// When specified, no tiles outside of the bounding box will be requested.
        /// Default [-180, -85.0511, 180, 85.0511].
        /// </summary>
        public BoundingBox? Bounds { get; set; }

        /// <summary>
        /// An integer specifying the minimum zoom level in which tiles are available from the tile source.
        /// Default 0.
        /// </summary>
        public int? MinSourceZoom { get; set; }

        /// <summary>
        /// An integer specifying the maximum zoom level in which tiles are available from the tile source.
        /// Default 22.
        /// </summary>
        public int? MaxSourceZoom { get; set; }

        /// <summary>
        /// An integer value that specifies the width and height dimensions of the map tiles.
        /// For a seamless experience, the tile size must be a multiplier of 2.
        /// Default 512.
        /// </summary>
        public int? TileSize { get; set; }

        /// <summary>
        /// Specifies if the tile systems coordinates uses the Tile Map Services specification,
        /// which reverses the Y coordinate axis.
        /// Default false.
        /// </summary>
        public bool? IsTMS { get; set; }

        /// <summary>
        /// An array of subdomain values to apply to the tile URL.
        /// </summary>
        public List<string>? Subdomains { get; set; }

        /// <summary>
        /// A http/https URL to a TileJSON resource or a tile URL template that uses the following parameters:
        /// {x}: X position of the tile. Usually also needs {y} and {z}.
        /// {y}: Y position of the tile. Usually also needs {x} and {z}.
        /// {z}: Zoom level of the tile. Usually also needs {x} and {y}.
        /// {quadkey}: Tile quadKey id based on the Bing Maps tile system naming convention.
        /// {bbox-epsg-3857}: A bounding box string with the format {west},{south},{east},{north}
        /// in the EPSG 4325 Spacial Reference System.
        /// {subdomain}: A placeholder where the subdomain values if specified will be added.
        /// </summary>
        public string? TileUrl { get; set; }

        public object Clone()
        {
            var clone = (TileLayerOptions)this.MemberwiseClone();
            clone.Bounds = (BoundingBox?)this.Bounds?.Clone();
            return clone;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
