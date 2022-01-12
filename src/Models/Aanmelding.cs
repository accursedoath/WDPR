using System;
using System.Linq;

public class Aanmelding
{
    public DatabaseContext _context;
    public int AanmeldingId {get; set;}

    public string Voornaam {get; set;}

    public string Achternaam {get; set;}

    public string Email {get; set;}

    public string Stoornis {get; set;}

    public string Leeftijdscategorie {get; set;}

    
    public Aanmelding( DatabaseContext context, string voornaam, string achternaam, string email, string stoornis, string leeftijdscategorie)
    {
        _context = context;

        //if aanmelding table is leeg
        if( _context.Aanmeldingen.Any() == false )
        {
            AanmeldingId = 1;
        }

        else
        {
            AanmeldingId = _context.Aanmeldingen.OrderByDescending( data => data.AanmeldingId ).Last().AanmeldingId + 1;
        }

        Voornaam = voornaam;
        Achternaam = achternaam;
        Email = email;
        Stoornis = stoornis;
        Leeftijdscategorie = leeftijdscategorie;

        Stuurmelding();
    }

    public string Stuurmelding()
    {
        return "er is een account aangemaakt"; //idk dit moeten we even bespreken
    }

}