using Bogus;
using Money.Domain.Responses;

namespace Money.TestsShared.Builders.Domain.Responses;

public class CreateCreditTransactionResponseBuilder : Faker<CreateCreditTransactionResponse>
{
    public CreateCreditTransactionResponseBuilder()
    {
        RuleFor(x => x.Id, faker => faker.Random.Guid());
        RuleFor(x => x.Type, faker => faker.Random.String());
        RuleFor(x => x.Amount, faker => Math.Round(faker.Random.Decimal(), 2));
        RuleFor(x => x.ParticipantId, faker => faker.Random.Guid());
        RuleFor(x => x.CreatedAt, faker => faker.Date.Recent().ToUniversalTime());
    }

    public CreateCreditTransactionResponseBuilder WithId(Guid id)
    {
        RuleFor(x => x.Id, faker => id);
        return this;
    }

    public CreateCreditTransactionResponseBuilder WithType(string type)
    {
        RuleFor(x => x.Type, faker => type);
        return this;
    }

    public CreateCreditTransactionResponseBuilder WithAmount(decimal amount)
    {
        RuleFor(x => x.Amount, faker => amount);
        return this;
    }

    public CreateCreditTransactionResponseBuilder WithParticipantId(Guid participantId)
    {
        RuleFor(x => x.ParticipantId, faker => participantId);
        return this;
    }

    public CreateCreditTransactionResponseBuilder WithCreatedAt(DateTime datetime)
    {
        RuleFor(x => x.CreatedAt, faker => datetime);
        return this;
    }
}
