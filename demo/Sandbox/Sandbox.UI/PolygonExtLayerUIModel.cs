using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.UI
{
    public class PolygonExtLayerUIModel : LayerUIModel<PolygonExtLayerDef>, ICloneable
    {
        private readonly PolygonExtrusionLayerOptionsUIModel options;

        public PolygonExtLayerUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            options = new(xmlService);
            Source = new();
        }

        public override PolygonExtLayerDef? Source
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
            items.RemoveAll(e => e.Name == nameof(PolygonExtLayerDef.Options));
            items.AddRange(options.ToUIList());

            return [.. items.OrderBy(e => e.SortOrder).ThenBy(e => e.NameDisplay)];
        }

        public object Clone()
        {
            var clone = (PolygonExtLayerUIModel)this.MemberwiseClone();
            clone.Source = Source?.Clone() as PolygonExtLayerDef;

            return clone;
        }

        public PolygonExtrusionLayerOptionsUIModel Options => options;
    }

    public class PolygonExtrusionLayerOptionsUIModel : SourceLayerOptionsUIModel<PolygonExtLayerOptions>
    {
        private readonly PixelUIModel translateUI;

        internal PolygonExtrusionLayerOptionsUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            translateUI = new(xmlService);
        }

        public IUIModelValue Base => GetItem(nameof(PolygonExtLayerOptions.Base))!;
        public IUIModelValue FillColor => GetItem(nameof(PolygonExtLayerOptions.FillColor))!;
        public IUIModelValue FillOpacity => GetItem(nameof(PolygonExtLayerOptions.FillOpacity))!;
        public IUIModelValue FillPattern => GetItem(nameof(PolygonExtLayerOptions.FillPattern))!;
        public IUIModelValue Height => GetItem(nameof(PolygonExtLayerOptions.Height))!;
        public IUIModelValue Translate => GetItem(nameof(PolygonExtLayerOptions.Translate))!;
        public PixelUIModel TranslateUI => translateUI;
        public IUIModelValue TranslateAnchor => GetItem(nameof(PolygonExtLayerOptions.TranslateAnchor))!;
        public IUIModelValue VerticalGradient => GetItem(nameof(PolygonExtLayerOptions.VerticalGradient))!;

    }
}
