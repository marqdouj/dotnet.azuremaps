using Marqdouj.DotNet.AzureMaps.Map.Common;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventPopupArgs : MapEventArgs<MapEventPopupPayload>
    {
    }

    public class MapEventPopupPayload
    {
        public JSInteropDef? Interop { get; set; }
    }
}
