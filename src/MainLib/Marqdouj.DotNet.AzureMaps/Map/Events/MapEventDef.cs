using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public enum MapEventTarget
    {
        Map,
        DataSource,
        HtmlMarker,
        Layer,
        Popup,
        Shape,
        StyleControl,
    }

    public static class MapEventTargetExtensions
    {
        /// <summary>
        /// Returns a MapEventDef list of all MapEventTypes that apply to the target.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IEnumerable<MapEventDef> GetMapEventDefs(this MapEventTarget target)
        {
            return target switch
            {
                MapEventTarget.Map => Enum.GetValues<MapEventTypeMap>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.Map)).OrderBy(e => e.TypeJs),
                MapEventTarget.DataSource => Enum.GetValues<MapEventTypeDataSource>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.DataSource)).OrderBy(e => e.TypeJs),
                MapEventTarget.HtmlMarker => Enum.GetValues<MapEventTypeHtmlMarker>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.HtmlMarker)).OrderBy(e => e.TypeJs),
                MapEventTarget.Layer => Enum.GetValues<MapEventTypeLayer>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.Layer)).OrderBy(e => e.TypeJs),
                MapEventTarget.Popup => Enum.GetValues<MapEventTypePopup>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.Popup)).OrderBy(e => e.TypeJs),
                MapEventTarget.Shape => Enum.GetValues<MapEventTypeShape>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.Shape)).OrderBy(e => e.TypeJs),
                MapEventTarget.StyleControl => Enum.GetValues<MapEventTypeStyleControl>().Select(e => new MapEventDef((MapEventType)e, MapEventTarget.StyleControl)).OrderBy(e => e.TypeJs),
                _ => throw new ArgumentException($"Invalid MapEventTarget: '{target}'"),
            };
        }

        /// <summary>
        /// Returns a MapEventType list of all MapEventTypes that apply to the target.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IEnumerable<MapEventType> GetMapEventTypes(this MapEventTarget target)
        {
            return target switch
            {
                MapEventTarget.Map => Enum.GetValues<MapEventTypeMap>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                MapEventTarget.DataSource => Enum.GetValues<MapEventTypeDataSource>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                MapEventTarget.HtmlMarker => Enum.GetValues<MapEventTypeHtmlMarker>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                MapEventTarget.Layer => Enum.GetValues<MapEventTypeLayer>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                MapEventTarget.Popup => Enum.GetValues<MapEventTypePopup>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                MapEventTarget.Shape => Enum.GetValues<MapEventTypeShape>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                MapEventTarget.StyleControl => Enum.GetValues<MapEventTypeStyleControl>().Cast<MapEventType>().OrderBy(e => e.ToString()),
                _ => throw new ArgumentException($"Invalid MapEventTarget: '{target}'"),
            };
        }
    }

    public class MapEventDef
    {
        [JsonConstructor]
        public MapEventDef() { }

        public MapEventDef(MapEventType type, MapEventTarget target)
        {
            Type = type;
            Target = target;
        }

        [JsonIgnore]
        public MapEventType? Type { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string? TypeJs { get => Type?.EnumToJson(); set => Type = value.JsonToEnumN<MapEventType>(); }

        /// <summary>
        /// Type of object associated with the event.
        /// </summary>
        [JsonIgnore]
        public MapEventTarget? Target { get; internal set; }

        [JsonInclude]
        [JsonPropertyName("target")]
        internal string? TargetJs { get => Target.EnumToJsonN(); set => Target = value.JsonToEnumN<MapEventTarget>(); }

        /// <summary>
        /// Required for any Target other than 'Map'.
        /// For the StyleControl use the 'InteropId'
        /// </summary>
        public string? TargetId { get; set; }

        /// <summary>
        /// Required for targets that belong to a source, i.e. Shape requires DataSourceId.
        /// </summary>
        public string? TargetSourceId { get; set; }

        /// <summary>
        /// If true, adds the event once (for events that support 'once'); otherwise continuous. Default is 'false'.
        /// </summary>
        public bool Once { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
