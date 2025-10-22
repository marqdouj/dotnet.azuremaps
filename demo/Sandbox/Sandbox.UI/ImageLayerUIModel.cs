using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.UI
{
    public class ImageLayerUIModel : LayerUIModel<ImageLayerDef>, ICloneable
    {
        private readonly ImageLayerOptionsUIModel options;

        public ImageLayerUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            options = new(xmlService);
            Source = new();
        }

        public override ImageLayerDef? Source
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
            items.RemoveAll(e => e.Name == nameof(ImageLayerDef.Options));
            items.AddRange(options.ToUIList());

            return [.. items.OrderBy(e => e.SortOrder).ThenBy(e => e.NameDisplay)];
        }

        public object Clone()
        {
            var clone = (ImageLayerUIModel)this.MemberwiseClone();
            clone.Source = Source?.Clone() as ImageLayerDef;

            return clone;
        }

        public ImageLayerOptionsUIModel Options => options;
    }

    public class ImageLayerOptionsUIModel : MediaLayerOptionsUIModel<ImageLayerOptions>
    {
        internal ImageLayerOptionsUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            Url?.NameAlias = "Image URL";
        }

        public IUIModelValue? Coordinates => GetItem(nameof(ImageLayerOptions.Coordinates));
        public IUIModelValue? Url => GetItem(nameof(ImageLayerOptions.Url));
    }
}
