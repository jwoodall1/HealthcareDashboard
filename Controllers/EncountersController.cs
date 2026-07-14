using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareDashboard.Data;
using HealthcareDashboard.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HealthcareDashboard.Controllers
{
    public class EncountersController : Controller
    {
        private readonly HealthcareContext _context;

        public EncountersController(HealthcareContext context)
        {
            _context = context;
        }

        // GET: /Encounters
        public async Task<IActionResult> Index()
        {
            // The Magic: We tell EF Core to "Include" the related models
            // so they aren't null when we try to display them!
            var encounters = await _context.Encounters
                .Include(e => e.Patient)
                .Include(e => e.Provider)
                .ToListAsync();

            return View(encounters);
        }

        public IActionResult Create()
        {
            // This creates the data for our dropdown menus!
            // "Id" is the value saved to the database. "FullName" is what the user actually sees.
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName");
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "FullName");
            return View();
        }

        // POST: /Encounters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Encounter encounter)
        {
            // MVC tries to validate the ENTIRE Encounter model, including the attached Provider and Patient.
            // But our form only sends the IDs (ProviderId, PatientId). 
            // We must tell MVC to ignore the complex objects during validation, or it will always fail!
            ModelState.Remove("Patient");
            ModelState.Remove("Provider");

            if (ModelState.IsValid)
            {
                _context.Add(encounter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            // If the form fails, we have to reload the dropdown lists before returning the view!
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", encounter.PatientId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "FullName", encounter.ProviderId);
            return View(encounter);
        }

        // GET: Encounters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encounter = await _context.Encounters
                .Include(e => e.Patient)
                .Include(e => e.Provider)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (encounter == null)
            {
                return NotFound();
            }

            return View(encounter);
        }

        // GET: Encounters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encounter = await _context.Encounters.FindAsync(id);
            if (encounter == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", encounter.PatientId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "FullName", encounter.ProviderId);
            return View(encounter);
        }

        // POST: Encounters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Encounter encounter)
        {
            if (id != encounter.Id)
            {
                return NotFound();
            }
            
            ModelState.Remove("Patient");
            ModelState.Remove("Provider");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encounter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EncounterExists(encounter.Id))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", encounter.PatientId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "FullName", encounter.ProviderId);
            return View(encounter);
        }

        // GET: Encounters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encounter = await _context.Encounters
                .Include(e => e.Patient)
                .Include(e => e.Provider)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (encounter == null)
            {
                return NotFound();
            }

            return View(encounter);
        }

        // POST: Encounters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var encounter = await _context.Encounters.FindAsync(id);
            if (encounter != null)
            {
                _context.Encounters.Remove(encounter);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool EncounterExists(int id)
        {
            return _context.Encounters.Any(e => e.Id == id);
        }
    }
}