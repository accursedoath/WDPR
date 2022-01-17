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

        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _roleManager = roleManager;
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

        public IActionResult Orthopedagogen()
        {
            return View();
        }

        public IActionResult Ricco()
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
