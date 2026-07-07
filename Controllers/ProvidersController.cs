using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareDashboard.Data; 
using HealthcareDashboard.Models;

namespace HealthcareDashboard.Controllers
{
    public class ProvidersController : Controller
    {
        private readonly HealthcareContext _context;

        public ProvidersController(HealthcareContext context)
        {
            _context = context;
        }

        // GET: /Providers
        public async Task<IActionResult> Index()
        {
            var allProviders = await _context.Providers.ToListAsync();
            return View(allProviders);
        }

        // GET: /Providers/Create
        // This just shows the empty form to the user
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Providers/Create
        // This catches the data when they click submit
        [HttpPost]
        [ValidateAntiForgeryToken] // A crucial security feature that prevents hackers from submitting fake forms
        public async Task<IActionResult> Create(Provider provider)
        {
            // ModelState.IsValid checks all the [Required] and [MaxLength] rules we set in the Model!
            if (ModelState.IsValid)
            {
                // 1. Stage the new provider to be added
                _context.Add(provider);
                
                // 2. Actually push it to the Docker SQL database
                await _context.SaveChangesAsync();
                
                // 3. Send the user back to the directory to see their newly added doctor
                return RedirectToAction(nameof(Index));
            }
            
            // If the data was invalid (e.g., they left a required field blank), 
            // return the form so they can fix their mistakes.
            return View(provider);
        }

        // GET: /Providers/Edit/5
        // This finds the specific doctor and loads their current info into the form
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var provider = await _context.Providers.FindAsync(id);
            if (provider == null) return NotFound();

            return View(provider);
        }

        // POST: /Providers/Edit/5
        // This catches the updated form and saves the changes to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Provider provider)
        {
            // Security check: Make sure hackers didn't swap the ID in the background
            if (id != provider.Id) return NotFound();

            if (ModelState.IsValid)
            {
                // Tell Entity Framework this record has been modified
                _context.Update(provider);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(provider);
        }

        // GET: /Providers/Delete/5
        // Loads the "Are you sure you want to delete this?" page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var provider = await _context.Providers.FindAsync(id);
            if (provider == null) return NotFound();

            return View(provider);
        }

        // POST: /Providers/Delete/5
        // Actually deletes the record from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var provider = await _context.Providers.FindAsync(id);
            if (provider != null)
            {
                _context.Providers.Remove(provider);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}