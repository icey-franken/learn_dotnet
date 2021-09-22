using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class removePayloadFromContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurais_Battles_BattleId",
                table: "BattleSamurais");

            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurais_Samurais_SamuraiId",
                table: "BattleSamurais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BattleSamurais",
                table: "BattleSamurais");

            migrationBuilder.DropIndex(
                name: "IX_BattleSamurais_SamuraiId",
                table: "BattleSamurais");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateJoined",
                table: "BattleSamurais",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BattleSamurais",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BattleSamurais",
                table: "BattleSamurais",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BattleSamurai",
                columns: table => new
                {
                    BattlesId = table.Column<int>(type: "int", nullable: false),
                    SamuraisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleSamurai", x => new { x.BattlesId, x.SamuraisId });
                    table.ForeignKey(
                        name: "FK_BattleSamurai_Battles_BattlesId",
                        column: x => x.BattlesId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BattleSamurai_Samurais_SamuraisId",
                        column: x => x.SamuraisId,
                        principalTable: "Samurais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattleSamurai_SamuraisId",
                table: "BattleSamurai",
                column: "SamuraisId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleSamurai");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BattleSamurais",
                table: "BattleSamurais");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BattleSamurais");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateJoined",
                table: "BattleSamurais",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BattleSamurais",
                table: "BattleSamurais",
                columns: new[] { "BattleId", "SamuraiId" });

            migrationBuilder.CreateIndex(
                name: "IX_BattleSamurais_SamuraiId",
                table: "BattleSamurais",
                column: "SamuraiId");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurais_Battles_BattleId",
                table: "BattleSamurais",
                column: "BattleId",
                principalTable: "Battles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurais_Samurais_SamuraiId",
                table: "BattleSamurais",
                column: "SamuraiId",
                principalTable: "Samurais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
