using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using src.Models;

namespace src.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicatieGebruiker> _signInManager;
        private readonly UserManager<ApplicatieGebruiker> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly DatabaseContext _context;

        public RegisterModel(
            UserManager<ApplicatieGebruiker> userManager,
            SignInManager<ApplicatieGebruiker> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            DatabaseContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Functie")]
            public string Functie { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Voornaam")]
            public string Voornaam { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Hulpverlener")]
            public string Hulpverlener { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ViewData["Hulpverleners"] = await _context.Hulpverleners.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicatieGebruiker { UserName = Input.Email, Email = Input.Email };
                    if(Input.Functie == "1") {
                        user.moderator = new Moderator(){Voornaam = Input.Voornaam};
                        }

                    if(Input.Functie == "2") {
                        user.hulpverlener = new Hulpverlener(){Voornaam = Input.Voornaam};
                        }
                    if(Input.Functie == "3") {
                        var hulpverlener = _context.Hulpverleners.Single(x => x.Id == Int32.Parse(Input.Hulpverlener));
                        var client = new Client(){Voornaam = Input.Voornaam, hulpverlener = hulpverlener};
                        user.client = client;
                        var chat = new Chat() { client = client, hulpverlener = hulpverlener };
                        _context.Chat.Add(chat);
                        await _context.SaveChangesAsync();
                        }
                    if(Input.Functie == "4") {
                        user.voogd = new Voogd(){Voornaam = Input.Voornaam};
                        }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if(Input.Functie == "1") {
                        await _userManager.AddToRoleAsync(user, "Moderator");
                        user.moderator = new Moderator(){Voornaam = "OMIN HARD GELUKT!"};
                        }

                    if(Input.Functie == "2") {
                        await _userManager.AddToRoleAsync(user, "Hulpverlener");
                        user.hulpverlener = new Hulpverlener();
                        }
                    if(Input.Functie == "3") {
                        await _userManager.AddToRoleAsync(user, "Client");
                        user.client = new Client();
                        }
                    if(Input.Functie == "4") {
                        await _userManager.AddToRoleAsync(user, "Voogd");
                        user.voogd = new Voogd();
                        }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
