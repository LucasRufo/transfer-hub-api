namespace Money.Domain.Requests;

public class CreateCreditTransactionRequest
{
    public required Guid ParticipantId { get; set; }
    public required decimal Amount { get; set; }
}
