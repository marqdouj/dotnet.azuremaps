using Marqdouj.DotNet.AzureMaps.Map.Configuration;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Controls
{
    /// <summary>
    /// <see cref="MapControlType.Style"/>
    /// </summary>
    public class StyleControl : MapControl
    {
        public StyleControl(MapControlPosition? position = null, StyleControlOptions? options = null)
        {
            Type = MapControlType.Style;
            ControlOptions = new MapControlOptions();
            if (position != null)
                ControlOptions.Position = position;
            Options = options;
            SortOrder = 4;
        }

        public StyleControlOptions? Options { get; set; }

        /// <summary>
        /// Makes a copy of this control
        /// </summary>
        /// <param name="fullCopy">if true, copies the internal settings</param>
        /// <returns></returns>
        public StyleControl Copy(bool fullCopy = true)
        {
            var control = new StyleControl
            {
                ControlOptions = (MapControlOptions?)ControlOptions?.Clone(),
                Id = Id,
                Options = (StyleControlOptions?)Options?.Clone(),
                SortOrder = SortOrder
            };

            if (fullCopy)
            {
                control.InteropId = InteropId;
            }

            return control;
        }
    }

    public enum StyleControlLayout
    {
        Icons,
        List,
    }

    public class StyleControlOptions : ICloneable
    {
        /// <summary>
        /// The layout to display the styles in.
        /// Default 'icons'
        /// </summary>
        [JsonIgnore]
        public StyleControlLayout? Layout { get; set; }

        [JsonInclude]
        [JsonPropertyName("layout")]
        internal string? LayoutJs { get => Layout.EnumToJsonN(); set => Layout = value.JsonToEnumN<StyleControlLayout>(); }

        /// <summary>
        /// The map styles to show in the control.
        /// Default = Road, GrayStyle_Light, Night, GrayStyle_Dark, Road_Shaded_Relief (Terra)
        /// </summary>
        [JsonIgnore]
        public List<MapStyle>? MapStyles { get; set; }

        [JsonInclude]
        [JsonPropertyName("mapStyles")]
        internal List<string>? MapStylesJs { get => MapStyles?.EnumToJson("_"); set => MapStyles = value.JsonToEnum<MapStyle>(); }

        /// <summary>
        /// The style of the control.
        /// Default 'Light'.
        /// </summary>
        [JsonIgnore]
        public MapControlStyle? Style { get; set; }

        [JsonInclude]
        [JsonPropertyName("style")]
        internal string? StyleJs { get => Style.EnumToJsonN(); set => Style = value.JsonToEnumN<MapControlStyle>(); }

        /// <summary>
        /// Whether to let style control automatically set the style, once user select a map style.
        /// If set to 'false', then clicking on style will not set the set the style automatically.
        /// Default 'true'
        /// </summary>
        public bool? AutoSelectionMode { get; set; }

        public object Clone()
        {
            var clone = (StyleControlOptions)MemberwiseClone();
            clone.MapStyles = MapStyles?.ToList();
            return clone;
        }
    }

}
