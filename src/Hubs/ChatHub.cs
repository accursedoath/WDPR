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
            Console.WriteLine(_context.Clienten.Any(x => x.User.Id == userId));
            if(_context.Clienten.Any(x => x.User.Id == userId)){
                _context.Clienten.Include(x => x.hulpverlener);
                _context.Hulpverleners.Include(x => x.User);
                _context.Users.Include(x => x.Email);
                var verzender = _context.Clienten.Single(x => x.User.Id == userId);
                var hulpverlener = _context.Hulpverleners.Single(x => x.Id == verzender.hulpverlenerId);
                _context.Entry(hulpverlener).Reference(x => x.User).Load(); //goddelijke explicit loading fout opgelost hier
                await Clients.User(hulpverlener.User.Id).SendAsync("ReceiveMessage", user, message);
            }
            else{
                _context.Hulpverleners.Include(x => x.User);
                var hulpverlener = _context.Hulpverleners.Single(x => x.User.Id == userId);            
                //hierzo send message naar client
                }
        }
    }
}