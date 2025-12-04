using Marqdouj.DotNet.AzureMaps.Map.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Settings
{
    /// <summary>
    /// Represents the configuration settings for map integration, including authentication and map-specific options.
    /// </summary>
    /// <remarks>Use this class to specify authentication credentials and additional options required for
    /// connecting to and displaying maps within an application. The settings provided by this class are typically
    /// required for initializing map services or components.</remarks>
    public class MapConfiguration
    {
        /// <summary>
        /// The authentication settings used for accessing map services.
        /// </summary>
        /// <remarks>Use this property to configure credentials or tokens required for map service
        /// requests. The settings specified here determine how authentication is handled when connecting to external
        /// map providers.</remarks>
        [JsonPropertyName("authOptions")]
        public MapAuthentication Authentication { get; set; } = new();

        /// <summary>
        /// The default global options used to configure the map's behavior and appearance.
        /// These options can be ovverridden at runtime as needed be assigning new values to the map component.
        /// </summary>
        /// <remarks>Assigning a value to this property allows customization of map features such as
        /// controls, display settings, and interaction modes. If set to <see langword="null"/>, default map options
        /// will be used.</remarks>
        public MapOptions? Options { get; set; }

        /// <summary>
        /// Gets or sets the minimum log level for messages to be recorded by the logger.
        /// </summary>
        /// <remarks>Messages with a severity lower than the specified log level will be ignored. Adjust
        /// this property to control the verbosity of logging output.</remarks>
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

#if DEBUG
        [JsonInclude]
        internal bool InDevelopment { get; } = true;
#endif

        internal bool IsValid => Authentication?.IsValid() ?? false;

        internal string ValidationMessage => GetValidateMessage();

        private string GetValidateMessage()
        {
            return Authentication == null
                ? "Map Authentication configuration is missing."
                : Authentication.InValidMessage();
        }
    }
}
