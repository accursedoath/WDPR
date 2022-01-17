using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class ortho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "hulpverlenerId",
                table: "Account",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_hulpverlenerId",
                table: "Account",
                column: "hulpverlenerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Account_hulpverlenerId",
                table: "Account",
                column: "hulpverlenerId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Account_hulpverlenerId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_hulpverlenerId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "hulpverlenerId",
                table: "Account");
        }
    }
}
