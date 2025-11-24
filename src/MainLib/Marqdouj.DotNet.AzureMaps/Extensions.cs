using Marqdouj.DotNet.AzureMaps.Map;
using Microsoft.Extensions.DependencyInjection;

namespace Marqdouj.DotNet.AzureMaps
{
    public static class Extensions
    {
        /// <summary>
        /// Adds and configures Marqdouj Azure Maps services to the specified service collection, optionally enabling
        /// configuration validation.
        /// </summary>
        /// <remarks>If <paramref name="addValidation"/> is set to <see langword="true"/>, the
        /// configuration will be validated using the <see cref="MapConfiguration.IsValid"/> property. An invalid
        /// configuration may result in application startup failure due to options validation.
        /// When <see langword="false"/> (default) the application will run but an error/validation message will be shown instead of the map.
        /// </remarks>
        /// <param name="services">The service collection to which the Azure Maps configuration and services will be added.</param>
        /// <param name="config">A delegate that configures the <see cref="MapConfiguration"/> instance used for Azure Maps integration.</param>
        /// <param name="addValidation">Specifies whether to enable validation of the <see cref="MapConfiguration"/> during configuration. Set to
        /// <see langword="true"/> to validate the configuration; otherwise, <see langword="false"/>.</param>
        /// <returns>The configured <see cref="MapConfiguration"/> instance for Azure Maps.</returns>
        public static MapConfiguration AddMarqdoujAzureMaps(this IServiceCollection services, Action<MapConfiguration> config, bool addValidation = false)
        {
            if (addValidation)
            {
                services
                    .AddOptions<MapConfiguration>()
                    .Configure(config)
                    .Validate(c => c.IsValid, config.InValidMessage());
            }
            else
            {
                services
                    .AddOptions<MapConfiguration>()
                    .Configure(config);
            }

            return config.GetConfiguration();  
        }

        private static string InValidMessage(this Action<MapConfiguration> config)
        {
            var c = config.GetConfiguration();
            return c.ValidationMessage;
        }

        private static MapConfiguration GetConfiguration(this Action<MapConfiguration> config)
        {
            var c = new MapConfiguration();
            config.Invoke(c);
            return c;
        }
    }
}
