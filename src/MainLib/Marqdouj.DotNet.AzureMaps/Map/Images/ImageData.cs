namespace Marqdouj.DotNet.AzureMaps.Map.Images
{
    public enum PredefinedColorSpace
    {
        srgb,
        display_p3,
    }

    public class ImageData(PredefinedColorSpace colorSpace, byte[] data, int width, int height)
    {
        /// <summary>
        /// Color space of the image data.
        /// </summary>
        public string ColorSpace { get; } = colorSpace.ToString().Replace("_", "-");

        /// <summary>
        ///Ppixel data.
        /// </summary>
        public byte[] Data { get; } = data ?? throw new ArgumentNullException(nameof(data));

        /// <summary>
        /// Number of rows in the ImageData object.
        /// </summary>
        public int Height { get; } = height;

        /// <summary>
        /// Number of pixels per row in the ImageData object.
        /// </summary>
        public int Width { get; } = width;
    }
}
