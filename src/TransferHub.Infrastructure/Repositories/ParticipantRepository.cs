using Microsoft.EntityFrameworkCore;
using TransferHub.Domain.Entities;
using TransferHub.Domain.Repositories;

namespace TransferHub.Infrastructure.Repositories;

public class ParticipantRepository(TransferHubContext context) : IParticipantRepository
{
    private readonly TransferHubContext _context = context;

    public async Task Save(Participant participant)
    {
        await _context.Participant.AddAsync(participant);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<Participant?> GetByCPF(string cpf)
        => await _context.Participant.FirstOrDefaultAsync(x => x.CPF == cpf);

    public virtual async Task<Participant?> GetById(Guid id)
        => await _context.Participant.FirstOrDefaultAsync(x => x.Id == id);
}
