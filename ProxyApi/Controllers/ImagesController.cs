using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProxyApi.Providers;
using ProxyApi.Providers.Models;

namespace ProxyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class ImagesController : ControllerBase
    {
        private readonly ICachedImagesProvider _imagesProvider;

        public ImagesController(ICachedImagesProvider imagesProvider)
        {
            _imagesProvider = imagesProvider;
        }

        /// <summary>
        ///     Return a list of photos matching some criteria.
        /// </summary>
        /// <param name="text">
        ///     A free text search. Photos who's title, description or tags contain the text will be returned. You
        ///     can exclude results that match a term by pre pending it with a - character.
        /// </param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="minLong">A valid latitude, in decimal format, for doing geo queries.</param>
        /// <param name="minLat">A valid longitude, in decimal format, for doing geo queries.</param>
        /// <param name="maxLong">A valid longitude, in decimal format, for doing geo queries.</param>
        /// <param name="maxLat">A valid latitude, in decimal format, for doing geo queries.</param>
        /// <returns>List of photos.</returns>
        [HttpGet]
        [Route("search")]
        public async Task Search(
            string text,
            int page = 1,
            float? minLong = null,
            float? minLat = null,
            float? maxLong = null,
            float? maxLat = null)
        {
            var box = Box.Create(minLong, minLat, maxLong, maxLat);

            var response = await _imagesProvider.Search(page, text, box).ConfigureAwait(false);

            await WriteResponse(response).ConfigureAwait(false);
        }

        /// <summary>
        ///     Get information about a photo. The calling user must have permission to view the photo.
        /// </summary>
        /// <param name="id">The id of the photo to get information for.</param>
        /// <returns>Information about a photo.</returns>
        [HttpGet]
        [Route("getInfo")]
        public async Task GetInfo(string id)
        {
            var response = await _imagesProvider.Info(id).ConfigureAwait(false);

            await WriteResponse(response).ConfigureAwait(false);
        }

        private async Task WriteResponse(Response response)
        {
            Response.StatusCode = response.StatusCode;
            Response.ContentType = response.ContentType;
            Response.ContentLength = response.Content.Length;

            await Response.WriteAsync(response.Content).ConfigureAwait(false);
        }
    }
}
