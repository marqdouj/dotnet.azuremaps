using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Layers
{
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
        public abstract MapLayerType LayerType { get; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string Type { get => LayerType.ToString(); }

        /// <summary>
        /// Custom Id for the layer (Optional).
        /// Default is an internally generated valid Css Id.
        /// </summary>
        public string Id { get; set; } = MapExtensions.GetRandomCssId();

        /// <summary>
        ///  Optionally specify a layer id to insert the new layer(s) before it.
        ///  Specify "labels" to place the new layer(s) just below the default label layer,
        ///  which will allow the labels to be visible on top of the custom layer.
        /// </summary>
        public string? Before { get; set; }

        /// <summary>
        /// Gets or sets the configuration options for the data source.
        /// </summary>
        public DataSourceDef DataSource { get; set; } = new();
    }
}
