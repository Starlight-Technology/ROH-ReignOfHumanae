using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ROH.Context.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUser = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    RealName = table.Column<string>(type: "text", nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

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
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Damage = table.Column<long>(type: "bigint", nullable: true),
                    Defense = table.Column<long>(type: "bigint", nullable: true),
                    ManaCost = table.Column<long>(type: "bigint", nullable: false),
                    Animation = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAccount = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Accounts_IdAccount",
                        column: x => x.IdAccount,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "AttackStatuses",
                columns: table => new
                {
                    IdCharacter = table.Column<long>(type: "bigint", nullable: false),
                    LongRangedWeaponLevel = table.Column<long>(type: "bigint", nullable: false),
                    MagicWeaponLevel = table.Column<long>(type: "bigint", nullable: false),
                    OneHandedWeaponLevel = table.Column<long>(type: "bigint", nullable: false),
                    TwoHandedWeaponLevel = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttackStatuses", x => x.IdCharacter);
                });

            migrationBuilder.CreateTable(
                name: "Champions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCharacter = table.Column<long>(type: "bigint", nullable: false),
                    IdKingdom = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Champions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterInventories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdItem = table.Column<long>(type: "bigint", nullable: false),
                    IdCharacter = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterInventories_Items_IdItem",
                        column: x => x.IdItem,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAccount = table.Column<long>(type: "bigint", nullable: false),
                    IdGuild = table.Column<long>(type: "bigint", nullable: true),
                    IdKingdom = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Race = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Accounts_IdAccount",
                        column: x => x.IdAccount,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Guilds_IdGuild",
                        column: x => x.IdGuild,
                        principalTable: "Guilds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "characterSkills",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCharacter = table.Column<long>(type: "bigint", nullable: false),
                    IdSkill = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_characterSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_characterSkills_Characters_IdCharacter",
                        column: x => x.IdCharacter,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_characterSkills_Skills_IdSkill",
                        column: x => x.IdSkill,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DefenseStatuses",
                columns: table => new
                {
                    IdCharacter = table.Column<long>(type: "bigint", nullable: false),
                    ArcaneDefenseLevel = table.Column<long>(type: "bigint", nullable: false),
                    DarknessDefenseLevel = table.Column<long>(type: "bigint", nullable: false),
                    EarthDefenseLevel = table.Column<long>(type: "bigint", nullable: false),
                    FireDefenseLevel = table.Column<long>(type: "bigint", nullable: false),
                    LightDefenseLevel = table.Column<long>(type: "bigint", nullable: false),
                    LightningDefenseLevel = table.Column<long>(type: "bigint", nullable: false),
                    MagicDefenseLevel = table.Column<long>(type: "bigint", nullable: false),
                    PhysicDefenseLevel = table.Column<long>(type: "bigint", nullable: false),
                    WaterDefenseLevel = table.Column<long>(type: "bigint", nullable: false),
                    WindDefenseLevel = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefenseStatuses", x => x.IdCharacter);
                    table.ForeignKey(
                        name: "FK_DefenseStatuses_Characters_IdCharacter",
                        column: x => x.IdCharacter,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipedItens",
                columns: table => new
                {
                    IdCharacter = table.Column<long>(type: "bigint", nullable: false),
                    IdArmor = table.Column<long>(type: "bigint", nullable: true),
                    IdHead = table.Column<long>(type: "bigint", nullable: true),
                    IdBoots = table.Column<long>(type: "bigint", nullable: true),
                    IdGloves = table.Column<long>(type: "bigint", nullable: true),
                    IdLegs = table.Column<long>(type: "bigint", nullable: true),
                    IdLeftBracelet = table.Column<long>(type: "bigint", nullable: true),
                    IdNecklace = table.Column<long>(type: "bigint", nullable: true),
                    IdRightBracelet = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipedItens", x => x.IdCharacter);
                    table.ForeignKey(
                        name: "FK_EquipedItens_Characters_IdCharacter",
                        column: x => x.IdCharacter,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipedItens_Items_IdArmor",
                        column: x => x.IdArmor,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipedItens_Items_IdBoots",
                        column: x => x.IdBoots,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipedItens_Items_IdGloves",
                        column: x => x.IdGloves,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipedItens_Items_IdHead",
                        column: x => x.IdHead,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipedItens_Items_IdLeftBracelet",
                        column: x => x.IdLeftBracelet,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipedItens_Items_IdLegs",
                        column: x => x.IdLegs,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipedItens_Items_IdNecklace",
                        column: x => x.IdNecklace,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipedItens_Items_IdRightBracelet",
                        column: x => x.IdRightBracelet,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Kingdoms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRuler = table.Column<long>(type: "bigint", nullable: false),
                    Reign = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kingdoms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kingdoms_Characters_IdRuler",
                        column: x => x.IdRuler,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MembersPositions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCharacter = table.Column<long>(type: "bigint", nullable: false),
                    IdGuild = table.Column<long>(type: "bigint", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembersPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembersPositions_Characters_IdCharacter",
                        column: x => x.IdCharacter,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembersPositions_Guilds_IdGuild",
                        column: x => x.IdGuild,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    IdCharacter = table.Column<long>(type: "bigint", nullable: false),
                    Level = table.Column<long>(type: "bigint", nullable: false),
                    MagicLevel = table.Column<long>(type: "bigint", nullable: false),
                    FullCarryWeight = table.Column<long>(type: "bigint", nullable: false),
                    CurrentCarryWeight = table.Column<long>(type: "bigint", nullable: false),
                    FullHealth = table.Column<long>(type: "bigint", nullable: false),
                    CurrentHealth = table.Column<long>(type: "bigint", nullable: false),
                    FullMana = table.Column<long>(type: "bigint", nullable: false),
                    CurrentMana = table.Column<long>(type: "bigint", nullable: false),
                    FullStamina = table.Column<long>(type: "bigint", nullable: false),
                    CurrentStamina = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.IdCharacter);
                    table.ForeignKey(
                        name: "FK_Statuses_Characters_IdCharacter",
                        column: x => x.IdCharacter,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RingsEquipped",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdEquippedItens = table.Column<long>(type: "bigint", nullable: false),
                    IdItem = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RingsEquipped", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RingsEquipped_EquipedItens_IdEquippedItens",
                        column: x => x.IdEquippedItens,
                        principalTable: "EquipedItens",
                        principalColumn: "IdCharacter",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RingsEquipped_Items_IdItem",
                        column: x => x.IdItem,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KingdomRelations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdKingdom = table.Column<long>(type: "bigint", nullable: false),
                    IdKingdom2 = table.Column<long>(type: "bigint", nullable: false),
                    Situation = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KingdomRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KingdomRelations_Kingdoms_IdKingdom2",
                        column: x => x.IdKingdom2,
                        principalTable: "Kingdoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Champions_IdCharacter",
                table: "Champions",
                column: "IdCharacter");

            migrationBuilder.CreateIndex(
                name: "IX_Champions_IdKingdom",
                table: "Champions",
                column: "IdKingdom");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterInventories_IdCharacter",
                table: "CharacterInventories",
                column: "IdCharacter");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterInventories_IdItem",
                table: "CharacterInventories",
                column: "IdItem");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_IdAccount",
                table: "Characters",
                column: "IdAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_IdGuild",
                table: "Characters",
                column: "IdGuild");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_IdKingdom",
                table: "Characters",
                column: "IdKingdom");

            migrationBuilder.CreateIndex(
                name: "IX_characterSkills_IdCharacter",
                table: "characterSkills",
                column: "IdCharacter");

            migrationBuilder.CreateIndex(
                name: "IX_characterSkills_IdSkill",
                table: "characterSkills",
                column: "IdSkill");

            migrationBuilder.CreateIndex(
                name: "IX_EquipedItens_IdArmor",
                table: "EquipedItens",
                column: "IdArmor");

            migrationBuilder.CreateIndex(
                name: "IX_EquipedItens_IdBoots",
                table: "EquipedItens",
                column: "IdBoots");

            migrationBuilder.CreateIndex(
                name: "IX_EquipedItens_IdGloves",
                table: "EquipedItens",
                column: "IdGloves");

            migrationBuilder.CreateIndex(
                name: "IX_EquipedItens_IdHead",
                table: "EquipedItens",
                column: "IdHead");

            migrationBuilder.CreateIndex(
                name: "IX_EquipedItens_IdLeftBracelet",
                table: "EquipedItens",
                column: "IdLeftBracelet");

            migrationBuilder.CreateIndex(
                name: "IX_EquipedItens_IdLegs",
                table: "EquipedItens",
                column: "IdLegs");

            migrationBuilder.CreateIndex(
                name: "IX_EquipedItens_IdNecklace",
                table: "EquipedItens",
                column: "IdNecklace");

            migrationBuilder.CreateIndex(
                name: "IX_EquipedItens_IdRightBracelet",
                table: "EquipedItens",
                column: "IdRightBracelet");

            migrationBuilder.CreateIndex(
                name: "IX_ItemEnchantments_IdEnchantment",
                table: "ItemEnchantments",
                column: "IdEnchantment");

            migrationBuilder.CreateIndex(
                name: "IX_ItemEnchantments_IdItem",
                table: "ItemEnchantments",
                column: "IdItem");

            migrationBuilder.CreateIndex(
                name: "IX_KingdomRelations_IdKingdom2",
                table: "KingdomRelations",
                column: "IdKingdom2");

            migrationBuilder.CreateIndex(
                name: "IX_Kingdoms_IdRuler",
                table: "Kingdoms",
                column: "IdRuler",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MembersPositions_IdCharacter",
                table: "MembersPositions",
                column: "IdCharacter",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MembersPositions_IdGuild",
                table: "MembersPositions",
                column: "IdGuild");

            migrationBuilder.CreateIndex(
                name: "IX_RingsEquipped_IdEquippedItens",
                table: "RingsEquipped",
                column: "IdEquippedItens");

            migrationBuilder.CreateIndex(
                name: "IX_RingsEquipped_IdItem",
                table: "RingsEquipped",
                column: "IdItem");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdAccount",
                table: "Users",
                column: "IdAccount",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AttackStatuses_Characters_IdCharacter",
                table: "AttackStatuses",
                column: "IdCharacter",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Champions_Characters_IdCharacter",
                table: "Champions",
                column: "IdCharacter",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Champions_Kingdoms_IdKingdom",
                table: "Champions",
                column: "IdKingdom",
                principalTable: "Kingdoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterInventories_Characters_IdCharacter",
                table: "CharacterInventories",
                column: "IdCharacter",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Kingdoms_IdKingdom",
                table: "Characters",
                column: "IdKingdom",
                principalTable: "Kingdoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kingdoms_Characters_IdRuler",
                table: "Kingdoms");

            migrationBuilder.DropTable(
                name: "AttackStatuses");

            migrationBuilder.DropTable(
                name: "Champions");

            migrationBuilder.DropTable(
                name: "CharacterInventories");

            migrationBuilder.DropTable(
                name: "characterSkills");

            migrationBuilder.DropTable(
                name: "DefenseStatuses");

            migrationBuilder.DropTable(
                name: "ItemEnchantments");

            migrationBuilder.DropTable(
                name: "KingdomRelations");

            migrationBuilder.DropTable(
                name: "MembersPositions");

            migrationBuilder.DropTable(
                name: "RingsEquipped");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Enchantments");

            migrationBuilder.DropTable(
                name: "EquipedItens");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Guilds");

            migrationBuilder.DropTable(
                name: "Kingdoms");
        }
    }
}
