using Marqdouj.DotNet.AzureMaps.Map.Options;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.UI
{
    public class DataSourceOptionsUIModel : XmlUIModel<DataSourceOptions>
    {
        internal DataSourceOptionsUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            Buffer?.SetBindMinMax(0, null);
            ClusterMaxZoom?.SetBindMinMax(1, 24);
            ClusterMinPoints?.SetBindMinMax(2, null);
            ClusterRadius?.SetBindMinMax(0, 360);
            MaxZoom?.SetBindMinMax(1, 24);
            Tolerance?.SetBindMinMax(0, 1);
        }

        public IUIModelValue? Buffer => GetItem(nameof(DataSourceOptions.Buffer));
        public IUIModelValue? Cluster => GetItem(nameof(DataSourceOptions.Cluster));
        public IUIModelValue? ClusterMaxZoom => GetItem(nameof(DataSourceOptions.ClusterMaxZoom));
        public IUIModelValue? ClusterMinPoints => GetItem(nameof(DataSourceOptions.ClusterMinPoints));
        public IUIModelValue? ClusterRadius => GetItem(nameof(DataSourceOptions.ClusterRadius));
        public IUIModelValue? GenerateId => GetItem(nameof(DataSourceOptions.GenerateId));
        public IUIModelValue? LineMetrics => GetItem(nameof(DataSourceOptions.LineMetrics));
        public IUIModelValue? MaxZoom => GetItem(nameof(DataSourceOptions.MaxZoom));
        public IUIModelValue? PromoteId => GetItem(nameof(DataSourceOptions.PromoteId));
        public IUIModelValue? Tolerance => GetItem(nameof(DataSourceOptions.Tolerance));
    }
}
