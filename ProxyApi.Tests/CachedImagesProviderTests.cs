using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using ProxyApi.Providers;
using ProxyApi.Providers.Models;
using Xunit;

namespace ProxyApi.Tests
{
    public sealed class CachedImagesProviderTests
    {
        public CachedImagesProviderTests()
        {
            _imagesProvider = new Mock<IImagesProvider>(MockBehavior.Strict);
            _cache = new Mock<ICache>(MockBehavior.Strict);
            _fixture = new Fixture();

            _provider = new CachedImagesProvider(_imagesProvider.Object, _cache.Object);
        }

        [Fact]
        public async Task Test_Info()
        {
            var id = _fixture.Create<string>();
            var key = $"info|{id}";
            var expected = _fixture.Create<Response>();

            // arrange
            SetupCache(key, expected);
            _imagesProvider.Setup(x => x.Info(id)).ReturnsAsync(expected);

            // act
            var result = await _provider.Info(id).ConfigureAwait(false);

            // assert
            Verify(result, expected);
        }

        [Fact]
        public async Task Test_Search()
        {
            var page = _fixture.Create<int>();
            var text = _fixture.Create<string>();
            var box = _fixture.Create<Box>();
            var key = $"search|{page}|{text}|{box.ToBbox()}";
            var expected = _fixture.Create<Response>();

            // arrange
            SetupCache(key, expected);
            _imagesProvider.Setup(x => x.Search(page, text, box)).ReturnsAsync(expected);

            // act
            var result = await _provider.Search(page, text, box).ConfigureAwait(false);

            // assert
            Verify(result, expected);
        }

        private void Verify(Response result, Response expected)
        {
            result.Should().BeEquivalentTo(expected);
            _cache.VerifyAll();
            _imagesProvider.VerifyAll();
        }

        private void SetupCache(string key, Response expected)
        {
            _cache.Setup(x => x.GetAsync(key, It.IsAny<Func<Task<Response>>>()))
                  .Callback(async (string _, Func<Task<Response>> f) =>
                  {
                      var response = await f().ConfigureAwait(false);
                      response.Should().BeEquivalentTo(expected);
                  })
                  .ReturnsAsync(expected);
        }

        private readonly Mock<IImagesProvider> _imagesProvider;
        private readonly Mock<ICache> _cache;
        private readonly CachedImagesProvider _provider;
        private readonly Fixture _fixture;
    }
}
