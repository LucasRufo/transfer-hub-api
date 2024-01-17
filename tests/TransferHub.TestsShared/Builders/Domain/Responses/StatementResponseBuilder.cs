using Bogus;
using TransferHub.Domain.Responses;

namespace TransferHub.TestsShared.Builders.Domain.Responses;

public class StatementResponseBuilder : Faker<StatementResponse>
{
    public StatementResponseBuilder()
    {
        RuleFor(x => x.Page, faker => faker.Random.Int());
        RuleFor(x => x.PageSize, faker => faker.Random.Int());

        RuleFor(x => x.ParticipantId, faker => faker.Random.Guid());
        RuleFor(x => x.Name, faker => faker.Random.String());
        RuleFor(x => x.Balance, faker => Math.Round(faker.Random.Decimal(), 2).ToString());
    }
}
