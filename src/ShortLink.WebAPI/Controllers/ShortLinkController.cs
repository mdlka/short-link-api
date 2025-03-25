using Microsoft.AspNetCore.Mvc;
using ShortLink.Core.Services;
using ShortLink.WebAPI.DTO;

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
        public async Task<IActionResult> CreateShortUrl([FromBody] CreateShortUrlRequest request)
        {
            var shortUrl = await shortLinkService.CreateShortUrl(request.Url);
            return CreatedAtAction(nameof(RedirectToOriginalUrl), new { code = shortUrl.ShortCode }, shortUrl);
        }
    }
}