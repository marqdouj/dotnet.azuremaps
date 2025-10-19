using Marqdouj.DotNet.AzureMaps.Map;
using Microsoft.Extensions.DependencyInjection;

namespace Marqdouj.DotNet.AzureMaps
{
    public static class Extensions
    {
        public static MapConfiguration AddMarqdoujAzureMaps(this IServiceCollection services, Action<MapConfiguration> config)
        {
            services
                .AddOptions<MapConfiguration>()
                .Configure(config)
                .Validate(c => c.IsValid, config.InValidMessage());

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
