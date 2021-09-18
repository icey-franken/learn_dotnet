using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class m2mpayload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleSamurai");

            migrationBuilder.CreateTable(
                name: "BattleSamurais",
                columns: table => new
                {
                    SamuraiId = table.Column<int>(type: "int", nullable: false),
                    BattleId = table.Column<int>(type: "int", nullable: false),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleSamurais", x => new { x.BattleId, x.SamuraiId });
                    table.ForeignKey(
                        name: "FK_BattleSamurais_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BattleSamurais_Samurais_SamuraiId",
                        column: x => x.SamuraiId,
                        principalTable: "Samurais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattleSamurais_SamuraiId",
                table: "BattleSamurais",
                column: "SamuraiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleSamurais");

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
    }
}
