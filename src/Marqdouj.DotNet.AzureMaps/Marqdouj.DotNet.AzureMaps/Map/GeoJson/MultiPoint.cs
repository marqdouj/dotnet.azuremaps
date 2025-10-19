using Marqdouj.DotNet.AzureMaps.Map.Common;

namespace Marqdouj.DotNet.AzureMaps.Map.GeoJson
{
    public class MultiPoint() : Geometry(GeometryType.MultiPoint)
    {
        private List<Position> coordinates = [];

        public MultiPoint(List<Position> coordinates) : this() => Coordinates = coordinates;

        public BoundingBox? Bbox { get; set; }
        public List<Position> Coordinates { get => coordinates; set => coordinates = value ?? []; }
    }
}
