using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyNews.Api.Migrations
{
    /// <inheritdoc />
    public partial class DeleteBNRSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 8);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "Name", "Section", "Url" },
                values: new object[] { 8, "BNR", null, "https://bnr.bg/rss/" });
        }
    }
}
