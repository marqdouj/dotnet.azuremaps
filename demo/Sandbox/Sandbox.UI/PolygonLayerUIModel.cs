using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.UI
{
    public class PolygonLayerUIModel : LayerUIModel<PolygonLayerDef>, ICloneable
    {
        private readonly PolygonLayerOptionsUIModel options;

        public PolygonLayerUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            options = new(xmlService);
            Source = new();
        }

        public override PolygonLayerDef? Source
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
            items.RemoveAll(e => e.Name == nameof(PolygonLayerDef.Options));
            items.AddRange(options.ToUIList());

            return [.. items.OrderBy(e => e.SortOrder).ThenBy(e => e.NameDisplay)];
        }

        public object Clone()
        {
            var clone = (PolygonLayerUIModel)this.MemberwiseClone();
            clone.Source = Source?.Clone() as PolygonLayerDef;

            return clone;
        }

        public PolygonLayerOptionsUIModel Options => options;
    }

    public class PolygonLayerOptionsUIModel : SourceLayerOptionsUIModel<PolygonLayerOptions>
    {
        internal PolygonLayerOptionsUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
        }

        public IUIModelValue FillAntialias => GetItem(nameof(PolygonLayerOptions.FillAntialias))!;
        public IUIModelValue FillColor => GetItem(nameof(PolygonLayerOptions.FillColor))!;
        public IUIModelValue FillOpacity => GetItem(nameof(PolygonLayerOptions.FillOpacity))!;
        public IUIModelValue FillPattern => GetItem(nameof(PolygonLayerOptions.FillPattern))!;
    }
}
