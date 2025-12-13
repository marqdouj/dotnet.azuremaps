using Marqdouj.DotNet.AzureMaps.Map.Common;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    public class MapEventHtmlMarkerArgs : MapEventArgs<MapEventHtmlMarkerPayload>
    {
    }

    public class MapEventHtmlMarkerPayload
    {
        public JSInteropDef? Interop { get; set; }
    }
}
