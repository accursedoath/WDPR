using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace src.Controllers
{
    public class MisbruikMeldingController : Controller
    {
        private readonly DatabaseContext _context;

        public MisbruikMeldingController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: MisbruikMelding
        public async Task<IActionResult> Index()
        {
            return View(await _context.MisbruikMelding.ToListAsync());
        }

        // GET: MisbruikMelding/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var misbruikMelding = await _context.MisbruikMelding
                .FirstOrDefaultAsync(m => m.Id == id);
            if (misbruikMelding == null)
            {
                return NotFound();
            }

            return View(misbruikMelding);
        }

        // GET: MisbruikMelding/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MisbruikMelding/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Melding")] MisbruikMelding misbruikMelding)
        {
            if (ModelState.IsValid)
            {
                _context.Add(misbruikMelding);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(misbruikMelding);
        }

        // GET: MisbruikMelding/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var misbruikMelding = await _context.MisbruikMelding.FindAsync(id);
            if (misbruikMelding == null)
            {
                return NotFound();
            }
            return View(misbruikMelding);
        }

        // POST: MisbruikMelding/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Melding")] MisbruikMelding misbruikMelding)
        {
            if (id != misbruikMelding.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(misbruikMelding);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MisbruikMeldingExists(misbruikMelding.Id))
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
            return View(misbruikMelding);
        }

        // GET: MisbruikMelding/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var misbruikMelding = await _context.MisbruikMelding
                .FirstOrDefaultAsync(m => m.Id == id);
            if (misbruikMelding == null)
            {
                return NotFound();
            }

            return View(misbruikMelding);
        }

        // POST: MisbruikMelding/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var misbruikMelding = await _context.MisbruikMelding.FindAsync(id);
            _context.MisbruikMelding.Remove(misbruikMelding);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MisbruikMeldingExists(int id)
        {
            return _context.MisbruikMelding.Any(e => e.Id == id);
        }
    }
}
