using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Settings
{
    public enum MapAuthenticationMode
    {
        SubscriptionKey,
        Aad,
        Anonymous,
        Sas,
    }

    /// <summary>
    /// Authentication settings for the Azure Map.
    /// </summary>
    public class MapAuthentication
    {
        /// <summary>
        /// The authentication mechanism to be used.
        /// </summary>
        [JsonIgnore]
        public MapAuthenticationMode Mode { get; set; }

        [JsonInclude]
        internal string AuthType => Mode.ToJsonName();

        /// <summary>
        /// Subscription key from your Azure Maps account.
        /// Must be specified for subscription key authentication type.
        /// </summary>
        public string? SubscriptionKey { get; set; }

        /// <summary>
        /// The URL for the Shared Access Signature (SAS) token for your Azure Maps Account.
        /// If Mode = SasToken and this value is set, it will override the getAuthTokenCallback configured in App.Razor.
        /// </summary>
        public string? SasTokenUrl { get; set; }

        /// <summary>
        /// The Azure AD registered app ID. This is the app ID of an app registered in your Azure AD tenant.
        /// Must be specified for AAD authentication type.
        /// </summary>
        public string? AadAppId { get; set; }

        /// <summary>
        /// The AAD tenant that owns the registered app specified by 'aadAppId'.
        /// Must be specified for AAD authentication type.</summary>
        public string? AadTenant { get; set; }

        /// <summary>
        /// The Azure Maps client ID, This is an unique identifier used to identify the maps account.
        /// Preferred to always be specified, but must be specified for AAD and anonymous authentication types.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Indicates whether the authentication options are valid for the specified authentication type.
        /// </summary>
        /// <returns></returns>
        internal bool IsValid()
        {
            return Mode switch
            {
                MapAuthenticationMode.SubscriptionKey => !string.IsNullOrWhiteSpace(SubscriptionKey),
                MapAuthenticationMode.Sas => true, //Requires configuring JSInvokable GetSasToken method in App.Razor
                MapAuthenticationMode.Aad => !string.IsNullOrWhiteSpace(AadAppId) && !string.IsNullOrWhiteSpace(AadTenant),
                MapAuthenticationMode.Anonymous => !string.IsNullOrWhiteSpace(ClientId),
                _ => false
            };
        }

        internal bool IsNotValid() => !IsValid();

        internal string InValidMessage()
        {
            return Mode switch
            {
                MapAuthenticationMode.SubscriptionKey => string.IsNullOrWhiteSpace(SubscriptionKey)
                    ? "SubscriptionKey is required when authentication Mode is SubscriptionKey."
                    : "",
                MapAuthenticationMode.Sas => "", //Requires configuring JSInvokable GetSasToken method in App.Razor
                MapAuthenticationMode.Aad => string.IsNullOrWhiteSpace(AadAppId) || string.IsNullOrWhiteSpace(AadTenant)
                    ? "AadAppId and AadTenant are required when authentication Mode is Aad."
                    : "",
                MapAuthenticationMode.Anonymous => string.IsNullOrWhiteSpace(ClientId)
                    ? "ClientId is required when authentication Mode is Anonymous."
                    : "",
                _ => ""
            };
        }
    }
}

