using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using src.Models;

namespace src.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DatabaseContext _context;

        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, DatabaseContext context)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
             if(_roleManager.Roles.Count() == 0)
            {
                await _roleManager.CreateAsync(new IdentityRole{Name = "Hulpverlener"});
                await _roleManager.CreateAsync(new IdentityRole{Name = "Voogd"});
                await _roleManager.CreateAsync(new IdentityRole{Name = "Moderator"});
                await _roleManager.CreateAsync(new IdentityRole{Name = "Client"});
            }
            return View();
        }

        public IActionResult Aanmelden()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Introductie()
        {
            return View();
        }
        public IActionResult Klacht()
        {
            return View();
        }

        public IQueryable<Hulpverlener> Zoek(IQueryable<Hulpverlener> lijst, string zoek)
        { 
            if(string.IsNullOrEmpty(zoek))
            {
                return lijst;  
            }
            else
            {
                return lijst.Where(s => s.Voornaam.Contains(zoek.ToLower()) || s.Achternaam.Contains(zoek.ToLower()));  
            }       
        }

        public IActionResult Orthopedagogen(string zoek)
        {
            return View(Zoek(_context.Hulpverleners,zoek));
        }

        public IActionResult Ricco()
        {
            return View();
        }

        public IActionResult Emma()
        {
            return View();
        }

        public IActionResult Anneke()
        {
            return View();
        }

        public IActionResult Harold()
        {
            return View();
        }

        public IActionResult OverOns()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
