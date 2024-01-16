using Money.Domain.Entities;

namespace Money.Domain.Responses;

public class CreateParticipantResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string CPF { get; set; } = null!;
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }

    public CreateParticipantResponse()
    {
    }

    public CreateParticipantResponse(Participant participant)
    {
        Id = participant.Id;
        Name = participant.Name;
        CPF = participant.CPF;
        Balance = participant.Balance;
        CreatedAt = participant.CreatedAt;
    }
}
