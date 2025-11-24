namespace Marqdouj.DotNet.AzureMaps.Map.Common
{
    /// <summary>
    /// Represent the amount of padding in pixels to add to the side of a BoundingBox when setting the camera of a map.
    /// </summary>
    public class Padding : ICloneable
    {
        public int Top { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString() => IsOneValue() ? $"{Top}" : $"{Top} {Right} {Bottom} {Left}";
        private bool IsOneValue() => Top == Right && Right == Bottom && Bottom == Left;
    }
}
