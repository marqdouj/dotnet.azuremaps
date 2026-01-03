using Marqdouj.DotNet.AzureMaps.Map.Common;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Layers
{
    public enum SourceType
    {
        DataSource,
        //ElevationTile,
        //VectorTile,
    }

    public abstract class SourceDef : JSInteropDef
    {
        [JsonIgnore]
        public SourceType? Type { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string? TypeJs { get => Type.ToString(); set => Type = value.JsonToEnumN<SourceType>(); }
    }
}
