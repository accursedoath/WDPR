using System;
using System.Collections.Generic;
using src.Models;

public class GroepsChat{
    public int Id {get; set;}
    public string Onderwerp {get; set;}
    public string LeeftijdsCategorie {get; set;}

    public Hulpverlener hulpverlener {get; set;}
    public List<Client> Deelnemers {get; set;}
    public List<Bericht> Berichten {get; set;}
    public DateTime eindDatum {get; set;}

    public GroepsChat(){
        Deelnemers = new List<Client>();
    }
}