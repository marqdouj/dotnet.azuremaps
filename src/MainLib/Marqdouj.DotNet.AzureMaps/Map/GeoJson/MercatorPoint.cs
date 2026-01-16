using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.GeoJson
{
    /// <summary>
    /// A `MercatorPoint` object represents a projected three dimensional position.
    ///
    /// `MercatorPoint` uses the web mercator projection ([EPSG:3857](https://epsg.io/3857)) with slightly different units:
    /// - the size of 1 unit is the width of the projected world instead of the "mercator meter"
    /// - the origin of the coordinate space is at the north-west corner instead of the middle.
    ///
    /// For example, `MercatorPoint(0, 0, 0)` is the north-west corner of the mercator world and
    /// `MercatorPoint(1, 1, 0)` is the south-east corner. If you are familiar with
    /// [vector tiles](https://github.com/mapbox/vector-tile-spec) it may be helpful to think
    /// of the coordinate space as the `0/0/0` tile with an extent of `1`.
    ///
    /// The `z` dimension of `MercatorPoint` is conformal. A cube in the mercator coordinate space would be rendered as a cube.
    /// </summary>
    public class MercatorPoint : List<double>, ICloneable
    {
        [JsonConstructor]
        public MercatorPoint() { }

        /// <summary>
        /// Initializes a new instance of the MercatorPoint class with the specified X and Y coordinates.
        /// </summary>
        /// <param name="x">The X coordinate of the point in Mercator projection units.</param>
        /// <param name="y">The Y coordinate of the point in Mercator projection units.</param>
        /// <param name="z">The Z coordinate of the point in Mercator projection units (optional).</param>
        public MercatorPoint(double x, double y, double? z = null)
        {
            Add(x);
            Add(y);
            if (z != null)
                Z = z;
        }

        public MercatorPoint(IEnumerable<double> items)
        {
            AddRange(items.Take(3));
        }

        //Not part of the specification, but useful for convenience.
        #region X,Y,Z
        [JsonIgnore]
        public double X
        {
            get { Verify(); return this[0]; }
            set { Verify(); this[0] = value; }
        }

        [JsonIgnore]
        public double Y
        {
            get { Verify(); return this[1]; }
            set { Verify(); this[1] = value; }
        }

        [JsonIgnore]
        public double? Z
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
        #endregion

        /// <summary>
        /// Checks if list has the minimum required elements (X,Y); if not adds them
        /// </summary>
        internal void Verify()
        {
            this.EnsureCount(2);
        }

        [JsonIgnore] public bool HasZ => Z.HasValue;

        public string ToString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return ToString();

            return Z.HasValue
                ? $"[{X.ToString(format)}, {Y.ToString(format)}, {Z.Value.ToString(format)}]"
                : $"[{X.ToString(format)}, {Y.ToString(format)}]";
        }

        public override string ToString()
        {
            return Z.HasValue
                ? $"[{X}, {Y}, {Z.Value}]"
                : $"[{X}, {Y}]";
        }

        public object Clone()
        {
            var clone = new MercatorPoint();
            clone.AddRange(this);

            return clone;
        }
    }
}
