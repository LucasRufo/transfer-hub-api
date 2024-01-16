namespace Money.Domain.Entities;

public class Transfer
{
    public Guid Id { get; set; }
    public required Guid FromParticipantId { get; set; }
    public required Guid ToParticipantId { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public Transfer()
    {
        Id = Guid.NewGuid();
    }
}
