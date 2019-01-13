using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProxyApi.Models;

namespace ProxyApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public sealed class ImagesController : ControllerBase
    {
        private readonly ImagesProvider _imagesProvider;

        public ImagesController(ImagesProvider imagesProvider)
        {
            _imagesProvider = imagesProvider;
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult> Get(
            string text,
            int page = 1,
            int? minimumLongitude = null,
            int? minimumLatitude = null,
            int? maximumLongitude = null,
            int? maximumLatitude = null)
        {
            var box = Box.Create(minimumLongitude, minimumLatitude, maximumLongitude, maximumLatitude);

            var result = await _imagesProvider.Search(page, text, box).ConfigureAwait(false);

            return Ok(result);
        }
    }
}
