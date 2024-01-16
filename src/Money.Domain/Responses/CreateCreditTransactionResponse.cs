using Money.Domain.Entities;

namespace Money.Domain.Responses;

public class CreateCreditTransactionResponse
{
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public Guid ParticipantId { get; set; } 
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; } 

    public CreateCreditTransactionResponse()
    {
    }

    public CreateCreditTransactionResponse(Transaction transaction)
    {
        Id = transaction.Id;
        Type = transaction.Type.ToString();
        ParticipantId = transaction.ParticipantId;
        Amount = transaction.Amount;
        CreatedAt = transaction.CreatedAt;
    }
}
