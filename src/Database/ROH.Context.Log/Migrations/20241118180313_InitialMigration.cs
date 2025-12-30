//-----------------------------------------------------------------------
// <copyright file="20241118180313_InitialMigration.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ROH.Context.Log.Migrations;

/// <inheritdoc/>
public partial class InitialMigration : Migration
{
    /// <inheritdoc/>
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(name: "Logs");

    /// <inheritdoc/>
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.CreateTable(
        name: "Logs",
        columns: table => new
        {
            Id = table.Column<long>(type: "bigint", nullable: false)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Severity = table.Column<int>(type: "integer", nullable: false),
            Message = table.Column<string>(type: "text", nullable: false)
        },
        constraints: table => table.PrimaryKey("PK_Logs", x => x.Id));
}