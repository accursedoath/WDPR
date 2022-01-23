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
        private string databaseName;
        
        private DatabaseContext GetNewInMemoryDatabase(bool NewDb) {
            if (NewDb) this.databaseName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(this.databaseName)
            .Options;
            return new DatabaseContext(options);
        }

        //Context
        private DatabaseContext GetInMemoryDBMetData() {        
            DatabaseContext context = GetNewInMemoryDatabase(true);
            context.Add(new Hulpverlener(){Id = 1, Voornaam = "Ricco"});
            context.SaveChanges();
            return GetNewInMemoryDatabase(false);
        }

        [Fact]
        public void CreateClientTest()
        {
            // Arrange
            var context = GetInMemoryDBMetData();
            var controller = new AanmeldingController(context);
            var hulp = new Hulpverlener(){Id = 1, Voornaam = "Ricco"};
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
            var aanmeldingHulpResult = context.Aanmeldingen.Where(m => m.Hulpverlener == hulp).SingleOrDefault().Hulpverlener;

            // Assert
            Assert.Equal(aanmelding,aanmeldingResult);
            Assert.Equal(hulp.Id,aanmeldingHulpResult.Id);
        }

        [Fact]
        public void CreateClientVoogdTest()
        {
            // Arrange
            var context = GetInMemoryDBMetData();
            var controller = new AanmeldingController(context);
            var hulp = new Hulpverlener(){Id = 1, Voornaam = "Ricco"};
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
            var aanmeldingHulpResult = context.Aanmeldingen.Where(m => m.Hulpverlener == hulp).SingleOrDefault().Hulpverlener;

            // Assert
            Assert.Equal(aanmelding,aanmeldingResult);
            Assert.Equal(hulp.Id,aanmeldingHulpResult.Id);

        }
    }
}