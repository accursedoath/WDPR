using System.ComponentModel.DataAnnotations;

public class Voogd : Account
{
    [Key]
    public int Id {get; set;}
    public virtual ApplicatieGebruiker User {get; set;}     //ef core link naar applicatiegebruiker class van identiy
    public string VoogdProperty {get; set;}
    public int? checkFrequentie()
    {
        return null;
        // Moet nog aanvulling krijgen
    }
}