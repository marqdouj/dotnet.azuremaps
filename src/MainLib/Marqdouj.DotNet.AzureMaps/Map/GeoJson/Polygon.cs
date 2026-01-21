using Marqdouj.DotNet.AzureMaps.Map.Common;

namespace Marqdouj.DotNet.AzureMaps.Map.GeoJson
{
    public class Polygon() : Geometry(GeometryType.Polygon)
    {
        private List<List<Position>> coordinates = [];

        public Polygon(List<List<Position>> coordinates) : this() => Coordinates = coordinates;
        public BoundingBox? Bbox { get; set; }
        public List<List<Position>> Coordinates { get => coordinates; set => coordinates = value ?? []; }
    }
}
