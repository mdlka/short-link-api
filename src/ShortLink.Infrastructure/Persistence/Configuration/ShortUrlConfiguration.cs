using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShortLink.Core.Models;

namespace ShortLink.Infrastructure.Persistence.Configuration
{
    public class ShortUrlConfiguration : IEntityTypeConfiguration<ShortUrl>
    {
        public void Configure(EntityTypeBuilder<ShortUrl> builder)
        {
            builder.ToTable("short_urls");

            builder.HasKey(s => s.Id);
            
            builder.Property(s => s.Id)
                .HasColumnName("id");

            builder.Property(s => s.OriginalUrl)
                .HasColumnName("original_url")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(s => s.ShortCode)
                .HasColumnName("short_code")
                .IsRequired();

            builder.HasIndex(s => s.OriginalUrl);
            
            builder.HasIndex(s => s.ShortCode)
                .IsUnique();
        }
    }
}