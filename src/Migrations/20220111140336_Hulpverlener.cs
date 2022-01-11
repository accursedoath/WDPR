using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class Hulpverlener : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Beschrijving",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Beschrijving",
                table: "AspNetUsers");
        }
    }
}
