using Marqdouj.DotNet.AzureMaps.Map.Common;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Controls
{
    public class MapControl : JSInteropDef
    {
        [JsonIgnore]
        public MapControlType? Type { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string? TypeJs { get => Type.EnumToJsonN(); set => Type = value.JsonToEnumN<MapControlType>(); }

        public MapControlOptions? ControlOptions { get; set; }

        /// <summary>
        /// Used for sorting or placement position on the map
        /// </summary>
        [JsonIgnore]
        public int SortOrder { get; set; }

        public override string ToString()
        {
            return $"{Type} : {Id}";
        }
    }

    /// <summary>
    /// Positions where the control can be placed on the map.
    /// </summary>
    public enum MapControlPosition
    {
        /// <summary>
        ///The control will place itself in its default location.
        ///Literal value 'non-fixed'
        /// </summary>
        Non_Fixed,

        /// <summary>
        ///Places the control in the top left of the map.
        ///Literal value 'top-left'
        /// </summary>
        Top_Left,

        /// <summary>
        ///Places the control in the top right of the map.
        ///Literal value 'top-right'
        /// </summary>
        Top_Right,

        /// <summary>
        ///Places the control in the bottom left of the map.
        ///Literal value 'bottom-left'
        /// </summary>
        Bottom_Left,

        /// <summary>
        ///Places the control in the bottom right of the map.
        ///Literal value 'bottom-right'
        /// </summary>
        Bottom_Right,
    }

    public enum MapControlStyle
    {
        /// <summary>
        /// The control will be in the light style.
        /// Literal value 'light'
        /// </summary>
        Light,

        /// <summary>
        /// The control will be in the dark style.
        /// Literal value 'dark'
        /// </summary>
        Dark,

        /// <summary>
        /// The control will automatically switch styles based on the style of the map.
        /// If a control doesn't support automatic styling the light style will be used by default.
        /// Literal value 'auto'
        /// </summary>
        Auto
    }

    public enum MapControlType
    {
        /// <summary>
        /// A control for changing the rotation of the map.
        /// </summary>
        Compass,

        /// <summary>
        /// A control to make the map or a specified element full screen.
        /// </summary>
        Fullscreen,

        /// <summary>
        /// A control for changing the pitch of the map.
        /// </summary>
        Pitch,

        /// <summary>
        /// A control to display a scale bar on the map.
        /// </summary>
        Scale,

        /// <summary>
        /// A control for changing the style of the map.
        /// </summary>
        Style,

        /// <summary>
        /// A control for changing the zoom of the map.
        /// </summary>
        Zoom,
    }

    /// <summary>
    /// The options for adding a control to the map.
    /// </summary>
    public class MapControlOptions : ICloneable
    {
        /// <summary>
        /// The position the control will be placed on the map. 
        /// </summary>
        [JsonIgnore]
        public MapControlPosition? Position { get; set; } = MapControlPosition.Top_Right; //Use Top_Right because a bug in the map with Non_Fixed (Default)

        [JsonInclude]
        [JsonPropertyName("position")]
        internal string? PositionJs { get => Position.EnumToJsonN(); set => Position = value.JsonToEnumN<MapControlPosition>(); }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

}
