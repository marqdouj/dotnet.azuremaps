using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Common
{
    /// <summary>
    /// A GeoJSON BoundingBox object - an array that defines a shape whose edges follow lines of constant longitude,
    /// latitude, and elevation. All axes of the most southwesterly point are followed by all axes of the more northeasterly
    /// point. The axes order of the BoundingBox follows the axes order of geometries. The full description is detailed in
    /// [RFC 7946]{@link https://tools.ietf.org/html/rfc7946#section-5}.
    /// </summary>
    /// <remarks>
    /// [west, south, east, north] or [west, south, elevation1, east, north, elevation2]
    /// </remarks>
    public class BoundingBox : List<double>, ICloneable
    {
        [JsonConstructor]
        public BoundingBox() {}

        public BoundingBox(Position southwest, Position northeast)
        {
            if (southwest.HasElevation && northeast.HasElevation)
            {
                Add(southwest.Longitude);
                Add(southwest.Latitude);
                Add(southwest.Elevation!.Value);
                Add(northeast.Longitude);
                Add(northeast.Latitude);
                Add(northeast.Elevation!.Value);
            }
            else
            {
                Add(southwest.Longitude);
                Add(southwest.Latitude);
                Add(northeast.Longitude);
                Add(northeast.Latitude);
            }
        }

        public BoundingBox(double west, double south, double east, double north)
        {
            Add(west);
            Add(south);
            Add(east);
            Add(north);
        }

        public BoundingBox(double west, double south, double elevation1, double east, double north, double elevation2)
        {
            Add(west);
            Add(south);
            Add(elevation1);
            Add(east);
            Add(north);
            Add(elevation2);
        }

        #region Non-GeoJSON Spec
        //Not part of the GeoJSON spec but useful
        [JsonIgnore]
        public Position SouthWest
        {
            get
            {
                return Count switch
                {
                    4 => new Position(this[0], this[1]),
                    6 => new Position(this[0], this[1], this[2]),
                    _ => new Position(0, 0),
                };
            }
        }

        [JsonIgnore]
        public Position NorthEast
        {
            get
            {
                return Count switch
                {
                    4 => new Position(this[2], this[3]),
                    6 => new Position(this[3], this[4], this[5]),
                    _ => new Position(0, 0),
                };
            }
        }

        #endregion

        public object Clone()
        {
            var clone = new BoundingBox();
            clone.AddRange(this);
            return clone;
        }

        public string ToString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return ToString();

            return Count switch
            {
                4 => $"[{this[0].ToString(format)}, {this[1].ToString(format)}] - [{this[2].ToString(format)}, {this[3].ToString(format)}]",
                6 => $"[{this[0].ToString(format)}, {this[1].ToString(format)}, {this[2].ToString(format)}] - [{this[3].ToString(format)}, {this[4].ToString(format)}, {this[5].ToString(format)}]",
                _ => "Invalid"
            };
        }

        public override string ToString()
        {
            return Count switch
            {
                4 => $"[{this[0]}, {this[1]}] - [{this[2]}, {this[3]}]",
                6 => $"[{this[0]}, {this[1]}, {this[2]}] - [{this[3]}, {this[4]}, {this[5]}]",
                _ => "Invalid"
            };
        }
    }
}
