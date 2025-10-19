using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Common
{
    public enum TileState
    {
        /// <summary>
        /// Tile data is in the process of loading.
        /// </summary>
        Loading,

        /// <summary>
        /// Tile data has been loaded.
        /// </summary>
        Loaded,

        /// <summary>
        /// Tile data has been loaded and is being updated.
        /// </summary>
        Reloading,

        /// <summary>
        /// The data has been deleted.
        /// </summary>
        Unloaded,

        /// <summary>
        /// Tile data was not loaded because of an error.
        /// </summary>
        Errored,

        /// <summary>
        ///Tile data was previously loaded, but has expired per its HTTP headers and is in the process of refreshing.
        /// </summary>
        Expired
    }

    /// <summary>
    /// Tile object returned by the map when a source data event occurs.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// The ID of the tile.
        /// </summary>
        [JsonInclude] public TileId? Id { get; internal set; }

        /// <summary>
        /// The size of the tile.
        /// </summary>
        [JsonInclude] public double? Size { get; internal set; }

        /// <summary>
        /// The state of the tile.
        /// </summary>
        [JsonIgnore] public TileState? State {  get; private set; }

        [JsonInclude]
        [JsonPropertyName("state")]
        internal string? StateJs { get => State.EnumToJsonN(); set => State = value.JsonToEnumN<TileState>(); }
    }
}
