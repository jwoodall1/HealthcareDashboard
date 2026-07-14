using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareDashboard.Models
{
    /// <summary>
    /// Represents a single patient encounter or visit.
    /// It includes details about the visit, the patient, and the provider.
    /// </summary>
    public class Encounter : IAuditable
    {
        // Primary key for the Encounter table
        public int Id { get; set; }

        // The date and time of the encounter. This is a required field.
        [Required]
        public DateTime EncounterDate { get; set; }

        // The patient's primary complaint or reason for the visit.
        // Limited to 500 characters.
        [MaxLength(500)]
        public string? ChiefComplaint { get; set; } 

        // --- IAuditable Implementation ---
        // These properties are from the IAuditable interface and will be
        // automatically populated by the DbContext when an entity is created or updated.
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // --- Foreign Keys ---
        // Foreign key for the Provider
        public int ProviderId { get; set; }
        // Navigation property to the associated Provider. EF Core uses this to load the provider details.
        [ForeignKey("ProviderId")]
        public Provider? Provider { get; set; }

        // Foreign key for the Patient
        public int PatientId { get; set; }
        // Navigation property to the associated Patient.
        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }
    
    }
}