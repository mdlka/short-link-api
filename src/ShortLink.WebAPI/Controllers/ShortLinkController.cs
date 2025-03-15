using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ShortLink.Core.Services;

namespace ShortLink.WebAPI.Controllers
{
    [ApiController]
    public class ShortLinkController(IShortLinkService shortLinkService) : ControllerBase
    {
        [HttpGet("{code}")]
        public async Task<string> GetOriginalUrl(string code)
        {
            return await shortLinkService.GetOriginalUrl(code);
        }

        [HttpPost("shorten/")]
        public async Task<IActionResult> CreateShortUrl([FromBody] [Url] string url)
        {
            var shortUrl = await shortLinkService.CreateShortUrl(url);
            return CreatedAtAction(nameof(GetOriginalUrl), new { code = shortUrl.ShortCode }, shortUrl);
        }
    }
}