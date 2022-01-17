using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class Chat
    {
        public int Id {get;set;}
        public string naam {get; set;}

        [ForeignKey("Bericht")]
        public List<Bericht> Berichten {get; set;}
        public int BerichteniD {get; set;}
    }
}