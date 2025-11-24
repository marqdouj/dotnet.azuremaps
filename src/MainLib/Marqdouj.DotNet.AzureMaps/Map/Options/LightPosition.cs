using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    /// <summary>
    /// Position of the light source relative to lit (extruded) geometries,
    /// in [r radial coordinate, a azimuthal angle, p polar angle]
    /// where r indicates the distance from the center of the base of an object to its light,
    /// a indicates the position of the light relative to 0°
    /// (0° when "anchor" is set to viewport corresponds to the top of the viewport,
    /// or 0° when "anchor" is set to map corresponds to due north, and degrees proceed clockwise),
    /// and p indicates the height of the light (from 0°, directly above, to 180°, directly below).
    /// </summary>
    public class LightPosition : List<double>, ICloneable
    {
        [JsonConstructor]
        public LightPosition() {}

        public LightPosition(double r, double a, double p)
        {
            Add(r);
            Add(a);
            Add(p);
        }

        /// <summary>
        /// radial coordinate
        /// </summary>
        public double R => Count >= 1 ? this[0] : 0;

        /// <summary>
        /// azimuthal angle
        /// </summary>
        public double A => Count >= 2 ? this[1] : 0;

        /// <summary>
        /// polar angle
        /// </summary>
        public double P => Count >= 3 ? this[2] : 0;

        public object Clone()
        {
            var clone = new LightPosition();
            clone.AddRange(this);

            return clone;
        }

        public string ToString(string format)
        {
            if (string.IsNullOrEmpty(format))
                return ToString();

            return $"[{R.ToString(format)}, {A.ToString(format)}, {P.ToString(format)}]";
        }

        public override string ToString()
        {
            return $"[{R}, {A}, {P}]";
        }
    }
}
