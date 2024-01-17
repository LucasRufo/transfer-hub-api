using ErrorOr;
using TransferHub.Domain.Entities;
using TransferHub.Domain.Providers.Datetime;
using TransferHub.Domain.Repositories;
using TransferHub.Domain.Requests;
using TransferHub.Domain.Responses;

namespace TransferHub.Domain.Services;

public class ParticipantService(
    IParticipantRepository participantRepository, 
    ITransactionRepository transactionRepository,
    IDateTimeProvider dateTimeProvider)
{
    private readonly IParticipantRepository _participantRepository = participantRepository;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task<ErrorOr<Participant>> Create(CreateParticipantRequest createParticipantRequest)
    {
        var existingParticipant = await _participantRepository.GetByCPF(createParticipantRequest.CPF);

        if(existingParticipant is not null)
        {
            //I could create an ErrorCatalog, but it does not look necessary right now
            return Error.Failure("ParticipantAlreadyExists", "A participant with this CPF already exists.");
        }

        var participant = new Participant() 
        { 
            Name = createParticipantRequest.Name,
            CPF = createParticipantRequest.CPF,
            CreatedAt = _dateTimeProvider.UtcNow
        };

        await _participantRepository.Save(participant);

        return participant;
    }

    public async Task<ErrorOr<StatementResponse>> Statement(Guid participantId, int page, int pageSize)
    {
        var participant = await _participantRepository.GetById(participantId);

        if (participant is null)
        {
            return Error.Failure("ParticipantNotFound", $"The participant with Id {participantId} was not found.");
        }

        var statementTransactions = await _transactionRepository.GetStatementTransactions(participantId, page, pageSize);

        var statement = new StatementResponse(
            page, 
            pageSize, 
            participantId, 
            participant.Name, 
            participant.Balance, 
            statementTransactions
            );

        return statement;
    }
}
