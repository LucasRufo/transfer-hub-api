using ErrorOr;
using Money.Domain.Entities;
using Money.Domain.Providers.Datetime;
using Money.Domain.Repositories;
using Money.Domain.Requests;

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

        participant.IncreaseBalance(createCreditTransactionRequest.Amount);

        var transaction = new Transaction()
        {
            Amount = createCreditTransactionRequest.Amount,
            Type = TransactionType.Credit,
            ParticipantId = participant.Id,
            CreatedAt = _dateTimeProvider.UtcNow,
            Participant = participant
        };

        await _transactionRepository.Save(transaction);

        return transaction;
    }
}
