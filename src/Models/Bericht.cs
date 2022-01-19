using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class Bericht
    {
        public int Id {get;set;}
        public string text {get; set;}
        public DateTime Datum {get; set;} //Tijd in uml (deze nog aanpassen in het uml)
        
        public Chat chat {get; set;}
        public int chatId {get; set;}

        [ForeignKey("Account")]
        public Account Verzender {get; set;}
        public int VerzenderId {get; set;}

    }
}