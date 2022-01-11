using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class ClientContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "magChatten",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "magChatten",
                table: "AspNetUsers");
        }
    }
}
