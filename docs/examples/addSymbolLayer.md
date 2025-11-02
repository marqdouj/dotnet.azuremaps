## Add Symbol Layer

````csharp
        private static async Task<MapLayerDef> AddSymbolLayer(MapInterop mapInterop, IDataService dataService, SymbolLayerOptions? options = null)
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

            await mapInterop.Layers.CreateLayer(layerDef, layerDef.GetDataSource());

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
            
            await mapInterop.Configuration.ZoomTo(data[0], 11);

            return layerDef;
        }
````