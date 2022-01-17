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
            var test = Context.User;
            Console.WriteLine(test.Identity.Name);
            _context.Clienten.Include(x => x.User);
            var verzender = _context.Clienten.Single(x => x.User.Id == userId);
            var bericht = new Bericht(){Verzender = verzender, text = message,  Datum = DateTime.Now};
            await _context.Berichten.AddAsync(bericht);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }
}