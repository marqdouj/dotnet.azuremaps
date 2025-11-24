using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Common
{
    /// <summary>
    /// Represent a pixel coordinate or offset. Extends an array of [x, y].
    /// </summary>
    public class Pixel : List<double>, ICloneable
    {
        /// <summary>
        /// Required of JS Interop; Do Not Use!
        /// </summary>
        [JsonConstructor]
        public Pixel() { }

        public Pixel(double x, double y)
        {
            Add(x);
            Add(y);
        }

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

        /// <summary>
        /// Checks if list has the minimum required elements (X, Y); if not adds them
        /// </summary>
        internal void Verify()
        {
            this.EnsureCount(2, 2);
        }

        public string ToString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return ToString();

            return $"[{X.ToString(format)}, {Y.ToString(format)}]";
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }

        public object Clone()
        {
            var clone = new Pixel();
            clone.AddRange(this);
            return clone;
        }
    }
}
