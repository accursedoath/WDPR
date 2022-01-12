using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using src.Models;

namespace src.Controllers
{
    [Authorize(Roles = "Client, Hulpverlener")]
    public class ChatController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ChatController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
