using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ROH.Context.Migrations
{
    /// <inheritdoc />
    public partial class UserNameMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Accounts",
                type: "text",
                nullable: true);
        }
    }
}
