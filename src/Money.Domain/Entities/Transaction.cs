namespace Money.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public TransactionType Type { get; set; }
    public Guid ParticipantId { get; set; }
    public required decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum TransactionType
{
    Credit = 0,
    Debit = 1
}