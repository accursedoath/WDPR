using System;
using Xunit;
using Moq;
using src;
using Microsoft.EntityFrameworkCore;
using src.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;

namespace test
{
    public class HulpverlenerTest
    {
        public DatabaseContext CreateContext()
        {
            DbContextOptionsBuilder<DatabaseContext> builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseInMemoryDatabase("test");
            DbContextOptions<DatabaseContext> options = builder.Options;
            DatabaseContext context = new DatabaseContext(options);

            return context;
        }

        private static UserManager<ApplicatieGebruiker> GetUsermanager()
        {
            //setup(mock) _usermanager
            var store = new Mock<IUserStore<ApplicatieGebruiker>>();
            var mum = new Mock<UserManager<ApplicatieGebruiker>>(store.Object, null, null, null, null, null, null, null, null);
            var user = new ApplicatieGebruiker() { Id = "test", UserName = "test", Email = "test@test.com" , hulpverlener = new Hulpverlener{Id = 1, Voornaam = "Ricco"}};
            
            mum.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mum.Setup(l => l.CreateAsync(It.IsAny<ApplicatieGebruiker>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Verifiable();
            mum.Setup(l => l.CreateAsync(It.IsAny<ApplicatieGebruiker>(), "Fail")).ReturnsAsync(IdentityResult.Failed()).Verifiable();
            mum.Setup(y => y.AddToRoleAsync(It.IsAny<ApplicatieGebruiker>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Verifiable();

            return mum.Object;
        }
        
        [Fact]
        public async void MaakAccountClientTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();

            var controllerMock = new Mock<HulpverlenerController>(context,GetUsermanager());
            var hulpverlener = new Hulpverlener(){Id = 1, Voornaam = "Ricco"};
            var aanmelding = new Aanmelding(){ 
                AanmeldingId = 1,
                Voornaam = "Joep", 
                Achternaam ="Geitenboer", 
                BSN = 34682,
                Stoornis = "ADHD",
                AfspraakDatum = "",
                Email ="j.g@mail.com",
                Leeftijdscategorie = "21-05-2007",
                Hulpverlener = hulpverlener,
                HulpverlenerId = hulpverlener.Id};
                
            context.Hulpverleners.Add(hulpverlener);
            context.Aanmeldingen.Add(aanmelding);
            context.SaveChanges();

            // Act
            var result = await controllerMock.Object.MaakAccount(1);
            var accountResult = new Client(){Voornaam = "Joep"};
            var actualAccount = context.Accounts.Where( a => a.Id == 2).SingleOrDefault();

            // Assert
            Console.WriteLine(actualAccount);            
            Assert.Equal(accountResult.Voornaam, actualAccount.Voornaam);
        }

        [Fact]
        public async void MaakAccountVoogdTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();
            var controllerMock = new Mock<HulpverlenerController>(context,GetUsermanager());           
            var hulpverlener = new Hulpverlener(){Id = 1, Voornaam = "Ricco"};
            var aanmelding = new Aanmelding(){ 
                AanmeldingId = 1,
                Voornaam = "Joep", 
                Achternaam ="Geitenboer", 
                BSN = 34682,
                Stoornis = "ADHD",
                AfspraakDatum = "",
                Email ="j.g@mail.com",
                Leeftijdscategorie = "21-05-2007",
                Hulpverlener = hulpverlener,
                HulpverlenerId = hulpverlener.Id,
                NaamVoogd = "Berry",
                TelefoonVoogd = "062837424",
                EmailVoogd= "b.g@mail.com"};
                
            context.Hulpverleners.Add(hulpverlener);
            context.Aanmeldingen.Add(aanmelding);
            context.SaveChanges();

            // Act
            var result = await controllerMock.Object.MaakAccount(1);
            var accountResult = new Voogd(){Voornaam = "Berry"};
            var actualAccount = context.Accounts.Where( a => a.Id == 3).SingleOrDefault();

            // Assert
            Console.WriteLine(actualAccount);            
            Assert.Equal(accountResult.Voornaam, actualAccount.Voornaam);
        }

        [Fact]
        public async void AanmeldingTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();

            var controllerMock = new Mock<HulpverlenerController>(context,GetUsermanager());
            var user = new ApplicatieGebruiker() { Id = "test", UserName = "test", Email = "test@test.com" , hulpverlener = new Hulpverlener{Id = 1, Voornaam = "Ricco"}};
            var hulpverlener = new Hulpverlener(){Id = 1, Voornaam = "Ricco", User =user};
            var aanmelding = new Aanmelding(){ 
                AanmeldingId = 1,
                Voornaam = "Joep", 
                Achternaam ="Geitenboer", 
                BSN = 34682,
                Stoornis = "ADHD",
                AfspraakDatum = "",
                Email ="j.g@mail.com",
                Leeftijdscategorie = "21-05-2007",
                Hulpverlener = hulpverlener,
                HulpverlenerId = hulpverlener.Id,
                NaamVoogd = "Berry",
                TelefoonVoogd = "062837424",
                EmailVoogd= "b.g@mail.com"};
                
            context.Hulpverleners.Add(hulpverlener);
            context.Aanmeldingen.Add(aanmelding);
            context.SaveChanges();

            var result = await controllerMock.Object.Aanmelding();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Aanmelding>>(
                viewResult.ViewData.Model);
            Assert.Equal(1, model.Count());
        }
    }
}