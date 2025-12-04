namespace Marqdouj.DotNet.AzureMaps.Map.Layers
{
    public enum MapLayerType
    {
        /// <summary>
        /// Represents an unspecified or unrecognized value.
        /// </summary>
        Unknown,

        /// <summary>
        /// Renders Point objects as scalable circles (bubbles).
        /// </summary>
        Bubble,

        /// <summary>
        /// Represent the density of data using different colors (HeatMap).
        /// </summary>
        HeatMap,

        /// <summary>
        /// Overlays an image on the map with each corner anchored to a coordinate on the map. 
        /// Also known as a ground or image overlay.
        /// </summary>
        Image,

        /// <summary>
        /// Renders line data on the map. Can be used with SimpleLine, SimplePolygon,
        /// CirclePolygon, LineString, MultiLineString, Polygon, and MultiPolygon objects.
        /// </summary>
        Line,

        /// <summary>
        /// Renders filled Polygon and MultiPolygon objects on the map.
        /// </summary>
        Polygon,

        /// <summary>
        /// Renders extruded filled `Polygon` and `MultiPolygon` objects on the map.
        /// </summary>
        PolygonExtrusion,

        /// <summary>
        /// Renders point based data as symbols on the map using text and/or icons.
        /// Symbols can also be created for line and polygon data as well.
        /// </summary>
        Symbol,

        /// <summary>
        /// Renders raster tiled images on top of the map tiles.
        /// </summary>
        Tile,
    }
}
