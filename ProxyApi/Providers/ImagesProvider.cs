using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace ProxyApi.Providers
{
    public sealed class ImagesProvider : IImagesProvider
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly UriBuilder _uriBuilder;

        public ImagesProvider(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["api_key"];
            _uriBuilder = new UriBuilder("https", "api.flickr.com", 443, "services/rest");
        }

        public async Task<HttpResponseMessage> Search(int page, string text, Box box = null)
        {
            var uri = BuildUri(page, text, box);

            return await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }

        private Uri BuildUri(int page, string text, Box box)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);

            parameters["method"] = "flickr.photos.search";
            parameters["api_key"] = _apiKey;
            parameters["format"] = "json";
            parameters["text"] = text;
            parameters["page"] = page.ToString();
            parameters["nojsoncallback"] = "1";

            if (box != null)
            {
                parameters["bbox"] = $"{box.MinLongitude},{box.MinLatitude},{box.MaxLongitude},{box.MaxLatitude}";
            }

            _uriBuilder.Query = parameters.ToString();

            return _uriBuilder.Uri;
        }
    }
}
