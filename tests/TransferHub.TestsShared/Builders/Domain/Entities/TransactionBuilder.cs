using Bogus;
using TransferHub.Domain.Entities;

namespace TransferHub.TestsShared.Builders.Domain.Entities;

public class TransactionBuilder : Faker<Transaction>
{
    public TransactionBuilder()
    {
        RuleFor(x => x.Id, faker => faker.Random.Guid());
        RuleFor(x => x.Type, faker => faker.PickRandom<TransactionType>());
        RuleFor(x => x.Amount, faker => Math.Round(faker.Random.Decimal(), 2));
        RuleFor(x => x.ParticipantId, faker => faker.Random.Guid());
        RuleFor(x => x.CreatedAt, faker => faker.Date.Recent().ToUniversalTime());
    }

    public TransactionBuilder WithId(Guid id)
    {
        RuleFor(x => x.Id, faker => id);
        return this;
    }

    public TransactionBuilder WithType(TransactionType type)
    {
        RuleFor(x => x.Type, faker => type);
        return this;
    }

    public TransactionBuilder WithAmount(decimal amount)
    {
        RuleFor(x => x.Amount, faker => amount);
        return this;
    }

    public TransactionBuilder WithParticipantId(Guid participantId)
    {
        RuleFor(x => x.ParticipantId, faker => participantId);
        return this;
    }

    public TransactionBuilder WithCreatedAt(DateTime datetime)
    {
        RuleFor(x => x.CreatedAt, faker => datetime);
        return this;
    }

    public TransactionBuilder WithParticipant(Participant participant)
    {
        RuleFor(x => x.Participant, faker => participant);
        return this;
    }
}
