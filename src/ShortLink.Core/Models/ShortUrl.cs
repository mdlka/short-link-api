namespace ShortLink.Core.Models
{
    public class ShortUrl
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortCode { get; set; }
    }
}