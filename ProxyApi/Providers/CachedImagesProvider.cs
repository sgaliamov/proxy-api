using System.Net.Http;
using System.Threading.Tasks;

namespace ProxyApi.Providers
{
    public sealed class CachedImagesProvider : IImagesProvider
    {
        private readonly ICache _cache;
        private readonly ImagesProvider _provider;

        public CachedImagesProvider(ImagesProvider provider, ICache cache)
        {
            _provider = provider;
            _cache = cache;
        }

        public Task<HttpResponseMessage> Search(int page, string text, Box box = null)
        {
            return _cache.GetAsync(
                $"search|{page}|{text}|{box}",
                async () =>
                {
                    var response = await _provider.Search(page, text, box).ConfigureAwait(false);

                    return await ReadContent(response).ConfigureAwait(false);
                });
        }

        public Task<HttpResponseMessage> Info(string id)
        {
            var info = _cache.GetAsync(
                $"info|{id}",
                async () =>
                {
                    var response = await _provider.Info(id).ConfigureAwait(false);

                    return await ReadContent(response).ConfigureAwait(false);
                });

            return info;
        }

        private static async Task<HttpResponseMessage> ReadContent(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseContent = new StringContent(content);
            response.Content = responseContent;

            return response;
        }
    }
}
