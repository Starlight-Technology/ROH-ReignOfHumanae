using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ROH.Context.File.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "GameFiles",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                Size = table.Column<long>(type: "bigint", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Path = table.Column<string>(type: "text", nullable: false),
                Format = table.Column<string>(type: "text", nullable: false),
                Active = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameFiles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "GameVersionFiles",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdVersion = table.Column<long>(type: "bigint", nullable: false),
                IdGameFile = table.Column<long>(type: "bigint", nullable: false),
                Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameVersionFiles", x => x.Id);
                table.ForeignKey(
                    name: "FK_GameVersionFiles_GameFiles_IdGameFile",
                    column: x => x.IdGameFile,
                    principalTable: "GameFiles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_GameVersionFiles_IdGameFile",
            table: "GameVersionFiles",
            column: "IdGameFile",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GameVersionFiles");

        migrationBuilder.DropTable(
            name: "GameFiles");
    }
}