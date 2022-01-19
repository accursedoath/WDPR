using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Hulpverlener : Account
{
    public string Beschrijving {get; set;}

    [ForeignKey("ApplicatieGebruiker")]
    public virtual ApplicatieGebruiker User {get; set;}     //ef core link naar applicatiegebruiker class van identiy
}