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
    [Authorize(Roles = "Hulpverlener,Client")]
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
            return View(await _context.Chats.ToListAsync());
        }

        public IActionResult Chat()
        {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.uId = userId;
                var client = _context.Clienten.Any(x => x.User.Id == userId);
                if(client){
                    _context.Clienten.Include(x => x.User);
                    var c1 = _context.Clienten.Where(x => x.User.Id == userId).FirstOrDefault();
                    ViewBag.UserName = c1.Voornaam;
                    ViewBag.accountid = c1.Id;
                }
                else {
                    _context.Hulpverleners.Include(x => x.User);
                    var h1 = _context.Hulpverleners.Where(x => x.User.Id == userId).FirstOrDefault();
                    ViewBag.UserName = h1.Voornaam;
                    ViewBag.accountid = h1.Id;
                    //ViewBag.reciever 
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
