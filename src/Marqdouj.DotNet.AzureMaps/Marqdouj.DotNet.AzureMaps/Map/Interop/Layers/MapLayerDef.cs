using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.Options;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop.Layers
{
    public enum MapLayerType
    {
        /// <summary>
        /// Renders Point objects as scalable circles (bubbles).
        /// </summary>
        Bubble,

        /// <summary>
        /// Represent the density of data using different colors (HeatMap).
        /// </summary>
        HeatMap,

        /// <summary>
        /// Overlays an image on the map with each corner anchored to a coordinate on the map. 
        /// Also known as a ground or image overlay.
        /// </summary>
        Image,

        /// <summary>
        /// Renders line data on the map. Can be used with SimpleLine, SimplePolygon,
        /// CirclePolygon, LineString, MultiLineString, Polygon, and MultiPolygon objects.
        /// </summary>
        Line,

        /// <summary>
        /// Renders filled Polygon and MultiPolygon objects on the map.
        /// </summary>
        Polygon,

        /// <summary>
        /// Renders extruded filled `Polygon` and `MultiPolygon` objects on the map.
        /// </summary>
        PolygonExtrusion,

        /// <summary>
        /// Renders point based data as symbols on the map using text and/or icons.
        /// Symbols can also be created for line and polygon data as well.
        /// </summary>
        Symbol,

        /// <summary>
        /// Renders raster tiled images on top of the map tiles.
        /// </summary>
        Tile,
    }

    /// <summary>
    /// defines a layer to be added to the map.
    /// </summary>
    public abstract class MapLayerDef
    {
        protected internal MapLayerDef() { }

        /// <summary>
        /// The type of layer.
        /// </summary>
        [JsonIgnore]
        public abstract MapLayerType Type { get; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string TypeJs { get => Type.ToString(); }

        /// <summary>
        /// Custom Id for the layer (Optional).
        /// Default is an internally generated valid Css Id.
        /// </summary>
        public string Id { get; set; } = MapExtensions.GetRandomCssId();

        /// <summary>
        /// The id of the data source which the layer will render (if applicable).
        /// Default is an internally generated valid Css Id.
        /// </summary>
        public string SourceId { get; set; } = MapExtensions.GetRandomCssId();
        /// <summary>
        /// The Url for the data source to fetch the data from (If applicable).
        /// </summary>
        public string? SourceUrl { get; set; }

        /// <summary>
        ///  Optionally specify a layer id to insert the new layer(s) before it.
        ///  Specify "labels" to place the new layer(s) just below the default label layer,
        ///  which will allow the labels to be visible on top of the custom layer.
        /// </summary>
        public string? Before { get; set; }

        /// <summary>
        /// Gets or sets the configuration options for the data source.
        /// </summary>
        public DataSourceOptions? SourceOptions { get; set; }

        /// <summary>
        /// Creates a new data source definition using the specified options or the default source options.
        /// </summary>
        /// <param name="options">The options to configure the data source. If null, the default source options are used.</param>
        /// <returns>A <see cref="DataSourceDef"/> instance representing the configured data source.</returns>
        public DataSourceDef GetDataSource(DataSourceOptions? options = null) => new(this, options ?? SourceOptions);
    }
}
