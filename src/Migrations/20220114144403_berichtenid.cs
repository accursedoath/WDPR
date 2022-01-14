using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class berichtenid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Berichten_Account_VerzenderId",
                table: "Berichten");

            migrationBuilder.AlterColumn<int>(
                name: "VerzenderId",
                table: "Berichten",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Berichten_Account_VerzenderId",
                table: "Berichten",
                column: "VerzenderId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Berichten_Account_VerzenderId",
                table: "Berichten");

            migrationBuilder.AlterColumn<int>(
                name: "VerzenderId",
                table: "Berichten",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

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
