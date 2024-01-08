namespace Money.Domain.Entities;

public class Participant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string CPF { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
