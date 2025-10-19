using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop.Features
{
    /// <summary>
    /// Defines a feature to be added to a data source.
    /// </summary>
    /// <param name="geometry"></param>
    public class MapFeatureDef (Geometry geometry)
    {
        public string? Id { get; set; }

        public object Geometry { get; } = geometry;

        public BoundingBox? Bbox { get; set; }

        public Properties? Properties { get; set; }

        /// <summary>
        /// When true, the feature will be added as a shape to the data source. 
        /// This is useful for features that require support for the additional functionality
        /// that a shape provides, such as editing and event handling.
        /// </summary>
        public bool AsShape { get; set; }
    }
}
