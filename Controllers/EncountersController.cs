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
        // This action returns the main view that will host our calendar.
        public async Task<IActionResult> Index()
        {
            var encounters = await _context.Encounters
                .Include(e => e.Patient)
                .Include(e => e.Provider)
                .OrderByDescending(e => e.EncounterDate)
                .ToListAsync();
            return View(encounters);
        }

        // GET: /Encounters/GetEncounters
        // This endpoint provides the encounter data in a JSON format that FullCalendar understands.
        [HttpGet]
        public async Task<IActionResult> GetEncounters()
        {
            var encounters = await _context.Encounters
                .Include(e => e.Patient)
                .Include(e => e.Provider)
                .Select(e => new {
                    id = e.Id,
                    title = $"{e.Patient!.FullName ?? "N/A"} - {e.Provider!.FullName ?? "N/A"}",
                    start = e.EncounterDate
                })
                .ToListAsync();
            return Json(encounters);
        }

        // This action is no longer needed for the calendar view, but can be kept for other purposes.
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName");
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "FullName");
            return View();
        }

        // POST: /Encounters/Create - This action is also not used by the new calendar interface.
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

        // GET: /Encounters/Details/5
        // Returns a partial view with the details of a specific encounter.
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

            return PartialView("_Details", encounter);
        }

        // POST: /Encounters/Edit/5
        // Handles the submission of the edit form.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Encounter encounter)
        {
            if (id != encounter.Id)
            {
                return NotFound();
            }
            
            // We must tell MVC to ignore the complex objects during validation for AJAX requests.
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
                return Json(new { success = true });
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "FullName", encounter.PatientId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "FullName", encounter.ProviderId);
            return PartialView("_Edit", encounter);
        }

        // POST: /Encounters/Delete/5
        // Confirms and executes the deletion.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var encounter = await _context.Encounters.FindAsync(id);
            if (encounter == null)
            {
                return Json(new { success = false, message = "Encounter not found." });
            }

            _context.Encounters.Remove(encounter);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // GET: /Encounters/Edit/5
        // Returns a partial view with the form to edit an encounter.
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
            
            // Check if the request is from AJAX (for the modal) or a direct request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Edit", encounter);
            }

            return View(encounter);
        }

        // GET: /Encounters/Delete/5
        // Returns a partial view with the delete confirmation.
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

            // Check if the request is from AJAX (for the modal) or a direct request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Delete", encounter);
            }

            return View(encounter);
        }

        private bool EncounterExists(int id)
        {
            return _context.Encounters.Any(e => e.Id == id);
        }
    }
}