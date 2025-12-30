//-----------------------------------------------------------------------
// <copyright file="20241203145934_InitialMigration.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ROH.Context.Player.Migrations;

/// <inheritdoc/>
public partial class InitialMigration : Migration
{
    /// <inheritdoc/>
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(name: "FK_Kingdoms_Characters_IdRuler", table: "Kingdoms");

        migrationBuilder.DropTable(name: "AttackStatuses");

        migrationBuilder.DropTable(name: "Champions");

        migrationBuilder.DropTable(name: "CharacterInventory");

        migrationBuilder.DropTable(name: "CharacterSkills");

        migrationBuilder.DropTable(name: "DefenseStatuses");

        migrationBuilder.DropTable(name: "KingdomRelations");

        migrationBuilder.DropTable(name: "MembersPositions");

        migrationBuilder.DropTable(name: "RingsEquipped");

        migrationBuilder.DropTable(name: "Statuses");

        migrationBuilder.DropTable(name: "Skills");

        migrationBuilder.DropTable(name: "EquippedItems");

        migrationBuilder.DropTable(name: "Characters");

        migrationBuilder.DropTable(name: "Guilds");

        migrationBuilder.DropTable(name: "Kingdoms");
    }

    /// <inheritdoc/>
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Guilds",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                Name = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Guilds", x => x.Id));

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
            constraints: table => table.PrimaryKey("PK_Skills", x => x.Id));

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
            constraints: table => table.PrimaryKey("PK_AttackStatuses", x => x.IdCharacter));

        migrationBuilder.CreateTable(
            name: "Champions",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdCharacter = table.Column<long>(type: "bigint", nullable: false),
                IdKingdom = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Champions", x => x.Id));

        migrationBuilder.CreateTable(
            name: "CharacterInventory",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdItem = table.Column<long>(type: "bigint", nullable: false),
                IdCharacter = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_CharacterInventory", x => x.Id));

        migrationBuilder.CreateTable(
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
                table.PrimaryKey("PK_Characters", x => x.Id);
                table.ForeignKey(
                    name: "FK_Characters_Guilds_IdGuild",
                    column: x => x.IdGuild,
                    principalTable: "Guilds",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
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
                table.PrimaryKey("PK_CharacterSkills", x => x.Id);
                table.ForeignKey(
                    name: "FK_CharacterSkills_Characters_IdCharacter",
                    column: x => x.IdCharacter,
                    principalTable: "Characters",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_CharacterSkills_Skills_IdSkill",
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
                table.PrimaryKey("PK_EquippedItems", x => x.IdCharacter);
                table.ForeignKey(
                    name: "FK_EquippedItems_Characters_IdCharacter",
                    column: x => x.IdCharacter,
                    principalTable: "Characters",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
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
                IdEquippedItems = table.Column<long>(type: "bigint", nullable: false),
                IdItem = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RingsEquipped", x => x.Id);
                table.ForeignKey(
                    name: "FK_RingsEquipped_EquippedItems_IdEquippedItems",
                    column: x => x.IdEquippedItems,
                    principalTable: "EquippedItems",
                    principalColumn: "IdCharacter",
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

        migrationBuilder.CreateIndex(name: "IX_Champions_IdCharacter", table: "Champions", column: "IdCharacter");

        migrationBuilder.CreateIndex(name: "IX_Champions_IdKingdom", table: "Champions", column: "IdKingdom");

        migrationBuilder.CreateIndex(
            name: "IX_CharacterInventory_IdCharacter",
            table: "CharacterInventory",
            column: "IdCharacter");

        migrationBuilder.CreateIndex(name: "IX_Characters_IdGuild", table: "Characters", column: "IdGuild");

        migrationBuilder.CreateIndex(name: "IX_Characters_IdKingdom", table: "Characters", column: "IdKingdom");

        migrationBuilder.CreateIndex(
            name: "IX_CharacterSkills_IdCharacter",
            table: "CharacterSkills",
            column: "IdCharacter");

        migrationBuilder.CreateIndex(name: "IX_CharacterSkills_IdSkill", table: "CharacterSkills", column: "IdSkill");

        migrationBuilder.CreateIndex(
            name: "IX_KingdomRelations_IdKingdom2",
            table: "KingdomRelations",
            column: "IdKingdom2");

        migrationBuilder.CreateIndex(name: "IX_Kingdoms_IdRuler", table: "Kingdoms", column: "IdRuler", unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_MembersPositions_IdCharacter",
            table: "MembersPositions",
            column: "IdCharacter",
            unique: true);

        migrationBuilder.CreateIndex(name: "IX_MembersPositions_IdGuild", table: "MembersPositions", column: "IdGuild");

        migrationBuilder.CreateIndex(
            name: "IX_RingsEquipped_IdEquippedItems",
            table: "RingsEquipped",
            column: "IdEquippedItems");

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
            name: "FK_CharacterInventory_Characters_IdCharacter",
            table: "CharacterInventory",
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
}