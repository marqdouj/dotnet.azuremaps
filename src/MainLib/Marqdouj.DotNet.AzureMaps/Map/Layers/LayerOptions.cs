namespace Marqdouj.DotNet.AzureMaps.Map.Layers
{
    /// <summary>
    /// A base class which all other layer options inherit from.
    /// </summary>
    public class LayerOptions
    {
        /// <summary>
        /// An expression specifying conditions on source features.
        /// Only features that match the filter are displayed.
        /// </summary>
        public object? Filter { get; set; }

        /// <summary>
        /// An integer specifying the minimum zoom level to render the layer at.
        /// This value is inclusive, i.e. the layer will be visible at 'maxZoom > zoom >= minZoom'.
        /// Default '0'.
        /// </summary>
        public double? MinZoom { get; set; }

        /// <summary>
        /// An integer specifying the maximum zoom level to render the layer at.
        /// This value is exclusive, i.e. the layer will be visible at 'maxZoom > zoom >= minZoom'.
        /// Default '24'.
        /// </summary>
        public double? MaxZoom { get; set; }

        /// <summary>
        /// Specifies if the layer is visible or not.
        /// Default 'true'.
        /// </summary>
        public bool? Visible { get; set; }
    }

    public class SourceLayerOptions : LayerOptions
    {
        /// <summary>
        /// The id of a data source which the layer will render.
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// Required when the source of the layer is a VectorTileSource.
        /// A vector source can have multiple layers within it, this identifies which one to render in this layer.
        /// Prohibited for all other types of sources.
        /// </summary>
        public string? SourceLayer { get; set; }
    }
}
