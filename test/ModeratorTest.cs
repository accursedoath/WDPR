using System;
using Xunit;
using Moq;
using src;
using Microsoft.EntityFrameworkCore;
using src.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace test
{
    public class ModeratorTest
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
            var controller = new ModeratorController(context);
            var client = new Client(){Id = 1, magChatten = true};
            context.Clienten.Add(client);
            context.SaveChanges();

            // Act
            var result = controller.Blokkeer(1);
            var clientResult = context.Clienten.Where(c => c.Id == 1).SingleOrDefault().magChatten;
            var meldingResult = context.HulpverlenerMeldingen.Where(m => m.Client == client).SingleOrDefault().Client;

            // Assert
            // var viewResult = Assert.IsType<ViewResult>(result);
            // var model = Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(
            //     viewResult.ViewData.Model);
            Assert.False(clientResult);
            Assert.Equal(client,meldingResult);
        }

        [Fact]
        public void DeblokkeerChatTest()
        {
            var context = CreateContext();
            context.Database.EnsureDeleted();
            var controller = new ModeratorController(context);
            var client = new Client(){Id = 1, magChatten = false};
            context.Clienten.Add(client);
            context.SaveChanges();

            // Act
            var result = controller.Deblokkeer(1);
            var clientResult = context.Clienten.Where(c => c.Id == 1).SingleOrDefault().magChatten;

            // Assert
            // var viewResult = Assert.IsType<ViewResult>(result);
            // var model = Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(
            //     viewResult.ViewData.Model);
            Assert.True(clientResult);
        }
    }
}
