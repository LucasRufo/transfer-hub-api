using Money.Domain.Entities;

namespace Money.Domain.Responses;

public class TransferResponse
{
    public Guid Id { get; set; }
    public Guid FromParticipantId { get; set; }
    public Guid ToParticipantId { get; set; }
    public DateTime CreatedAt { get; set; }

    public TransferResponse()
    {
    }

    public TransferResponse(Transfer transfer)
    {
        Id = transfer.Id;
        FromParticipantId = transfer.FromParticipantId;
        ToParticipantId = transfer.ToParticipantId;
        CreatedAt = transfer.CreatedAt;
    }
}
