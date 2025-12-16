// For Azure Maps Anonymous authentication
//using Microsoft.Identity.Client;
using Marqdouj.DotNet.AzureMaps;
using Marqdouj.DotNet.AzureMaps.Map.Settings;

namespace Sandbox
{
    public static class MapSetup
    {
        private static MapConfiguration? mapConfiguration;
        //private static string clientSecret = "";
        //private static readonly string authorityFormat = "https://login.microsoftonline.com/{0}/oauth2/v2.0";
        //private static readonly string graphScope = "https://atlas.microsoft.com/.default";

        public static IServiceCollection AddMapConfiguration(this IServiceCollection services, IConfiguration configuration)
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
            config.Authentication.Mode = MapAuthenticationMode.SubscriptionKey;
            config.Authentication.SubscriptionKey = configuration["AzureMaps:SubscriptionKey"];

            //Optional settings:
            config.LogLevel = LogLevel.Debug;
        }

        //private static void ConfigureForAad(IConfiguration configuration, MapConfiguration config)
        //{
        //    config.Authentication.Mode = MapAuthenticationMode.Aad;
        //    config.Authentication.AadAppId = configuration["AzureMaps:AadAppId"];
        //    config.Authentication.AadTenant = configuration["AzureMaps:AadTenant"];
        //    config.Authentication.ClientId = configuration["AzureMaps:ClientId"];
        //}

        //private static void ConfigureForAnonymous(IConfiguration configuration, MapConfiguration config)
        //{
        //    //NOTE: See GetAccessToken().
        //    config.Authentication.Mode = MapAuthenticationMode.Anonymous;
        //    config.Authentication.AadAppId = configuration["AzureMaps:AadAppId"];
        //    config.Authentication.AadTenant = configuration["AzureMaps:AadTenant"];
        //    config.Authentication.ClientId = configuration["AzureMaps:ClientId"];
        //    clientSecret = configuration["AzureMaps:ClientSecret"] ?? "";
        //}

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
