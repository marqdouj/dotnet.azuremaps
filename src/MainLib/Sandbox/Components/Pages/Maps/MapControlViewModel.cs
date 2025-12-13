using Marqdouj.DotNet.AzureMaps.Map.Controls;

namespace Sandbox.Components.Pages.Maps
{
    internal class MapControlViewModel(MapControl control)
    {
        public MapControl Control { get; set; } = control;
        public bool Loaded { get; set; }
        public bool IsNotLoaded => !Loaded;
    }
}
