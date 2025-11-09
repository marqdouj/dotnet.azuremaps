## Add Image Layer

````CSharp
        private static async Task<MapLayerDef> AddImageLayer(MapInterop mapInterop, IDataService dataService)
        {
            var layerDef = new ImageLayerDef();

            var data = await dataService.GetImageLayerData();
            layerDef.Options = new ImageLayerOptions
            {
                Url = data.Url,
                Coordinates = data.Coordinates,
            };

            await mapInterop.Layers.CreateLayer(layerDef);

            await mapInterop.Configuration.ZoomTo(new Position(-74.172363, 40.735657), 11);

            return layerDef;
        }
````
