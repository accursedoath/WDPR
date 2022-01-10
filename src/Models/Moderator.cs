using System;

public class Moderator : Account
{
    public void BlokkeerChat(Client client)
    {
        client.magChatten = false;   
    }

    public void DeblokkeerChat(Client client)
    {
         client.magChatten = true; 
    }
}