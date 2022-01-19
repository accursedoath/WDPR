using System.ComponentModel.DataAnnotations;

public class MisbruikMelding
{
    [Key]
    public int Id {get; set;}
    public string Melding {get; set;}
    public src.Models.Bericht Bericht {get; set;}
}