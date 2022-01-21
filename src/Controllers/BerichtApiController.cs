using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BerichtApiController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public BerichtApiController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("freq/{id}")]    //geef chat id
        public async Task<ActionResult<List<string>>> GetFrequentie(int id)
        {
            _context.Berichten.Include(x => x.chat);
            var berichtenlijst = await _context.Berichten.Where(x => x.chatId == id).ToListAsync();
            var tijdLijst = new List<string>();
            foreach(var x in berichtenlijst){
                DateTime date = DateTime.ParseExact(x.Datum.ToShortDateString(), "M/dd/yyyy", CultureInfo.InvariantCulture);
                string formattedDate = date.ToString( "dd/M/yyyy");

                if(!tijdLijst.Contains(formattedDate))
                {
                    tijdLijst.Add(formattedDate);
                }
            }
            return tijdLijst;
        }

        // GET: api/BerichtApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bericht>>> GetBerichten()
        {
            return await _context.Berichten.ToListAsync();
        }

        [HttpGet("all/{id}")]    //geef chat id
        public async Task<ActionResult<IEnumerable<Bericht>>> GetBerichten(int id)
        {
            // _context.Berichten.Include(x => x.Verzender);
            // _context.Berichten.Include(x => x.Verzender.Id);
            // return await  _context.Berichten.Where(x => x.Verzender.Id == _context.Clienten.Single(x => x.Id == id).Id).ToListAsync();
            _context.Berichten.Include(x => x.chat);
            var berichtenlijst = await _context.Berichten.Where(x => x.chatId == id).ToListAsync();
            foreach(var x in berichtenlijst){
                await _context.Entry(x).Reference(x => x.Verzender).LoadAsync();
            }
            return berichtenlijst;
        }

        [HttpGet("allGroup/{id}")]    //geef groep id
        public async Task<ActionResult<IEnumerable<Bericht>>> GetGroepBerichten(int id)
        {
            _context.groepsChats.Include(x => x.Berichten);
            var groep = await _context.groepsChats.SingleAsync(x => x.Id == id);
            await _context.Entry(groep).Collection(x => x.Berichten).LoadAsync();
            var newlist = groep.Berichten;
            foreach(var x in newlist){
                await _context.Entry(x).Reference(x => x.Verzender).LoadAsync();
                Console.WriteLine(x.Verzender);
            }
            return newlist;
        }
        // GET: api/BerichtApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bericht>> GetBericht(int id)
        {
            var bericht = await _context.Berichten.FindAsync(id);

            if (bericht == null)
            {
                return NotFound();
            }
            return bericht;
        }

        // PUT: api/BerichtApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBericht(int id, Bericht bericht)
        {
            if (id != bericht.Id)
            {
                return BadRequest();
            }

            _context.Entry(bericht).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BerichtExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BerichtApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bericht>> PostBericht(Bericht bericht)
        {
            _context.Berichten.Add(bericht);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBericht", new { id = bericht.Id }, bericht);
        }

        // DELETE: api/BerichtApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBericht(int id)
        {
            var bericht = await _context.Berichten.FindAsync(id);
            if (bericht == null)
            {
                return NotFound();
            }

            _context.Berichten.Remove(bericht);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BerichtExists(int id)
        {
            return _context.Berichten.Any(e => e.Id == id);
        }
    }
}
