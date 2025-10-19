namespace Marqdouj.DotNet.AzureMaps.Map.Interop.Layers
{
    public class MediaLayerOptions : LayerOptions
    {
        /// <summary>
        /// A number between -1 and 1 that increases or decreases the contrast of the overlay.
        /// Default 0.
        /// </summary>
        public double? Contrast { get; set; }

        /// <summary>
        /// The duration in milliseconds of a fade transition when a new tile is added.
        /// Must be greater or equal to 0.
        /// Default 300.
        /// </summary>
        public int? FadeDuration { get; set; } 

        /// <summary>
        /// Rotates hues around the color wheel.
        /// A number in degrees.
        /// Default 0.
        /// </summary>
        public double? HueRotation { get; set; } 

        /// <summary>
        /// A number between 0 and 1 that increases or decreases the maximum brightness of the overlay.
        /// Default 1.
        /// </summary>
        public double? MaxBrightness { get; set; }

        /// <summary>
        /// A number between 0 and 1 that increases or decreases the minimum brightness of the overlay.
        /// Default 0.
        /// </summary>
        public double? MinBrightness { get; set; } 

        /// <summary>
        /// A number between 0 and 1 that indicates the opacity at which the overlay will be drawn.
        /// Default 1.
        /// </summary>
        public double? Opacity { get; set; } 

        /// <summary>
        /// A number between -1 and 1 that increases or decreases the saturation of the overlay.
        /// Default 0.
        /// </summary>
        public double? Saturation { get; set; } 
    }
}
