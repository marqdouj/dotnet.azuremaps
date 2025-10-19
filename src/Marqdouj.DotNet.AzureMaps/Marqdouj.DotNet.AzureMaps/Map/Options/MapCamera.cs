using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;

namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    /// <summary>
    /// Combines camera options and camera bounds options for getting the view of a map.
    /// </summary>
    public class MapCamera : ICloneable
    {
        //Camera options.

        /// <summary>
        /// <see cref="CameraOptions.Bearing"/>
        /// </summary>
        public double? Bearing { get; set; }

        /// <summary>
        /// <see cref="CameraOptions.Center"/>
        /// </summary>
        public Position? Center { get; set; }

        /// <summary>
        /// <see cref="CameraOptions.CenterOffset"/>
        /// </summary>
        public Pixel? CenterOffset { get; set; }

        /// <summary>
        /// <see cref="CameraOptions.MaxPitch"/>
        /// </summary>
        public double? MaxPitch { get; set; }

        /// <summary>
        /// <see cref="CameraOptions.MinPitch"/>
        /// </summary>
        public double? MinPitch { get; set; }

        /// <summary>
        /// <see cref="CameraOptions.MinZoom"/>
        /// </summary>
        public double? MinZoom { get; set; }

        /// <summary>
        /// <see cref="CameraOptions.Pitch"/>
        /// </summary>
        public double? Pitch { get; set; }

        /// <summary>
        /// <see cref="CameraOptions.Zoom"/>
        /// </summary>
        public double? Zoom { get; set; }

        //Camera bounds options.

        /// <summary>
        /// <see cref="CameraBoundsOptions.Bounds"/>
        /// </summary>
        public BoundingBox? Bounds { get; set; }

        /// <summary>
        /// <see cref="CameraBoundsOptions.Offset"/>
        /// </summary>
        public Pixel? Offset { get; set; }

        /// <summary>
        /// <see cref="CameraBoundsOptions.Padding"/>
        /// </summary>
        public Padding? Padding { get; set; }

        //Both Camera and Camera bounds options.

        /// <summary>
        /// <see cref="CameraOptions.MaxBounds"/>
        /// </summary>
        public BoundingBox? MaxBounds { get; set; }

        /// <summary>
        /// <see cref="CameraOptions.MaxZoom"/>
        /// </summary>
        public double? MaxZoom { get; set; }

        public object Clone()
        {
            var clone = (MapCamera)MemberwiseClone();
            clone.Center = (Position?)Center?.Clone();
            clone.CenterOffset = (Pixel?)CenterOffset?.Clone();
            clone.Bounds = (BoundingBox?)Bounds?.Clone();
            clone.Offset = (Pixel?)Offset?.Clone();
            clone.Padding = (Padding?)Padding?.Clone();
            clone.MaxBounds = (BoundingBox?)MaxBounds?.Clone();

            return clone;
        }

        public CameraOptions ToCameraOptions() =>
            new()
            {
                Bearing = Bearing,
                Center = Center,
                CenterOffset = CenterOffset,
                MaxBounds = MaxBounds,
                MaxZoom = MaxZoom,
                MaxPitch = MaxPitch,
                MinPitch = MinPitch,
                MinZoom = MinZoom,
                Pitch = Pitch,
                Zoom = Zoom,
            };

        public CameraBoundsOptions ToCameraBoundsOptions() =>
            new()
            {
                Bounds = Bounds,
                MaxBounds = MaxBounds,
                MaxZoom = MaxZoom,
                Offset = Offset,
                Padding = Padding,
            };
    }
}
