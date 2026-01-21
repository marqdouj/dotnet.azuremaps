using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Layers
{
    /// <summary>
    /// Defines a feature to be added to a data source.
    /// </summary>
    /// <param name="geometry"></param>
    public class MapFeatureDef (Geometry geometry) : JSInteropDef
    {
        public object Geometry { get; } = geometry;

        [JsonIgnore]
        public GeometryType GeometryType => ((Geometry)Geometry).Type;

        [JsonIgnore]
        public object Coordinates => GetCoordinates();

        public BoundingBox? Bbox { get; set; }

        public Properties? Properties { get; set; }

        /// <summary>
        /// When true, the feature will be added as a shape to the data source. 
        /// This is useful for features that require support for the additional functionality
        /// that a shape provides, such as editing and event handling.
        /// </summary>
        public bool AsShape { get; set; }

        private object GetCoordinates()
        {
            var geometry = (Geometry)Geometry;

            return GeometryType switch
            {
                GeometryType.Point => ((Point)geometry).Coordinates,
                GeometryType.MultiPoint => ((MultiPoint)geometry).Coordinates,
                GeometryType.LineString => ((LineString)geometry).Coordinates,
                GeometryType.MultiLineString => ((MultiLineString)geometry).Coordinates,
                GeometryType.Polygon => ((Polygon)geometry).Coordinates,
                GeometryType.MultiPolygon => ((MultiPolygon)geometry).Coordinates,
                _ => throw new ArgumentOutOfRangeException(nameof(GeometryType)),
            };
        }
    }
}
