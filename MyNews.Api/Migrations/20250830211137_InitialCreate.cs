using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyNews.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "NewsItems",
                columns: new[] { "Id", "Date", "Source", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 10, 12, 0, 0, 0, DateTimeKind.Unspecified), "System", "Welcome to MyNews!" },
                    { 2, new DateTime(2025, 8, 30, 12, 5, 0, 0, DateTimeKind.Unspecified), "Demo", "Test News 1" },
                    { 3, new DateTime(2025, 8, 20, 12, 10, 0, 0, DateTimeKind.Unspecified), "Demo", "Test News 2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsItems");
        }
    }
}
