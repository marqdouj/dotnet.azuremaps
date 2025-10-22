using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.UI
{
    public class TileLayerUIModel : LayerUIModel<TileLayerDef>, ICloneable
    {
        private readonly TileLayerOptionsUIModel options;

        public TileLayerUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            options = new(xmlService);
            Source = new();
        }

        public override TileLayerDef? Source
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
            items.RemoveAll(e => e.Name == nameof(TileLayerDef.Options));
            items.AddRange(options.ToUIList());

            return [.. items.OrderBy(e => e.SortOrder).ThenBy(e => e.NameDisplay)];
        }

        public object Clone()
        {
            var clone = (TileLayerUIModel)this.MemberwiseClone();
            clone.Source = Source?.Clone() as TileLayerDef;

            return clone;
        }

        public TileLayerOptionsUIModel Options => options;
    }

    public class TileLayerOptionsUIModel : MediaLayerOptionsUIModel<TileLayerOptions>
    {
        internal TileLayerOptionsUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
        }

        public IUIModelValue Bounds => GetItem(nameof(TileLayerOptions.Bounds))!;
        public IUIModelValue IsTMS => GetItem(nameof(TileLayerOptions.IsTMS))!;
        public IUIModelValue MinSourceZoom => GetItem(nameof(TileLayerOptions.MinSourceZoom))!;
        public IUIModelValue MaxSourceZoom => GetItem(nameof(TileLayerOptions.MaxSourceZoom))!;
        public IUIModelValue TileSize => GetItem(nameof(TileLayerOptions.TileSize))!;
        public IUIModelValue Subdomains => GetItem(nameof(TileLayerOptions.Subdomains))!;
        public IUIModelValue TileUrl => GetItem(nameof(TileLayerOptions.TileUrl))!;
    }
}
