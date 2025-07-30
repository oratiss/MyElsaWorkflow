using AspNetCore.Authentication.ApiKey;
using System.Security.Claims;

namespace ElsaServer
{
    public class TafahomApiKeyProvider : IApiKeyProvider
    {
        private readonly ILogger<TafahomApiKeyProvider> _logger;

        public TafahomApiKeyProvider(ILogger<TafahomApiKeyProvider> logger)
        {
            _logger = logger;
        }

        public Task<IApiKey> ProvideAsync(string key)
        {
            _logger.LogInformation($"API Key provided: {key}");
            return Task.FromResult((IApiKey)new ApiKey(key));
        }
    }

    public class ApiKey(string key) : IApiKey
    {
        public string Key => key;
        public string OwnerName => "TaskManagementApplication";
        public IReadOnlyCollection<Claim> Claims => new[]
        {
            new Claim("permissions", "*"),
            new Claim("role", "admin"),
            new Claim("name", "admin"),
        };
    }
}
