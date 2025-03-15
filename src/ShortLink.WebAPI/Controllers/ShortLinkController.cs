using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ShortLink.Core.Services;

namespace ShortLink.WebAPI.Controllers
{
    [ApiController]
    public class ShortLinkController(IShortLinkService shortLinkService) : ControllerBase
    {
        [HttpGet("{code}")]
        public async Task<IActionResult> RedirectToOriginalUrl(string code)
        {
            return Redirect(await shortLinkService.GetOriginalUrl(code));
        }

        [HttpPost("shorten/")]
        public async Task<IActionResult> CreateShortUrl([FromBody] [Url] string url)
        {
            var shortUrl = await shortLinkService.CreateShortUrl(url);
            return CreatedAtAction(nameof(RedirectToOriginalUrl), new { code = shortUrl.ShortCode }, shortUrl);
        }
    }
}