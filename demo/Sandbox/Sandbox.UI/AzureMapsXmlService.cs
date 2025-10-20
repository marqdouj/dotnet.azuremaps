using LoxSmoke.DocXml;
using Microsoft.Extensions.Logging;

namespace Sandbox.UI
{
    public interface IAzureMapsXmlService 
    {
        string? FilePath { get; }
        bool Loaded { get; }
        Exception? LoadException { get; }
        Dictionary<string, string?> GetSummary<T>();
    }

    public sealed class AzureMapsXmlService : IAzureMapsXmlService
    {
        private readonly string assemblyName = "Marqdouj.DotNet.AzureMaps";
        private readonly DocXmlReader? reader;

        public AzureMapsXmlService(ILogger<AzureMapsXmlService> logger)
        {
            try
            {
                FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName}.xml");
                if (!File.Exists(FilePath)) 
                    throw new FileNotFoundException(FilePath);

                reader = new DocXmlReader(FilePath);
                Loaded = true;
            }
            catch (Exception ex)
            {
                LoadException = ex;
                logger.LogError(ex, null);
            }
        }

        public string? FilePath { get; }
        public bool Loaded { get; }
        public Exception? LoadException { get; }

        public Dictionary<string, string?> GetSummary<T>()
        {
            var summary = new Dictionary<string, string?>();
            var type = typeof(T);

            if (!Loaded)
                return summary;

            foreach (var info in type.GetMembers())
            {
                if (info.MemberType == System.Reflection.MemberTypes.Property)
                {
                    var comments = reader?.GetMemberComments(info);
                    summary.Add(info.Name, comments?.Summary);
                }
            }

            return summary;
        }
    }
}
