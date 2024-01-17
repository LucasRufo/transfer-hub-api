using TransferHub.Domain.Entities;

namespace TransferHub.Domain.Responses;

public class StatementResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public Guid ParticipantId { get; set; }
    public string Name { get; set; } = null!;
    public string Balance { get; set; } = null!;

    public List<StatementTransactionResponse> Transactions { get; set; } = [];

    public StatementResponse()
    {
    }

    public StatementResponse(int page, int pageSize, Guid participantId, string name, decimal balance, List<StatementTransactionResponse> transactions)
    {
        Page = page;
        PageSize = pageSize;
        ParticipantId = participantId;
        Name = name;
        Balance = balance.ToString("0.00");
        Transactions = transactions;
    }
}

public class StatementTransactionResponse
{
    public DateTime CreatedAt { get; set; }
    public string Type { get; set; } = null!;
    public string Amount { get; set; } = null!;

    public Guid? FromParticipantId { get; set; }
    public string? FromParticipantName { get; set; }

    public Guid? ToParticipantId { get; set; }
    public string? ToParticipantName { get; set; }

    public StatementTransactionResponse()
    {
    }

    public StatementTransactionResponse(DateTime createAt, TransactionType type, decimal amount, Guid? fromParticipantId, string? fromParticipantName, Guid? toParticipantId, string? toParticipantName)
    {
        CreatedAt = createAt;
        Type = type.ToString();
        Amount = amount.ToString("0.00");
        FromParticipantId = fromParticipantId;
        FromParticipantName = fromParticipantName;
        ToParticipantId = toParticipantId;
        ToParticipantName = toParticipantName;
    }

    public List<StatementTransactionResponse> Convert(List<Transaction> transactions)
    {
        var list = new List<StatementTransactionResponse>();

        foreach (var transaction in transactions)
        {
            list.Add(new StatementTransactionResponse(
                transaction.CreatedAt,
                transaction.Type,
                transaction.Amount,
                transaction.Transfer?.FromParticipantId,
                transaction.Transfer?.FromParticipant.Name,
                transaction.Transfer?.ToParticipantId,
                transaction.Transfer?.ToParticipant.Name));
        }

        return list;
    }
}
