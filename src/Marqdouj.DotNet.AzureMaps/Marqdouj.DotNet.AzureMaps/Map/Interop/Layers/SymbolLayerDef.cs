using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop.Layers
{
    public class SymbolLayerDef : MapLayerDef, ICloneable
    {
        [JsonIgnore]
        public override MapLayerType Type => MapLayerType.Symbol;
        public SymbolLayerOptions? Options { get; set; } = new();

        public object Clone()
        {
            var clone = (SymbolLayerDef)MemberwiseClone();
            clone.Options = (SymbolLayerOptions?)Options?.Clone();
            clone.SourceOptions = (DataSourceOptions?)(clone.SourceOptions?.Clone());

            return clone;
        }
    }

    #region Options
    public enum SymbolLayerPlacement
    {
        Point,
        Line,
        LineCenter,
    }

    public enum SymbolLayerZOrder
    {
        Auto,
        ViewportY,
        Source,
    }

    /// <summary>
    /// Options used when rendering geometries in a SymbolLayer.
    /// </summary>
    public class SymbolLayerOptions : SourceLayerOptions, ICloneable
    {
        /// <summary>
        /// Options used to customize the icons of the symbols.
        /// <see cref="Layers.IconOptions"/>
        /// </summary>
        public IconOptions? IconOptions { get; set; } = new();

        /// <summary>
        /// Options used to customize the text of the symbols.
        /// <see cref="Layers.TextOptions"/>
        /// </summary>
        public TextOptions? TextOptions { get; set; } = new();

        /// <summary>
        /// Specifies the label placement relative to its geometry.
        /// "point": The label is placed at the point where the geometry is located.
        /// "line": The label is placed along the line of the geometry.
        /// Can only be used on LineString and Polygon geometries.
        /// "line-center": The label is placed at the center of the line of the geometry.
        /// Can only be used on 'LineString' and 'Polygon' geometries 
        /// Default 'point'.
        /// </summary>
        [JsonIgnore]
        public SymbolLayerPlacement? Placement { get; set; }

        [JsonInclude]
        [JsonPropertyName("placement")]
        internal string? PlacementJ { get => Placement.EnumToJsonN(); set => Placement = value.JsonToEnumN<SymbolLayerPlacement>(); }

        /// <summary>
        /// Sorts features in ascending order based on this value. Features with
        /// lower sort keys are drawn and placed first.
        /// Default 'null'.
        /// </summary>
        public double? SortKey { get; set; }

        /// <summary>
        /// Determines whether overlapping symbols in the same layer are rendered in the order
        /// that they appear in the data source, or by their y position relative to the viewport.
        /// To control the order and prioritization of symbols otherwise, use 'sortKey'.
        /// "auto": Sorts symbols by 'sortKey' if set. Otherwise behaves like "viewport-y".
        /// "viewport-y": Sorts symbols by their y position if 'allowOverlap' is 'true' or
        /// if 'ignorePlacement' is 'false'.
        /// "source": Sorts symbols by 'sortKey' if set. Otherwise, symbols are rendered in the
        /// same order as the source data.
        /// Default 'auto'.
        /// </summary>
        [JsonIgnore]
        public SymbolLayerZOrder? ZOrder { get; set; }

        [JsonInclude]
        [JsonPropertyName("zOrder")]
        internal string? ZOrderJ { get => ZOrder.EnumToJsonN(); set => ZOrder = value.JsonToEnumN<SymbolLayerZOrder>(); }

        /// <summary>
        /// Distance in pixels between two symbol anchors along a line. Must be greater or equal to 1.
        /// Default '250'.
        /// </summary>
        public double? LineSpacing { get; set; }

        public object Clone()
        {
            var clone = (SymbolLayerOptions)MemberwiseClone();
            clone.IconOptions = (IconOptions?)IconOptions?.Clone();
            clone.TextOptions = (TextOptions?)TextOptions?.Clone();

            return clone;
        }
    }
    #endregion

    #region IconOptions
    public enum IconImage
    {
        Marker_Black,
        Marker_Blue,
        Marker_DarkBlue,
        Marker_Red,
        Marker_Yellow,
        Pin_Blue,
        Pin_DarkBlue,
        Pin_Red,
        Pin_Round_Blue,
        Pin_Round_DarkBlue,
        Pin_Round_Red
    }

    /// <summary>
    /// Options used to customize the icons in a SymbolLayer
    /// </summary>
    public class IconOptions : ICloneable
    {
        /// <summary>
        /// Specifies if the symbol icon can overlay other symbols on the map.
        /// If true, the icon will be visible even if it collides with other previously drawn symbols.
        /// Tip: Set this to true if animating an symbol to ensure smooth rendering.
        /// Default 'false'.
        /// </summary>
        public bool? AllowOverlap { get; set; }

        /// <summary>
        /// Specifies which part of the icon is placed closest to the icons anchor position on the map.
        /// "center": The center of the icon is placed closest to the anchor.
        /// "left": The left side of the icon is placed closest to the anchor.
        /// "right": The right side of the icon is placed closest to the anchor.
        /// "top": The top of the icon is placed closest to the anchor.
        /// "bottom": The bottom of the icon is placed closest to the anchor.
        /// "top-left": The top left corner of the icon is placed closest to the anchor.
        /// "top-right": The top right corner of the icon is placed closest to the anchor.
        /// "bottom-left": The bottom left corner of the icon is placed closest to the anchor.
        /// "bottom-right": The bottom right corner of the icon is placed closest to the anchor.
        /// Default 'bottom'.
        /// </summary>
        [JsonIgnore]
        public PositionAnchor? Anchor { get; set; }

        [JsonInclude]
        [JsonPropertyName("anchor")]
        internal string? AnchorJs { get => Anchor.EnumToJsonN(); set => Anchor = value.JsonToEnumN<PositionAnchor>(); }

        /// <summary>
        /// Specifies if other symbols can overlap this symbol.
        /// If true, other symbols can be visible even if they collide with the icon.
        /// Default 'false'.
        /// </summary>
        public bool? IgnorePlacement { get; set; }

        /// <summary>
        /// The name of the image in the map's image sprite to use for drawing the icon.
        /// Default 'marker-blue'.
        /// </summary>
        [JsonIgnore]
        public IconImage? Image { get; set; }

        [JsonInclude]
        [JsonPropertyName("image")]
        internal string? ImageJs { get => Image.EnumToJsonN(); set => Image = value.JsonToEnumN<IconImage>(); }

        /// <summary>
        /// Specifies an offset distance of the icon from its anchor in pixels.
        /// Positive values indicate right and down, while negative values indicate left and up.
        /// Each component is multiplied by the value of size to obtain the final offset in pixels.
        /// When combined with rotation the offset will be as if the rotated direction was up.
        /// Default '[0, 0]'.
        /// </summary>
        public Pixel? Offset { get; set; }

        /// <summary>
        /// Specifies if a symbols icon can be hidden but its text displayed if it is overlapped with another symbol.
        /// If true, text will display without their corresponding icons
        /// when the icon collides with other symbols and the text does not.
        /// Default 'false'.
        /// </summary>
        public bool? Optional { get; set; }

        /// <summary>
        /// Size of the additional area around the icon bounding box used for detecting symbol collisions.
        /// Default '2'.
        /// </summary>
        public double? Padding { get; set; }

        /// <summary>
        /// Specifies the orientation of the icon when the map is pitched.
        /// "auto": Automatically matches the value of 'rotationAlignment'.
        /// "map": The icon is aligned to the plane of the map.
        /// "viewport": The icon is aligned to the plane of the viewport
        /// Default 'auto'.
        /// </summary>
        [JsonIgnore]
        public MapItemAlignment? PitchAlignment { get; set; }

        [JsonInclude]
        [JsonPropertyName("pitchAlignment")]
        internal string? PitchAlignmentJs { get => PitchAlignment.EnumToJsonN(); set => PitchAlignment = value.JsonToEnumN<MapItemAlignment>(); }

        /// <summary>
        /// The amount to rotate the icon clockwise in degrees
        /// Default '0'.
        /// </summary>
        public double? Rotation { get; set; }

        /// <summary>
        /// In combination with the placement property of a SymbolLayerOptions
        /// this determines the rotation behavior of icons.
        /// "auto": When placement is "point" this is equivalent to "viewport".
        /// When placement is "line" this is equivalent to "map".
        /// "map": When placement is "point" aligns icons east-west.
        /// When placement is "line" aligns the icons' x-axes with the line.
        /// "viewport": Icons' x-axes will align with the x-axis of the viewport.
        /// Default 'auto'.
        /// </summary>
        [JsonIgnore]
        public MapItemAlignment? RotationAlignment { get; set; }

        [JsonInclude]
        [JsonPropertyName("rotationAlignment")]
        internal string? RotationAlignmentJs { get => RotationAlignment.EnumToJsonN(); set => RotationAlignment = value.JsonToEnumN<MapItemAlignment>(); }

        /// <summary>
        /// Scales the original size of the icon by the provided factor.
        /// Must be greater or equal to 0.
        /// Default '1'.
        /// </summary>
        public double? Size { get; set; }

        /// <summary>
        /// A number between 0 and 1 that indicates the opacity at which the icon will be drawn.
        /// Default '1'.
        /// </summary>
        public double? Opacity { get; set; }

        public object Clone()
        {
            var clone = (IconOptions)MemberwiseClone();
            clone.Offset = (Pixel?)Offset?.Clone();

            return clone;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
    #endregion

    #region TextOptions
    public enum TextJustify
    {
        /// <summary>
        /// The text is aligned towards the anchor position.
        /// </summary>
        Auto,

        /// <summary>
        /// The text is aligned to the left.
        /// </summary>
        Left,

        /// <summary>
        /// The text is centered.
        /// </summary>
        Center,

        /// <summary>
        /// The text is aligned to the right.
        /// </summary>
        Right
    }

    /// <summary>
    /// Options used to customize the text in a SymbolLayer
    /// </summary>
    public class TextOptions : ICloneable
    {
        /// <summary>
        /// Specifies if the text will be visible if it collides with other symbols.
        /// If true, the text will be visible even if it collides with other previously drawn symbols.
        /// Default 'false'.
        /// </summary>
        public bool? AllowOverlap { get; set; }

        /// <summary>
        /// Specifies which part of the icon is placed closest to the icons anchor position on the map.
        /// Default 'center'.
        /// </summary>
        [JsonIgnore]
        public PositionAnchor? Anchor { get; set; }

        [JsonInclude]
        [JsonPropertyName("anchor")]
        internal string? AnchorJs { get => Anchor.EnumToJsonN(); set => Anchor = value.JsonToEnumN<PositionAnchor>(); }

        /// <summary>
        /// Specifies the name of a property on the features to use for a text label.
        /// </summary>
        public string? TextField { get; set; }

        /// <summary>
        /// Possible values: "SegoeFrutigerHelveticaMYingHei-Bold", "SegoeFrutigerHelveticaMYingHei-Medium",
        /// "SegoeFrutigerHelveticaMYingHei-Regular", "SegoeUi-Bold", "SegoeUi-Light", "SegoeUi-Regular",
        /// "SegoeUi-SemiBold", "SegoeUi-SemiLight", "SegoeUi-SymbolRegular", "StandardCondensedSegoeUi-Black",
        /// "StandardCondensedSegoeUi-Bold", "StandardCondensedSegoeUi-Light", "StandardCondensedSegoeUi-Regular",
        /// "StandardFont-Black", "StandardFont-Bold", "StandardFont-Light", "StandardFont-Regular",
        /// "StandardFontCondensed-Black", "StandardFontCondensed-Bold", "StandardFontCondensed-Light",
        /// "StandardFontCondensed-Regular".
        /// Default 'StandardFont-Regular'.
        /// </summary>
        public List<string>? Font { get; set; }

        /// <summary>
        /// Specifies if the other symbols are allowed to collide with the text.
        /// If true, other symbols can be visible even if they collide with the text.
        /// Default 'false'.
        /// </summary>
        public bool? IgnorePlacement { get; set; }

        /// <summary>
        /// Text justification options.
        /// Default 'center'.
        /// </summary>
        [JsonIgnore]
        public TextJustify? Justify { get; set; }

        [JsonInclude]
        [JsonPropertyName("justify")]
        internal string? JustifyJs { get => Justify.EnumToJsonN(); set => Justify = value.JsonToEnumN<TextJustify>(); }

        /// <summary>
        /// Specifies an offset distance of the icon from its anchor in ems.
        /// Positive values indicate right and down, while negative values indicate left and up.
        /// Default '[0, 0]'. 
        /// </summary>
        public Pixel? Offset { get; set; }

        /// <summary>
        /// Specifies if the text can be hidden if it is overlapped by another symbol.
        /// If true, icons will display without their corresponding text
        /// when the text collides wit other symbols and the icon does not.
        /// Default 'false'.
        /// </summary>
        public bool? Optional { get; set; }

        /// <summary>
        /// Size of the additional area around the text bounding box used for detecting symbol collisions.
        /// Default '2'.
        /// </summary>
        public double? Padding { get; set; }

        /// <summary>
        /// Specifies the orientation of the text when the map is pitched.
        /// "auto": Automatically matches the value of 'rotationAlignment'.
        /// "map": The text is aligned to the plane of the map.
        /// "viewport": The text is aligned to the plane of the viewport.
        /// Default: 'auto'.
        /// </summary>
        [JsonIgnore]
        public MapItemAlignment? PitchAlignment { get; set; }

        [JsonInclude]
        [JsonPropertyName("pitchAlignment")]
        internal string? PitchAlignmentJs { get => PitchAlignment.EnumToJsonN(); set => PitchAlignment = value.JsonToEnumN<MapItemAlignment>(); }

        /// <summary>
        /// Radial offset of text, in the direction of the symbol's anchor. Useful in combination
        /// with 'variableAnchor', which defaults to using the two-dimensional 'offset' if present.
        /// Default: '0'.
        /// </summary>
        public double? RadialOffset { get; set; }

        /// <summary>
        /// The amount to rotate the text clockwise in degrees.
        ///  Default '0'.
        /// </summary>
        public double? Rotation { get; set; }

        /// <summary>
        /// In combination with the 'placement' property of the 'SymbolLayerOptions',
        /// specifies the rotation behavior of the individual glyphs forming the text.
        /// "auto": When the 'placement' is set to "point", this is equivalent to "map".
        /// When the 'placement' is set to "line" this is equivalent to "map".
        /// "map": When the 'placement' is set to "point", aligns text east-west.
        /// When the 'placement' is set to "line", aligns text x-axes with the line.
        /// "viewport": Produces glyphs whose x-axes are aligned with the x-axis of the viewport,
        /// regardless of the value of 'placement'.
        /// Default: 'auto'
        /// </summary>
        [JsonIgnore]
        public MapItemAlignment? RotationAlignment { get; set; }

        [JsonInclude]
        [JsonPropertyName("rotationAlignment")]
        internal string? RotationAlignmentJs { get => RotationAlignment.EnumToJsonN(); set => RotationAlignment = value.JsonToEnumN<MapItemAlignment>(); }

        /// <summary>
        /// List of potential anchor locations, to increase the chance of placing high-priority
        /// labels on the map. The renderer will attempt to place the label at each location,
        /// in order, before moving onto the next label. Use 'justify: "auto" to choose text
        /// justification based on anchor position. To apply an offset use the 'radialOffset' or
        /// two-dimensional 'offset' options.
        /// "center": The center of the icon is placed closest to the anchor.
        /// "left": The left side of the icon is placed closest to the anchor.
        /// "right": The right side of the icon is placed closest to the anchor.
        /// "top": The top of the icon is placed closest to the anchor.
        /// "bottom": The bottom of the icon is placed closest to the anchor.
        /// "top-left": The top left corner of the icon is placed closest to the anchor.
        /// "top-right": The top right corner of the icon is placed closest to the anchor.
        /// "bottom-left": The bottom left corner of the icon is placed closest to the anchor.
        /// "bottom-right": The bottom right corner of the icon is placed closest to the anchor.
        /// Default: 'null'.
        /// </summary>
        [JsonIgnore]
        public List<PositionAnchor>? VariableAnchor { get; set; }

        [JsonInclude]
        [JsonPropertyName("variableAnchor")]
        public List<string>? VariableAnchorJs { get => VariableAnchor?.EnumToJson(); set => VariableAnchor = value.JsonToEnum<PositionAnchor>(); }

        /// <summary>
        /// The size of the font in pixels.
        /// Must be a number greater or equal to 0.
        /// Default '16'.
        /// </summary>
        public double? Size { get; set; }

        /// <summary>
        /// The color of the text.
        /// Default '#000000'.
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// The halo's fadeout distance towards the outside in pixels.
        /// Must be a number greater or equal to 0.
        /// Default '0'.
        /// </summary>
        public double? HaloBlur { get; set; }

        /// <summary>
        /// The color of the text's halo, which helps it stand out from backgrounds.
        /// Default 'rgba(0,0,0,0)'.
        /// </summary>
        public string? HaloColor { get; set; }

        /// <summary>
        /// The distance of the halo to the font outline in pixels.
        /// Must be a number greater or equal to 0.
        /// The maximum text halo width is 1/4 of the font size.
        /// Default '0'.
        /// </summary>
        public double? HaloWidth { get; set; }

        /// <summary>
        /// A number between 0 and 1 that indicates the opacity at which the text will be drawn.
        /// Default '1'.
        /// </summary>
        public double? Opacity { get; set; }

        public object Clone()
        {
            var clone = (TextOptions)MemberwiseClone();
            clone.Font = Font == null ? null : [.. Font];
            clone.Offset = Offset == null ? null : (Pixel)Offset.Clone();
            clone.VariableAnchor = VariableAnchor == null ? null : [.. VariableAnchor];
            return clone;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
    #endregion
}
