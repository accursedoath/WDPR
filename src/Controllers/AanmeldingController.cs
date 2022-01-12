using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace src.Controllers
{
    public class AanmeldingController : Controller
    {
        private readonly DatabaseContext _context;

        public AanmeldingController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Aanmelding
        public async Task<IActionResult> Index()
        {
            return View(await _context.Aanmeldingen.ToListAsync());
        }

        // GET: Aanmelding/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aanmelding = await _context.Aanmeldingen
                .FirstOrDefaultAsync(m => m.AanmeldingId == id);
            if (aanmelding == null)
            {
                return NotFound();
            }

            return View(aanmelding);
        }

        // GET: Aanmelding/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aanmelding/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AanmeldingId,Voornaam,Achternaam,Email,Stoornis,Leeftijdscategorie")] Aanmelding aanmelding)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aanmelding);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aanmelding);
        }

        // GET: Aanmelding/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aanmelding = await _context.Aanmeldingen.FindAsync(id);
            if (aanmelding == null)
            {
                return NotFound();
            }
            return View(aanmelding);
        }

        // POST: Aanmelding/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AanmeldingId,Voornaam,Achternaam,Email,Stoornis,Leeftijdscategorie")] Aanmelding aanmelding)
        {
            if (id != aanmelding.AanmeldingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aanmelding);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AanmeldingExists(aanmelding.AanmeldingId))
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
            return View(aanmelding);
        }

        // GET: Aanmelding/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aanmelding = await _context.Aanmeldingen
                .FirstOrDefaultAsync(m => m.AanmeldingId == id);
            if (aanmelding == null)
            {
                return NotFound();
            }

            return View(aanmelding);
        }

        // POST: Aanmelding/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aanmelding = await _context.Aanmeldingen.FindAsync(id);
            _context.Aanmeldingen.Remove(aanmelding);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AanmeldingExists(int id)
        {
            return _context.Aanmeldingen.Any(e => e.AanmeldingId == id);
        }
    }
}
