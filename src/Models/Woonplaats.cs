using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class Woonplaats
{
    [Key]
    public int Id {get; set;}
    public string Adres {get; set;}
    public string plaats {get; set;}
    public string Postcode {get; set;}

    public Woonplaats(string adres, string woonplaats, string postcode)
    {
        Adres = adres;
        plaats = woonplaats;
        Postcode = postcode;
    }
}