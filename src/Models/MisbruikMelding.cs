using System.ComponentModel.DataAnnotations;
using src.Models;

public class MisbruikMelding
{
    [Key]
    public int Id {get; set;}
    public string Melding {get; set;}
    
    public Bericht Bericht {get; set;}
    public int BerichtId {get; set;}
}