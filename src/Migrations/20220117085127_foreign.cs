using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class foreign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Berichten_Chat_Chat",
                table: "Berichten");

            migrationBuilder.DropIndex(
                name: "IX_Berichten_Chat",
                table: "Berichten");

            migrationBuilder.DropColumn(
                name: "Chat",
                table: "Berichten");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Chat",
                table: "Berichten",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Berichten_Chat",
                table: "Berichten",
                column: "Chat");

            migrationBuilder.AddForeignKey(
                name: "FK_Berichten_Chat_Chat",
                table: "Berichten",
                column: "Chat",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
