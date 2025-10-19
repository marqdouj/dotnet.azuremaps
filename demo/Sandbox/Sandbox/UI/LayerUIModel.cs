using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.UI
{
    internal interface ILayerUIModel
    {
        DataSourceOptionsUIModel? SourceOptionsUI { get; }
        DataSourceDef? GetDataSource();
    }

    internal abstract class LayerUIModel<T> : XmlUIModel<T>, ILayerUIModel where T : MapLayerDef
    {
        protected private static bool xmlSubWasNotSet = true;
        private readonly DataSourceOptionsUIModel sourceOptionsUI;

        protected LayerUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            sourceOptionsUI = new(xmlService);
            Id.ReadOnly = true;
            SourceId.ReadOnly = true;

            if (xmlSubWasNotSet)
            {
                if (xmlService == null) return;
                xmlSubWasNotSet = false;

                var xml = xmlService?.GetSummary<MapLayerDef>() ?? [];
                foreach (var item in xml)
                    xmlDisplay.Add(item.Key, item.Value);
            }
        }

        public DataSourceOptionsUIModel? SourceOptionsUI => sourceOptionsUI;
        public DataSourceDef? GetDataSource() => Source?.GetDataSource(SourceOptionsUI?.Source);

        public IUIModelValue Before => GetItem(nameof(MapLayerDef.Before))!;
        public IUIModelValue Id => GetItem(nameof(MapLayerDef.Id))!;
        public IUIModelValue SourceId => GetItem(nameof(MapLayerDef.SourceId))!;
    }
}
