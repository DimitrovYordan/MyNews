using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyNews.Api.Migrations
{
    /// <inheritdoc />
    public partial class StartIntegrationChatGpt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NewsItems",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "NewsItems",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "NewsItems",
                newName: "Summary");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "NewsItems",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "NewsItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserNewsReads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNewsReads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNewsReads_NewsItems_NewsItemId",
                        column: x => x.NewsItemId,
                        principalTable: "NewsItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNewsReads_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsItems_Title_SourceId",
                table: "NewsItems",
                columns: new[] { "Title", "SourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserNewsReads_NewsItemId",
                table: "UserNewsReads",
                column: "NewsItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNewsReads_UserId1",
                table: "UserNewsReads",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserNewsReads");

            migrationBuilder.DropIndex(
                name: "IX_NewsItems_Title_SourceId",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "NewsItems");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "NewsItems",
                newName: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "NewsItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "NewsItems",
                columns: new[] { "Id", "Content", "PublishedAt", "Section", "SourceId", "Title" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Example news content", new DateTime(2025, 8, 20, 12, 10, 0, 0, DateTimeKind.Unspecified), 5, 1, "First News" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Example news content 2", new DateTime(2025, 8, 10, 12, 10, 0, 0, DateTimeKind.Unspecified), 12, 2, "First News 2" }
                });
        }
    }
}
