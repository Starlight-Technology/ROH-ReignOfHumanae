using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ROH.Context.Migrations;

/// <inheritdoc />
public partial class UserMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "Password",
            table: "Users");

        _ = migrationBuilder.AddColumn<byte[]>(
            name: "PasswordHash",
            table: "Users",
            type: "bytea",
            nullable: true);

        _ = migrationBuilder.AddColumn<byte[]>(
            name: "Salt",
            table: "Users",
            type: "bytea",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "PasswordHash",
            table: "Users");

        _ = migrationBuilder.DropColumn(
            name: "Salt",
            table: "Users");

        _ = migrationBuilder.AddColumn<string>(
            name: "Password",
            table: "Users",
            type: "text",
            nullable: true);
    }
}
