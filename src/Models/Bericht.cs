using System;

namespace src.Models
{
    public class Bericht
    {
        public int Id {get;set;}
        public string text {get; set;}
        public DateTime Datum {get; set;} //Tijd in uml (deze nog aanpassen in het uml)

        public Account Verzender {get; set;}
        public int VerzenderId {get; set;}
    }
}