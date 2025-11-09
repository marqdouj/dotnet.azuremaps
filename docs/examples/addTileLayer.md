## Add Tile Layer

````csharp
        private static async Task<MapLayerDef> AddTileLayer(MapInterop mapInterop, IDataService dataService, TileLayerOptions? options = null)
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

            await mapInterop.Layers.CreateLayer(layerDef));

            await mapInterop.Configuration.ZoomTo(new Position(-122.426181, 47.608070), 10.75);

            return layerDef;
        }
````