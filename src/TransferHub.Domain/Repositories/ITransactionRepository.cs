using TransferHub.Domain.Entities;
using TransferHub.Domain.Responses;

namespace TransferHub.Domain.Repositories;

public interface ITransactionRepository
{
    Task Save(Transaction transaction);
    Task Save(List<Transaction> transaction);
    Task<List<StatementTransactionResponse>> GetStatementTransactions(Guid participantId, int page, int pageSize);
}
