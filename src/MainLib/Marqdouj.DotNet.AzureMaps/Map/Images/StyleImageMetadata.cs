namespace Marqdouj.DotNet.AzureMaps.Map.Images
{
    public class StyleImageMetadata
    {
        /// <summary>
        /// Ratio of pixels in the image to physical pixels on the screen (default: 1)
        /// </summary>
        public double? PixelRatio { get; set; }

        /// <summary>
        /// Whether the image should be interpreted as an SDF image (default: false)
        /// </summary>
        public bool? Sdf { get; set; }

        /// <summary>
        /// [[x1, x2], ...] if icon-text-fit is used in a layer with this image, defines the part(s) that can be stretched horizontally
        /// </summary>
        public List<List<double>>? StretchX { get; set; }

        /// <summary>
        /// [[y1, y2], ...] if icon-text-fit is used in a layer with this image, defines the part(s) that can be stretched vertically
        /// </summary>
        public List<List<double>>? StretchY { get; set; }

        /// <summary>
        /// [x1, y1, x2, y2] if icon-text-fit is used in a layer with this image, defines the part of the image that can be covered by the content in text-field
        /// </summary>
        public List<double>? Content { get; set; }
    }
}
