using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace src.Controllers
{
    public class VoogdController : Controller
    {
        private readonly DatabaseContext _context;

        public VoogdController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Voogd
        public async Task<IActionResult> Index()
        {
            return View(await _context.Voogd.ToListAsync());
        }

        // GET: Voogd/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voogd = await _context.Voogd
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voogd == null)
            {
                return NotFound();
            }

            return View(voogd);
        }

        // GET: Voogd/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Clienten"] = await _context.Clienten.ToListAsync();
            return View();
        }

        // POST: Voogd/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Voornaam,Achternaam,Client")] Voogd voogd)
        {
            ViewData["Clienten"] = await _context.Clienten.ToListAsync();
            if (ModelState.IsValid)
            {
                _context.Add(voogd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voogd);
        }

        // GET: Voogd/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voogd = await _context.Voogd.FindAsync(id);
            if (voogd == null)
            {
                return NotFound();
            }
            return View(voogd);
        }

        // POST: Voogd/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Voornaam,Achternaam")] Voogd voogd)
        {
            if (id != voogd.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voogd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoogdExists(voogd.Id))
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
            return View(voogd);
        }

        // GET: Voogd/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voogd = await _context.Voogd
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voogd == null)
            {
                return NotFound();
            }

            return View(voogd);
        }

        // POST: Voogd/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var voogd = await _context.Voogd.FindAsync(id);
            _context.Voogd.Remove(voogd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoogdExists(int id)
        {
            return _context.Voogd.Any(e => e.Id == id);
        }
    }
}
