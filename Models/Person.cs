using System.ComponentModel.DataAnnotations;

namespace HealthcareDashboard.Models
{
    // The 'abstract' keyword means you can never create just a "Person". 
    // It exists solely to be inherited by other classes.
    public abstract class Person
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;


        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
        
        // A computed property that doesn't map to the database, 
        // just makes displaying the name easier in your Views!
        public string FullName => $"{FirstName} {LastName}"; 
    }
}