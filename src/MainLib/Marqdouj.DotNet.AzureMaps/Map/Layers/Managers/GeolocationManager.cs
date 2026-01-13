using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Marqdouj.DotNet.AzureMaps.Map.Interop;

namespace Marqdouj.DotNet.AzureMaps.Map.Layers.Managers
{
    public class GeolocationManager
    {
        private readonly DataSourceDef dataSourceDef = new();
        private readonly SymbolLayerDef positionDef = new();
        private readonly PolygonLayerDef accuracyDef = new();

        public GeolocationManager()
        {
            positionDef.DataSource = dataSourceDef;

            //Render Point or MultiPoints in this layer.
            positionDef.Options!.Filter = new List<object>
            {
                "any",
                new List<object> { "==", new List<string> { "geometry-type" }, "Point" },
                new List<object> { "==", new List<string> { "geometry-type" }, "MultiPoint" }
            };

            accuracyDef.DataSource = dataSourceDef;
            accuracyDef.Options!.FillColor = "rgba(0, 153, 255, 0.5)";
        }

        /// <summary>
        /// Options for the Accuracy layer. If desired, customize this before adding the layers to the map.
        /// </summary>
        public PolygonLayerOptions AccuracyOptions => accuracyDef.Options!;

        /// <summary>
        /// Options for the Position layer. If desired, customize this before adding the layers to the map.
        /// </summary>
        public SymbolLayerOptions PositionOptions => positionDef.Options!;

        /// <summary>
        /// Adds the layers to be used with Geolocation positions.
        /// For customization, update the Position/Accuracy options before you create the layers.
        /// </summary>
        /// <param name="mapContainer"></param>
        /// <returns></returns>
        public async Task AddLayers(IAzureMapContainer mapContainer)
        {
            if (LayersAdded) return;
            await mapContainer.Layers.CreateLayers([positionDef, accuracyDef]);
            LayersAdded = true;
        }

        /// <summary>
        /// Removes the geolocation layers from the map.
        /// </summary>
        /// <param name="mapContainer"></param>
        /// <returns></returns>
        public async Task RemoveLayers(IAzureMapContainer mapContainer)
        {
            if (!LayersAdded) return;
            await mapContainer.Layers.RemoveLayers([positionDef, accuracyDef]);
            LayersAdded = false;
        }

        /// <summary>
        /// Indicates if the layers have been added to the map.
        /// </summary>
        public bool LayersAdded { get; private set; }

        /// <summary>
        /// Adds a geographic position to the Azure map container.
        /// </summary>
        /// <param name="mapContainer">The map container to which the position will be added. Cannot be null.</param>
        /// <param name="position">The geographic position to add to the map.</param>
        /// <param name="showAccuracy">true to display the accuracy radius for the position; otherwise, false. The default is true.</param>
        /// <param name="asShape">Indicates if the features will be added to the map as a Shape</param>
        /// <returns>A list of MapFeatureDefs that were added.</returns>
        public async Task<List<MapFeatureDef>> AddPosition(IAzureMapContainer mapContainer, Position position, bool showAccuracy = true, bool asShape = false)
        {
            return await AddPositions(mapContainer, [position], showAccuracy);
        }

        /// <summary>
        /// Adds a collection of geographic positions to the specified Azure map container as map features.
        /// </summary>
        /// <remarks>If showAccuracy is set to true and a position includes accuracy information, an
        /// additional feature representing the accuracy (typically as a circle) is added to the map for that
        /// position.</remarks>
        /// <param name="mapContainer">The map container to which the position features will be added. Cannot be null.</param>
        /// <param name="positions">The collection of positions to add as features on the map. Each position represents a geographic coordinate.
        /// Cannot be null.</param>
        /// <param name="showAccuracy">true to display an accuracy indicator (such as a circle) for each position with accuracy information;
        /// otherwise, false. The default is true.</param>
        /// <param name="asShape">Indicates if the features will be added to the map as a Shape</param>
        /// <returns>A list of MapFeatureDefs that were added.</returns>
        public async Task<List<MapFeatureDef>> AddPositions(IAzureMapContainer mapContainer, IEnumerable<Position> positions, bool showAccuracy = true, bool asShape = false)
        {
            var features = new List<MapFeatureDef>();

            foreach (var position in positions)
            {
                var point = new Point(new Position(position.Longitude, position.Latitude));
                var pointDef = new MapFeatureDef(point) { AsShape = asShape };
                pointDef.Properties ??= [];
                pointDef.Properties.Add("geolocationType", "position");

                features.Add(pointDef);

                if (showAccuracy && position.Accuracy != null)
                {
                    var accuracyDef = new MapFeatureDef(point);
                    accuracyDef.Properties ??= [];
                    accuracyDef.Properties.Add("geolocationType", "accuracy");
                    accuracyDef.Properties.Add("subType", "Circle");
                    accuracyDef.Properties.Add("radius", position.Accuracy);
                    features.Add(accuracyDef);
                }
            }

            await mapContainer.Layers.AddMapFeatures(features, dataSourceDef.Id!);

            return features;
        }

        /// <summary>
        /// Clears the datasource for the Geolocation layers.
        /// </summary>
        /// <param name="mapContainer"></param>
        /// <returns></returns>
        public async Task Clear(IAzureMapContainer mapContainer) 
        { 
            await mapContainer.Maps.ClearDatasource(dataSourceDef.Id!);
        }
    }
}
