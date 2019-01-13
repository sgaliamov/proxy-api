using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using ProxyApi.Providers;
using ProxyApi.Providers.Models;
using Xunit;

namespace ProxyApi.Tests
{
    public sealed class CacheTests
    {
        public CacheTests()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _fixture = new Fixture();

            _cache = new Cache(configuration);
        }

        [Fact]
        public async Task Add_And_Get_Value_Works()
        {
            var expected = _fixture.Create<Response>();
            var key = _fixture.Create<string>();

            var result1 = await _cache.GetAsync(key, () => Task.FromResult(expected)).ConfigureAwait(false);
            result1.Should().Be(expected);

            var result2 = await _cache.GetAsync<Response>(key, () => null).ConfigureAwait(false);
            result2.Should().BeEquivalentTo(expected);

            var result3 = await _cache.GetAsync<Response>(key).ConfigureAwait(false);
            result3.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Unknown_Key_Returns_Null()
        {
            var result = await _cache.GetAsync<object>("unknown").ConfigureAwait(false);
            result.Should().BeNull();
        }

        private readonly Cache _cache;
        private readonly Fixture _fixture;
    }
}
