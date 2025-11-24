using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Controls
{
    /// <summary>
    /// <see cref="MapControlType.Zoom"/>
    /// </summary>
    public class ZoomControl : MapControl
    {
        public ZoomControl(MapControlPosition? position = null, ZoomControlOptions? options = null)
        {
            Type = MapControlType.Zoom;
            ControlOptions = new MapControlOptions();
            if (position != null)
                ControlOptions.Position = position;
            Options = options;
            SortOrder = 1;
        }

        public ZoomControlOptions? Options { get; set; }
        /// <summary>
        /// Makes a copy of this control
        /// </summary>
        /// <param name="fullCopy">if true, copies the internal settings</param>
        /// <returns></returns>
        public ZoomControl Copy(bool fullCopy = true)
        {
            var control = new ZoomControl
            {
                ControlOptions = (MapControlOptions?)ControlOptions?.Clone(),
                Id = Id,
                Options = (ZoomControlOptions?)Options?.Clone(),
                SortOrder = SortOrder
            };

            if (fullCopy)
            {
                control.InteropId = InteropId;
            }

            return control;
        }
    }

    public class ZoomControlOptions : ICloneable
    {
        /// <summary>
        /// The extent to which the map will zoom with each click of the control.
        /// Default '1'.
        /// </summary>
        public double ZoomDelta { get; set; } = 1;

        /// <summary>
        /// The style of the control.
        /// Default 'Light'.
        /// </summary>
        [JsonIgnore]
        public MapControlStyle? Style { get; set; }

        [JsonInclude]
        [JsonPropertyName("style")]
        internal string? StyleJs { get => Style.EnumToJsonN(); set => Style = value.JsonToEnumN<MapControlStyle>(); }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
