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
        private readonly IImagesProvider _imagesProvider;

        public ImagesController(IImagesProvider imagesProvider)
        {
            _imagesProvider = imagesProvider;
        }

        [HttpGet]
        [Route("search")]
        public async Task Search(
            string text,
            int page = 1,
            int? minimumLongitude = null,
            int? minimumLatitude = null,
            int? maximumLongitude = null,
            int? maximumLatitude = null)
        {
            var box = Box.Create(minimumLongitude, minimumLatitude, maximumLongitude, maximumLatitude);

            var response = await _imagesProvider.Search(page, text, box).ConfigureAwait(false);

            await WriteResponse(response).ConfigureAwait(false);
        }

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
