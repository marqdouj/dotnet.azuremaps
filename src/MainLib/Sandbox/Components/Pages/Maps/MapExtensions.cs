using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Marqdouj.DotNet.AzureMaps.Map.Interop;
using Marqdouj.DotNet.AzureMaps.Map.Layers;
using Marqdouj.DotNet.Web.Components.Css;
using Sandbox.Services;

namespace Sandbox.Components.Pages.Maps
{
    internal static class MapExtensions
    {
        public static async Task<List<MapFeatureDef>> GetDefaultSymbolLayerFeatures(this IMapDataService dataService)
        {
            var results = new List<MapFeatureDef>();
            var data = await dataService.GetSymbolLayerData();
            var counter = 0;

            foreach (var position in data)
            {
                counter++;

                var feature = new MapFeatureDef(new Point(position))
                {
                    Id = $"demoSymbol-{counter}",
                    Properties = new Properties
                    {
                        { "title", $"my symbol #{counter}" },
                        { "description", $"my symbol #{counter} description" },
                        { "demo", true },
                    }
                };

                results.Add(feature);
            }

            return results;
        }

        public static async Task<MapLayerDef> AddBasicMapLayer(
            this IAzureMapContainer mapInterop,
            IMapDataService dataService,
            MapLayerType layerType,
            IEnumerable<MapEventDef>? events = null)
        {
            return layerType switch
            {
                MapLayerType.Bubble => await AddBubbleLayer(mapInterop, dataService, null, events),
                MapLayerType.HeatMap => await AddHeatMapLayer(mapInterop, dataService, events),
                MapLayerType.Image => await AddImageLayer(mapInterop, dataService, events),
                MapLayerType.Line => await AddLineLayer(mapInterop, dataService, null, events),
                MapLayerType.Polygon => await AddPolygonLayer(mapInterop, dataService, null, events),
                MapLayerType.PolygonExtrusion => await AddPolygonExtLayer(mapInterop, dataService, null, events),
                MapLayerType.Symbol => await AddSymbolLayer(mapInterop, dataService, null, events),
                MapLayerType.Tile => await AddTileLayer(mapInterop, dataService, null, events),
                _ => throw new ArgumentOutOfRangeException(nameof(layerType)),
            };
        }

        private static async Task<MapLayerDef> AddTileLayer(IAzureMapContainer mapInterop,
                                                            IMapDataService dataService,
                                                            TileLayerOptions? options = null,
                                                            IEnumerable<MapEventDef>? events = null)
        {
            var layerDef = new TileLayerDef();

            if (options != null)
            {
                layerDef.Options = options;
            }
            else
            {
                layerDef.Options = new TileLayerOptions
                {
                    Opacity = 0.8,
                    TileSize = 256,
                    MinSourceZoom = 7,
                    MaxSourceZoom = 17,
                };
            }

            layerDef.Options.TileUrl = await dataService.GetTileLayerUrl();

            await mapInterop.Layers.CreateLayer(layerDef, events);

            await mapInterop.Configuration.ZoomTo(new Position(-122.426181, 47.608070), 10.75);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddSymbolLayer(
            IAzureMapContainer mapInterop,
            IMapDataService dataService,
            SymbolLayerOptions? options = null,
            IEnumerable<MapEventDef>? events = null)
        {
            var layerDef = new SymbolLayerDef();

            if (options != null)
            {
                layerDef.Options = options;
            }
            else
            {
                layerDef.Options!.IconOptions!.Image = IconImage.Pin_Red;
            }

            await mapInterop.Layers.CreateLayer(layerDef, events);

            var features = await dataService.GetDefaultSymbolLayerFeatures();
            await mapInterop.Layers.AddMapFeatures(features, layerDef.DataSource.Id);

            var pt = (Point)features[0].Geometry;
            await mapInterop.Configuration.ZoomTo(pt.Coordinates, 11);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddPolygonExtLayer(
            IAzureMapContainer mapInterop,
            IMapDataService dataService,
            PolygonExtLayerOptions? options = null,
            IEnumerable<MapEventDef>? events = null)
        {
            var layerDef = new PolygonExtLayerDef();

            if (options != null)
            {
                layerDef.Options = options;
            }
            else
            {
                layerDef.Options = new PolygonExtLayerOptions
                {
                    FillColor = HtmlColorName.Red.ToString(),
                    FillOpacity = 0.7,
                    Height = 500,
                };
            }

            await mapInterop.Layers.CreateLayer(layerDef, events);

            var data = await dataService.GetPolygonExtLayerData();
            var feature = new MapFeatureDef(new Polygon(data))
            {
                Properties = new Properties
                {
                    { "title", "my PolygonExt layer" },
                    { "demo", true },
                },
                AsShape = true
            };

            await mapInterop.Layers.AddMapFeature(feature, layerDef.DataSource.Id);
            await mapInterop.Configuration.ZoomTo(data[0][0], 11);

            var camera = await mapInterop.Configuration.GetCamera();
            camera.Pitch = 60;
            await mapInterop.Configuration.SetCamera(camera.ToCameraOptions());

            return layerDef;
        }

        private static async Task<MapLayerDef> AddPolygonLayer(
            IAzureMapContainer mapInterop,
            IMapDataService dataService,
            PolygonLayerOptions? options = null, 
            IEnumerable<MapEventDef>? events = null)
        {
            var layerDef = new PolygonLayerDef();

            if (options != null)
            {
                layerDef.Options = options;
            }
            else
            {
                layerDef.Options = new PolygonLayerOptions
                {
                    FillColor = HtmlColorName.Red.ToString(),
                    FillOpacity = 0.7,
                };
            }

            await mapInterop.Layers.CreateLayer(layerDef, events);

            var data = await dataService.GetPolygonLayerData();
            var feature = new MapFeatureDef(new Polygon(data))
            {
                Properties = new Properties
                {
                    { "title", "my Polygon layer" },
                    { "demo", true },
                },
                AsShape = true
            };

            await mapInterop.Layers.AddMapFeature(feature, layerDef.DataSource.Id);
            await mapInterop.Configuration.ZoomTo(data[0][0], 11);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddLineLayer(
            IAzureMapContainer mapInterop,
            IMapDataService dataService,
            LineLayerOptions? options = null, 
            IEnumerable<MapEventDef>? events = null)
        {
            var layerDef = new LineLayerDef
            {
                Before = "labels"
            };

            if (options != null)
            {
                layerDef.Options = options;
            }
            else
            {
                layerDef.Options = new LineLayerOptions
                {
                    StrokeColor = HtmlColorName.Blue.ToString(),
                    StrokeWidth = 4,
                };
            }

            await mapInterop.Layers.CreateLayer(layerDef, events);

            var data = await dataService.GetLineLayerData();
            var feature = new MapFeatureDef(new LineString(data))
            {
                Properties = new Properties
                {
                    { "title", "my line" },
                    { "demo", true },
                }
            };

            await mapInterop.Layers.AddMapFeature(feature, layerDef.DataSource.Id);
            await mapInterop.Configuration.ZoomTo(data[0], 10);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddImageLayer(
            IAzureMapContainer mapInterop,
            IMapDataService dataService,
            IEnumerable<MapEventDef>? events = null)
        {
            var layerDef = new ImageLayerDef();

            var data = await dataService.GetImageLayerData();
            layerDef.Options = new ImageLayerOptions
            {
                Url = data.Url,
                Coordinates = data.Coordinates,
            };

            await mapInterop.Layers.CreateLayer(layerDef, events);

            await mapInterop.Configuration.ZoomTo(new Position(-74.172363, 40.735657), 11);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddHeatMapLayer(
            IAzureMapContainer mapInterop,
            IMapDataService dataService, 
            IEnumerable<MapEventDef>? events = null)
        {
            var layerDef = new HeatMapLayerDef();

            layerDef.DataSource.Url = await dataService.GetHeatMapLayerUrl();

            await mapInterop.Layers.CreateLayer(layerDef, events);

            await mapInterop.Configuration.ZoomTo(new Position(-122.33, 47.6), 1);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddBubbleLayer(
            IAzureMapContainer mapInterop,
            IMapDataService dataService,
            BubbleLayerOptions? options = null, 
            IEnumerable<MapEventDef>? events = null)
        {
            var layerDef = new BubbleLayerDef();

            if (options != null)
            {
                layerDef.Options = options;
            }

            await mapInterop.Layers.CreateLayer(layerDef, events);

            var data = await dataService.GetBubbleLayerData();
            MapFeatureDef featureDef = new(new MultiPoint(data))
            {
                Properties = new Properties
                    {
                        { "title", "my bubble layer" },
                        { "demo", true },
                    }
            };
            await mapInterop.Layers.AddMapFeature(featureDef, layerDef.DataSource.Id);
            await mapInterop.Configuration.ZoomTo(data[0], 11);

            return layerDef;
        }
    }
}
