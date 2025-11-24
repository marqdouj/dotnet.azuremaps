using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Common
{
    public class Properties : Dictionary<string, object> 
    {
        public override string ToString() => this.Any() ? JsonSerializer.Serialize(this) : "";
    }
}
