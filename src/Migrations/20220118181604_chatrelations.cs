using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class chatrelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "clientId",
                table: "Chat",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "hulpverlenerId",
                table: "Chat",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chat_clientId",
                table: "Chat",
                column: "clientId");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_hulpverlenerId",
                table: "Chat",
                column: "hulpverlenerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_Account_clientId",
                table: "Chat",
                column: "clientId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_Account_hulpverlenerId",
                table: "Chat",
                column: "hulpverlenerId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chat_Account_clientId",
                table: "Chat");

            migrationBuilder.DropForeignKey(
                name: "FK_Chat_Account_hulpverlenerId",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_clientId",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_hulpverlenerId",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "clientId",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "hulpverlenerId",
                table: "Chat");
        }
    }
}
