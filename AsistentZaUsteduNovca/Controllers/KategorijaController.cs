using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsistentZaUsteduNovca.Models;

namespace AsistentZaUsteduNovca.Controllers
{
    public class KategorijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KategorijaController(ApplicationDbContext context) // konstruktor kontrolera
        {
            // vrednost instance klase ApplicationDbContext bice prosledjena iz dependency injectiona
            // i bice dodeljena _context promenljivoj
            _context = context;
        }

        // GET: Kategorija
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
            // ovaj metod pokusava da uzme sve requestove iz Categories table koje ce biti prikazane
            // u http://localhost:5054/Category/Index
        }

       

        // GET: Kategorija/AddOrEddit  // Ex create metoda
        public IActionResult AddOrEdit( int id = 0) // ako je id = 0 za add ili edit onda prosledjujemo svezu instancu modela 
            // ako nije moramo vratiti form? za edit operaciju
        {
            if(id == 0)
                return View(new Kategorija());
            else
                return View(_context.Categories.Find(id));

        }

        // POST: Kategorija/AddOrDelete
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CategoryId,Naziv,Ikonica,Tip")] Kategorija kategorija)
        {
            if (ModelState.IsValid)
            {
                if(kategorija.CategoryId == 0)
                _context.Add(kategorija);
                else
                    _context.Update(kategorija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kategorija);
        }

        

       

        // POST: Kategorija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kategorija = await _context.Categories.FindAsync(id);
            if (kategorija != null)
            {
                _context.Categories.Remove(kategorija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KategorijaExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
