using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShortLink.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "short_urls",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    original_url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    short_code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_short_urls", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_short_urls_original_url",
                table: "short_urls",
                column: "original_url");

            migrationBuilder.CreateIndex(
                name: "IX_short_urls_short_code",
                table: "short_urls",
                column: "short_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "short_urls");
        }
    }
}
