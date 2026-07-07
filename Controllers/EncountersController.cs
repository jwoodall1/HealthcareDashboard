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
    }
}