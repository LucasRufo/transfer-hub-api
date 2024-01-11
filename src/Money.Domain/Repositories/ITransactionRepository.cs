using Money.Domain.Entities;

namespace Money.Domain.Repositories;

public interface ITransactionRepository
{
    Task Save(Transaction transaction);
}
