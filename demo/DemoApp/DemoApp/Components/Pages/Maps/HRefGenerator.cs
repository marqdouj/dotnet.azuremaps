using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;

namespace DemoApp.Components.Pages.Maps
{
    internal enum HRefSource
    {
        AzureMaps,
        DemoApp,
        AzureDocs,
        Examples,
    }

    internal static class HRefGenerator
    {
        public static string? ToHRefAzureDocs(this MapLayerDef? layerDef) => layerDef?.Type.ToHRefAzureDocs();

        public static string ToHRefAzureDocs(this MapLayerType layerType)
        {
            string codePath = layerType switch
            {
                MapLayerType.Bubble => "map-add-bubble-layer",
                MapLayerType.HeatMap => "map-add-heat-map-layer",
                MapLayerType.Image => "map-add-image-layer",
                MapLayerType.Line => "map-add-line-layer",
                MapLayerType.Polygon => "map-add-shape",
                MapLayerType.PolygonExtrusion => "map-extruded-polygon",
                MapLayerType.Symbol => "map-add-pin",
                MapLayerType.Tile => "map-add-tile-layer",
                _ => throw new NotImplementedException(),
            };

            var hRef = HRefSource.AzureDocs.CodeUrl(codePath);
            return hRef;
        }

        public static string? ToHRefAddLayerExample(this MapLayerDef? layerDef) => layerDef?.Type.ToHRefAddLayerExample();

        public static string ToHRefAddLayerExample(this MapLayerType layerType)
        {
            var hRef = HRefSource.Examples.CodeUrl($"add{layerType}Layer.md");
            return hRef;
        }

        public static string ToPageSource(this string name) => HRefSource.DemoApp.CodeUrl($"{name}.razor");

        public static string CodeUrl(this HRefSource source, string path)
        {
            string? url = source switch
            {
                HRefSource.AzureDocs => "https://learn.microsoft.com/en-us/azure/azure-maps",
                HRefSource.AzureMaps => "https://github.com/marqdouj/dotnet.azuremaps/blob/master/src/Marqdouj.DotNet.AzureMaps/Marqdouj.DotNet.AzureMaps",
                HRefSource.DemoApp => "https://github.com/marqdouj/dotnet.azuremaps/blob/master/demo/DemoApp/DemoApp/Components/Pages/Maps",
                HRefSource.Examples => "https://github.com/marqdouj/dotnet.azuremaps/blob/master/docs/examples/",
                _ => throw new NotImplementedException(),
            };
            return Path.Combine(url, path);
        }
    }
}
