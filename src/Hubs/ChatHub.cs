using Microsoft.AspNetCore.Identity;
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
        private readonly SignInManager<ApplicatieGebruiker> _signInManager;

        public ChatHub(DatabaseContext context, SignInManager<ApplicatieGebruiker> signInManager){
            _context = context;
            _signInManager = signInManager;
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

                    await Groups.AddToGroupAsync(Context.ConnectionId, cid);
                    await Clients.Group(cid).SendAsync("ReceiveMessage", client.Voornaam, message);
                    await Clients.Group(cid).SendAsync("ReceiveMessage", DateTime.Now.ToString("ddd dd MMM yyyy"));
                    // await Clients.Caller.SendAsync("ReceiveMessage", client.Voornaam, message);
                    // await Clients.User(hulpverlener.User.Id).SendAsync("ReceiveMessage", client.Voornaam, message);
                }
                else {
                    await saveBericht(chat, message, false);    //hulpverlener route
                    
                    await Groups.AddToGroupAsync(Context.ConnectionId, cid);
                    await Clients.Group(cid).SendAsync("ReceiveMessage", hulpverlener.Voornaam, message);
                    await Clients.Group(cid).SendAsync("ReceiveMessage", DateTime.Now.ToString("ddd dd MMM yyyy"));
                    // await Clients.Caller.SendAsync("ReceiveMessage", hulpverlener.Voornaam, message);
                    // await Clients.User(client.User.Id).SendAsync("ReceiveMessage", hulpverlener.Voornaam, message);
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
            // public async Task gm(string groepnaam, int groepid, string message){
            public async Task gm(string groepnaam, string groepidn, string message){
                
                int groepid = Int32.Parse(groepidn);
                var groep = await _context.groepsChats.FindAsync(groepid);
                var verzender = await _context.Users.FindAsync(Context.ConnectionId);
                _context.Clienten.Include(x => x.User);
                _context.Hulpverleners.Include(x => x.User);

                string user;
                Console.WriteLine("line 84");
                Bericht bericht;
                    Console.WriteLine(_context.Clienten.Any(x => x.User.Id == Context.UserIdentifier));
                if(_context.Clienten.Any(x => x.User.Id == Context.UserIdentifier)){;
                    var client = await _context.Clienten.SingleAsync(x => x.User.Id == Context.UserIdentifier);
                    Console.WriteLine(await _context.Clienten.SingleAsync(x => x.User.Id == Context.UserIdentifier));
                    user = client.Voornaam;
                    Console.WriteLine("client = " + client);
                    bericht = new Bericht(){Verzender = client, text = message, Datum = DateTime.Now, };
                }
                else {
                    var hulpverlener = await _context.Hulpverleners.SingleAsync(x => x.User.Id == Context.UserIdentifier);
                    Console.WriteLine("debug hierzo ");
                    user = hulpverlener.Voornaam;
                    bericht = new Bericht(){Verzender = hulpverlener, text = message, Datum = DateTime.Now};
                }
                await _context.Entry(groep).Collection(x => x.Berichten).LoadAsync();
                groep.Berichten.Add(bericht);
                await _context.SaveChangesAsync();
                await Clients.Group(groepnaam).SendAsync("ReceiveMessage",user ,message);
            }
                public async Task RemoveFromGroup(string groupName)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                    await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
                }
                public async Task AddToGroup(string groupName)
                {
                    Console.WriteLine(groupName);
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                    await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
                }
    }
}