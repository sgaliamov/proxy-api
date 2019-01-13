using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ProxyApi.Tests
{
    public sealed class ApiIntegrationTests
    {
        public ApiIntegrationTests()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            _httpClient = new HttpClient();
            _apiUrl = configuration["api_url"];
        }

        [Fact]
        public async Task Get_By_Id_Contains_Page_Url()
        {
            var expected = "https://www.flickr.com/photos/165144492@N02/43385281890/".Replace("/", "\\/");

            var response = await _httpClient.GetAsync($"{_apiUrl}/getInfo/?id=43385281890").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            content.Should().Contain(expected);
        }

        [Fact]
        public async Task Search_By_Coordinates_Contains_Amsterdam()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/search/?text=test&minLong=3.8285836&minLat=51.3546274&maxLong=5.8285836&maxLat=53.3546274").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            content.Should().Contain("\"stat\":\"ok\"");
            content.Should().Contain("Amsterdam");
        }

        [Fact]
        public async Task Search_By_Text_Returns_Status_Ok()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/search/?text=test+text").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            content.Should().Contain("\"stat\":\"ok\"");
        }

        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
    }
}
