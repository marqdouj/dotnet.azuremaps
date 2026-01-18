using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop
{
    internal static class JsInteropExtensions
    {
        private static readonly JsonSerializerOptions serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static string SerializeToJson(this object model)
        {
            var json = JsonSerializer.Serialize(model, serializerOptions);
            return json;
        }
    }
}
