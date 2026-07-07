using System.ComponentModel.DataAnnotations;

namespace HealthcareDashboard.Models
{
    // "Provider inherits from Person"
    public class Provider : Person 
    {
        [Required]
        [MaxLength(10)]
        public string NpiNumber { get; set; } = null!;


        [Required]
        [MaxLength(100)]
        public string Specialty { get; set; } = null!;


        public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();
    }
}