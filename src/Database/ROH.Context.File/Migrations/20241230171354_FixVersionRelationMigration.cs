using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ROH.Context.File.Migrations;

/// <inheritdoc />
public partial class FixVersionRelationMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IdVersion",
            table: "GameVersionFiles");

        migrationBuilder.AddColumn<Guid>(
            name: "GuidVersion",
            table: "GameVersionFiles",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "GuidVersion",
            table: "GameVersionFiles");

        migrationBuilder.AddColumn<long>(
            name: "IdVersion",
            table: "GameVersionFiles",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);
    }
}
