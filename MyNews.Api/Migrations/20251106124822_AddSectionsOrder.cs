using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyNews.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSectionsOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1009);

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "UserSectionPreference",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "UserSectionPreference");

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "LanguageCode", "Name", "Section", "Url" },
                values: new object[,]
                {
                    { 1001, null, "Offnews - Политика", null, "https://feed.offnews.bg/rss/%D0%9F%D0%BE%D0%BB%D0%B8%D1%82%D0%B8%D0%BA%D0%B0_8" },
                    { 1002, null, "Offnews - Общество", null, "https://feed.offnews.bg/rss/%D0%9E%D0%B1%D1%89%D0%B5%D1%81%D1%82%D0%B2%D0%BE_4" },
                    { 1003, null, "Offnews - Икономика", null, "https://feed.offnews.bg/rss/%D0%98%D0%BA%D0%BE%D0%BD%D0%BE%D0%BC%D0%B8%D0%BA%D0%B0_59" },
                    { 1004, null, "Offnews - Темида", null, "https://feed.offnews.bg/rss/%D0%A2%D0%B5%D0%BC%D0%B8%D0%B4%D0%B0_18762" },
                    { 1005, null, "Offnews - Инциденти", null, "https://feed.offnews.bg/rss/%D0%98%D0%BD%D1%86%D0%B8%D0%B4%D0%B5%D0%BD%D1%82%D0%B8_6" },
                    { 1006, null, "Offnews - Медии", null, "https://feed.offnews.bg/rss/%D0%9C%D0%B5%D0%B4%D0%B8%D0%B8_73" },
                    { 1007, null, "Offnews - Свят", null, "https://feed.offnews.bg/rss/%D0%A1%D0%B2%D1%8F%D1%82%20_12" },
                    { 1008, null, "Offnews - Туризъм", null, "https://feed.offnews.bg/rss/%D0%A2%D1%83%D1%80%D0%B8%D0%B7%D1%8A%D0%BC_75" },
                    { 1009, null, "Offnews - Здраве", null, "https://feed.offnews.bg/rss/%D0%97%D0%B4%D1%80%D0%B0%D0%B2%D0%B5_18753" }
                });
        }
    }
}
