using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Marqdouj.DotNet.AzureMaps.Map.Interop;
using Marqdouj.DotNet.AzureMaps.Map.Interop.Features;
using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.Web.Components.Css;
using DemoApp.Services;

namespace DemoApp.Components.Pages.Maps
{
    internal static class MapExtensions
    {
        public static async Task<MapLayerDef> AddBasicMapLayer(this MapInterop mapInterop, IDataService dataService, MapLayerType layerType, bool zoomTo = true)
        {
            return layerType switch
            {
                MapLayerType.Bubble => await AddBubbleLayer(mapInterop, dataService, zoomTo: zoomTo),
                MapLayerType.HeatMap => await AddHeatMapLayer(mapInterop, dataService, zoomTo: zoomTo),
                MapLayerType.Image => await AddImageLayer(mapInterop, dataService, zoomTo: zoomTo),
                MapLayerType.Line => await AddLineLayer(mapInterop, dataService, zoomTo: zoomTo),
                MapLayerType.Polygon => await AddPolygonLayer(mapInterop, dataService, zoomTo: zoomTo),
                MapLayerType.PolygonExtrusion => await AddPolygonExtLayer(mapInterop, dataService, zoomTo: zoomTo),
                MapLayerType.Symbol => await AddSymbolLayer(mapInterop, dataService, zoomTo: zoomTo),
                MapLayerType.Tile => await AddTileLayer(mapInterop, dataService, zoomTo: zoomTo),
                _ => throw new ArgumentOutOfRangeException(nameof(layerType)),
            };
        }

        public static async Task<MapLayerDef> AddTileLayer(MapInterop mapInterop, IDataService dataService, TileLayerOptions? options = null, bool zoomTo = true)
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

            await mapInterop.Map.CreateDatasource(layerDef.GetDataSource());
            await mapInterop.Layers.CreateLayer(layerDef);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(new Position(-122.426181, 47.608070), 10.75);

            return layerDef;
        }

        public static async Task<MapLayerDef> AddSymbolLayer(MapInterop mapInterop, IDataService dataService, SymbolLayerOptions? options = null, bool zoomTo = true)
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

            await mapInterop.Map.CreateDatasource(layerDef.GetDataSource());
            await mapInterop.Layers.CreateLayer(layerDef);

            var data = await dataService.GetSymbolLayerData();

            foreach (var position in data)
            {
                var feature = new MapFeatureDef(new Point(position))
                {
                    Properties = new Properties
                    {
                        { "title", "my symbol" },
                        { "description", "my symbol description" },
                        { "demo", true },
                    }
                };
                await mapInterop.Layers.AddMapFeature(feature, layerDef.SourceId);
            }
            
            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(data[0], 11);

            return layerDef;
        }

        public static async Task<MapLayerDef> AddPolygonExtLayer(MapInterop mapInterop, IDataService dataService, PolygonExtLayerOptions? options = null, bool zoomTo = true)
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

            await mapInterop.Map.CreateDatasource(layerDef.GetDataSource());
            await mapInterop.Layers.CreateLayer(layerDef);

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

            await mapInterop.Layers.AddMapFeature(feature, layerDef.SourceId);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(data[0][0], 11);

            var camera = await mapInterop.Configuration.GetCamera();
            camera.Pitch = 60;
            await mapInterop.Configuration.SetCamera(camera.ToCameraOptions());

            return layerDef;
        }

        public static async Task<MapLayerDef> AddPolygonLayer(MapInterop mapInterop, IDataService dataService, PolygonLayerOptions? options = null, bool zoomTo = true)
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

            await mapInterop.Map.CreateDatasource(layerDef.GetDataSource());
            await mapInterop.Layers.CreateLayer(layerDef);

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

            await mapInterop.Layers.AddMapFeature(feature, layerDef.SourceId);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(data[0][0], 11);

            return layerDef;
        }

        public static async Task<MapLayerDef> AddLineLayer(MapInterop mapInterop, IDataService dataService, LineLayerOptions? options = null, bool zoomTo = true)
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

            await mapInterop.Map.CreateDatasource(layerDef.GetDataSource());
            await mapInterop.Layers.CreateLayer(layerDef);

            var data = await dataService.GetLineLayerData();
            var feature = new MapFeatureDef(new LineString(data))
            {
                Properties = new Properties
                {
                    { "title", "my line" },
                    { "demo", true },
                }
            };

            await mapInterop.Layers.AddMapFeature(feature, layerDef.SourceId);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(data[0], 10);

            return layerDef;
        }

        public static async Task<MapLayerDef> AddImageLayer(MapInterop mapInterop, IDataService dataService, bool zoomTo = true)
        {
            var layerDef = new ImageLayerDef();

            var data = await dataService.GetImageLayerData();
            layerDef.Options = new ImageLayerOptions
            {
                Url = data.Url,
                Coordinates = data.Coordinates,
            };

            await mapInterop.Map.CreateDatasource(layerDef.GetDataSource());
            await mapInterop.Layers.CreateLayer(layerDef);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(new Position(-74.172363, 40.735657), 11);

            return layerDef;
        }

        public static async Task<MapLayerDef> AddHeatMapLayer(MapInterop mapInterop, IDataService dataService, bool zoomTo = true)
        {
            var layerDef = new HeatMapLayerDef();

            var ds = layerDef.GetDataSource();
            ds.Url = await dataService.GetHeatMapLayerUrl();

            await mapInterop.Map.CreateDatasource(ds);
            await mapInterop.Layers.CreateLayer(layerDef);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(new Position(-122.33, 47.6), 1);

            return layerDef;
        }

        public static async Task<MapLayerDef> AddBubbleLayer(MapInterop mapInterop, IDataService dataService, BubbleLayerOptions? options = null, bool zoomTo = true)
        {
            var layerDef = new BubbleLayerDef();

            if (options != null)
            {
                layerDef.Options = options;
            }

            await mapInterop.Map.CreateDatasource(layerDef.GetDataSource());
            await mapInterop.Layers.CreateLayer(layerDef);

            var data = await dataService.GetBubbleLayerData();
            MapFeatureDef featureDef = new(new MultiPoint(data))
            {
                Properties = new Properties
                    {
                        { "title", "my bubble layer" },
                        { "demo", true },
                    }
            };
            await mapInterop.Layers.AddMapFeature(featureDef, layerDef.SourceId);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(data[0], 11);

            return layerDef;
        }
    }
}
