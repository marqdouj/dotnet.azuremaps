using Marqdouj.DotNet.AzureMaps.Map.Common;

namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    /// <summary>
    /// The options for setting the bounds of the map control's camera.
    /// </summary>
    public class CameraBoundsOptions : ICloneable
    {
        /// <summary>
        /// The bounds of the map control's camera.
        /// Default '[-180, -89, 180, 90]'.
        /// </summary>
        public BoundingBox? Bounds { get; set; }

        /// <summary>
        /// A bounding box in which to constrain the viewable map area to.
        /// Users won't be able to pan the center of the map outside of this bounding box.
        /// Set maxBounds to null to remove maxBounds.
        /// Default 'null'.
        /// </summary>
        public BoundingBox? MaxBounds { get; set; }

        /// <summary>
        /// The maximum zoom level to allow when the map view transitions to the specified bounds.
        /// Default '20'.
        /// </summary>
        public double? MaxZoom { get; set; }

        /// <summary>
        /// An offset of the center of the given bounds relative to the map's center, measured in pixels.
        /// Default '[0, 0]'.
        /// </summary>
        public Pixel? Offset { get; set; }

        /// <summary>
        /// The amount of padding in pixels to add to the given bounds.
        /// Default {top: 0, bottom: 0, left: 0, right: 0}.
        /// </summary>
        public Padding? Padding { get; set; }

        public object Clone()
        {
            var clone = (CameraBoundsOptions)MemberwiseClone();
            clone.Bounds = (BoundingBox?)Bounds?.Clone();
            clone.MaxBounds = (BoundingBox?)MaxBounds?.Clone();
            clone.Offset = (Pixel?)Offset?.Clone();
            clone.Padding = (Padding?)Padding?.Clone();

            return clone;
        }
    }
}
