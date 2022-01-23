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
    public class ModeratorControllerTest
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
        public void BlokkeerChatTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();
            var controller = new Mock<ModeratorController>(context);
            var client = new Client(){Id = 1, magChatten = true};
            context.Clienten.Add(client);
            context.SaveChanges();

            // Act
            var result = controller.Object.Blokkeer(1);
            var clientResult = context.Clienten.Where(c => c.Id == 1).SingleOrDefault().magChatten;
            var meldingResult = context.HulpverlenerMeldingen.Where(m => m.Client == client).SingleOrDefault().Client;

            // Assert
            Assert.False(clientResult);
            Assert.Equal(client,meldingResult);
        }

        [Fact]
        public void DeblokkeerChatTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();
             var controller = new Mock<ModeratorController>(context);
            var client = new Client(){Id = 1, magChatten = false};
            context.Clienten.Add(client);
            context.SaveChanges();

            // Act
            var result = controller.Object.Deblokkeer(1);
            var clientResult = context.Clienten.Where(c => c.Id == 1).SingleOrDefault().magChatten;

            // Assert
            Assert.True(clientResult);
        }

        [Fact]
        public async void BehandelingenTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();
             var controller = new Mock<ModeratorController>(context);
            var hulpverlener = new Hulpverlener(){Id = 2, Voornaam = "Ricco"};
            var client = new Client(){Id = 1, magChatten = false, hulpverlener = hulpverlener};
            context.Clienten.Add(client);
            context.Hulpverleners.Add(hulpverlener);
            context.SaveChanges();

            // Act
            var result = await controller.Object.Behandelingen(2);
            var behandelingResult = context.Clienten.Where(c => c.hulpverlenerId == 2).SingleOrDefault();

            // Assert
            Assert.Equal(client, behandelingResult);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Client>>(
                viewResult.ViewData.Model);
            Assert.Equal(1, model.Count());
        }
        
        [Fact]
        public async void MeldingTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();
             var controller = new Mock<ModeratorController>(context);
            var Melding = new MisbruikMelding(){Id = 3};
            context.MisbruikMelding.Add(Melding);
            context.SaveChanges();

            // Act
            var result = await controller.Object.Melding();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<MisbruikMelding>>(
                viewResult.ViewData.Model);
            Assert.Equal(1, model.Count());
        }

        [Fact]
        public async void HulpverlenerTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();
            var controller = new Mock<ModeratorController>(context);
            var hulpverlener = new Hulpverlener(){Id = 2, Voornaam = "Ricco"};
            context.Hulpverleners.Add(hulpverlener);
            context.SaveChanges();

            // Act
            var result = await controller.Object.Hulpverlener();
            var hulpverleners = context.Hulpverleners.ToList();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Hulpverlener>>(
                viewResult.ViewData.Model);
            Assert.Equal(1, model.Count());
            Assert.Equal(hulpverleners, model);
        }

        [Fact]
        public async void ClientTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();
            var controller = new Mock<ModeratorController>(context);
            var client = new Client(){Id = 2, Voornaam = "Ricco"};
            context.Clienten.Add(client);
            context.SaveChanges();

            // Act
            var result = await controller.Object.Client();
            var clienten = context.Clienten.ToList();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Client>>(
                viewResult.ViewData.Model);
            Assert.Equal(1, model.Count());
            Assert.Equal(clienten, model);
        }
    }
}
