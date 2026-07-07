using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareDashboard.Data; 
using HealthcareDashboard.Models;

namespace HealthcareDashboard.Controllers
{
    public class PatientsController : Controller
    {
        private readonly HealthcareContext _context;

        public PatientsController(HealthcareContext context)
        {
            _context = context;
        }

        // GET: /Patients
        public async Task<IActionResult> Index()
        {
            // Grab all patients from the database
            var allPatients = await _context.Patients.ToListAsync();
            return View(allPatients);
        }

        // GET: /Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }
    }
}