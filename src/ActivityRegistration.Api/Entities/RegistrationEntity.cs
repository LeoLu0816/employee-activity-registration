namespace ActivityRegistration.Api.Entities;

public class RegistrationEntity
{
    public int Id { get; set; }
    public required int ActivityId { get; set; }
    public ActivityEntity Activity { get; set; } = null!;
    public required string EmployeeId { get; set; }
    public required RegistrationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

