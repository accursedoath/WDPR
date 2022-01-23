using System;
using Xunit;
using Moq;
using src.Models;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Hubs;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using src.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace test
{
    public class Chat_tests
    {
        private string databaseName; // zonder deze property kun je geen clean context maken.
        private DatabaseContext GetInMemoryDBMetData() {        //dit is je _context
        DatabaseContext context = GetNewInMemoryDatabase(true);
        var hulpverlener = new Hulpverlener() {Voornaam = "Bamster", Achternaam = "trionhe"};
        var client = new Client() {Voornaam = "Kuga", Achternaam = "Yuma", hulpverlener = hulpverlener};
        context.Hulpverleners.Add(hulpverlener);
        context.Clienten.Add(client);
        //context.Chats.Add(chat);
        
        context.SaveChanges();
        return GetNewInMemoryDatabase(false); // gebruik een nieuw (clean) object voor de context
        }
        private DatabaseContext GetNewInMemoryDatabase(bool NewDb) {
        if (NewDb) this.databaseName = Guid.NewGuid().ToString(); // unieke naam
        var options = new DbContextOptionsBuilder<DatabaseContext>()
        .UseInMemoryDatabase(this.databaseName)
        .Options;
        return new DatabaseContext(options);
        }

        public Mock<UserManager<ApplicatieGebruiker>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicatieGebruiker>>();
            return new Mock<UserManager<ApplicatieGebruiker>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        //testen of een verzender correct wordt opgeslagen. 
        [Fact]
        public void verzender_van_bericht_in_chat_isgelijk()  //requirement voor prive chat, aan de chatid kunnen we de verzender van een bericht herleiden
        {
            //arrange
            var _context = GetInMemoryDBMetData();

            _context.SaveChanges();
            
            var client = _context.Clienten.First();
            var hulpverlener = _context.Hulpverleners.First();

            var chat = new Chat() {hulpverlener = hulpverlener, client = client, Id = 1};
            _context.Add(chat);

            var bericht = new Bericht(){Verzender = client, text = "Trigger on!", chatId = chat.Id};
            _context.Add(bericht);
            _context.SaveChanges();

            Assert.True(_context.Chats.Find(chat.Id).client == bericht.Verzender);
        }

        [Fact]
        public void zelfhulpgroep_aanmelding_mogelijkheid(){ //kijken of een client een zelfhulp groep kan joinen
            var _context = GetInMemoryDBMetData();
            var client = _context.Clienten.First();
            var hulpverlener = _context.Hulpverleners.First();

            var groepschat = new GroepsChat(){ hulpverlener = hulpverlener, Onderwerp = "World Trigger"};

            groepschat.Deelnemers.Add(client);
            _context.groepsChats.Add(groepschat);
            _context.SaveChanges();

            _context.groepsChats.Include(x => x.Deelnemers);
            var loadedgroepschat = _context.groepsChats.First();
            var loadeddeelnemers = groepschat.Deelnemers;
            bool result = false;
            foreach(var x in loadeddeelnemers){
                if(x == client) result = true;
            }
            Assert.True(result);
        }

        [Fact]
        public void ZelfHulpGroep_aanmaken()
        {
            // Given
            var _context = GetInMemoryDBMetData();
            var client = _context.Clienten.First();
            var hulpverlener = _context.Hulpverleners.First();
            // When
            var zelfhulpgroep = new GroepsChat(){Onderwerp = "World Trigger", hulpverlener = hulpverlener};
            _context.groepsChats.Add(zelfhulpgroep);
            _context.SaveChanges();
            // Then
            var result = _context.groepsChats.Any(x => x.Onderwerp == "World Trigger");
            Assert.True(result);
        }

        [Fact]
        public void chatten_in_eenzelfde_hulpgroep_model_tests()
        {
            // Given
            var _context = GetInMemoryDBMetData();
            var hulpverlener = _context.Hulpverleners.First();
            var kuga = _context.Clienten.First();
            var osamu = new Client(){Voornaam = "Osamu", hulpverlener = hulpverlener};
            var chika = new Client(){Voornaam = "Osamu", hulpverlener = hulpverlener};
            _context.Clienten.Add(osamu);
            _context.Clienten.Add(chika);
            var zelfhulpgroep = new GroepsChat(){Onderwerp = "World Trigger", hulpverlener = hulpverlener};
            zelfhulpgroep.Deelnemers.Add(osamu);
            zelfhulpgroep.Deelnemers.Add(kuga);
            zelfhulpgroep.Deelnemers.Add(chika);
            _context.groepsChats.Add(zelfhulpgroep);
            _context.SaveChanges();
            // When
            var kugabericht = new Bericht(){Verzender = kuga, text = "Trigger on!"};
            var osamubericht = new Bericht(){Verzender = osamu, text = "Raygust on!"};
            var chikabericht = new Bericht(){Verzender = chika, text = "Meteor!"};
            _context.groepsChats.Include(x => x.Deelnemers);
            var groepschat = _context.groepsChats.First();
            groepschat.Berichten.AddRange(new List<Bericht>(){kugabericht, osamubericht, chikabericht});
            _context.Update(groepschat);
            _context.SaveChanges();
            // Then
            var result = groepschat.Berichten.Count() == 3;

            Assert.True(result);
        }

        [Fact]
            public void GroepsChat_Zoek_list()
            {
                var _context = GetInMemoryDBMetData();
                var HCAmock = new Mock<IHttpContextAccessor>();
                var chatmock = new Mock<IHubContext<ChatHub>>();

                var hulpverlener = _context.Hulpverleners.First();
                var kuga = _context.Clienten.First();
                var osamu = new Client(){Voornaam = "Osamu", hulpverlener = hulpverlener};
                var chika = new Client(){Voornaam = "Osamu", hulpverlener = hulpverlener};
                var usui = new Client(){Voornaam = "Usui", hulpverlener = hulpverlener};

                _context.Clienten.Add(osamu);
                _context.Clienten.Add(chika);
                _context.Clienten.Add(usui);

                var clientenlijst1 = new List<Client>(){usui, chika};
                var clientenlijst12 = new List<Client>(){osamu, kuga};

                var groep1 = new GroepsChat(){hulpverlener = hulpverlener, Deelnemers = clientenlijst1, Onderwerp = "One Piece"};
                var groep2 = new GroepsChat(){hulpverlener = hulpverlener, Deelnemers = clientenlijst12, Onderwerp = "World Trigger"};

                _context.groepsChats.Add(groep1);
                _context.groepsChats.Add(groep2);
                _context.SaveChanges();

                var sut = new GroepsChatController(_context, HCAmock.Object, chatmock.Object, GetMockUserManager().Object);

                //Act
                _context.groepsChats.Include(x => x.Deelnemers);
                var results = sut.Zoek(_context.groepsChats, "One", null);

                //Assert
                var result = results.First().Onderwerp;
                Assert.True(result == "One Piece");
            }

        [Fact]
        public void MisbruikMelding_test()
        {
            // Given
            var _context = GetInMemoryDBMetData();   
            var client = _context.Clienten.First();
            var hulpverlener = _context.Hulpverleners.First();
            
            var meldingstring = "Dit is sensitive";
            var chat = new Chat() {hulpverlener = hulpverlener, client = client, Id = 1};
            _context.Add(chat);

            var bericht = new Bericht(){Verzender = client, text = "Java is beter dan C#"};
            _context.Berichten.Add(bericht);

            // When
            var melding = new MisbruikMelding(){Melding = "Dit is sensitive", BerichtId = bericht.Id};
            _context.MisbruikMelding.Add(melding);
            _context.SaveChanges();
            // Then
            var loadedmelding = _context.MisbruikMelding.First().Melding;

            Assert.True(loadedmelding.Equals(meldingstring));
        }

    }
}
