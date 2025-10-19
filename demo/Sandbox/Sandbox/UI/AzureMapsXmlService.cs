using System.Text;
using System.Xml.Linq;

namespace Sandbox.UI
{
    public interface IAzureMapsXmlService 
    {
        string? FilePath { get; }
        bool Loaded { get; }
        Exception? LoadException { get; }
        Dictionary<string, string?> GetSummary<T>();
    }

    internal sealed class AzureMapsXmlService : IAzureMapsXmlService
    {
        private readonly XElement? xmlDoc;
        private readonly string assemblyName = "Marqdouj.DotNet.AzureMaps";

        public AzureMapsXmlService()
        {
            try
            {
                FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName}.xml");
                xmlDoc = XElement.Load(FilePath);
                Loaded = true;
            }
            catch (Exception ex)
            {
                LoadException = ex;
            }
        }

        public string? FilePath { get; }
        public bool Loaded { get; }
        public Exception? LoadException { get; }

        public Dictionary<string, string?> GetSummary<T>()
        {
            var summary = new Dictionary<string, string?>();
            var type = typeof(T);
            var fullName = type.FullName ?? "";
            var name = type.Name;

            if (xmlDoc == null)
                return summary;

            var nodes = xmlDoc.Descendants("member")
                .Where(e => e.Attribute("name")?.ToString()?.Contains(fullName, StringComparison.OrdinalIgnoreCase) ?? false);

            AddElementsToDictionary(summary, name, nodes);

            return summary;
        }

        private static void AddElementsToDictionary(Dictionary<string, string?> items, string typeName, IEnumerable<XElement> elements)
        {
            if (elements == null) return;

            foreach (var element in elements )
            {
                var attribName = element.Attribute("name")?.Value?.ToString() ?? "";
                var attribType = attribName[..2];
                string? key;

                switch (attribType)
                {
                    case "T:":
                        key = typeName;
                        break;
                    case "F:":
                    case "P:":
                        key = attribName[(attribName.LastIndexOf('.') + 1)..];
                        break;
                    default:
                        return;
                }

                var summary = element.Descendants("summary").FirstOrDefault()?.Value ?? "";
                var lines = summary.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var sb = new StringBuilder();

                foreach ( var line in lines )
                    sb.AppendLine(line);

                summary = sb.ToString();

                items.Add(key, summary);
            }
        }
    }
}
