## Add Bubble Layer

````csharp
        private static async Task<MapLayerDef> AddBubbleLayer(MapInterop mapInterop, IDataService dataService, BubbleLayerOptions? options = null)
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
            await mapInterop.Configuration.ZoomTo(data[0], 11);

            return layerDef;
        }

 ````
 