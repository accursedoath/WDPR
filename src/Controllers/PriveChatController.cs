using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Controllers
{
    public class PriveChatController : Controller
    //Als een hulpverlener op een link klikt, dan wordt die link zo ingestelt met asp taggs
    //Op de hulpverlener pagina zie een lijst met links naar prive chats, deze links geven de id mee
    //De privechat controller neemt deze parameter tot zich en stuurt deze naar de hub
    //via de hub wordt de bericht opgeslagen in de desbetreffende chat.
    {
        private readonly DatabaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PriveChatController(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: PriveChat
        public async Task<IActionResult> Index()
        {
            _context.Chats.Include(x => x.client);
            _context.Hulpverleners.Include(x => x.User);
            var hulpverlenerIdentityid = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var hulpverlener = _context.Hulpverleners.Single(x => x.User.Id == hulpverlenerIdentityid);
            var clientenchats = await _context.Chats.Where(x => x.hulpverlener.Id == hulpverlener.Id).ToListAsync();
            foreach(var x in clientenchats){
                _context.Entry(x).Reference(x => x.client).Load();
            }
            return View(clientenchats);
        }

        public IActionResult Chat(int id)
        {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.uId = userId;
                var client = _context.Clienten.Any(x => x.User.Id == userId);
                if(client){
                    _context.Chats.Include(x => x.client);
                    var c1 = _context.Clienten.Where(x => x.User.Id == userId).FirstOrDefault();
                    _context.Entry(c1).Reference(x => x.hulpverlener).LoadAsync();
                    ViewBag.Chatnaam = "Hulpverlener " + c1.hulpverlener.Voornaam;
                    ViewBag.chattid = _context.Chats.FirstOrDefault(x => x.client.Id == c1.Id).Id;;
                    ViewBag.clientid = 0;
                    ViewBag.verstuurder = "client";
                }
                else {
                    _context.Hulpverleners.Include(x => x.User);
                    _context.Chats.Include(x => x.hulpverlener);
                    _context.Chats.Include(x => x.client);
                    var h1 = _context.Hulpverleners.Where(x => x.User.Id == userId).FirstOrDefault();
                    var client2 = _context.Clienten.Find(id);
                    ViewBag.Chatnaam ="Client " + client2.Voornaam;
                    ViewBag.clientid = id;
                    ViewBag.chattid = _context.Chats.Single(x => x.hulpverlener.Id == h1.Id && x.client.Id == client2.Id).Id;
                    ViewBag.verstuurder = "hulpverlener";
                }
                return View();
        }

        // GET: PriveChat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // GET: PriveChat/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PriveChat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,naam")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

        // GET: PriveChat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return View(chat);
        }

        // POST: PriveChat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,naam")] Chat chat)
        {
            if (id != chat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatExists(chat.Id))
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
            return View(chat);
        }

        // GET: PriveChat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // POST: PriveChat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatExists(int id)
        {
            return _context.Chats.Any(e => e.Id == id);
        }
    }
}
