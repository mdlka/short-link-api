using Microsoft.EntityFrameworkCore;
using ShortLink.Core.Models;

namespace ShortLink.Core.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}