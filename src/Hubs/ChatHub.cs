using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DatabaseContext _context;

        public ChatHub(DatabaseContext context){
            _context = context;
        }
        public async Task SendMessage(string user, string message, string userId)
        {
            var test = Context.UserIdentifier;
            //Console.WriteLine(test);
            _context.Clienten.Include(x => x.User);
            var verzender = _context.Clienten.Single(x => x.User.Id == userId);
            var bericht = new Bericht(){Verzender = verzender, text = message,  Datum = DateTime.Now};
            await _context.Berichten.AddAsync(bericht);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            //await SendPrivateMessage(user, message, userId);
        }

            public async Task pm(string verstuurder, string message, string cid){ //cid = chatid
                int chatid = Int32.Parse(cid);
                var chat = _context.Chats.Find(chatid);
                _context.Entry(chat).Reference(x => x.hulpverlener).Load();
                _context.Entry(chat).Reference(x => x.client).Load();

                var client = chat.client;
                var hulpverlener = chat.hulpverlener;

                _context.Entry(hulpverlener).Reference(x => x.User).Load();
                _context.Entry(client).Reference(x => x.User).Load();

                Console.WriteLine(chat);
                if(verstuurder == "client"){    //client route
                    await saveBericht(chat, message, true);
                    await Clients.Caller.SendAsync("ReceiveMessage", client.Voornaam, message);
                    await Clients.User(hulpverlener.User.Id).SendAsync("ReceiveMessage", client.Voornaam, message);
                }
                else {
                    await saveBericht(chat, message, false);    //hulpverlener route
                    await Clients.Caller.SendAsync("ReceiveMessage", hulpverlener.Voornaam, message);
                    await Clients.User(client.User.Id).SendAsync("ReceiveMessage", hulpverlener.Voornaam, message);
                }
        }

            public async Task saveBericht(Chat chat, string message, bool client){
                Bericht bericht;
                if(client) {
                    _context.Chats.Include(x => x.client);
                    bericht = new Bericht(){ Verzender = chat.client , text= message, Datum = DateTime.Now, chatId = chat.Id};
                    }
                else      {
                    _context.Chats.Include(x => x.hulpverlener);
                     bericht = new Bericht(){ Verzender = chat.hulpverlener, text = message, Datum = DateTime.Now, chatId = chat.Id};
                     }
                     _context.Chats.Include(x => x.Berichten);
                chat.Berichten.Add(bericht);
                await _context.Berichten.AddAsync(bericht);
                await _context.SaveChangesAsync();
            }
    }
}