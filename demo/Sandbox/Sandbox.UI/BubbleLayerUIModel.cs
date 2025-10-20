using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.UI
{
    public class BubbleLayerUIModel(IAzureMapsXmlService? xmlService) 
        : LayerUIModel<BubbleLayerDef>(xmlService), ICloneable
    {
        private readonly BubbleLayerOptionsUIModel options = new(xmlService);

        public override BubbleLayerDef? Source 
        { 
            get => base.Source; 
            set
            {
                base.Source = value;
                value?.Options ??= new();
                options.Source = value?.Options;
            }
        }

        public override List<IUIModelValue> ToUIList()
        {
            var items = base.ToUIList();
            items.RemoveAll(e => e.Name == nameof(BubbleLayerDef.Options));
            items.AddRange(options.ToUIList());

            return [.. items.OrderBy(e => e.SortOrder).ThenBy(e => e.NameDisplay)];
        }

        public object Clone()
        {
            var clone = (BubbleLayerUIModel)this.MemberwiseClone();
            clone.Source = Source?.Clone() as BubbleLayerDef;

            return clone;
        }

        public BubbleLayerOptionsUIModel Options => options;
    }

    public class BubbleLayerOptionsUIModel : SourceLayerOptionsUIModel<BubbleLayerOptions>
    {
        internal BubbleLayerOptionsUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
        }

        public IUIModelValue Color => GetItem(nameof(BubbleLayerOptions.Color))!;
        public IUIModelValue Blur => GetItem(nameof(BubbleLayerOptions.Blur))!;
        public IUIModelValue Opacity => GetItem(nameof(BubbleLayerOptions.Opacity))!;
        public IUIModelValue StrokeColor => GetItem(nameof(BubbleLayerOptions.StrokeColor))!;
        public IUIModelValue StrokeOpacity => GetItem(nameof(BubbleLayerOptions.StrokeOpacity))!;
        public IUIModelValue StrokeWidth => GetItem(nameof(BubbleLayerOptions.StrokeWidth))!;
        public IUIModelValue PitchAlignment => GetItem(nameof(BubbleLayerOptions.PitchAlignment))!;
        public IUIModelValue Radius => GetItem(nameof(BubbleLayerOptions.Radius))!;
    }
}
