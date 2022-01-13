using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class REVAMP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Berichten_AspNetUsers_VerzenderId",
                table: "Berichten");

            migrationBuilder.DropColumn(
                name: "Achternaam",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Adres",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Beschrijving",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Postcode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Voornaam",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Woonplaats",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "magChatten",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "VerzenderId",
                table: "Berichten",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

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

            migrationBuilder.CreateTable(
                name: "Woonplaats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Adres = table.Column<string>(type: "TEXT", nullable: true),
                    plaats = table.Column<string>(type: "TEXT", nullable: true),
                    Postcode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Woonplaats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Voornaam = table.Column<string>(type: "TEXT", nullable: true),
                    Achternaam = table.Column<string>(type: "TEXT", nullable: true),
                    Woonplaats = table.Column<int>(type: "INTEGER", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    magChatten = table.Column<bool>(type: "INTEGER", nullable: true),
                    ApplicatieGebruiker = table.Column<string>(type: "TEXT", nullable: true),
                    Beschrijving = table.Column<string>(type: "TEXT", nullable: true),
                    Hulpverlener_ApplicatieGebruiker = table.Column<string>(type: "TEXT", nullable: true),
                    Moderator_ApplicatieGebruiker = table.Column<string>(type: "TEXT", nullable: true),
                    Voogd_ApplicatieGebruiker = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_AspNetUsers_ApplicatieGebruiker",
                        column: x => x.ApplicatieGebruiker,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_AspNetUsers_Hulpverlener_ApplicatieGebruiker",
                        column: x => x.Hulpverlener_ApplicatieGebruiker,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_AspNetUsers_Moderator_ApplicatieGebruiker",
                        column: x => x.Moderator_ApplicatieGebruiker,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_AspNetUsers_Voogd_ApplicatieGebruiker",
                        column: x => x.Voogd_ApplicatieGebruiker,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_Woonplaats_Woonplaats",
                        column: x => x.Woonplaats,
                        principalTable: "Woonplaats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_ApplicatieGebruiker",
                table: "Account",
                column: "ApplicatieGebruiker",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_Hulpverlener_ApplicatieGebruiker",
                table: "Account",
                column: "Hulpverlener_ApplicatieGebruiker",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_Moderator_ApplicatieGebruiker",
                table: "Account",
                column: "Moderator_ApplicatieGebruiker",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_Voogd_ApplicatieGebruiker",
                table: "Account",
                column: "Voogd_ApplicatieGebruiker",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_Woonplaats",
                table: "Account",
                column: "Woonplaats",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Berichten_Account_VerzenderId",
                table: "Berichten",
                column: "VerzenderId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Berichten_Account_VerzenderId",
                table: "Berichten");

            migrationBuilder.DropTable(
                name: "Aanmeldingen");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Woonplaats");

            migrationBuilder.AlterColumn<string>(
                name: "VerzenderId",
                table: "Berichten",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Achternaam",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Beschrijving",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Postcode",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Voornaam",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Woonplaats",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "magChatten",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Berichten_AspNetUsers_VerzenderId",
                table: "Berichten",
                column: "VerzenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
