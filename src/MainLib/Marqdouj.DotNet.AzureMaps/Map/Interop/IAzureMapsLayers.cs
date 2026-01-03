using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.Layers;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop
{
    public interface IAzureMapsLayers
    {
        /// <summary>
        /// Adds a map feature to the specified data source.
        /// </summary>
        /// <param name="feature">The definition of the map feature to add. Cannot be null.</param>
        /// <param name="datasourceId">The identifier of the data source to which the feature will be added. Cannot be null or empty.</param>
        /// <param name="replace">If <see langword="true"/>, replaces any existing feature with the same identifier; otherwise, adds the
        /// feature without replacing.</param>
        Task AddMapFeature(MapFeatureDef feature, string datasourceId, bool replace = false);
        Task AddMapFeatures(List<MapFeatureDef> features, string datasourceId, bool replace = false);

        /// <summary>
        /// Creates a new map layer on the client using the specified layer definition.
        /// </summary>
        /// <param name="layerDef">The definition of the map layer to create.</param>
        /// <param name="events">MapEventDefs for the layer and it's datasource. 
        /// TargetId is not required; it will be resolved when creating the layer</param>
        Task CreateLayer(MapLayerDef layerDef, IEnumerable<MapEventDef>? events = null);
        Task CreateLayers(IEnumerable<MapLayerDef> layerDefs, IEnumerable<MapEventDef>? events = null);

        /// <summary>
        /// Removes the specified layer from the map.
        /// </summary>
        /// <param name="layerId">The unique identifier of the layer to remove. Cannot be null or empty.</param>
        Task RemoveLayer(string layerId);

        /// <summary>
        /// Removes a layer and its associated datasource from the map.
        /// </summary>
        /// <param name="layerDef"></param>
        /// <returns></returns>
        Task RemoveLayer(MapLayerDef layerDef);
        Task RemoveLayers(IEnumerable<MapLayerDef> layerDefS);
        Task UpdateMapFeature(MapFeatureDef feature, string datasourceId);
        Task UpdateMapFeatures(List<MapFeatureDef> features, string datasourceId);
    }
}
