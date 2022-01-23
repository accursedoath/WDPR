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
using src.Models;

namespace test
{
    public class VoogdControllerTest
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
        public async void ChatFreqTest()
        {
            // Arrange
            var context = CreateContext();
            context.Database.EnsureDeleted();
            var controller = new VoogdController(context);
            var hulp = new Hulpverlener(){Id = 2, Voornaam = "Ricco"};
            var client = new Client(){Id = 1, Voornaam = "John", Leeftijdscategorie="2007-07-12",magChatten = true, hulpverlener = hulp};
            var chat = new Chat(){Id = 3, client = client, hulpverlener = hulp};
            var bericht = new Bericht(){chatId = 3, text = "Goedenmorgen", Datum = DateTime.Now, Verzender = client};
            context.Clienten.Add(client);
            context.Hulpverleners.Add(hulp);
            context.Chats.Add(chat);
            context.Berichten.Add(bericht);
            context.SaveChanges();

            // Act
            var result = await controller.ChatFreq(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(
                viewResult.ViewData.Model);
            Assert.Equal(1, model.Count());
        }
    }
}