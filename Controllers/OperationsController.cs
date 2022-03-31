using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Banque.Data;
using Banque.Models;

namespace Banque.Controllers
{
    public class OperationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OperationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Operations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Operations.Include(o => o.Compte);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Operations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations
                .Include(o => o.Compte)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View(operation);
        }

        // GET: Operations/Create
        public IActionResult Create()
        {
            //IEnumerable<Compte> comptes = _context.Comptes;//.Concat(_context.ComptesEpargnes).Concat(_context.ComptesPayants);
            ViewData["CompteId"] = new SelectList(_context.Comptes, "Id", "Id");
            return View();
        }

        // POST: Operations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Montant,DateOperation,CompteId")] Operation operation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(operation);
                Compte compte = _context.Comptes.FirstOrDefault(c => c.Id == operation.CompteId);
                compte.Solde += operation.Montant;
                if (_context.Comptes.Find(operation.CompteId) is ComptePayant comptePayant && comptePayant.Discriminator == nameof(ComptePayant))
                {
                    _context.Add
                        (
                            new Operation()
                            {
                                DateOperation = DateTime.Now,
                                CompteId = operation.CompteId,
                                Montant = -comptePayant.CoutOperation
                            }
                        );
                    compte.Solde -= comptePayant.CoutOperation;
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompteId"] = new SelectList(_context.Comptes, "Id", "Discriminator", operation.CompteId);
            return View(operation);
        }

        // GET: Operations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operation = await _context.Operations
                .Include(o => o.Compte)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (operation == null)
            {
                return NotFound();
            }

            return View(operation);
        }

        // POST: Operations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var operation = await _context.Operations.FindAsync(id);
            _context.Operations.Remove(operation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperationExists(int id)
        {
            return _context.Operations.Any(e => e.Id == id);
        }
    }
}
