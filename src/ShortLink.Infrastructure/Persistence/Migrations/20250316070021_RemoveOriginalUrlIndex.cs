using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShortLink.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOriginalUrlIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_short_urls_original_url",
                table: "short_urls");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_short_urls_original_url",
                table: "short_urls",
                column: "original_url");
        }
    }
}
