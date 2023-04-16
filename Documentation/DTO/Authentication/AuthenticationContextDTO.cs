using System.Text.Json.Serialization;

namespace PeasieLib.DTO.Authentication
{
    // AuthenticationContextDTO myDeserializedClass = JsonSerializer.Deserialize<AuthenticationContextDTO>(myJsonResponse);

    // curl --location --request POST 'https://sandbox.openpayd.com/api/oauth/token?grant_type=client_credentials --header 'Authorization: Basic base64(username:password)' --header 'ContentDTO-Type: application/x-www-form-urlencoded'
    public class AuthenticationContextDTO
    {
        [JsonConstructor]
        public AuthenticationContextDTO(
            string accessToken,
            string tokenType,
            int? expiresIn,
            string scope,
            string accountHolderId,
            string clientId,
            string referralId,
            AccountHoldersDTO accountHolders,
            string accountHolderStatus,
            string clientTenantId,
            List<string> authorities,
            string jti,
            string accountHolderType
        )
        {
            AccessToken = accessToken;
            TokenType = tokenType;
            ExpiresIn = expiresIn;
            Scope = scope;
            AccountHolderId = accountHolderId;
            ClientId = clientId;
            ReferralId = referralId;
            AccountHolders = accountHolders;
            AccountHolderStatus = accountHolderStatus;
            ClientTenantId = clientTenantId;
            Authorities = authorities;
            Jti = jti;
            AccountHolderType = accountHolderType;
        }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; }

        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; }

        [JsonPropertyName("scope")]
        public string Scope { get; }

        [JsonPropertyName("accountHolderId")]
        public string AccountHolderId { get; }

        [JsonPropertyName("clientId")]
        public string ClientId { get; }

        [JsonPropertyName("referralId")]
        public string ReferralId { get; }

        [JsonPropertyName("accountHolders")]
        public AccountHoldersDTO AccountHolders { get; }

        [JsonPropertyName("accountHolderStatus")]
        public string AccountHolderStatus { get; }

        [JsonPropertyName("clientTenantId")]
        public string ClientTenantId { get; }

        [JsonPropertyName("authorities")]
        public IReadOnlyList<string> Authorities { get; }

        [JsonPropertyName("jti")]
        public string Jti { get; }

        [JsonPropertyName("accountHolderType")]
        public string AccountHolderType { get; }
    }


}
