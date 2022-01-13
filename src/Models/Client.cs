using System;
using System.ComponentModel.DataAnnotations;

public class Client : Account
{
    [Key]
    public int Id {get; set;}
    public bool magChatten {get; set;}

    public virtual ApplicatieGebruiker User {get; set;}     //ef core link naar applicatiegebruiker class van identiy
    public string ClientProperty {get; set;}
}

