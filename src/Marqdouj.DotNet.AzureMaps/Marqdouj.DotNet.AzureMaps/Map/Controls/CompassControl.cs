using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Controls
{
    /// <summary>
    /// <see cref="MapControlType.Compass"/>
    /// </summary>
    public class CompassControl : MapControl
    {
        public CompassControl(MapControlPosition? position = null, CompassControlOptions? options = null)
        {
            Type = MapControlType.Compass;
            ControlOptions = new MapControlOptions();
            if (position != null)
                ControlOptions.Position = position;
            Options = options;
            SortOrder = 3;
        }

        public CompassControlOptions? Options { get; set; }

        /// <summary>
        /// Makes a copy of this control
        /// </summary>
        /// <param name="fullCopy">if true, copies the internal settings</param>
        /// <returns></returns>
        public CompassControl Copy(bool fullCopy = true)
        {
            var control = new CompassControl
            {
                ControlOptions = (MapControlOptions?)ControlOptions?.Clone(),
                Id = Id,
                Options = (CompassControlOptions?)Options?.Clone(),
                SortOrder = SortOrder
            };

            if (fullCopy)
            {
                control.InteropId = InteropId;
            }

            return control;
        }
    }

    public class CompassControlOptions : ICloneable
    {
        /// <summary>
        /// The angle that the map will rotate with each click of the control.
        /// Default '15'.
        /// </summary>
        public double RotationDegreesDelta { get; set; } = 15;

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
        /// Inverts the direction of map rotation controls.
        /// </summary>
        public bool? Inverted { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
