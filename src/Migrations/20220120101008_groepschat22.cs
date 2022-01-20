using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class groepschat22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroepsChatId",
                table: "Berichten",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroepsChatId",
                table: "Accounts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "groepsChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    hulpverlenerId = table.Column<int>(type: "INTEGER", nullable: true),
                    eindDatum = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groepsChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_groepsChats_Accounts_hulpverlenerId",
                        column: x => x.hulpverlenerId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Berichten_GroepsChatId",
                table: "Berichten",
                column: "GroepsChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_GroepsChatId",
                table: "Accounts",
                column: "GroepsChatId");

            migrationBuilder.CreateIndex(
                name: "IX_groepsChats_hulpverlenerId",
                table: "groepsChats",
                column: "hulpverlenerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_groepsChats_GroepsChatId",
                table: "Accounts",
                column: "GroepsChatId",
                principalTable: "groepsChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Berichten_groepsChats_GroepsChatId",
                table: "Berichten",
                column: "GroepsChatId",
                principalTable: "groepsChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_groepsChats_GroepsChatId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Berichten_groepsChats_GroepsChatId",
                table: "Berichten");

            migrationBuilder.DropTable(
                name: "groepsChats");

            migrationBuilder.DropIndex(
                name: "IX_Berichten_GroepsChatId",
                table: "Berichten");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_GroepsChatId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "GroepsChatId",
                table: "Berichten");

            migrationBuilder.DropColumn(
                name: "GroepsChatId",
                table: "Accounts");
        }
    }
}
