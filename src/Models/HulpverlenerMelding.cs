using System.ComponentModel.DataAnnotations;

public class HulpverlenerMelding
{
    [Key]
    public int Id {get; set;}
    public string Melding = "Uw client is geblokkeer van de chat.";
    public Client Client {get; set;}
}