using ErrorOr;
using Money.Domain.Entities;
using Money.Domain.Providers.Datetime;
using Money.Domain.Repositories;
using Money.Domain.Requests;

namespace Money.Domain.Services;

public class ParticipantService(IParticipantRepository participantRepository, IDateTimeProvider dateTimeProvider)
{
    private readonly IParticipantRepository _participantRepository = participantRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task<ErrorOr<Participant>> Create(CreateParticipantRequest createParticipantRequest)
    {
        var existingParticipant = await _participantRepository.GetByCPF(createParticipantRequest.CPF);

        if(existingParticipant is not null)
        {
            //I could create an ErrorCatalog, but it does not look necessary right now
            return Error.Failure("ParticipantAlreadyExists", "A participant with this CPF already exists.");
        }

        var participant = new Participant() 
        { 
            Name = createParticipantRequest.Name,
            CPF = createParticipantRequest.CPF,
            CreatedAt = _dateTimeProvider.UtcNow
        };

        await _participantRepository.Save(participant);

        return participant;
    }
}
