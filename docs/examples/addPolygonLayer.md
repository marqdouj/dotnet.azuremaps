## DotNet.AzureMaps Documentation - Add Polygon Layer

### [<- Go Back](../Layers.md)

````CSharp
        private static async Task<MapLayerDef> AddPolygonLayer(IAzureMapContainer mapInterop, IDataService dataService, PolygonLayerOptions? options = null)
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
            await mapInterop.Configuration.ZoomTo(data[0][0], 11);

            return layerDef;
        }

````
