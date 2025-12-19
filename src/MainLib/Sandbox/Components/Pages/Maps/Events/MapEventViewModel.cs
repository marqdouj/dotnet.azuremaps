using Marqdouj.DotNet.AzureMaps.Map.Events;

namespace Sandbox.Components.Pages.Maps.Events
{
    internal class MapEventViewModel(MapEventDef eventDef)
    {
        public MapEventDef EventDef { get; } = eventDef;
        public string? Name => EventDef.Type.ToString();
        public MapEventType? Type => EventDef.Type;
        public bool IsChecked { get; set; }
        public bool IsNotChecked => !IsChecked;
        public bool IsLoaded { get; set; }
        public bool IsNotLoaded => !IsLoaded;
    }
}
