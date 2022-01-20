using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class groepoe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LeeftijdsCategorie",
                table: "groepsChats",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Onderwerp",
                table: "groepsChats",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeeftijdsCategorie",
                table: "groepsChats");

            migrationBuilder.DropColumn(
                name: "Onderwerp",
                table: "groepsChats");
        }
    }
}
