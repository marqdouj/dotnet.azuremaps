using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.Components.Pages.Maps.UI
{
    internal static class Extensions
    {
        internal static bool IsReadOnly(this IUIModelValue item, bool readOnly)
        {
            return readOnly || item.ReadOnly;
        }
    }
}
