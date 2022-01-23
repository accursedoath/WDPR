using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

public class Aanmelding
{
    public int AanmeldingId {get; set;}

    public string Voornaam {get; set;}

    public string Achternaam {get; set;}

    public int BSN {get;set;}

    public string Email {get; set;}

    public string Stoornis {get; set;}

    public string Leeftijdscategorie {get; set;}

    public string AfspraakDatum {get; set;}

    public string NaamVoogd {get; set;}

    public string EmailVoogd {get; set;}

    public string TelefoonVoogd {get; set;}

    public Hulpverlener Hulpverlener {get; set;}

    public int HulpverlenerId {get; set;}

}