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
        
        [Fact]
        public async void MaakAccountTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();

            var userManagerMock = new UserManager<ApplicatieGebruiker>(
               Mock.Of<IUserStore<ApplicatieGebruiker>>(),null,null,null,null,null,null,null,null
            );

            var signInManagerMock = new SignInManager<ApplicatieGebruiker>(
            userManagerMock,Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicatieGebruiker>>(),
            null,null,null,null);

            var controllerMock = new Mock<HulpverlenerController>(context,signInManagerMock,userManagerMock);
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
            //var result = await controllerMock.Object.MaakAccount(1);
            var accountResult = new Account(){Id = 1, Voornaam ="Joep", Achternaam="Geitenboer"};
            var actualAccount = context.Accounts.Where( a => a.Id == 1).SingleOrDefault();

            // Assert
            Assert.Equal(accountResult,actualAccount);
        }
    }
}