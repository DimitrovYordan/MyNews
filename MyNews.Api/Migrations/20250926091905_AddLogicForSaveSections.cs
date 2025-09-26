using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyNews.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLogicForSaveSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 2);

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

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 18);

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

            migrationBuilder.CreateTable(
                name: "UserSectionPreferences",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionType = table.Column<int>(type: "int", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSectionPreferences", x => new { x.UserId, x.SectionType });
                    table.ForeignKey(
                        name: "FK_UserSectionPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSectionPreferences");

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "Name", "Section", "Url" },
                values: new object[,]
                {
                    { 1, "Dnevnik", null, "https://www.dnevnik.bg/rss/" },
                    { 2, "Novinite", null, "https://www.novinite.com/rss/news" },
                    { 3, "Capital", null, "https://www.capital.bg/rss/" },
                    { 4, "Mediapool", null, "https://www.mediapool.bg/rss" },
                    { 5, "Actualno", null, "https://www.actualno.com/rss" },
                    { 6, "24 Chasa", null, "https://www.24chasa.bg/rss_category/2/novini.html" },
                    { 7, "BTA", null, "https://www.bta.bg/bg/rss/free" },
                    { 8, "BNR", null, "https://bnr.bg/rss/" },
                    { 9, "SofiaGlobe", null, "http://feeds.feedburner.com/TheSofiaGlobe" },
                    { 10, "Pogled", null, "https://pogled.info/rss" },
                    { 11, "BurgasNews", null, "https://www.burgasnews.com/feed" },
                    { 12, "Gong", null, "https://gong.bg/rss" },
                    { 13, "Investor", null, "https://www.investor.bg/rss/news" },
                    { 14, "Vesti", null, "https://www.vesti.bg/rss" },
                    { 15, "Dnes", null, "https://www.dnes.bg/rss/news" },
                    { 16, "Blitz", null, "https://www.blitz.bg/rss" },
                    { 17, "Standart", null, "https://www.standartnews.com/rss" },
                    { 18, "Banker", null, "https://banker.bg/feed/" },
                    { 1001, "Offnews - Политика", null, "https://feed.offnews.bg/rss/%D0%9F%D0%BE%D0%BB%D0%B8%D1%82%D0%B8%D0%BA%D0%B0_8" },
                    { 1002, "Offnews - Общество", null, "https://feed.offnews.bg/rss/%D0%9E%D0%B1%D1%89%D0%B5%D1%81%D1%82%D0%B2%D0%BE_4" },
                    { 1003, "Offnews - Икономика", null, "https://feed.offnews.bg/rss/%D0%98%D0%BA%D0%BE%D0%BD%D0%BE%D0%BC%D0%B8%D0%BA%D0%B0_59" },
                    { 1004, "Offnews - Темида", null, "https://feed.offnews.bg/rss/%D0%A2%D0%B5%D0%BC%D0%B8%D0%B4%D0%B0_18762" },
                    { 1005, "Offnews - Инциденти", null, "https://feed.offnews.bg/rss/%D0%98%D0%BD%D1%86%D0%B8%D0%B4%D0%B5%D0%BD%D1%82%D0%B8_6" },
                    { 1006, "Offnews - Медии", null, "https://feed.offnews.bg/rss/%D0%9C%D0%B5%D0%B4%D0%B8%D0%B8_73" },
                    { 1007, "Offnews - Свят", null, "https://feed.offnews.bg/rss/%D0%A1%D0%B2%D1%8F%D1%82%20_12" },
                    { 1008, "Offnews - Туризъм", null, "https://feed.offnews.bg/rss/%D0%A2%D1%83%D1%80%D0%B8%D0%B7%D1%8A%D0%BC_75" },
                    { 1009, "Offnews - Здраве", null, "https://feed.offnews.bg/rss/%D0%97%D0%B4%D1%80%D0%B0%D0%B2%D0%B5_18753" }
                });
        }
    }
}
