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
            _ = migrationBuilder.CreateTable(
              name: "Accounts",
              columns: table => new
              {
                  Id = table.Column<long>(type: "bigint", nullable: false)
                      .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  IdUser = table.Column<long>(type: "bigint", nullable: false),
                  Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                  UserName = table.Column<string>(type: "text", nullable: true),
                  RealName = table.Column<string>(type: "text", nullable: true),
                  BirthDate = table.Column<DateOnly>(type: "date", nullable: false)
              },
              constraints: table =>
              {
                  _ = table.PrimaryKey("PK_Accounts", x => x.Id);
              });

            _ = migrationBuilder.CreateTable(
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
                  _ = table.PrimaryKey("PK_Enchantments", x => x.Id);
              });

            _ = migrationBuilder.CreateTable(
              name: "GameVersions",
              columns: table => new
              {
                  Id = table.Column<long>(type: "bigint", nullable: false)
                      .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                  VersionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                  Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                  Version = table.Column<int>(type: "integer", nullable: false),
                  Release = table.Column<int>(type: "integer", nullable: false),
                  Review = table.Column<int>(type: "integer", nullable: false),
                  Released = table.Column<bool>(type: "boolean", nullable: false)
              },
              constraints: table =>
              {
                  _ = table.PrimaryKey("PK_GameVersions", x => x.Id);
              });

            _ = migrationBuilder.CreateTable(
              name: "Guilds",
              columns: table => new
              {
                  Id = table.Column<long>(type: "bigint", nullable: false)
                      .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                  Name = table.Column<string>(type: "text", nullable: false),
                  Description = table.Column<string>(type: "text", nullable: false)
              },
              constraints: table =>
              {
                  _ = table.PrimaryKey("PK_Guilds", x => x.Id);
              });

            _ = migrationBuilder.CreateTable(
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
                  _ = table.PrimaryKey("PK_Items", x => x.Id);
              });

            _ = migrationBuilder.CreateTable(
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
                  _ = table.PrimaryKey("PK_Skills", x => x.Id);
              });

            _ = migrationBuilder.CreateTable(
              name: "Users",
              columns: table => new
              {
                  Id = table.Column<long>(type: "bigint", nullable: false)
                      .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  IdAccount = table.Column<long>(type: "bigint", nullable: false),
                  Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                  Email = table.Column<string>(type: "text", nullable: true),
                  Password = table.Column<string>(type: "text", nullable: true)
              },
              constraints: table =>
              {
                  _ = table.PrimaryKey("PK_Users", x => x.Id);
                  _ = table.ForeignKey(
                      name: "FK_Users_Accounts_IdAccount",
                      column: x => x.IdAccount,
                      principalTable: "Accounts",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

            _ = migrationBuilder.CreateTable(
              name: "GameVersionFiles",
              columns: table => new
              {
                  Id = table.Column<long>(type: "bigint", nullable: false)
                      .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  IdVersion = table.Column<long>(type: "bigint", nullable: false),
                  Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                  Size = table.Column<long>(type: "bigint", nullable: false),
                  Name = table.Column<string>(type: "text", nullable: false),
                  Path = table.Column<string>(type: "text", nullable: false),
                  Format = table.Column<string>(type: "text", nullable: false)
              },
              constraints: table =>
              {
                  _ = table.PrimaryKey("PK_GameVersionFiles", x => x.Id);
                  _ = table.ForeignKey(
                      name: "FK_GameVersionFiles_GameVersions_IdVersion",
                      column: x => x.IdVersion,
                      principalTable: "GameVersions",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

            _ = migrationBuilder.CreateTable(
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
                  _ = table.PrimaryKey("PK_ItemEnchantments", x => x.Id);
                  _ = table.ForeignKey(
                      name: "FK_ItemEnchantments_Enchantments_IdEnchantment",
                      column: x => x.IdEnchantment,
                      principalTable: "Enchantments",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                  _ = table.ForeignKey(
                      name: "FK_ItemEnchantments_Items_IdItem",
                      column: x => x.IdItem,
                      principalTable: "Items",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

            _ = migrationBuilder.CreateTable(
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
                  _ = table.PrimaryKey("PK_AttackStatuses", x => x.IdCharacter);
              });

            _ = migrationBuilder.CreateTable(
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
                  _ = table.PrimaryKey("PK_Champions", x => x.Id);
              });

            _ = migrationBuilder.CreateTable(
              name: "CharacterInventory",
              columns: table => new
              {
                  Id = table.Column<long>(type: "bigint", nullable: false)
                      .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  IdItem = table.Column<long>(type: "bigint", nullable: false),
                  IdCharacter = table.Column<long>(type: "bigint", nullable: false)
              },
              constraints: table =>
              {
                  _ = table.PrimaryKey("PK_CharacterInventory", x => x.Id);
                  _ = table.ForeignKey(
                      name: "FK_CharacterInventory_Items_IdItem",
                      column: x => x.IdItem,
                      principalTable: "Items",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

            _ = migrationBuilder.CreateTable(
              name: "Characters",
              columns: table => new
              {
                  Id = table.Column<long>(type: "bigint", nullable: false)
                      .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  IdAccount = table.Column<long>(type: "bigint", nullable: false),
                  IdGuild = table.Column<long>(type: "bigint", nullable: true),
                  IdKingdom = table.Column<long>(type: "bigint", nullable: false),
                  Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                  Name = table.Column<string>(type: "text", nullable: true),
                  Race = table.Column<int>(type: "integer", nullable: false),
                  DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
              },
              constraints: table =>
              {
                  _ = table.PrimaryKey("PK_Characters", x => x.Id);
                  _ = table.ForeignKey(
                      name: "FK_Characters_Accounts_IdAccount",
                      column: x => x.IdAccount,
                      principalTable: "Accounts",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                  _ = table.ForeignKey(
                      name: "FK_Characters_Guilds_IdGuild",
                      column: x => x.IdGuild,
                      principalTable: "Guilds",
                      principalColumn: "Id");
              });

            _ = migrationBuilder.CreateTable(
              name: "CharacterSkills",
              columns: table => new
              {
                  Id = table.Column<long>(type: "bigint", nullable: false)
                      .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  IdCharacter = table.Column<long>(type: "bigint", nullable: false),
                  IdSkill = table.Column<long>(type: "bigint", nullable: false)
              },
              constraints: table =>
              {
                  _ = table.PrimaryKey("PK_CharacterSkills", x => x.Id);
                  _ = table.ForeignKey(
                      name: "FK_CharacterSkills_Characters_IdCharacter",
                      column: x => x.IdCharacter,
                      principalTable: "Characters",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                  _ = table.ForeignKey(
                      name: "FK_CharacterSkills_Skills_IdSkill",
                      column: x => x.IdSkill,
                      principalTable: "Skills",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

            _ = migrationBuilder.CreateTable(
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
                  _ = table.PrimaryKey("PK_DefenseStatuses", x => x.IdCharacter);
                  _ = table.ForeignKey(
                      name: "FK_DefenseStatuses_Characters_IdCharacter",
                      column: x => x.IdCharacter,
                      principalTable: "Characters",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

            _ = migrationBuilder.CreateTable(
              name: "EquippedItems",
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
                  _ = table.PrimaryKey("PK_EquippedItems", x => x.IdCharacter);
                  _ = table.ForeignKey(
                      name: "FK_EquippedItems_Characters_IdCharacter",
                      column: x => x.IdCharacter,
                      principalTable: "Characters",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                  _ = table.ForeignKey(
                      name: "FK_EquippedItems_Items_IdArmor",
                      column: x => x.IdArmor,
                      principalTable: "Items",
                      principalColumn: "Id");
                  _ = table.ForeignKey(
                      name: "FK_EquippedItems_Items_IdBoots",
                      column: x => x.IdBoots,
                      principalTable: "Items",
                      principalColumn: "Id");
                  _ = table.ForeignKey(
                      name: "FK_EquippedItems_Items_IdGloves",
                      column: x => x.IdGloves,
                      principalTable: "Items",
                      principalColumn: "Id");
                  _ = table.ForeignKey(
                      name: "FK_EquippedItems_Items_IdHead",
                      column: x => x.IdHead,
                      principalTable: "Items",
                      principalColumn: "Id");
                  _ = table.ForeignKey(
                      name: "FK_EquippedItems_Items_IdLeftBracelet",
                      column: x => x.IdLeftBracelet,
                      principalTable: "Items",
                      principalColumn: "Id");
                  _ = table.ForeignKey(
                      name: "FK_EquippedItems_Items_IdLegs",
                      column: x => x.IdLegs,
                      principalTable: "Items",
                      principalColumn: "Id");
                  _ = table.ForeignKey(
                      name: "FK_EquippedItems_Items_IdNecklace",
                      column: x => x.IdNecklace,
                      principalTable: "Items",
                      principalColumn: "Id");
                  _ = table.ForeignKey(
                      name: "FK_EquippedItems_Items_IdRightBracelet",
                      column: x => x.IdRightBracelet,
                      principalTable: "Items",
                      principalColumn: "Id");
              });

            _ = migrationBuilder.CreateTable(
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
                  _ = table.PrimaryKey("PK_Kingdoms", x => x.Id);
                  _ = table.ForeignKey(
                      name: "FK_Kingdoms_Characters_IdRuler",
                      column: x => x.IdRuler,
                      principalTable: "Characters",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

            _ = migrationBuilder.CreateTable(
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
                  _ = table.PrimaryKey("PK_MembersPositions", x => x.Id);
                  _ = table.ForeignKey(
                      name: "FK_MembersPositions_Characters_IdCharacter",
                      column: x => x.IdCharacter,
                      principalTable: "Characters",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                  _ = table.ForeignKey(
                      name: "FK_MembersPositions_Guilds_IdGuild",
                      column: x => x.IdGuild,
                      principalTable: "Guilds",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

            _ = migrationBuilder.CreateTable(
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
                  _ = table.PrimaryKey("PK_Statuses", x => x.IdCharacter);
                  _ = table.ForeignKey(
                      name: "FK_Statuses_Characters_IdCharacter",
                      column: x => x.IdCharacter,
                      principalTable: "Characters",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

            _ = migrationBuilder.CreateTable(
              name: "RingsEquipped",
              columns: table => new
              {
                  Id = table.Column<long>(type: "bigint", nullable: false)
                      .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                  IdEquippedItems = table.Column<long>(type: "bigint", nullable: false),
                  IdItem = table.Column<long>(type: "bigint", nullable: false)
              },
              constraints: table =>
              {
                  _ = table.PrimaryKey("PK_RingsEquipped", x => x.Id);
                  _ = table.ForeignKey(
                      name: "FK_RingsEquipped_EquippedItems_IdEquippedItems",
                      column: x => x.IdEquippedItems,
                      principalTable: "EquippedItems",
                      principalColumn: "IdCharacter",
                      onDelete: ReferentialAction.Cascade);
                  _ = table.ForeignKey(
                      name: "FK_RingsEquipped_Items_IdItem",
                      column: x => x.IdItem,
                      principalTable: "Items",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

            _ = migrationBuilder.CreateTable(
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
                  _ = table.PrimaryKey("PK_KingdomRelations", x => x.Id);
                  _ = table.ForeignKey(
                      name: "FK_KingdomRelations_Kingdoms_IdKingdom2",
                      column: x => x.IdKingdom2,
                      principalTable: "Kingdoms",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

            _ = migrationBuilder.CreateIndex(
              name: "IX_Champions_IdCharacter",
              table: "Champions",
              column: "IdCharacter");

            _ = migrationBuilder.CreateIndex(
              name: "IX_Champions_IdKingdom",
              table: "Champions",
              column: "IdKingdom");

            _ = migrationBuilder.CreateIndex(
              name: "IX_CharacterInventory_IdCharacter",
              table: "CharacterInventory",
              column: "IdCharacter");

            _ = migrationBuilder.CreateIndex(
              name: "IX_CharacterInventory_IdItem",
              table: "CharacterInventory",
              column: "IdItem");

            _ = migrationBuilder.CreateIndex(
              name: "IX_Characters_IdAccount",
              table: "Characters",
              column: "IdAccount");

            _ = migrationBuilder.CreateIndex(
              name: "IX_Characters_IdGuild",
              table: "Characters",
              column: "IdGuild");

            _ = migrationBuilder.CreateIndex(
              name: "IX_Characters_IdKingdom",
              table: "Characters",
              column: "IdKingdom");

            _ = migrationBuilder.CreateIndex(
              name: "IX_CharacterSkills_IdCharacter",
              table: "CharacterSkills",
              column: "IdCharacter");

            _ = migrationBuilder.CreateIndex(
              name: "IX_CharacterSkills_IdSkill",
              table: "CharacterSkills",
              column: "IdSkill");

            _ = migrationBuilder.CreateIndex(
              name: "IX_EquippedItems_IdArmor",
              table: "EquippedItems",
              column: "IdArmor");

            _ = migrationBuilder.CreateIndex(
              name: "IX_EquippedItems_IdBoots",
              table: "EquippedItems",
              column: "IdBoots");

            _ = migrationBuilder.CreateIndex(
              name: "IX_EquippedItems_IdGloves",
              table: "EquippedItems",
              column: "IdGloves");

            _ = migrationBuilder.CreateIndex(
              name: "IX_EquippedItems_IdHead",
              table: "EquippedItems",
              column: "IdHead");

            _ = migrationBuilder.CreateIndex(
              name: "IX_EquippedItems_IdLeftBracelet",
              table: "EquippedItems",
              column: "IdLeftBracelet");

            _ = migrationBuilder.CreateIndex(
              name: "IX_EquippedItems_IdLegs",
              table: "EquippedItems",
              column: "IdLegs");

            _ = migrationBuilder.CreateIndex(
              name: "IX_EquippedItems_IdNecklace",
              table: "EquippedItems",
              column: "IdNecklace");

            _ = migrationBuilder.CreateIndex(
              name: "IX_EquippedItems_IdRightBracelet",
              table: "EquippedItems",
              column: "IdRightBracelet");

            _ = migrationBuilder.CreateIndex(
              name: "IX_GameVersionFiles_IdVersion",
              table: "GameVersionFiles",
              column: "IdVersion");

            _ = migrationBuilder.CreateIndex(
              name: "IX_ItemEnchantments_IdEnchantment",
              table: "ItemEnchantments",
              column: "IdEnchantment");

            _ = migrationBuilder.CreateIndex(
              name: "IX_ItemEnchantments_IdItem",
              table: "ItemEnchantments",
              column: "IdItem");

            _ = migrationBuilder.CreateIndex(
              name: "IX_KingdomRelations_IdKingdom2",
              table: "KingdomRelations",
              column: "IdKingdom2");

            _ = migrationBuilder.CreateIndex(
              name: "IX_Kingdoms_IdRuler",
              table: "Kingdoms",
              column: "IdRuler",
              unique: true);

            _ = migrationBuilder.CreateIndex(
              name: "IX_MembersPositions_IdCharacter",
              table: "MembersPositions",
              column: "IdCharacter",
              unique: true);

            _ = migrationBuilder.CreateIndex(
              name: "IX_MembersPositions_IdGuild",
              table: "MembersPositions",
              column: "IdGuild");

            _ = migrationBuilder.CreateIndex(
              name: "IX_RingsEquipped_IdEquippedItems",
              table: "RingsEquipped",
              column: "IdEquippedItems");

            _ = migrationBuilder.CreateIndex(
              name: "IX_RingsEquipped_IdItem",
              table: "RingsEquipped",
              column: "IdItem");

            _ = migrationBuilder.CreateIndex(
              name: "IX_Users_IdAccount",
              table: "Users",
              column: "IdAccount",
              unique: true);

            _ = migrationBuilder.AddForeignKey(
              name: "FK_AttackStatuses_Characters_IdCharacter",
              table: "AttackStatuses",
              column: "IdCharacter",
              principalTable: "Characters",
              principalColumn: "Id",
              onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
              name: "FK_Champions_Characters_IdCharacter",
              table: "Champions",
              column: "IdCharacter",
              principalTable: "Characters",
              principalColumn: "Id",
              onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
              name: "FK_Champions_Kingdoms_IdKingdom",
              table: "Champions",
              column: "IdKingdom",
              principalTable: "Kingdoms",
              principalColumn: "Id",
              onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
              name: "FK_CharacterInventory_Characters_IdCharacter",
              table: "CharacterInventory",
              column: "IdCharacter",
              principalTable: "Characters",
              principalColumn: "Id",
              onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
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
            _ = migrationBuilder.DropForeignKey(
              name: "FK_Kingdoms_Characters_IdRuler",
              table: "Kingdoms");

            _ = migrationBuilder.DropTable(
              name: "AttackStatuses");

            _ = migrationBuilder.DropTable(
              name: "Champions");

            _ = migrationBuilder.DropTable(
              name: "CharacterInventory");

            _ = migrationBuilder.DropTable(
              name: "CharacterSkills");

            _ = migrationBuilder.DropTable(
              name: "DefenseStatuses");

            _ = migrationBuilder.DropTable(
              name: "GameVersionFiles");

            _ = migrationBuilder.DropTable(
              name: "ItemEnchantments");

            _ = migrationBuilder.DropTable(
              name: "KingdomRelations");

            _ = migrationBuilder.DropTable(
              name: "MembersPositions");

            _ = migrationBuilder.DropTable(
              name: "RingsEquipped");

            _ = migrationBuilder.DropTable(
              name: "Statuses");

            _ = migrationBuilder.DropTable(
              name: "Users");

            _ = migrationBuilder.DropTable(
              name: "Skills");

            _ = migrationBuilder.DropTable(
              name: "GameVersions");

            _ = migrationBuilder.DropTable(
              name: "Enchantments");

            _ = migrationBuilder.DropTable(
              name: "EquippedItems");

            _ = migrationBuilder.DropTable(
              name: "Items");

            _ = migrationBuilder.DropTable(
              name: "Characters");

            _ = migrationBuilder.DropTable(
              name: "Accounts");

            _ = migrationBuilder.DropTable(
              name: "Guilds");

            _ = migrationBuilder.DropTable(
              name: "Kingdoms");
        }
    }
}