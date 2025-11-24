using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.GeoJson
{
    public enum GeometryType
    {
        Point,
        MultiPoint,
        LineString,
        MultiLineString,
        Polygon,
        MultiPolygon,
    }

    /// <summary>
    /// Geometry 'type' (as defined in Azure Maps SDK) that all geometry shapes extend; 
    /// </summary>
    public abstract class Geometry
    {
        internal Geometry(GeometryType geometryType)
        {
            Type = geometryType;
        }

        [JsonIgnore]
        public GeometryType Type { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string? TypeJs { get => Type.EnumToJson(); set => Type = value.JsonToEnum<GeometryType>(); }
    }
}