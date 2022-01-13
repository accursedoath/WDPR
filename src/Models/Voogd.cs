using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Voogd : Account
{
    [Key]
    public int Id {get; set;}

    [ForeignKey("ApplicatieGebruiker")]
    public virtual ApplicatieGebruiker User {get; set;}     //ef core link naar applicatiegebruiker class van identiy
    public int? checkFrequentie()
    {
        return null;
        // Moet nog aanvulling krijgen
    }
}