using System.ComponentModel.DataAnnotations;

public class Hulpverlener : Account
{
    [Key]
    public int Id {get; set;}
    public string Beschrijving {get; set;}

    public virtual ApplicatieGebruiker User {get; set;}     //ef core link naar applicatiegebruiker class van identiy
    public string HulpverlenerProperty {get; set;}
}