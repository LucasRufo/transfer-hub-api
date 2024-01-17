namespace TransferHub.Domain.Requests;

public class TransferRequest
{
    public Guid FromParticipantId { get; set; }
    public Guid ToParticipantId { get; set; }
    public required decimal Amount { get; set; }
}
