using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyNews.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateOfSources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Section",
                table: "Sources",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "Section", "Url" },
                values: new object[] { "Nova", 3, "https://nova.bg/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "Section", "Url" },
                values: new object[] { "BTV", 3, "https://btvnovinite.bg/" });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "Name", "Section", "Url" },
                values: new object[,]
                {
                    { 3, "Dnevnik", 3, "https://www.dnevnik.bg/" },
                    { 4, "Offnews", 3, "https://offnews.bg/" },
                    { 5, "Sportal", 5, "https://www.sportal.bg/" },
                    { 6, "Gong", 5, "https://gong.bg/" },
                    { 7, "24Chasa", 3, "https://www.24chasa.bg/" },
                    { 8, "Dir.bg", 3, "https://www.dir.bg/" },
                    { 9, "Investor", 4, "https://www.investor.bg/" },
                    { 10, "Vesti.bg", 3, "https://www.vesti.bg/" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DropColumn(
                name: "Section",
                table: "Sources");

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Sportal", "sportal.bg" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "Url" },
                values: new object[] { "IMDB", "imdb.com" });
        }
    }
}
