using Marqdouj.DotNet.AzureMaps.Map.Common;

namespace Marqdouj.DotNet.AzureMaps.Map.GeoJson
{
    public class LineString() : Geometry(GeometryType.LineString)
    {
        private List<Position> coordinates = [];

        public LineString(List<Position> coordinates) : this() => Coordinates = coordinates;

        public BoundingBox? Bbox { get; set; }

        public List<Position> Coordinates { get => coordinates; set => coordinates = value ?? []; }
    }
}
