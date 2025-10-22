using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.UI
{
    public class LineLayerUIModel : LayerUIModel<LineLayerDef>, ICloneable
    {
        private readonly LineLayerOptionsUIModel options;

        public LineLayerUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            options = new(xmlService);
            Source = new();
        }

        public override LineLayerDef? Source
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
            items.RemoveAll(e => e.Name == nameof(LineLayerDef.Options));
            items.AddRange(options.ToUIList());

            return [.. items.OrderBy(e => e.SortOrder).ThenBy(e => e.NameDisplay)];
        }

        public object Clone()
        {
            var clone = (LineLayerUIModel)this.MemberwiseClone();
            clone.Source = Source?.Clone() as LineLayerDef;

            return clone;
        }

        public LineLayerOptionsUIModel Options => options;
    }

    public class LineLayerOptionsUIModel : SourceLayerOptionsUIModel<LineLayerOptions>
    {
        private readonly PixelUIModel translateUI;

        internal LineLayerOptionsUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            translateUI = new(xmlService);
        }

        public IUIModelValue Blur => GetItem(nameof(LineLayerOptions.Blur))!;
        public IUIModelValue LineCap => GetItem(nameof(LineLayerOptions.LineCap))!;
        public IUIModelValue LineJoin => GetItem(nameof(LineLayerOptions.LineJoin))!;
        public IUIModelValue Offset => GetItem(nameof(LineLayerOptions.Offset))!;
        public IUIModelValue StrokeColor => GetItem(nameof(LineLayerOptions.StrokeColor))!;
        public IUIModelValue StrokeOpacity => GetItem(nameof(LineLayerOptions.StrokeOpacity))!;
        public IUIModelValue StrokeWidth => GetItem(nameof(LineLayerOptions.StrokeWidth))!;
        public IUIModelValue Translate => GetItem(nameof(LineLayerOptions.Translate))!;
        public PixelUIModel TranslateUI => translateUI;
        public IUIModelValue TranslateAnchor => GetItem(nameof(LineLayerOptions.TranslateAnchor))!;
    }
}
