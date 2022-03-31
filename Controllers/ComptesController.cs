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
    public class ComptesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Dictionary<string, string> typesComptes = new Dictionary<string, string>()
        {
            {nameof(Compte), "Compte" },
            {nameof(CompteEpargne), "Compte Epargne" },
            {nameof(ComptePayant), "Compte Payant" }
        };

        public ComptesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comptes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comptes.Include(c => c.Client);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comptes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compte = await _context.Comptes
                .Include(c => c.Client)
                .Include(c => c.Operations)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compte == null)
            {
                return NotFound();
            }

            return View(compte);
        }

        // GET: Comptes/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email");
            ViewData["TypesComptes"] = new SelectList(typesComptes, "Key", "Value");
            return View();
        }

        // POST: Comptes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Solde,ClientId,Discriminator,Taux,CoutOperation")] ToutCompteDTO compte)
        {
            if (ModelState.IsValid)
            {
                if (compte.Discriminator == "CompteEpargne" && compte.Taux != null)
                {
                    CompteEpargne compteEpargne = new CompteEpargne()
                    {
                        Solde = compte.Solde,
                        ClientId = compte.ClientId,
                        Taux = (decimal)compte.Taux,
                    };
                    _context.Add(compteEpargne);
                }
                else if (compte.Discriminator == "ComptePayant" && compte.CoutOperation != null)
                {
                    ComptePayant comptePayant = new ComptePayant()
                    {
                        Solde = compte.Solde,
                        ClientId = compte.ClientId,
                        CoutOperation = (decimal)compte.CoutOperation,
                    };
                    _context.Add(comptePayant);
                }
                else if (compte.Discriminator == "Compte")
                {
                    _context.Add(compte);
                }
                else
                    return BadRequest("Error during creation of an account");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", compte.ClientId);
            ViewData["TypesComptes"] = new SelectList(typesComptes, "Key", "Value");
            return View(compte);
        }

        // GET: Comptes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compte = await _context.Comptes.FindAsync(id);
            if (compte == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", compte.ClientId);
            return View(compte);
        }

        // POST: Comptes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Solde,ClientId")] Compte compte)
        {
            if (id != compte.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compte);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompteExists(compte.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", compte.ClientId);
            return View(compte);
        }

        // GET: Comptes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compte = await _context.Comptes
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compte == null)
            {
                return NotFound();
            }

            return View(compte);
        }

        // POST: Comptes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compte = await _context.Comptes.FindAsync(id);
            _context.Comptes.Remove(compte);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompteExists(int id)
        {
            return _context.Comptes.Any(e => e.Id == id);
        }
    }
}
