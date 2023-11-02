using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ROH.Context.Migrations
{
    /// <inheritdoc />
    public partial class GuidMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValueSql: "newsequentialid()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValueSql: "newsequentialid()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Guilds",
                type: "uuid",
                nullable: false,
                defaultValueSql: "newsequentialid()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "VersionDate",
                table: "GameVersions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "GameVersions",
                type: "uuid",
                nullable: false,
                defaultValueSql: "newsequentialid()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "GameVersionFiles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "newsequentialid()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Characters",
                type: "uuid",
                nullable: false,
                defaultValueSql: "newsequentialid()");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Accounts",
                type: "uuid",
                nullable: false,
                defaultValueSql: "newsequentialid()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "GameVersions");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "GameVersionFiles");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Accounts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "VersionDate",
                table: "GameVersions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
