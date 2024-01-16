namespace Money.Domain.Entities;

public class Transfer
{
    public Guid Id { get; set; }
    public Guid FromParticipantId { get; set; }
    public Guid ToParticipantId { get; set; }
    public decimal Amount { get; set; }

    public Transaction Transaction { get; set; } = null!;
}
