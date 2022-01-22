using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Hubs;

namespace src.Controllers
{
    public class GroepsChatController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<ApplicatieGebruiker> _userManager;

        public GroepsChatController(DatabaseContext context, IHttpContextAccessor httpContextAccessor,IHubContext<ChatHub> hubContext, UserManager<ApplicatieGebruiker> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        // GET: GroepsChat 
        public async Task<IActionResult> Index(string onderwerp, string leeftijd)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _context.Users.FindAsync(userId);
            ViewBag.onderwerp = onderwerp;
            ViewBag.leeftijd = leeftijd;

            _context.Clienten.Include(x => x.User);
            if(_context.Clienten.Any(x => x.User.Id == userId)){
                ViewBag.hulpverlener = false;
                var client = await _context.Clienten.SingleAsync(x => x.User.Id == userId);
                //var x = Zoek(clientengroepenlijst ,onderwerp, leeftijd);
                    return View(Zoek(getClientList(client), onderwerp, leeftijd));
                }

            else {
                _context.Hulpverleners.Include(x => x.User);
                ViewBag.hulpverlener = true;
                return View(Zoek(_context.groepsChats.Where(x => x.hulpverlener.User.Id == userId), onderwerp, leeftijd));
            }
            // return View(await _context.groepsChats.ToListAsync()); Miss voor moderator?
        }
        [HttpPost]
        public IActionResult ZoekGroep(string onderwerp, string leeftijd){
            return RedirectToAction("Index", new {onderwerp = onderwerp, leeftijd = leeftijd} );
        }

        private IQueryable<GroepsChat> getClientList(Client client)
        {
            _context.groepsChats.Include(x => x.Deelnemers);
            var clientenlijst = _context.groepsChats.Where(x => !x.Deelnemers.Contains(client));
            return clientenlijst;
        }

        public IQueryable<GroepsChat> Zoek(IQueryable<GroepsChat> clientenlijst, string onderwerp, string leeftijd){
            if(onderwerp == null && leeftijd == null) return clientenlijst;
            else if (leeftijd == null) return clientenlijst.Where(x => x.Onderwerp.Contains(onderwerp));
            else return clientenlijst.Where(x =>x.LeeftijdsCategorie.Contains(leeftijd));
        }
        public async Task<IActionResult> MijnChats(){  //heel die frontend maken van deze, of we geven mee aan index en gebruiken die frontend
            _context.Clienten.Include(x => x.User);
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(_context.Clienten.Any(x => x.User.Id == userId)){
                ViewBag.ishulpverlener = false;
                var client = await _context.Clienten.SingleAsync(x => x.User.Id == userId);
                return View(await _context.groepsChats.Where(x => x.Deelnemers.Contains(client)).ToListAsync());
            }
            else{
                ViewBag.ishulpverlener = true;
                _context.Hulpverleners.Include(x => x.User);
                var hulpverlener = await _context.Hulpverleners.SingleAsync(x => x.User.Id == userId);
                return View(await _context.groepsChats.Where(x => x.hulpverlener == hulpverlener).ToListAsync());
            }
        } 
        

        // GET: GroepsChat EnterGroepsChat
        public async Task<IActionResult> EnterGroepsChat(int id)        //nog compatible maken voor hulpverlener
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _context.Clienten.Include(x => x.User);
            var client = await _context.Clienten.SingleAsync(x => x.User.Id == userId);
            var groepschat = await _context.groepsChats.FindAsync(id);
            _context.groepsChats.Include(x => x.Deelnemers);
            groepschat.Deelnemers.Add(client);
            await _context.SaveChangesAsync();

            await _hubContext.Groups.AddToGroupAsync(userId, groepschat.Onderwerp);
            await _hubContext.Clients.Group(groepschat.Onderwerp).SendAsync("Send", $" "+ userId +" has joined the group "+groepschat.Onderwerp+".");
            Console.WriteLine(client.Voornaam + " Succesfully joined the " + groepschat.Onderwerp + " Group");
            return RedirectToAction("Chat",new {id = id});
        }

        // GET: Chat met iedereen
        public async Task<IActionResult> Chat(int id){
            var groepsChat = await _context.groepsChats
                .FirstOrDefaultAsync(m => m.Id == id);


            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            _context.Clienten.Include(x => x.User);
            if(_context.Clienten.Any(x => x.User.Id == userId)){
            var client = await _context.Clienten.SingleAsync(x => x.User.Id == userId);
                ViewBag.naam = client.Voornaam;
                await _context.Entry(client).ReloadAsync();
                //Console.WriteLine(client.magChatten);
                if(client.magChatten) ViewBag.magchatten = "ja";
                else ViewBag.magchatten = "nee";
            }
            else{
                await _context.Entry(groepsChat).Reference(x => x.hulpverlener).LoadAsync();
                ViewBag.naam = groepsChat.hulpverlener.Voornaam;
                ViewBag.magchatten = "ja";
            }
            

            Console.WriteLine(groepsChat.Onderwerp);
            ViewBag.groepid = id;
            ViewBag.gp = groepsChat.Onderwerp;
            // await _context.Entry(groepsChat).Collection(x => x.Berichten).LoadAsync();
            
            return View(new GroepsChat() {Onderwerp = "Jouw kke moeder"});
        }

        // GET: GroepsChat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groepsChat = await _context.groepsChats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groepsChat == null)
            {
                return NotFound();
            }

            return View(groepsChat);
        }

        // GET: GroepsChat/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GroepsChat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Onderwerp,LeeftijdsCategorie,eindDatum")] GroepsChat groepsChat)
        {
            if (ModelState.IsValid)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.Hulpverleners.Include(x => x.User);
                var hulpverlener = await _context.Hulpverleners.SingleAsync(x => x.User.Id == userId);
                groepsChat.hulpverlener = hulpverlener;
                _context.Add(groepsChat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groepsChat);
        }

        // GET: GroepsChat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groepsChat = await _context.groepsChats.FindAsync(id);
            if (groepsChat == null)
            {
                return NotFound();
            }
            return View(groepsChat);
        }

        // POST: GroepsChat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Onderwerp,LeeftijdsCategorie,eindDatum")] GroepsChat groepsChat)
        {
            if (id != groepsChat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groepsChat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroepsChatExists(groepsChat.Id))
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
            return View(groepsChat);
        }

        // GET: GroepsChat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groepsChat = await _context.groepsChats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groepsChat == null)
            {
                return NotFound();
            }

            return View(groepsChat);
        }

        // POST: GroepsChat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groepsChat = await _context.groepsChats.FindAsync(id);
            _context.groepsChats.Remove(groepsChat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroepsChatExists(int id)
        {
            return _context.groepsChats.Any(e => e.Id == id);
        }
    }
}
