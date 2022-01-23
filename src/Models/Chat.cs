using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SignalRChat.Hubs;

namespace src.Models
{
    public class Chat   //bij het aanmaken van een client wordt er een chat automatisch aangemaakt?
    {
        public int Id {get;set;}
        public string naam {get; set;}

        [ForeignKey("Bericht")]
        public List<Bericht> Berichten {get; set;}
        public Hulpverlener hulpverlener {get; set;}
        public int hulpverlenerId {get; set;}
        public Client client {get; set;}
        public int clientId {get; set;}

        public Chat(){
            Berichten = new List<Bericht>();
        }
    }


}