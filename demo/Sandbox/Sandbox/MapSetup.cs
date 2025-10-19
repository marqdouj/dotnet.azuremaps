// For Azure Maps Anonymous authentication
//using Microsoft.Identity.Client;
using Marqdouj.DotNet.AzureMaps;
using Marqdouj.DotNet.AzureMaps.Map;
using Marqdouj.DotNet.AzureMaps.Map.Options;

namespace Sandbox
{
    public static class MapSetup
    {
        private static MapConfiguration? mapConfiguration;
        private static string clientSecret = "";
        //private static readonly string authorityFormat = "https://login.microsoftonline.com/{0}/oauth2/v2.0";
        //private static readonly string graphScope = "https://atlas.microsoft.com/.default";

        public static IServiceCollection AddMapConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //User Secrets for local development; Azure Key Vault for Production?:
            //"AzureMaps": {
            //    "AuthenticationType": "SubscriptionKey",
            //    "AadAppId": "",
            //    "AadTenant": "",
            //    "ClientId": "",
            //    "ClientSecret": "",
            //    "SubscriptionKey": "[YOUR KEY]"
            //  }

            mapConfiguration = services.AddMarqdoujAzureMaps(config =>
            {
                ConfigureForSubscriptionKey(configuration, config);
                //ConfigureForAad(configuration, config);
                //ConfigureForAnonymous(configuration, config);
            });

            return services;
        }

        private static void ConfigureForSubscriptionKey(IConfiguration configuration, MapConfiguration config)
        {
            config.AuthOptions.AuthType = AuthType.SubscriptionKey;
            config.AuthOptions.SubscriptionKey = configuration["AzureMaps:SubscriptionKey"];
        }

        private static void ConfigureForAad(IConfiguration configuration, MapConfiguration config)
        {
            config.AuthOptions.AuthType = AuthType.Aad;
            config.AuthOptions.AadAppId = configuration["AzureMaps:AadAppId"];
            config.AuthOptions.AadTenant = configuration["AzureMaps:AadTenant"];
            config.AuthOptions.ClientId = configuration["AzureMaps:ClientId"];
        }

        private static void ConfigureForAnonymous(IConfiguration configuration, MapConfiguration config)
        {
            //NOTE: See GetAccessToken().
            config.AuthOptions.AuthType = AuthType.Anonymous;
            config.AuthOptions.AadAppId = configuration["AzureMaps:AadAppId"];
            config.AuthOptions.AadTenant = configuration["AzureMaps:AadTenant"];
            config.AuthOptions.ClientId = configuration["AzureMaps:ClientId"];
            clientSecret = configuration["AzureMaps:ClientSecret"] ?? "";
        }

        /// <summary>
        /// Only used for Anonymous authentication.
        /// Requires token callback be configured in App.razor.
        /// </summary>
        /// <returns></returns>
        //[JSInvokable]
        //public static async Task<string> GetAccessToken()
        //{
        //    var auth = mapConfiguration!.AuthOptions;

        //    IConfidentialClientApplication daemonClient;
        //    daemonClient = ConfidentialClientApplicationBuilder.Create(auth.AadAppId)
        //        .WithAuthority(string.Format(authorityFormat, auth.AadTenant))
        //        .WithClientSecret(clientSecret)
        //        .Build();
        //    AuthenticationResult authResult =
        //    await daemonClient.AcquireTokenForClient([graphScope]).ExecuteAsync();
        //    return authResult.AccessToken;
        //}
    }
}
