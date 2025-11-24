namespace Marqdouj.DotNet.AzureMaps.Map.GeoJson
{
    public class Point() : Geometry(GeometryType.Point), ICloneable
    {
        private Position coordinates = [0, 0];

        public Point(Position coordinates) : this() => Coordinates = coordinates;

        public Position Coordinates 
        { 
            get => coordinates; 
            set 
            { 
                if (value == null)
                {
                    coordinates = [0, 0];
                    return;
                }
                coordinates = value;
                coordinates.Verify();
            } 
        }

        public object Clone()
        {
            var clone = new Point
            {
                Coordinates = (Position)Coordinates.Clone()
            };
            return clone;
        }
    }
}
