using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.UI
{
    public class PixelUIModel(IAzureMapsXmlService? xmlService) : XmlUIModel<Pixel>(xmlService)
    {
        public IUIModelValue? X => GetItem(nameof(Pixel.X));
        public IUIModelValue? Y => GetItem(nameof(Pixel.Y));
    }

}
