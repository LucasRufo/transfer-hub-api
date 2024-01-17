using ErrorOr;
using Money.Domain.Entities;
using Money.Domain.Providers.Datetime;
using Money.Domain.Repositories;
using Money.Domain.Requests;
using Money.Domain.Responses;

namespace Money.Domain.Services;
public class TransactionService(
    IParticipantRepository participantRepository,
    ITransactionRepository transactionRepository,
    IDateTimeProvider dateTimeProvider
    )
{
    private readonly IParticipantRepository _participantRepository = participantRepository;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task<ErrorOr<Transaction>> Credit(CreateCreditTransactionRequest createCreditTransactionRequest)
    {
        var participant = await _participantRepository.GetById(createCreditTransactionRequest.ParticipantId);

        if (participant is null)
        {
            return Error.Failure("ParticipantNotFound", $"The participant with Id {createCreditTransactionRequest.ParticipantId} was not found.");
        }

        var dateUtcNow = _dateTimeProvider.UtcNow;

        participant.IncreaseBalance(createCreditTransactionRequest.Amount);
        participant.UpdatedAt = dateUtcNow;

        var transaction = new Transaction()
        {
            Amount = createCreditTransactionRequest.Amount,
            Type = TransactionType.Credit,
            ParticipantId = participant.Id,
            CreatedAt = dateUtcNow,
            Participant = participant
        };

        await _transactionRepository.Save(transaction);

        return transaction;
    }

    public async Task<ErrorOr<Transfer>> Transfer(TransferRequest transferRequest)
    {
        var fromParticipant = await _participantRepository.GetById(transferRequest.FromParticipantId);

        if (fromParticipant is null)
        {
            return Error.Failure("ParticipantNotFound", $"The participant with Id {transferRequest.FromParticipantId} was not found.");
        }

        var toParticipant = await _participantRepository.GetById(transferRequest.ToParticipantId);

        if (toParticipant is null)
        {
            return Error.Failure("ParticipantNotFound", $"The participant with Id {transferRequest.ToParticipantId} was not found.");
        }

        var dateUtcNow = _dateTimeProvider.UtcNow;

        if (!fromParticipant.TryDecreaseBalance(transferRequest.Amount))
        {
            return Error.Failure("InsufficientBalance", $"The participant doesn't have enough balance for the transfer.");
        }

        toParticipant.IncreaseBalance(transferRequest.Amount);

        fromParticipant.UpdatedAt = dateUtcNow;
        toParticipant.UpdatedAt = dateUtcNow;

        var transfer = new Transfer()
        {
            FromParticipantId = fromParticipant.Id,
            ToParticipantId = toParticipant.Id,
            CreatedAt = dateUtcNow
        };

        var payerTransaction = new Transaction()
        {
            Amount = -transferRequest.Amount,
            Type = TransactionType.Debit,
            ParticipantId = fromParticipant.Id,
            CreatedAt = dateUtcNow,
            Participant = fromParticipant,
            Transfer = transfer
        };

        var payeeTransaction = new Transaction()
        {
            Amount = transferRequest.Amount,
            Type = TransactionType.Credit,
            ParticipantId = toParticipant.Id,
            CreatedAt = dateUtcNow,
            Participant = toParticipant,
            Transfer = transfer
        };

        await _transactionRepository.Save([payerTransaction, payeeTransaction]);

        return transfer;
    }
}
