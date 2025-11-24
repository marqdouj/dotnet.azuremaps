using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.GeoJson
{
    /// <summary>
    /// A GeoJSON Position object - an array that specifies the longitude and latitude of a location. The
    /// full description is detailed in [RFC 7946]{@link https://tools.ietf.org/html/rfc7946#section-3.1.1}.
    /// </summary>
    /// <remarks>
    /// [longitude, latitude] or [longitude, latitude, elevation]
    /// </remarks>
    public class Position : List<double>, ICloneable
    {
        [JsonConstructor]
        public Position() {}

        public Position(double longitude, double latitude)
        {
            Add(longitude);
            Add(latitude);
        }

        public Position(double longitude, double latitude, double? elevation) :this(longitude, latitude)
        {
            Elevation = elevation;
        }

        public Position(IEnumerable<double> items)
        {
            AddRange(items.Take(3));
        }

        //Not part of the GeoJSON specification, but useful for convenience.

        [JsonIgnore] 
        public double Longitude 
        {
            get { Verify(); return this[0]; }
            set { Verify(); this[0] = value; }
        }

        [JsonIgnore] 
        public double Latitude
        {
            get { Verify(); return this[1]; }
            set { Verify(); this[1] = value; }
        }

        [JsonIgnore] 
        public double? Elevation
        {
            get => Count > 2 ? this[2] : null;
            set
            { 
                if (value == null)
                {
                    this.EnsureCount(2, 2);
                    return;
                }

                this.EnsureCount(3, 3);

                this[2] = value.Value; 
            }
        }

        /// <summary>
        /// Checks if list has the minimum required elements (Lon, Lat); if not adds them
        /// </summary>
        internal void Verify()
        {
            this.EnsureCount(2);
        }

        [JsonIgnore] public bool HasElevation => Elevation.HasValue;

        public string ToString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return ToString();

            return Elevation.HasValue
                ? $"[{Longitude.ToString(format)}, {Latitude.ToString(format)}, {Elevation.Value.ToString(format)}]"
                : $"[{Longitude.ToString(format)}, {Latitude.ToString(format)}]";
        }

        public override string ToString()
        {
            return Elevation.HasValue 
                ? $"[{Longitude}, {Latitude}, {Elevation.Value}]"
                : $"[{Longitude}, {Latitude}]";
        }

        public object Clone()
        {
            var clone = new Position();
            clone.AddRange(this);

            return clone;
        }
    }
}
