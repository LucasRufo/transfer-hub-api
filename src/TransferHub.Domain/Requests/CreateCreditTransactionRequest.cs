namespace TransferHub.Domain.Requests;

public class CreateCreditTransactionRequest
{
    public Guid ParticipantId { get; set; }
    public required decimal Amount { get; set; }
}
