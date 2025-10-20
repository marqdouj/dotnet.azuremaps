using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.Components.Pages.Maps.UI
{
    public interface ILayerUIModel
    {
        DataSourceOptionsUIModel? SourceOptionsUI { get; }
        UIViewStyle ViewStyle { get; set; }
        DataSourceDef? GetDataSource();
    }

    public abstract class LayerUIModel<T> : XmlUIModel<T>, ILayerUIModel where T : MapLayerDef
    {
        private readonly DataSourceOptionsUIModel sourceOptionsUI;

        protected LayerUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            sourceOptionsUI = new(xmlService);
            Id.ReadOnly = true;
            SourceId.ReadOnly = true;
        }

        public UIViewStyle ViewStyle { get; set; }
        public DataSourceOptionsUIModel? SourceOptionsUI => sourceOptionsUI;
        public DataSourceDef? GetDataSource() => Source?.GetDataSource(SourceOptionsUI?.Source);

        public IUIModelValue Before => GetItem(nameof(MapLayerDef.Before))!;
        public IUIModelValue Id => GetItem(nameof(MapLayerDef.Id))!;
        public IUIModelValue SourceId => GetItem(nameof(MapLayerDef.SourceId))!;
    }
}
