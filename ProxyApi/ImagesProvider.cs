using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProxyApi.Dto;
using ProxyApi.Models;

namespace ProxyApi
{
    public sealed class ImagesProvider
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

        public async Task<PhotoCollection> Search(int page, string text, Box box = null)
        {
            var uri = BuildUri(page, text, box);

            var response = await Get(uri).ConfigureAwait(false);

            return await Deserialize<PhotoCollection>(response).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> Get(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }

        private static async Task<T> Deserialize<T>(HttpResponseMessage response)
        {
            using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return JsonSerializer.CreateDefault().Deserialize<T>(jsonReader);
            }
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
