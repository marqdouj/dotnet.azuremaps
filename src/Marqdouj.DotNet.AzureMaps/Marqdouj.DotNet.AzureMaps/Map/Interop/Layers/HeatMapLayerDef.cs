using Marqdouj.DotNet.AzureMaps.Map.Options;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Interop.Layers
{
    public class HeatMapLayerDef : MapLayerDef
    {
        [JsonIgnore]
        public override MapLayerType Type => MapLayerType.HeatMap;

        public HeatMapLayerOptions? Options { get; set; } = new();

        public object Clone()
        {
            var clone = (HeatMapLayerDef)MemberwiseClone();
            clone.Options = (HeatMapLayerOptions?)Options?.Clone();
            clone.SourceOptions = (DataSourceOptions?)(clone.SourceOptions?.Clone());

            return clone;
        }
    }

    /// <summary>
    /// Options used when rendering Point objects in a HeatMapLayer.
    /// </summary>
    public class HeatMapLayerOptions : SourceLayerOptions, ICloneable
    {
        /// <summary>
        /// JSON array that specifies the color gradient used to colorize the pixels in the heatmap.
        /// This is defined using an expression that uses ["heatmap-density"] as input.
        /// Default ["interpolate",["linear"],["heatmap-density"],0,"rgba(0,0, 255,0)",0.1,"royalblue",0.3,"cyan",0.5,"lime",0.7,"yellow",1,"red"]
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// Similar to heatmap-weight but specifies the global heatmap intensity.
        /// The higher this value is, the more ‘weight’ each point will contribute to the appearance.
        /// Default '1'.
        /// </summary>
        public double? Intensity { get; set; }

        /// <summary>
        /// The opacity at which the heatmap layer will be rendered defined as a number between 0 and 1.
        /// Default '1'.
        /// </summary>
        public double? Opacity { get; set; }

        /// <summary>
        /// The radius in pixels used to render a data point on the heatmap.
        /// The radius must be a number greater or equal to 1.
        /// Default '30'.
        /// </summary>
        public double? Radius { get; set; }

        /// <summary>
        /// Specifies how much an individual data point contributes to the heatmap.
        /// Must be a number greater than 0. A value of 5 would be equivalent to having 5 points of weight 1 in the same spot.
        /// This is useful when clustering points to allow heatmap rendering or large datasets.
        /// Default '1'.
        /// </summary>
        public double? Weight { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
