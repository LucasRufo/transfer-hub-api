using Money.Domain.Entities;

namespace Money.Domain.Repositories;

public interface IParticipantRepository
{
    Task Save(Participant participant);
    Task<Participant?> GetByCPF(string cpf);
    Task<Participant?> GetById(Guid id);
}
