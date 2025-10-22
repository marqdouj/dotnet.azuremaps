using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.UI
{
    public class HeatMapLayerUIModel : LayerUIModel<HeatMapLayerDef>, ICloneable
    {
        private readonly HeatMapLayerOptionsUIModel options;

        public HeatMapLayerUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            options = new(xmlService);
            Source = new();
        }

        public override HeatMapLayerDef? Source
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
            items.RemoveAll(e => e.Name == nameof(HeatMapLayerDef.Options));
            items.AddRange(options.ToUIList());

            return [.. items.OrderBy(e => e.SortOrder).ThenBy(e => e.NameDisplay)];
        }

        public object Clone()
        {
            var clone = (HeatMapLayerUIModel)this.MemberwiseClone();
            clone.Source = Source?.Clone() as HeatMapLayerDef;

            return clone;
        }

        public HeatMapLayerOptionsUIModel Options => options;
    }

    public class HeatMapLayerOptionsUIModel : SourceLayerOptionsUIModel<HeatMapLayerOptions>
    {
        internal HeatMapLayerOptionsUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
        }

        public IUIModelValue Color => GetItem(nameof(HeatMapLayerOptions.Color))!;
        public IUIModelValue? Intensity => GetItem(nameof(HeatMapLayerOptions.Intensity));
        public IUIModelValue? Opacity => GetItem(nameof(HeatMapLayerOptions.Opacity));
        public IUIModelValue? Radius => GetItem(nameof(HeatMapLayerOptions.Radius));
        public IUIModelValue? Weight => GetItem(nameof(HeatMapLayerOptions.Weight));
    }
}
