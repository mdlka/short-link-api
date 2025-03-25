using System.ComponentModel.DataAnnotations;

namespace ShortLink.WebAPI.DTO
{
    public class CreateShortUrlRequest
    {
        [Url, MaxLength(200)]
        public string Url { get; set; }
    }
}