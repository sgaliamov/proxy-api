﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProxyApi.Providers;

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
        public async Task Get(
            string text,
            int page = 1,
            int? minimumLongitude = null,
            int? minimumLatitude = null,
            int? maximumLongitude = null,
            int? maximumLatitude = null)
        {
            var box = Box.Create(minimumLongitude, minimumLatitude, maximumLongitude, maximumLatitude);

            var response = await _imagesProvider.Search(page, text, box).ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Response.StatusCode = (int)response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType.ToString();
            Response.ContentLength = response.Content.Headers.ContentLength;
            await Response.WriteAsync(content).ConfigureAwait(false);
        }
    }
}
