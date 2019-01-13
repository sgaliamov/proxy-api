using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ProxyApi.Providers
{
    public sealed class Cache : ICache
    {
        private readonly ConnectionMultiplexer _redis;

        public Cache(IConfiguration configuration)
        {
            _redis = ConnectionMultiplexer.Connect(configuration["redis_configuration"]);
        }

        public async Task<T> Get<T>(string key, Func<T> addMethod = null)
        {
            var database = _redis.GetDatabase();
            var value = await database.StringGetAsync(key).ConfigureAwait(false);
            if (value.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            if (addMethod == null)
            {
                return default;
            }

            var toAdd = addMethod();
            var serialized = JsonConvert.SerializeObject(toAdd);
            await database.StringSetAsync(key, serialized).ConfigureAwait(false);

            return toAdd;
        }
    }
}
