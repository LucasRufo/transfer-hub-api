using Microsoft.EntityFrameworkCore;
using Money.Domain.Entities;
using Money.Domain.Repositories;

namespace Money.Infrastructure.Repositories;

public class ParticipantRepository(MoneyContext context) : IParticipantRepository
{
    private readonly MoneyContext _context = context;

    public async Task Save(Participant participant)
    {
        await _context.Participant.AddAsync(participant);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<Participant?> GetByCPF(string cpf)
        => await _context.Participant.FirstOrDefaultAsync(x => x.CPF == cpf);
}
