using Marqdouj.DotNet.AzureMaps.Map.Common;

namespace Marqdouj.DotNet.AzureMaps.Map.GeoJson
{
    public class MultiPolygon() : Geometry(GeometryType.MultiPolygon)
    {
        private List<List<List<Position>>> coordinates = [];

        public BoundingBox? Bbox { get; set; }
        public List<List<List<Position>>> Coordinates { get => coordinates; set => coordinates = value ?? []; }
    }
}
