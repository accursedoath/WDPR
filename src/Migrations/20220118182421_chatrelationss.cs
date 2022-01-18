using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class chatrelationss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BerichteniD",
                table: "Chat");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BerichteniD",
                table: "Chat",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
