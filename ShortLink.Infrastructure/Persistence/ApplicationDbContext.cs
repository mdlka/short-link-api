using Microsoft.EntityFrameworkCore;
using ShortLink.Core.Models;

namespace ShortLink.Infrastructure.Persistence
{
    internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}