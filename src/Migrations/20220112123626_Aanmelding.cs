using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class Aanmelding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aanmeldingen",
                columns: table => new
                {
                    AanmeldingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Voornaam = table.Column<string>(type: "TEXT", nullable: true),
                    Achternaam = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Stoornis = table.Column<string>(type: "TEXT", nullable: true),
                    Leeftijdscategorie = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aanmeldingen", x => x.AanmeldingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aanmeldingen");
        }
    }
}
