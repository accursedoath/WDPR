using System;
using Xunit;
using Moq;
using src;
using Microsoft.EntityFrameworkCore;
using src.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace test
{
    public class AanmeldingControllerTest
    {
        public DatabaseContext CreateContext()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseInMemoryDatabase("test");
            DbContextOptions<DatabaseContext> options = builder.Options;
            DatabaseContext context = new DatabaseContext(options);

            return context;
        }

        [Fact]
        public void CreateClientTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();
            var controller = new AanmeldingController(context);
            var hulp = new Hulpverlener(){Id = 1, Voornaam = "Ricco"};
            context.SaveChanges();
            var aanmelding = new Aanmelding(){
                AanmeldingId = 1,
                Voornaam = "Jan",
                Achternaam= "Ballenbak",
                BSN = 19263,
                Email = "j@b.gmail.com",
                Stoornis = "ADHD",
                Leeftijdscategorie = "24-05-2007",
                AfspraakDatum = "27-02-2022"};

            // Act
            var result = controller.Create(aanmelding, "1");
            var aanmeldingResult = context.Aanmeldingen.Where(c => c.AanmeldingId == 1).SingleOrDefault();
            var aanmeldingHulpResult = context.Aanmeldingen.Where(m => m.HulpverlenerId == hulp.Id).SingleOrDefault().Hulpverlener;

            // Assert
            Assert.Equal(aanmelding,aanmeldingResult);
            Assert.Equal(hulp,aanmeldingHulpResult);
        }

        [Fact]
        public void CreateClientVoogdTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();
            var controller = new AanmeldingController(context);
            var hulp = new Hulpverlener(){Id = 1, Voornaam = "Ricco"};
            context.SaveChanges();
            var aanmelding = new Aanmelding(){
                AanmeldingId = 1,
                Voornaam = "Jan",
                Achternaam= "Ballenbak",
                BSN = 19263,
                Email = "j@b.gmail.com",
                Stoornis = "ADHD",
                Leeftijdscategorie = "24-05-2007",
                AfspraakDatum = "27-02-2022",
                NaamVoogd = "Koen",
                EmailVoogd = "k.b@gmail.com",
                TelefoonVoogd = "061081231"};

            // Act
            var result = controller.Create(aanmelding, "1");
            var aanmeldingResult = context.Aanmeldingen.Where(c => c.AanmeldingId == 1).SingleOrDefault();
            var aanmeldingHulpResult = context.Aanmeldingen.Where(m => m.HulpverlenerId == hulp.Id).SingleOrDefault().Hulpverlener;

            // Assert
            Assert.Equal(aanmelding,aanmeldingResult);
            Assert.Equal(hulp,aanmeldingHulpResult);

        }
    }
}