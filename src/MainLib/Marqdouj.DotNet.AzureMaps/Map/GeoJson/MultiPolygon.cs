using Marqdouj.DotNet.AzureMaps.Map.Common;

namespace Marqdouj.DotNet.AzureMaps.Map.GeoJson
{
    public class MultiPolygon() : Geometry(GeometryType.MultiPolygon)
    {
        public BoundingBox? Bbox { get; set; }
        public List<List<List<Position>>>? Coordinates { get; set; }
    }
}
