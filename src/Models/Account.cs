using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class Account
{
    public string Voornaam {get; set;}
    public string Achternaam {get; set;}

    [ForeignKey("Woonplaats")]
    public Woonplaats woonplaats {get; set;}

    public Account(string voornaam, string achternaam, Woonplaats Woonplaats)
    {
        Voornaam = voornaam;
        Achternaam = achternaam;
        woonplaats = Woonplaats;
    }

    public Account() {}

    public Account WijzigAccount(int Id, string voornaam, string achternaam, string adres, string woonplaats, string postcode, string email)
    {
        Account OriginalAccount = null;// where account id = id
        Account ChangedAccount = OriginalAccount; // .edit() OR new Account()
        return ChangedAccount;
    }
}