using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Layers
{
    public class ImageLayerDef : MapLayerDef, ICloneable
    {
        [JsonIgnore]
        public override MapLayerType LayerType => MapLayerType.Image;
        public ImageLayerOptions? Options { get; set; } = new();

        public object Clone()
        {
            var clone = (ImageLayerDef)MemberwiseClone();
            clone.Options = (ImageLayerOptions?)Options?.Clone();
            clone.DataSource = (DataSourceDef)(clone.DataSource.Clone());

            return clone;
        }
    }

    public class ImageLayerOptions : MediaLayerOptions, ICloneable
    {
        /// <summary>
        /// An array of positions for the corners of the image listed in clockwise order: 
        /// [top left, top right, bottom right, bottom left].
        /// </summary>
        public ImageCoordinates? Coordinates { get; set; }

        /// <summary>
        /// URL to an image to overlay. Images hosted on other domains must have CORs enabled.
        /// </summary>
        public string? Url { get; set; }

        public object Clone()
        {
            var clone = (ImageLayerOptions)MemberwiseClone();

            return clone;
        }
    }

    public class ImageCoordinates : List<Position>, ICloneable
    {
        public ImageCoordinates() { }

        public ImageCoordinates(Position topleft, Position topRight, Position bottomRight, Position bottomLeft)
        {
            Add(topleft);
            Add(topRight);
            Add(bottomRight);
            Add(bottomLeft);
        }

        [JsonIgnore]
        public Position TopLeft
        {
            get { Verify(); return this[0]; }
            set { Verify(); this[0] = value; }
        }

        [JsonIgnore]
        public Position TopRight
        {
            get { Verify(); return this[1]; }
            set { Verify(); this[1] = value; }
        }

        [JsonIgnore]
        public Position BottomRight
        {
            get { Verify(); return this[2]; }
            set { Verify(); this[2] = value; }
        }

        [JsonIgnore]
        public Position BottomLeft
        {
            get { Verify(); return this[3]; }
            set { Verify(); this[3] = value; }
        }

        /// <summary>
        /// Checks if list has the minimum required elements; if not adds them
        /// </summary>
        internal void Verify()
        {
            this.EnsureCount(4, 4);
        }

        public object Clone()
        {
            var clone = (ImageCoordinates)MemberwiseClone();
            clone.Add((Position)TopLeft.Clone());
            clone.Add((Position)TopRight.Clone());
            clone.Add((Position)BottomRight.Clone());
            clone.Add((Position)BottomLeft.Clone());

            return clone;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
