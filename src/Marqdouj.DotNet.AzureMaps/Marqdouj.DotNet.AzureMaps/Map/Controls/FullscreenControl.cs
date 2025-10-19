using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Controls
{
    /// <summary>
    /// <see cref="MapControlType.Fullscreen"/>
    /// </summary>
    public class FullscreenControl : MapControl
    {
        public FullscreenControl(MapControlPosition? position = null, FullscreenControlOptions? options = null)
        {
            Type = MapControlType.Fullscreen;
            ControlOptions = new MapControlOptions();
            if (position != null)
                ControlOptions.Position = position;
            Options = options;
            SortOrder = 0;
        }

        public FullscreenControlOptions? Options { get; set; }

        /// <summary>
        /// Makes a copy of this control
        /// </summary>
        /// <param name="fullCopy">if true, copies the internal settings</param>
        /// <returns></returns>
        public FullscreenControl Copy(bool fullCopy = true)
        {
            var control = new FullscreenControl
            {
                ControlOptions = (MapControlOptions?)ControlOptions?.Clone(),
                Id = Id,
                Options = (FullscreenControlOptions?)Options?.Clone(),
                SortOrder = SortOrder
            };

            if (fullCopy)
            {
                control.InteropId = InteropId;
            }

            return control;
        }
    }

    public class FullscreenControlOptions : ICloneable
    {
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
        /// Id of the HTML element which should be made full screen.
        /// If not specified, the map container element will be used.
        /// </summary>
        public string? ContainerId { get; set; }

        /// <summary>
        /// Indicates if the control should be hidden if the browser does not support full screen mode.
        /// Default 'false'
        /// </summary>
        public bool? HideIfUnsupported { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
