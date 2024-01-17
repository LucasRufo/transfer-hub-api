using TransferHub.Domain.Entities;

namespace TransferHub.Domain.Repositories;

public interface IParticipantRepository
{
    Task Save(Participant participant);
    Task<Participant?> GetByCPF(string cpf);
    Task<Participant?> GetById(Guid id);
}
