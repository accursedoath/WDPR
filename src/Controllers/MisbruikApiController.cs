using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MisbruikApiController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public MisbruikApiController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/MisbruikApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MisbruikMelding>>> GetMisbruikMelding()
        {
            var meldingen = await _context.MisbruikMelding.ToListAsync();
            foreach(var x in meldingen){
                await _context.Entry(x).Reference(x => x.Bericht).LoadAsync();
            }
            return meldingen;
        }

        // GET: api/MisbruikApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MisbruikMelding>> GetMisbruikMelding(int id)
        {
            var misbruikMelding = await _context.MisbruikMelding.FindAsync(id);

            if (misbruikMelding == null)
            {
                return NotFound();
            }

            return misbruikMelding;
        }

        // PUT: api/MisbruikApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMisbruikMelding(int id, MisbruikMelding misbruikMelding)
        {
            if (id != misbruikMelding.Id)
            {
                return BadRequest();
            }

            _context.Entry(misbruikMelding).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MisbruikMeldingExists(id))
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

        // POST: api/MisbruikApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MisbruikMelding>> PostMisbruikMelding(MisbruikMelding misbruikMelding)
        {
            // await _context.Entry(misbruikMelding).Reference(x => x.Bericht).LoadAsync();
            // var bericht = misbruikMelding.Bericht;
            // await _context.Entry(bericht).Reference(x => x.Verzender).LoadAsync();
            _context.MisbruikMelding.Add(misbruikMelding);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMisbruikMelding", new { id = misbruikMelding.Id }, misbruikMelding);
        }

        // DELETE: api/MisbruikApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMisbruikMelding(int id)
        {
            var misbruikMelding = await _context.MisbruikMelding.FindAsync(id);
            if (misbruikMelding == null)
            {
                return NotFound();
            }

            _context.MisbruikMelding.Remove(misbruikMelding);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MisbruikMeldingExists(int id)
        {
            return _context.MisbruikMelding.Any(e => e.Id == id);
        }
    }
}
