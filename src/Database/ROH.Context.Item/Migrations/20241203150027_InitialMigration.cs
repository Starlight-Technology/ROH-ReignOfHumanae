using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ROH.Context.Item.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Enchantments",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Damage = table.Column<long>(type: "bigint", nullable: true),
                Defense = table.Column<long>(type: "bigint", nullable: true),
                Animation = table.Column<string>(type: "text", nullable: true),
                Name = table.Column<string>(type: "text", nullable: false),
                Type = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Enchantments", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Items",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                Attack = table.Column<int>(type: "integer", nullable: true),
                Defense = table.Column<int>(type: "integer", nullable: true),
                Weight = table.Column<int>(type: "integer", nullable: false),
                Name = table.Column<string>(type: "text", nullable: true),
                Descricao = table.Column<string>(type: "text", nullable: true),
                Sprite = table.Column<string>(type: "text", nullable: true),
                File = table.Column<string>(type: "text", nullable: true),
                Format = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Items", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ItemEnchantments",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdItem = table.Column<long>(type: "bigint", nullable: false),
                IdEnchantment = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ItemEnchantments", x => x.Id);
                table.ForeignKey(
                    name: "FK_ItemEnchantments_Enchantments_IdEnchantment",
                    column: x => x.IdEnchantment,
                    principalTable: "Enchantments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ItemEnchantments_Items_IdItem",
                    column: x => x.IdItem,
                    principalTable: "Items",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ItemEnchantments_IdEnchantment",
            table: "ItemEnchantments",
            column: "IdEnchantment");

        migrationBuilder.CreateIndex(
            name: "IX_ItemEnchantments_IdItem",
            table: "ItemEnchantments",
            column: "IdItem");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ItemEnchantments");

        migrationBuilder.DropTable(
            name: "Enchantments");

        migrationBuilder.DropTable(
            name: "Items");
    }
}