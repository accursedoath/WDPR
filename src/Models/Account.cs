using Microsoft.AspNetCore.Identity;
using System;

public class Account : IdentityUser
{
    public string Voornaam {get; set;}
    public string Achternaam {get; set;}
    public string Adres {get; set;}
    public string Woonplaats {get; set;}
    public string Postcode {get; set;}
   
    public Account(string voornaam, string achternaam, string adres, string woonplaats, string postcode, string email)
    {
        Voornaam = voornaam;
        Achternaam = achternaam;
        Adres = adres;
        Woonplaats = woonplaats;
        Postcode = postcode;
        Email = email;
    }

    public Account WijzigAccount(int Id, string voornaam, string achternaam, string adres, string woonplaats, string postcode, string email)
    {
        Account OriginalAccount = null;// where account id = id
        Account ChangedAccount = OriginalAccount; // .edit() OR new Account()
        return ChangedAccount;
    }
}