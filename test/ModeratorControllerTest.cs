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
            context.Add(new Client(){Id = 1, magChatten = true});
            context.Add(new Client(){Id = 2, magChatten = false});
            context.Add(new Hulpverlener(){Id = 3, Voornaam = "Ricco"});
            context.Add(new Client(){Id = 4, magChatten = false, hulpverlenerId = 3});
            context.Add(new MisbruikMelding(){Id = 3});
            context.SaveChanges();
            return GetNewInMemoryDatabase(false);
        }

        [Fact]
        public void BlokkeerChatTest()
        {
            // Arrange
            var context = GetInMemoryDBMetData();
            var controller = new Mock<ModeratorController>(context);
            var client = new Client(){Id = 1, magChatten = true};

            // Act
            var result = controller.Object.Blokkeer(1);
            var clientResult = context.Clienten.Where(c => c.Id == 1).SingleOrDefault().magChatten;
            var meldingResult = context.HulpverlenerMeldingen.Where(m => m.Client == client).SingleOrDefault().Client;

            // Assert
            Assert.False(clientResult);
            Assert.Equal(client.Id,meldingResult.Id);
        }

        [Fact]
        public void DeblokkeerChatTest()
        {
            // Arrange
            var context = GetInMemoryDBMetData();
            var controller = new ModeratorController(context);

            // Act
            var result = controller.Deblokkeer(2);
            var clientResult = context.Clienten.Where(c => c.Id == 2).SingleOrDefault().magChatten;

            // Assert
            Assert.True(clientResult);
        }

        [Fact]
        public async void BehandelingenTest()
        {
            // Arrange
            var context = GetInMemoryDBMetData();
            var controller = new ModeratorController(context);
            var client = new Client(){Id = 4, magChatten = false, hulpverlenerId = 3};

            // Act
            var result = await controller.Behandelingen(3);
            var behandelingResult = context.Clienten.Where(c => c.hulpverlenerId == 3).SingleOrDefault();

            // Assert
            Assert.Equal(client.Id, behandelingResult.Id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Client>>(
                viewResult.ViewData.Model);
            Assert.Equal(1, model.Count());
        }
        
        [Fact]
        public async void MeldingTest()
        {
            // Arrange
            var context = GetInMemoryDBMetData();
            var controller = new ModeratorController(context);

            // Act
            var result = await controller.Melding();

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
            var context = GetInMemoryDBMetData();
            var controller = new ModeratorController(context);

            // Act
            var result = await controller.Hulpverlener();
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
            var context = GetInMemoryDBMetData();
            var controller = new ModeratorController(context);

            // Act
            var result = await controller.Client();
            var clienten = context.Clienten.ToList();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Client>>(
                viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
            Assert.Equal(clienten, model);
        }
    }
}
