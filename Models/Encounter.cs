using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareDashboard.Models
{
    // We add the interface after the class name
    public class Encounter : IAuditable
    {
        public int Id { get; set; }

        [Required]
        public DateTime EncounterDate { get; set; }

        [MaxLength(500)]
        public string? ChiefComplaint { get; set; } 

        // --- IAuditable Implementation ---
        // If we forget to add these, VS Code will throw a red error!
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // --- Foreign Keys ---
        public int ProviderId { get; set; }
        [ForeignKey("ProviderId")]
        public Provider Provider { get; set; } 

        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } 
    }
}