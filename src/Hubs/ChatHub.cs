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
            await SendPrivateMessage(user, message, userId);
        }

        public async Task SendPrivateMessage(string user, string message, string userId){
            _context.Clienten.Include(x => x.User);
            _context.Clienten.Include(x => x.hulpverlener);
            _context.Hulpverleners.Include(x => x.User);
        
            if(_context.Clienten.Any(x => x.User.Id == userId)){
                var verzender = _context.Clienten.Single(x => x.User.Id == userId); //client ophalen
                var hulpverlener = _context.Hulpverleners.Single(x => x.Id == verzender.hulpverlenerId);
                _context.Chats.Include(x => x.client);
                var chat = await _context.Chats.SingleAsync(x => x.client.User.Id == userId);
                await saveBericht(chat, message, true);
                _context.Entry(hulpverlener).Reference(x => x.User).Load(); //goddelijke explicit loading fout opgelost hier
                await Clients.User(hulpverlener.User.Id).SendAsync("ReceiveMessage", user, message);
            }
            else{
                _context.Chats.Include(x => x.hulpverlener);
                _context.Hulpverleners.Include(x => x.User);
                var verzender = _context.Hulpverleners.Single(x => x.User.Id == userId); //hulpverlener ophalen
                await _context.Entry(verzender).Reference(x => x.User).LoadAsync();
                var chat = await _context.Chats.SingleAsync(x => x.hulpverlener.User.Id == userId);
                Console.WriteLine(chat.hulpverlener.Voornaam);
                await saveBericht(chat, message, false);
                await _context.Entry(chat).Reference(x => x.client).LoadAsync(); //goddelijke explicit loading fout opgelost hier
                var client = chat.client;
                await _context.Entry(client).Reference(x => x.User).LoadAsync();;
                Console.WriteLine(client.User.Id);
                await Clients.User(client.User.Id).SendAsync("ReceiveMessage", user, message);
            }

        }
            // //user is voornaam, message is bericht, userid hebben we nodig voor het verzenden naar 1 account
            // public async Task SendPrivateMessageH(string user, string message, string clientid){
            //     _context.Chats.Include(x => x.client);
            //     _context.Clienten.Include(x => x.User);
            //     //hier explicit loading als het klapt
            //     var chat = await _context.Chats.SingleAsync(x => x.client.User.Id == clientid);
            //     await saveBericht(chat, message, false);
            //     await Clients.User(clientid).SendAsync("ReceiveMessage", user, message);
            // }

            public async Task saveBericht(Chat chat, string message, bool client){
                Bericht bericht;
                if(client) {
                    _context.Chats.Include(x => x.client);
                    bericht = new Bericht(){ Verzender = chat.client , text= message, Datum = DateTime.Now};
                    }
                else      {
                    _context.Chats.Include(x => x.hulpverlener);
                    Console.WriteLine(chat.Berichten.Count());
                     bericht = new Bericht(){ Verzender = chat.hulpverlener, text = message, Datum = DateTime.Now};
                     }
                     _context.Chats.Include(x => x.Berichten);
                chat.Berichten.Add(bericht);
                await _context.Berichten.AddAsync(bericht);
                await _context.SaveChangesAsync();
            }
    }
}