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
    public class TransakcijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransakcijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transakcija
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transactions.Include(t => t.Category);
            return View(await applicationDbContext.ToListAsync());
        }

       

        // GET: Transakcija/AddOrEdit
        public IActionResult AddOrEdit(int id = 0)
        {
            PopulateCategories();
            if(id == 0) 
            return View(new Transakcija());
            else
                return View(_context.Transactions.Find(id));
        }

        // POST: Transakcija/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId,CategoryId,Iznos,Beleska,Datum")] Transakcija transakcija)
        {
            if (ModelState.IsValid)
            {
                if(transakcija.TransactionId==0)
                _context.Add(transakcija);
                else
                _context.Update(transakcija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCategories();
            return View(transakcija);
        }

      


        // POST: Transakcija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transakcija = await _context.Transactions.FindAsync(id);
            if (transakcija != null)
            {
                _context.Transactions.Remove(transakcija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public void PopulateCategories()
        {
            var CategoryCollection = _context.Categories.ToList(); // vraca sve kategorije u dropdown listi
            // ovo smo primenili u transaction add or edit

            Kategorija DefaultCategory = new Kategorija() {CategoryId = 0, Naziv = "Izaberi kategoriju"}; // difoltna kategorija
            CategoryCollection.Insert(0, DefaultCategory); // insertujemo difoltnu kategoriju u CategoryCollection
            ViewBag.Kategorija = CategoryCollection;
        }
      
    }
}
