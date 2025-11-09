## Add Line Layer

````CSharp
        private static async Task<MapLayerDef> AddLineLayer(MapInterop mapInterop, IDataService dataService, LineLayerOptions? options = null)
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
            await mapInterop.Configuration.ZoomTo(data[0], 10);

            return layerDef;
        }
````
