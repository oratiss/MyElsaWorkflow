using AspNetCore.Authentication.ApiKey;
using System.Security.Claims;

namespace ElsaServer
{
    public class MasoudApiKeyProvider : IApiKeyProvider
    {
        public Task<IApiKey> ProvideAsync(string key)
        {
            return Task.FromResult((IApiKey)new ApiKey(key));
        }
    }

    public class ApiKey(string key) : IApiKey
    {
        public string Key => key;

        public string OwnerName => "Masoud";

        public IReadOnlyCollection<Claim> Claims => [];
    }
}
