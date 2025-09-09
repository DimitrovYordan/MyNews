using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyNews.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "Name");
        }
    }
}
