using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using src.Models;

namespace src.Controllers
{
    [Authorize(Roles = "Client, Hulpverlener")]
    public class ChatController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly UserManager<IdentityUser> _userManager;
        public ChatController(ILogger<HomeController> logger, DatabaseContext context, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                //var usa = _context.Account.Where
                ViewBag.UserName = _context.Users.Where(x => x.Id == userId).Single().Email;
                //ViewBag.Account = _context.Account.Where(x => x.Voornaam == "David").Single().Achternaam
            return View();
        }
    }
}
