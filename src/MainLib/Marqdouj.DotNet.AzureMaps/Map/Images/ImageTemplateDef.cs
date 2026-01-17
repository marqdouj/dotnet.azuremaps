namespace Marqdouj.DotNet.AzureMaps.Map.Images
{
    public enum ImageTemplateName
    {
        marker,
        marker_thick,
        marker_circle,
        pin,
        pin_round,
        marker_flat,
        marker_arrow,
        marker_ball_pin,
        marker_square,
        marker_square_cluster,
        marker_square_rounded,
        marker_square_rounded_cluster,
        flag,
        flag_triangle,
        rounded_square,
        rounded_square_thick,
        triangle,
        triangle_thick,
        hexagon,
        hexagon_thick,
        hexagon_rounded,
        hexagon_rounded_thick,
        triangle_arrow_up,
        triangle_arrow_left,
        arrow_up,
        arrow_up_thin,
        car,
        checker,
        checker_rotated,
        zig_zag,
        zig_zag_vertical,
        circles_spaced,
        circles,
        diagonal_lines_up,
        diagonal_lines_down,
        diagonal_stripes_up,
        diagonal_stripes_down,
        grid_lines,
        rotated_grid_lines,
        rotated_grid_stripes,
        x_fill,
        dots
    }

    /// <summary>
    /// Represents the definition of an image template, including its name, identifier, and optional customization
    /// properties such as color and scale.
    /// </summary>
    /// <remarks>Use this class to specify the configuration for an image template that can be applied to map
    /// elements or other visual components. The template name must correspond to a supported template. The identifier
    /// can be provided or automatically generated to ensure uniqueness within CSS contexts. Properties such as Color,
    /// SecondaryColor, and Scale allow further customization of the template's appearance.</remarks>
    public class ImageTemplateDef : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the ImageTemplateDef class with the specified template name and optional
        /// identifier.
        /// </summary>
        /// <param name="templateName">The name of the image template, which is supported by the map <see cref="ImageTemplateName"/>.</param>
        /// <param name="id">An optional identifier for the template. If null or whitespace, a random CSS-compatible identifier
        /// is generated.</param>
        public ImageTemplateDef(string templateName, string? id = null)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(templateName);
            TemplateName = templateName;
            Id = string.IsNullOrWhiteSpace(id) ? MapExtensions.GetRandomCssId() : id.Trim();
        }

        /// <summary>
        /// <see cref="ImageTemplateDef(string, string?)"/>
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="id"></param>
        public ImageTemplateDef(ImageTemplateName templateName, string? id = null) 
            : this(templateName.ToString().Replace("_", "-"), id) { }

        public string Id { get; }
        public string TemplateName { get; }
        public string? Color { get; set; }
        public string? SecondaryColor { get; set; }
        public double? Scale { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
