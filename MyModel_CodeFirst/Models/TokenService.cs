using Microsoft.Extensions.Caching.Memory;

namespace MyModel_CodeFirst.Models
{

    public class TokenService
    {
        private readonly IMemoryCache _cache;

        public TokenService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string CreateToken(string action, string value, TimeSpan expiration)
        {
            var token = Guid.NewGuid().ToString();
            var cacheKey = $"{action}:{value}:{token}";
            _cache.Set(cacheKey, true, expiration);
            return token;
        }
    }
}
