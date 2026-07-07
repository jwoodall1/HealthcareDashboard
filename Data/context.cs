using Microsoft.EntityFrameworkCore;
using HealthcareDashboard.Models; // Remember to use your actual namespace

namespace HealthcareDashboard.Data
{
    public class HealthcareContext : DbContext
    {
        // The constructor passes configuration (like your connection string) to the base DbContext
        public HealthcareContext(DbContextOptions<HealthcareContext> options) : base(options)
        {
        }

        // These become your actual SQL Tables
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Encounter> Encounters { get; set; }

        // --- The Interface Magic ---
        // We override the save method to automatically update our IAuditable timestamps
        public override int SaveChanges()
        {
            // Find all entities being saved that implement IAuditable
            var entries = ChangeTracker.Entries<IAuditable>();

            foreach (var entry in entries)
            {
                // If it's a brand new record
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                // If it's an existing record being updated
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
    }
}