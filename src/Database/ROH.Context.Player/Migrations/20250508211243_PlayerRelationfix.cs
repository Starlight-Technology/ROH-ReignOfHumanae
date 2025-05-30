using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ROH.Context.Player.Migrations;

/// <inheritdoc />
public partial class PlayerRelationfix : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Characters_Kingdoms_IdKingdom",
            table: "Characters");

        migrationBuilder.DropForeignKey(
            name: "FK_Statuses_Characters_IdCharacter",
            table: "Statuses");

        migrationBuilder.AlterColumn<long>(
            name: "IdKingdom",
            table: "Characters",
            type: "bigint",
            nullable: true,
            oldClrType: typeof(long),
            oldType: "bigint");

        migrationBuilder.AddForeignKey(
            name: "FK_Characters_Kingdoms_IdKingdom",
            table: "Characters",
            column: "IdKingdom",
            principalTable: "Kingdoms",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Statuses_Characters_IdCharacter",
            table: "Statuses",
            column: "IdCharacter",
            principalTable: "Characters",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Characters_Kingdoms_IdKingdom",
            table: "Characters");

        migrationBuilder.DropForeignKey(
            name: "FK_Statuses_Characters_IdCharacter",
            table: "Statuses");

        migrationBuilder.AlterColumn<long>(
            name: "IdKingdom",
            table: "Characters",
            type: "bigint",
            nullable: false,
            defaultValue: 0L,
            oldClrType: typeof(long),
            oldType: "bigint",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Characters_Kingdoms_IdKingdom",
            table: "Characters",
            column: "IdKingdom",
            principalTable: "Kingdoms",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Statuses_Characters_IdCharacter",
            table: "Statuses",
            column: "IdCharacter",
            principalTable: "Characters",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
