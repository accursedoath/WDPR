using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class Account
{
    [Key]
    public int Id {get; set;}
    public string Voornaam {get; set;}
    public string Achternaam {get; set;}

    [ForeignKey("Woonplaats")]
    public Woonplaats woonplaats {get; set;}

    public Account(string voornaam, string achternaam, Woonplaats Woonplaats)
    {
        Voornaam = voornaam;
        Achternaam = achternaam;
        woonplaats = Woonplaats;
    }

    public Account() {}
}