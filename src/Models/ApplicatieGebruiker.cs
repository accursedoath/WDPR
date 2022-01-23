using Microsoft.AspNetCore.Identity;

public class ApplicatieGebruiker : IdentityUser
{
    public virtual Client client {get; set;}
    public virtual Hulpverlener hulpverlener {get; set;}
    public virtual Voogd voogd {get; set;}
    public virtual Moderator moderator {get; set;}
}