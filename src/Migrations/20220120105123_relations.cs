using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_groepsChats_GroepsChatId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_GroepsChatId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "GroepsChatId",
                table: "Accounts");

            migrationBuilder.CreateTable(
                name: "ClientGroepsChat",
                columns: table => new
                {
                    DeelnemersId = table.Column<int>(type: "INTEGER", nullable: false),
                    groepChatsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientGroepsChat", x => new { x.DeelnemersId, x.groepChatsId });
                    table.ForeignKey(
                        name: "FK_ClientGroepsChat_Accounts_DeelnemersId",
                        column: x => x.DeelnemersId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientGroepsChat_groepsChats_groepChatsId",
                        column: x => x.groepChatsId,
                        principalTable: "groepsChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientGroepsChat_groepChatsId",
                table: "ClientGroepsChat",
                column: "groepChatsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientGroepsChat");

            migrationBuilder.AddColumn<int>(
                name: "GroepsChatId",
                table: "Accounts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_GroepsChatId",
                table: "Accounts",
                column: "GroepsChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_groepsChats_GroepsChatId",
                table: "Accounts",
                column: "GroepsChatId",
                principalTable: "groepsChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
