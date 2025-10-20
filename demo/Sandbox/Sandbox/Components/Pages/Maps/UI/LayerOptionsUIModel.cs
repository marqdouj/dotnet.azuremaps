using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.Components.Pages.Maps.UI
{
    public abstract class LayerOptionsUIModel<T> : XmlUIModel<T> where T : LayerOptions
    {
        protected internal LayerOptionsUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            MaxZoom?.SetBindMinMax(0, 24);
            MinZoom?.SetBindMinMax(0, 24);
        }

        public IUIModelValue? MaxZoom => GetItem(nameof(LayerOptions.MaxZoom));
        public IUIModelValue? MinZoom => GetItem(nameof(LayerOptions.MinZoom));
        public IUIModelValue? Visible => GetItem(nameof(LayerOptions.Visible));
    }

    public abstract class MediaLayerOptionsUIModel<T> : LayerOptionsUIModel<T> where T : MediaLayerOptions
    {
        protected internal MediaLayerOptionsUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            Contrast?.SetBindMinMax(-1, 1);
            FadeDuration?.SetBindMinMax(0, int.MaxValue);
            HueRotation?.SetBindMinMax(-360, 360);
            MaxBrightness?.SetBindMinMax(0, 1);
            MinBrightness?.SetBindMinMax(0, 1);
            Opacity?.SetBindMinMax(0, 1);
            Saturation?.SetBindMinMax(-1, 1);
        }

        public IUIModelValue? Contrast => GetItem(nameof(MediaLayerOptions.Contrast));
        public IUIModelValue? FadeDuration => GetItem(nameof(MediaLayerOptions.FadeDuration));
        public IUIModelValue? HueRotation => GetItem(nameof(MediaLayerOptions.HueRotation));
        public IUIModelValue? MaxBrightness => GetItem(nameof(MediaLayerOptions.MaxBrightness));
        public IUIModelValue? MinBrightness => GetItem(nameof(MediaLayerOptions.MinBrightness));
        public IUIModelValue? Opacity => GetItem(nameof(MediaLayerOptions.Opacity));
        public IUIModelValue? Saturation => GetItem(nameof(MediaLayerOptions.Saturation));
    }

    public abstract class SourceLayerOptionsUIModel<T> : LayerOptionsUIModel<T> where T : SourceLayerOptions
    {
        protected internal SourceLayerOptionsUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            SourceId?.NameAlias = "SourceId";
            SourceLayerId?.NameAlias = "SourceLayerId";
        }

        public override List<IUIModelValue> ToUIList()
        {
            var items = base.ToUIList();
            items.RemoveAll(e => e.Name == nameof(SourceLayerOptions.Source));
            return items;
        }

        public IUIModelValue? SourceId => GetItem(nameof(SourceLayerOptions.Source));
        public IUIModelValue? SourceLayerId => GetItem(nameof(SourceLayerOptions.SourceLayer));
    }
}
