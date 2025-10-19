using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    /// <summary>
    /// The options for setting the map control's camera.
    /// </summary>
    public class CameraOptions : ICloneable
    {
        /// <summary>
        /// The bearing of the map (rotation) in degrees.
        /// When the bearing is 0, 90, 180, or 270 the top of the map container 
        /// will be north, east, south or west respectively.
        /// Default '0'.
        /// </summary>
        public double? Bearing { get; set; }

        /// <summary>
        /// The position to align the center of the map view with.
        /// Default '[0, 0]'.
        /// </summary>
        public Position? Center { get; set; }

        /// <summary>
        /// A pixel offset to apply to the center of the map.
        /// This is useful if you want to programmatically pan the map to another location 
        /// or if you want to center the map over a shape, then offset the maps view to make room for a popup.
        /// Default '[0, 0]'.
        /// </summary>
        public Pixel? CenterOffset { get; set; }

        /// <summary>
        /// A bounding box in which to constrain the viewable map area to.
        /// Users won't be able to pan the center of the map outside of this bounding box.
        /// Set maxBounds to null to remove maxBounds.
        /// Default 'null'.
        /// </summary>
        public BoundingBox? MaxBounds { get; set; }

        /// <summary>
        /// The maximum pitch that the map can be pitched to during the animation. 
        /// Must be between 0 and 85, and greater than or equal to `minPitch`.
        /// Default '60'.
        /// </summary>
        public double? MaxPitch { get; set; }

        /// <summary>
        /// The maximum zoom level that the map can be zoomed into during the animation. 
        /// Must be between 0 and 24, and greater than or equal to `minZoom`.
        /// Default '20'.
        /// </summary>
        public double? MaxZoom { get; set; }

        /// <summary>
        /// The minimum pitch that the map can be pitched to during the animation. 
        /// Must be between 0 and 85, and less than or equal to `maxPitch`.
        /// Default '0'.
        /// </summary>
        public double? MinPitch { get; set; }

        /// <summary>
        /// The minimum zoom level that the map can be zoomed out to during the animation. 
        /// Must be between 0 and 24, and less than or equal to `maxZoom`.
        /// Setting `minZoom` below 1 may result in an empty map when the zoom level is less than 1.
        /// Default '1'.
        /// </summary>
        public double? MinZoom { get; set; }

        /// <summary>
        /// The pitch (tilt) of the map in degrees between 0 and 60, where 0 is looking straight down on the map.
        /// Default '0'.
        /// </summary>
        public double? Pitch { get; set; }

        /// <summary>
        /// The zoom level of the map view.
        /// Default '1'.
        /// </summary>
        public double? Zoom { get; set; }

        public object Clone()
        {
            var clone = (CameraOptions)MemberwiseClone();
            clone.Center = Center == null ? null : new Position(Center.Longitude, Center.Latitude, Center.Elevation);
            clone.CenterOffset = CenterOffset == null ? null : new Pixel(CenterOffset.X, CenterOffset.Y);
            clone.MaxBounds = MaxBounds == null ? null : new BoundingBox(MaxBounds.SouthWest, MaxBounds.NorthEast);

            return clone;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
