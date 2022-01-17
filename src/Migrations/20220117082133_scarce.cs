using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class scarce : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Berichten_Account_VerzenderId",
                table: "Berichten");

            migrationBuilder.DropIndex(
                name: "IX_Berichten_VerzenderId",
                table: "Berichten");

            migrationBuilder.AddColumn<int>(
                name: "Account",
                table: "Berichten",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Bericht",
                table: "Berichten",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Chat",
                table: "Berichten",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "chatId",
                table: "Berichten",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    naam = table.Column<string>(type: "TEXT", nullable: true),
                    BerichteniD = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Berichten_Account",
                table: "Berichten",
                column: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_Berichten_Bericht",
                table: "Berichten",
                column: "Bericht");

            migrationBuilder.CreateIndex(
                name: "IX_Berichten_Chat",
                table: "Berichten",
                column: "Chat");

            migrationBuilder.AddForeignKey(
                name: "FK_Berichten_Account_Account",
                table: "Berichten",
                column: "Account",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Berichten_Chat_Bericht",
                table: "Berichten",
                column: "Bericht",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Berichten_Chat_Chat",
                table: "Berichten",
                column: "Chat",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Berichten_Account_Account",
                table: "Berichten");

            migrationBuilder.DropForeignKey(
                name: "FK_Berichten_Chat_Bericht",
                table: "Berichten");

            migrationBuilder.DropForeignKey(
                name: "FK_Berichten_Chat_Chat",
                table: "Berichten");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Berichten_Account",
                table: "Berichten");

            migrationBuilder.DropIndex(
                name: "IX_Berichten_Bericht",
                table: "Berichten");

            migrationBuilder.DropIndex(
                name: "IX_Berichten_Chat",
                table: "Berichten");

            migrationBuilder.DropColumn(
                name: "Account",
                table: "Berichten");

            migrationBuilder.DropColumn(
                name: "Bericht",
                table: "Berichten");

            migrationBuilder.DropColumn(
                name: "Chat",
                table: "Berichten");

            migrationBuilder.DropColumn(
                name: "chatId",
                table: "Berichten");

            migrationBuilder.CreateIndex(
                name: "IX_Berichten_VerzenderId",
                table: "Berichten",
                column: "VerzenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Berichten_Account_VerzenderId",
                table: "Berichten",
                column: "VerzenderId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
