using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventConfigArgs : MapEventArgs<MapEventConfigPayload> { }

    public class MapEventConfigPayload : MapEventPayloadBase
    {
        [JsonInclude] public string? Created { get; internal set; }
        [JsonInclude] public string? DefaultConfiguration { get; internal set; }
        [JsonInclude] public string? Description { get; internal set; }
        [JsonInclude] public List<MapEventConfigurationStyle>? Configurations { get; internal set; }
        [JsonInclude] public int? Version { get; internal set; }
    }

    public class MapEventConfigurationStyle
    {
        [JsonInclude] public string? Copyright { get; internal set; }
        [JsonInclude] public string? DisplayName { get; internal set; }
        [JsonInclude] public string? Name { get; internal set; }
        [JsonInclude] public string? ShortcutKey { get; internal set; }
        [JsonInclude] public object? Style { get; internal set; }
        [JsonInclude] public string? Theme { get; internal set; }
        [JsonInclude] public string? Thumbnail { get; internal set; }
        [JsonInclude] public string? Url { get; internal set; }
    }
}
