using Money.Domain.Entities;
using Money.Domain.Responses;

namespace Money.Domain.Repositories;

public interface ITransactionRepository
{
    Task Save(Transaction transaction);
    Task Save(List<Transaction> transaction);
    Task<List<StatementTransactionResponse>> GetStatementTransactions(Guid participantId, int page, int pageSize);
}
