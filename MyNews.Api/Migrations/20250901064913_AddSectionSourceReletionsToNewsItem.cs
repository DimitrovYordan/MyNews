using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyNews.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSectionSourceReletionsToNewsItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NewsItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "NewsItems",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "NewsItems",
                newName: "PublishedAt");

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "NewsItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "NewsItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "NewsItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Content", "PublishedAt", "SectionId", "SourceId", "Title" },
                values: new object[] { "Example news content", new DateTime(2025, 8, 20, 12, 10, 0, 0, DateTimeKind.Unspecified), 1, 1, "First News" });

            migrationBuilder.UpdateData(
                table: "NewsItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Content", "PublishedAt", "SectionId", "SourceId", "Title" },
                values: new object[] { "Example news content 2", new DateTime(2025, 8, 10, 12, 10, 0, 0, DateTimeKind.Unspecified), 2, 2, "First News 2" });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Sports" },
                    { 2, "Movies" }
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[,]
                {
                    { 1, "Sportal", "sportal.bg" },
                    { 2, "IMDB", "imdb.com" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsItems_SectionId",
                table: "NewsItems",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsItems_SourceId",
                table: "NewsItems",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsItems_Sections_SectionId",
                table: "NewsItems",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsItems_Sources_SourceId",
                table: "NewsItems",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsItems_Sections_SectionId",
                table: "NewsItems");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsItems_Sources_SourceId",
                table: "NewsItems");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_NewsItems_SectionId",
                table: "NewsItems");

            migrationBuilder.DropIndex(
                name: "IX_NewsItems_SourceId",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "NewsItems");

            migrationBuilder.RenameColumn(
                name: "PublishedAt",
                table: "NewsItems",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "NewsItems",
                newName: "Source");

            migrationBuilder.UpdateData(
                table: "NewsItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "Source", "Title" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 0, 0, 0, DateTimeKind.Unspecified), "System", "Welcome to MyNews!" });

            migrationBuilder.UpdateData(
                table: "NewsItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "Source", "Title" },
                values: new object[] { new DateTime(2025, 8, 30, 12, 5, 0, 0, DateTimeKind.Unspecified), "Demo", "Test News 1" });

            migrationBuilder.InsertData(
                table: "NewsItems",
                columns: new[] { "Id", "Date", "Source", "Title" },
                values: new object[] { 3, new DateTime(2025, 8, 20, 12, 10, 0, 0, DateTimeKind.Unspecified), "Demo", "Test News 2" });
        }
    }
}
