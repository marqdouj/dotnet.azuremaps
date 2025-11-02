## Add Polygon Extrusion Layer

````CSharp
        private static async Task<MapLayerDef> AddPolygonExtLayer(MapInterop mapInterop, IDataService dataService, PolygonExtLayerOptions? options = null)
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

            await mapInterop.Layers.CreateLayer(layerDef, layerDef.GetDataSource());

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
            await mapInterop.Configuration.ZoomTo(data[0][0], 11);

            var camera = await mapInterop.Configuration.GetCamera();
            camera.Pitch = 60;
            await mapInterop.Configuration.SetCamera(camera.ToCameraOptions());

            return layerDef;
        }
````
