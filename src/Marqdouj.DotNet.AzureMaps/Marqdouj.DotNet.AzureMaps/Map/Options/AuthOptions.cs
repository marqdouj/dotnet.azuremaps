using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    public enum AuthType
    {
        SubscriptionKey,
        Aad,
        Anonymous,
    }

    /// <summary>
    /// Authentication options for the Azure Map.
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// The authentication mechanism to be used.
        /// </summary>
        [JsonIgnore]
        public AuthType AuthType { get; set; }

        [JsonInclude]
        [JsonPropertyName("authType")]
        internal string AuthTypeJs => AuthType.ToJsonName();

        /// <summary>
        /// Subscription key from your Azure Maps account.
        /// Must be specified for subscription key authentication type.
        /// </summary>
        public string? SubscriptionKey { get; set; }

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
            return AuthType switch
            {
                AuthType.SubscriptionKey => !string.IsNullOrWhiteSpace(SubscriptionKey),
                AuthType.Aad => !string.IsNullOrWhiteSpace(AadAppId) && !string.IsNullOrWhiteSpace(AadTenant),
                AuthType.Anonymous => !string.IsNullOrWhiteSpace(ClientId),
                _ => false
            };
        }

        internal bool IsNotValid() => !IsValid();

        internal string InValidMessage()
        {
            return AuthType switch
            {
                AuthType.SubscriptionKey => string.IsNullOrWhiteSpace(SubscriptionKey)
                    ? "SubscriptionKey is required when AuthenticationType is SubscriptionKey."
                    : "",
                AuthType.Aad => string.IsNullOrWhiteSpace(AadAppId) || string.IsNullOrWhiteSpace(AadTenant)
                    ? "AadAppId and AadTenant are required when AuthenticationType is Aad."
                    : "",
                AuthType.Anonymous => string.IsNullOrWhiteSpace(ClientId)
                    ? "ClientId is required when AuthenticationType is Anonymous."
                    : "",
                _ => ""
            };
        }
    }
}

