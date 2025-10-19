using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Controls
{
    /// <summary>
    /// <see cref="MapControlType.Scale"/>
    /// </summary>
    public class ScaleControl : MapControl
    {
        public ScaleControl(MapControlPosition? position = null, ScaleControlOptions? options = null)
        {
            Type = MapControlType.Scale;
            ControlOptions = new MapControlOptions
            {
                Position = position == null ? MapControlPosition.Bottom_Right : position
            };
            Options = options;
            SortOrder = 5;
        }

        public ScaleControlOptions? Options { get; set; }

        /// <summary>
        /// Makes a copy of this control
        /// </summary>
        /// <param name="fullCopy">if true, copies the internal settings</param>
        /// <returns></returns>
        public ScaleControl Copy(bool fullCopy = true)
        {
            var control = new ScaleControl
            {
                ControlOptions = (MapControlOptions?)ControlOptions?.Clone(),
                Id = Id,
                Options = (ScaleControlOptions?)Options?.Clone(),
                SortOrder = SortOrder
            };

            if (fullCopy)
            {
                control.InteropId = InteropId;
            }

            return control;
        }
    }

    public enum ScaleControlUnit
    {
        Metric,
        Imperial,
        Nautical,
    }

    public class ScaleControlOptions : ICloneable
    {
        /// <summary>
        /// The maximum length of the scale control in pixels.
        /// Default '100'
        /// </summary>
        public double? MaxWidth { get; set; }

        /// <summary>
        /// Unit of the distance.
        /// Default 'metric'.
        /// </summary>
        [JsonIgnore]
        public ScaleControlUnit? Unit {  get; set; }

        [JsonInclude]
        [JsonPropertyName("unit")]
        internal string? UnitJs { get => Unit.EnumToJsonN(); set => Unit = value.JsonToEnumN<ScaleControlUnit>(); }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
