using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareDashboard.Data; 
using HealthcareDashboard.Models;

namespace HealthcareDashboard.Controllers
{
    public class PatientsController : Controller
    {
        // My database context, injected via dependency injection.
        private readonly HealthcareContext _context;

        // Constructor to initialize the controller with the database context.
        public PatientsController(HealthcareContext context)
        {
            _context = context;
        }

        // GET: /Patients
        // This is the main page for patients, showing a list of all of them.
        public async Task<IActionResult> Index()
        {
            // Grab all patients from the database and pass them to the view.
            var allPatients = await _context.Patients.ToListAsync();
            return View(allPatients);
        }

        // GET: /Patients/Create
        // Shows the form to create a new patient.
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Patients/Create
        // Handles the form submission for creating a new patient.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            // Check if the submitted data is valid based on my model's validation attributes.
            if (ModelState.IsValid)
            {
                // If it's valid, add the new patient to the context...
                _context.Add(patient);
                // ...save the changes to the database...
                await _context.SaveChangesAsync();
                // ...and redirect back to the main patient list.
                return RedirectToAction(nameof(Index));
            }
            // If the model state is not valid, return to the Create view with the entered data to show validation errors.
            return View(patient);
        }

        // GET: Patients/Details/5
        // Shows the details for a specific patient.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the patient by their ID.
            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Edit/5
        // Shows the form to edit an existing patient's details.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the patient by their ID to pre-populate the edit form.
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // Handles the form submission for editing a patient.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient patient)
        {
            // Make sure the ID from the URL matches the ID from the form data.
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Mark the patient entity as modified and save the changes.
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // This handles the rare case where the patient was deleted by another user
                    // between the time the edit form was loaded and submitted.
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
            // If the model state is invalid, return to the Edit view to show errors.
            return View(patient);
        }

        // GET: Patients/Delete/5
        // Shows the confirmation page for deleting a patient.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the patient to show their details on the confirmation page.
            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        // Actually deletes the patient after confirmation.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the patient to be deleted.
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                // If found, remove them from the context and save changes.
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
            // Redirect back to the patient list.
            return RedirectToAction(nameof(Index));
        }

        // A private helper method to check if a patient exists in the database.
        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}