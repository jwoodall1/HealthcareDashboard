using System.ComponentModel.DataAnnotations;

namespace HealthcareDashboard.Models
{
    // "Patient inherits from Person"
    public class Patient : Person 
    {
        [Required]
        public DateTime DateOfBirth { get; set; }

        public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();
    }
}