using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ROH.Context.Migrations
{
    /// <inheritdoc />
    public partial class FixVersionRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid-ossp()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid-ossp()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Guilds",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid-ossp()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "GameVersions",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid-ossp()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "GameVersionFiles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid-ossp()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Characters",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid-ossp()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Accounts",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid-ossp()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid-ossp()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid-ossp()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Guilds",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid-ossp()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "GameVersions",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid-ossp()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "GameVersionFiles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid-ossp()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Characters",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid-ossp()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Accounts",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid-ossp()",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");
        }
    }
}