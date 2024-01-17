using Bogus;
using Money.Domain.Responses;

namespace Money.TestsShared.Builders.Domain.Responses;

public class StatementTransactionResponseBuilder : Faker<StatementTransactionResponse>
{
    public StatementTransactionResponseBuilder()
    {
        RuleFor(x => x.Type, faker => faker.Random.String());
        RuleFor(x => x.Amount, faker => Math.Round(faker.Random.Decimal(), 2).ToString());
        RuleFor(x => x.CreatedAt, faker => faker.Date.Recent().ToUniversalTime());

        RuleFor(x => x.FromParticipantId, faker => faker.Random.Guid());
        RuleFor(x => x.FromParticipantName, faker => faker.Random.String());

        RuleFor(x => x.ToParticipantId, faker => faker.Random.Guid());
        RuleFor(x => x.ToParticipantName, faker => faker.Random.String());
    }
}
