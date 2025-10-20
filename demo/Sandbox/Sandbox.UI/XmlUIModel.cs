using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.UI
{
    public class XmlUIModel<T> : UIModel<T> where T : class
    {
        protected private static Dictionary<string, string?> xmlDisplay = [];
        protected private static bool xmlWasNotSet = true;

        public XmlUIModel(IAzureMapsXmlService? xmlService)
        {
            if (xmlWasNotSet)
            {
                if (xmlService == null) return;
                xmlWasNotSet = false;
                xmlDisplay = xmlService?.GetSummary<T>() ?? [];
            }

            foreach (var item in Items)
            {
                item.UpdateDescription(xmlDisplay);
            }
        }
    }
}
