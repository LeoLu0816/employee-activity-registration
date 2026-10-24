namespace ActivityRegistration.Api.Entities;

public class Activity
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Category { get; set; }
    public required int Capacity { get; set; }
}

