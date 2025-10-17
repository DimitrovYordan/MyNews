using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyNews.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSourceLanguageCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 2,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 3,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 4,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 5,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 6,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 7,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 9,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 10,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 11,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 12,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 13,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 14,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 15,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 16,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 17,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 18,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 25,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 26,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 27,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 28,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 29,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 30,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 31,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 32,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 33,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 34,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 35,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 36,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 37,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 38,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 39,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 40,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 41,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 42,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 43,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 44,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 45,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 46,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 47,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 48,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 49,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 50,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 51,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 52,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 53,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 54,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 55,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 56,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 57,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 58,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 59,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 60,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 61,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 62,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 63,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 64,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 65,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 66,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 67,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 68,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 69,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 70,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 71,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 72,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 73,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 74,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 75,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 76,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 77,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 78,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 79,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 80,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 81,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 82,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 83,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 84,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 85,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 86,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 87,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 88,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 89,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 90,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 91,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 92,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 93,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 94,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 95,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 96,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 97,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 98,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 99,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 100,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 101,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 102,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 103,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 104,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 105,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1001,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1002,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1003,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1004,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1005,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1006,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1007,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1008,
                column: "LanguageCode",
                value: null);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 1009,
                column: "LanguageCode",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "Sources");
        }
    }
}
