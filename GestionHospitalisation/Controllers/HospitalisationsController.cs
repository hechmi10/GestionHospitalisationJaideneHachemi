using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionHospitalisation.Data;
using GestionHospitalisation.Models;

namespace GestionHospitalisation.Controllers
{
    public class HospitalisationsController : Controller
    {
        private readonly GestionHospitalisationContext _context;

        public HospitalisationsController(GestionHospitalisationContext context)
        {
            _context = context;
        }

        // GET: Hospitalisations
        public async Task<IActionResult> Index()
        {
            var hospitalisationContext = await _context.Hospitalisation.Include(h => h.Service).Include(h => h.Patient).ToListAsync();
            return View(hospitalisationContext);
        }

        // GET: Hospitalisations/Details/5
        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalisation = await _context.Hospitalisation
                .FirstOrDefaultAsync(m => m.DateEntree == id);
            if (hospitalisation == null)
            {
                return NotFound();
            }

            return View(hospitalisation);
        }

        // GET: Hospitalisations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hospitalisations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumServ,CodePat,DateEntree,DateSortie,Frais")] Hospitalisation hospitalisation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hospitalisation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hospitalisation);
        }

        // GET: Hospitalisations/Edit/5
        public async Task<IActionResult> Edit(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalisation = await _context.Hospitalisation.FindAsync(id);
            if (hospitalisation == null)
            {
                return NotFound();
            }
            return View(hospitalisation);
        }

        // POST: Hospitalisations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("NumServ,CodePat,DateEntree,DateSortie,Frais")] Hospitalisation hospitalisation)
        {
            if (id != hospitalisation.DateEntree)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hospitalisation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalisationExists(hospitalisation.DateEntree))
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
            return View(hospitalisation);
        }

        // GET: Hospitalisations/Delete/5
        public async Task<IActionResult> Delete(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalisation = await _context.Hospitalisation
                .FirstOrDefaultAsync(m => m.DateEntree == id);
            if (hospitalisation == null)
            {
                return NotFound();
            }

            return View(hospitalisation);
        }

        // POST: Hospitalisations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id)
        {
            var hospitalisation = await _context.Hospitalisation.FindAsync(id);
            if (hospitalisation != null)
            {
                _context.Hospitalisation.Remove(hospitalisation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalisationExists(DateTime id)
        {
            return _context.Hospitalisation.Any(e => e.DateEntree == id);
        }
    }
}
