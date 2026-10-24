namespace ActivityRegistration.Api.Entities;

public class ActivityEntity
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Category { get; set; }
    public required int Capacity { get; set; }

    public List<RegistrationEntity> Registrations { get; set; } = [];
}

