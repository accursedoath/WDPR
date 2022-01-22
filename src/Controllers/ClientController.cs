using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace src.Controllers
{
    public class ClientController : Controller
    {
        private readonly DatabaseContext _context;
        public Dossier aanmeldingToCS { get; set;}
        private readonly IHttpClientFactory _clientFactory;
        private readonly UserManager<ApplicatieGebruiker> _userManager;

        public ClientController(DatabaseContext context, IHttpClientFactory clientFactory, UserManager<ApplicatieGebruiker> userManager)
        {
            _context = context;
            _clientFactory = clientFactory;
            _userManager = userManager;
        }

        // GET: Client
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clienten.ToListAsync());
        }

        public async Task<IActionResult> Dossier()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var clientId = _context.Clienten.Where(c => c.User.Id == userId).SingleOrDefault().Id;
            string url = "https://orthopedagogie-zmdh.herokuapp.com/clienten?sleutel=147699692&clientid=" + clientId;
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.SendAsync(request);
            if(response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                aanmeldingToCS = await JsonSerializer.DeserializeAsync<Dossier>(responseStream);
            }
            else
            {
                aanmeldingToCS = new Dossier();
            }
            return View(aanmeldingToCS);
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clienten
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Client/Woonplaats
        public IActionResult Woonplaats()
        {
            return View();
        }

        // POST: Client/Woonplaats
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Woonplaats([Bind("Adres,plaats,Postcode")] Woonplaats woonplaats)
        {
            if (ModelState.IsValid)
            {
                var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Clienten.Include(s => s.User);
                var id = _context.Clienten.Where(s => s.User.Id == userId).SingleOrDefault().Id;
                var client = _context.Clienten.Where(c => c.Id == id).SingleOrDefault();
                client.woonplaats = woonplaats;
                _context.Update(client);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(woonplaats);
        }

        // GET: Client/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("magChatten,Id,Voornaam,Achternaam")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clienten.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("magChatten,Id,Voornaam,Achternaam")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clienten
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clienten.FindAsync(id);
            _context.Clienten.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clienten.Any(e => e.Id == id);
        }
    }
}
