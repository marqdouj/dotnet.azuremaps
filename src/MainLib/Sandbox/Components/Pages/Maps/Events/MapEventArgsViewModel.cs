using Marqdouj.DotNet.AzureMaps.Map.Events;

namespace Sandbox.Components.Pages.Maps.Events
{
    public class MapEventArgsViewModel(MapEventArgs args)
    {
        public MapEventArgs Args { get; } = args;
        public string Value { get; set; } = args.ToString();
        public TimeSpan TimeStamp { get; set; } = new TimeSpan(DateTime.Now.Ticks);
    }
}
