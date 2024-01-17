using TransferHub.Domain.Entities;

namespace TransferHub.Domain.Responses;

public class CreateParticipantResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string CPF { get; set; } = null!;
    public string Balance { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public CreateParticipantResponse()
    {
    }

    public CreateParticipantResponse(Participant participant)
    {
        Id = participant.Id;
        Name = participant.Name;
        CPF = participant.CPF;
        Balance = participant.Balance.ToString("0.00");
        CreatedAt = participant.CreatedAt;
    }
}
