## Add HeatMap Layer

````CSharp
        private static async Task<MapLayerDef> AddHeatMapLayer(MapInterop mapInterop, IDataService dataService)
        {
            var layerDef = new HeatMapLayerDef();

            var ds = layerDef.GetDataSource();
            ds.Url = await dataService.GetHeatMapLayerUrl();

            await mapInterop.Layers.CreateLayer(layerDef, ds);

            await mapInterop.Configuration.ZoomTo(new Position(-122.33, 47.6), 1);

            return layerDef;
        }
````
