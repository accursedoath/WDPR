using System;
using System.ComponentModel.DataAnnotations;

public class Moderator : Account
{
    [Key]
    public int Id {get; set;}
    public virtual ApplicatieGebruiker User {get; set;}     //ef core link naar applicatiegebruiker class van identiy
    public string ModeratorProperty {get; set;}
    public void BlokkeerChat(Client client)
    {
        client.magChatten = false;   
    }

    public void DeblokkeerChat(Client client)
    {
         client.magChatten = true; 
    }
}