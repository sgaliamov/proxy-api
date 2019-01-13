using System.Threading.Tasks;
using ProxyApi.Providers.Models;

namespace ProxyApi.Providers
{
    public sealed class CachedImagesProvider : ICachedImagesProvider
    {
        private readonly ICache _cache;
        private readonly IImagesProvider _provider;

        public CachedImagesProvider(IImagesProvider provider, ICache cache)
        {
            _provider = provider;
            _cache = cache;
        }

        public Task<Response> Search(int page, string text, Box box = null)
        {
            return _cache.GetAsync(
                $"search|{page}|{text}|{box?.ToBbox()}",
                () => _provider.Search(page, text, box));
        }

        public Task<Response> Info(string id)
        {
            return _cache.GetAsync(
                $"info|{id}",
                () => _provider.Info(id));
        }
    }
}
