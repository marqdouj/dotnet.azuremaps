using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Controls
{
    /// <summary>
    /// <see cref="MapControlType.Pitch"/>
    /// </summary>
    public class PitchControl : MapControl
    {
        public PitchControl(MapControlPosition? position = null, PitchControlOptions? options = null)
        {
            Type = MapControlType.Pitch;
            ControlOptions = new MapControlOptions();
            if (position != null)
                ControlOptions.Position = position;
            Options = options;
            SortOrder = 2;
        }

        public PitchControlOptions? Options { get; set; }

        /// <summary>
        /// Makes a copy of this control
        /// </summary>
        /// <param name="fullCopy">if true, copies the internal settings</param>
        /// <returns></returns>
        public PitchControl Copy(bool fullCopy = true)
        {
            var control = new PitchControl
            {
                ControlOptions = (MapControlOptions?)ControlOptions?.Clone(),
                Id = Id,
                Options = (PitchControlOptions?)Options?.Clone(),
                SortOrder = SortOrder
            };

            if (fullCopy)
            {
                control.InteropId = InteropId;
            }

            return control;
        }
    }

    public class PitchControlOptions : ICloneable
    {
        /// <summary>
        /// The angle that the map will tilt with each click of the control.
        /// Default '10'.
        /// </summary>
        public double PitchDegreesDelta { get; set; } = 10;

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
        /// Inverts the direction of map pitch controls.
        /// Default 'false'.
        /// </summary>
        public bool? Inverted { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
