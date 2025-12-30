using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ROH.Context.Player.Migrations
{
    /// <inheritdoc />
    public partial class FixedCharacterRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttackStatuses_Characters_IdCharacter",
                table: "AttackStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterInventory_Characters_IdCharacter",
                table: "CharacterInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterSkills_Characters_IdCharacter",
                table: "CharacterSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_DefenseStatuses_Characters_IdCharacter",
                table: "DefenseStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_EquippedItems_Characters_IdCharacter",
                table: "EquippedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayersPosition_Characters_IdPlayer",
                table: "PlayersPosition");

            migrationBuilder.AddForeignKey(
                name: "FK_AttackStatuses_Characters_IdCharacter",
                table: "AttackStatuses",
                column: "IdCharacter",
                principalTable: "Characters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterInventory_Characters_IdCharacter",
                table: "CharacterInventory",
                column: "IdCharacter",
                principalTable: "Characters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterSkills_Characters_IdCharacter",
                table: "CharacterSkills",
                column: "IdCharacter",
                principalTable: "Characters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DefenseStatuses_Characters_IdCharacter",
                table: "DefenseStatuses",
                column: "IdCharacter",
                principalTable: "Characters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EquippedItems_Characters_IdCharacter",
                table: "EquippedItems",
                column: "IdCharacter",
                principalTable: "Characters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersPosition_Characters_IdPlayer",
                table: "PlayersPosition",
                column: "IdPlayer",
                principalTable: "Characters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttackStatuses_Characters_IdCharacter",
                table: "AttackStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterInventory_Characters_IdCharacter",
                table: "CharacterInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterSkills_Characters_IdCharacter",
                table: "CharacterSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_DefenseStatuses_Characters_IdCharacter",
                table: "DefenseStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_EquippedItems_Characters_IdCharacter",
                table: "EquippedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayersPosition_Characters_IdPlayer",
                table: "PlayersPosition");

            migrationBuilder.AddForeignKey(
                name: "FK_AttackStatuses_Characters_IdCharacter",
                table: "AttackStatuses",
                column: "IdCharacter",
                principalTable: "Characters",
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
                name: "FK_CharacterSkills_Characters_IdCharacter",
                table: "CharacterSkills",
                column: "IdCharacter",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefenseStatuses_Characters_IdCharacter",
                table: "DefenseStatuses",
                column: "IdCharacter",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquippedItems_Characters_IdCharacter",
                table: "EquippedItems",
                column: "IdCharacter",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersPosition_Characters_IdPlayer",
                table: "PlayersPosition",
                column: "IdPlayer",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}