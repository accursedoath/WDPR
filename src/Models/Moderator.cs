using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Moderator : Account
{
    [ForeignKey("ApplicatieGebruiker")]
    public virtual ApplicatieGebruiker User {get; set;}     //ef core link naar applicatiegebruiker class van identity
}