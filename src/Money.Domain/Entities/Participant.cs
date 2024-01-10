namespace Money.Domain.Entities;

public class Participant
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string CPF { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Participant()
    {
        Id = Guid.NewGuid();
    }
}
