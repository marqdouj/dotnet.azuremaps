using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.AzureMaps.Map.Options;
using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Common
{
    public class DataSourceDef(MapLayerDef layerDef, DataSourceOptions? options = null) : ICloneable
    {
        /// <summary>
        /// Datasource Id (If applicable).
        /// Not all layer types require a data source (Image, Tile, and WebGL).
        /// </summary>
        public string? Id { get; set; } = layerDef.SourceId;

        /// <summary>
        /// URL to fetch the data from (If applicable).
        /// </summary>
        public string? Url { get; set; } = layerDef.SourceUrl;

        public DataSourceOptions? Options { get; set; } = options;

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
}
