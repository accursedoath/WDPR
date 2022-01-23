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
            context.Add(new Client(){Id = 1, Voornaam = "John", Leeftijdscategorie="2007-07-12",magChatten = true, hulpverlenerId = 2});
            context.Add(new Hulpverlener(){Id = 2, Voornaam = "Ricco"});
            context.Add(new Chat(){Id = 3, clientId = 1, hulpverlenerId = 2});
            context.Add(new Bericht(){chatId = 3, text = "Goedenmorgen", Datum = DateTime.Now, VerzenderId = 1});
            context.SaveChanges();
            return GetNewInMemoryDatabase(false);
        }

        [Fact]
        public async void ChatFreqTest()
        {
            // Arrange
            var context = GetInMemoryDBMetData();
            var controller = new VoogdController(context);

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