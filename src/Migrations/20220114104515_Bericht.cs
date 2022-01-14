using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class Bericht : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Berichten_Account_VerzenderId",
                table: "Berichten");

            migrationBuilder.AlterColumn<string>(
                name: "VerzenderId",
                table: "Berichten",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Berichten_AspNetUsers_VerzenderId",
                table: "Berichten",
                column: "VerzenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Berichten_AspNetUsers_VerzenderId",
                table: "Berichten");

            migrationBuilder.AlterColumn<int>(
                name: "VerzenderId",
                table: "Berichten",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Berichten_Account_VerzenderId",
                table: "Berichten",
                column: "VerzenderId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
