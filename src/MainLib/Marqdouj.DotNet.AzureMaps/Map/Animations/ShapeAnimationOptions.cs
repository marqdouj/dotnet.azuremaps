using Marqdouj.DotNet.AzureMaps.Map.Layers;

namespace Marqdouj.DotNet.AzureMaps.Map.Animations
{
    public class ShapeAnimationOptions
    {
        public ShapeAnimationOptions(
            MapFeatureDef feature,
            string dataSourceId,
            AnimationAction action = AnimationAction.SetCoordinates,
            AnimationEasing easing = AnimationEasing.linear)
        {
            Feature = feature;
            DataSourceId = dataSourceId;
            Action = action.ToString();
            AnimationOptions = new PlayableAnimationOptions { AutoPlay = true, Easing = easing.ToString(), Duration = 1500 };
        }

        public string Action { get; } 
        public MapFeatureDef Feature { get; }
        public string DataSourceId { get; set; }
        public PlayableAnimationOptions AnimationOptions { get; set; }
    }
}
