namespace HealthcareDashboard.Models
{
    // This is a contract. Any class that implements this interface 
    // MUST have these two properties.
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}