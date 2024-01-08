using Money.Domain.Entities;

namespace Money.Domain.Repositories;

public interface IParticipantRepository
{
    Task Save(Participant participant);
}
