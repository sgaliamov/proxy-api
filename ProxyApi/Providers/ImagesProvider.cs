using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using ProxyApi.Providers.Models;

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

        public async Task<Response> Search(int page, string text, Box box = null)
        {
            var uri = BuildSearchUri(page, text, box);

            return await GetResponse(uri).ConfigureAwait(false);
        }

        public async Task<Response> Info(string id)
        {
            var uri = BuildInfoUri(id);

            return await GetResponse(uri).ConfigureAwait(false);
        }

        private async Task<Response> GetResponse(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);

            return new Response
            {
                Content = await response.Content.ReadAsStringAsync().ConfigureAwait(false),
                ContentType = response.Content.Headers.ContentType.ToString(),
                StatusCode = (int)response.StatusCode
            };
        }

        private Uri BuildInfoUri(string id)
        {
            var parameters = BuildCommonParameters("flickr.photos.getInfo");
            parameters["photo_id"] = id;

            _uriBuilder.Query = parameters.ToString();

            return _uriBuilder.Uri;
        }

        private Uri BuildSearchUri(int page, string text, Box box)
        {
            var parameters = BuildCommonParameters("flickr.photos.search");
            parameters["text"] = text;
            parameters["page"] = page.ToString();

            if (box != null)
            {
                parameters["bbox"] = $"{box.MinLongitude},{box.MinLatitude},{box.MaxLongitude},{box.MaxLatitude}";
            }

            _uriBuilder.Query = parameters.ToString();

            return _uriBuilder.Uri;
        }

        private NameValueCollection BuildCommonParameters(string method)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);

            parameters["method"] = method;
            parameters["api_key"] = _apiKey;
            parameters["format"] = "json";
            parameters["nojsoncallback"] = "1";

            return parameters;
        }
    }
}
