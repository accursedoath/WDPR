using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Moderator : Account
{
    [ForeignKey("ApplicatieGebruiker")]
    public virtual ApplicatieGebruiker User {get; set;}     //ef core link naar applicatiegebruiker class van identiy
    public void BlokkeerChat(Client client)
    {
        client.magChatten = false;   
    }

    public void DeblokkeerChat(Client client)
    {
         client.magChatten = true; 
    }
}