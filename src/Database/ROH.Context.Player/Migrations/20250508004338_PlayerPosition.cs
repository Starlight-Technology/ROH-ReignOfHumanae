using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ROH.Context.Player.Migrations;

/// <inheritdoc />
public partial class PlayerPosition : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_KingdomRelations_Kingdoms_IdKingdom2",
            table: "KingdomRelations");

        migrationBuilder.DropColumn(
            name: "IdAccount",
            table: "Characters");

        migrationBuilder.AddColumn<Guid>(
            name: "GuidAccount",
            table: "Characters",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateTable(
            name: "Positions",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdPlayer = table.Column<long>(type: "bigint", nullable: false),
                X = table.Column<float>(type: "real", nullable: false),
                Y = table.Column<float>(type: "real", nullable: false),
                Z = table.Column<float>(type: "real", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Positions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Rotations",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                X = table.Column<float>(type: "real", nullable: false),
                Y = table.Column<float>(type: "real", nullable: false),
                Z = table.Column<float>(type: "real", nullable: false),
                W = table.Column<float>(type: "real", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rotations", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PlayersPosition",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                IdPlayer = table.Column<long>(type: "bigint", nullable: false),
                PositionId = table.Column<long>(type: "bigint", nullable: false),
                RotationId = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PlayersPosition", x => x.Id);
                table.ForeignKey(
                    name: "FK_PlayersPosition_Characters_IdPlayer",
                    column: x => x.IdPlayer,
                    principalTable: "Characters",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PlayersPosition_Positions_PositionId",
                    column: x => x.PositionId,
                    principalTable: "Positions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PlayersPosition_Rotations_RotationId",
                    column: x => x.RotationId,
                    principalTable: "Rotations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_KingdomRelations_IdKingdom",
            table: "KingdomRelations",
            column: "IdKingdom");

        migrationBuilder.CreateIndex(
            name: "IX_PlayersPosition_IdPlayer",
            table: "PlayersPosition",
            column: "IdPlayer",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PlayersPosition_PositionId",
            table: "PlayersPosition",
            column: "PositionId");

        migrationBuilder.CreateIndex(
            name: "IX_PlayersPosition_RotationId",
            table: "PlayersPosition",
            column: "RotationId");

        migrationBuilder.AddForeignKey(
            name: "FK_KingdomRelations_Kingdoms_IdKingdom",
            table: "KingdomRelations",
            column: "IdKingdom",
            principalTable: "Kingdoms",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_KingdomRelations_Kingdoms_IdKingdom2",
            table: "KingdomRelations",
            column: "IdKingdom2",
            principalTable: "Kingdoms",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_KingdomRelations_Kingdoms_IdKingdom",
            table: "KingdomRelations");

        migrationBuilder.DropForeignKey(
            name: "FK_KingdomRelations_Kingdoms_IdKingdom2",
            table: "KingdomRelations");

        migrationBuilder.DropTable(
            name: "PlayersPosition");

        migrationBuilder.DropTable(
            name: "Positions");

        migrationBuilder.DropTable(
            name: "Rotations");

        migrationBuilder.DropIndex(
            name: "IX_KingdomRelations_IdKingdom",
            table: "KingdomRelations");

        migrationBuilder.DropColumn(
            name: "GuidAccount",
            table: "Characters");

        migrationBuilder.AddColumn<long>(
            name: "IdAccount",
            table: "Characters",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);

        migrationBuilder.AddForeignKey(
            name: "FK_KingdomRelations_Kingdoms_IdKingdom2",
            table: "KingdomRelations",
            column: "IdKingdom2",
            principalTable: "Kingdoms",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
