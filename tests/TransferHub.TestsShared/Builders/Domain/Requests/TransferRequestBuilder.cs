using Bogus;
using TransferHub.Domain.Requests;

namespace TransferHub.TestsShared.Builders.Domain.Requests;

public class TransferRequestBuilder : Faker<TransferRequest>
{
    public TransferRequestBuilder()
    {
        RuleFor(x => x.FromParticipantId, faker => faker.Random.Guid());
        RuleFor(x => x.ToParticipantId, faker => faker.Random.Guid());
        RuleFor(x => x.Amount, faker => Math.Round(faker.Random.Decimal(), 2));
    }

    public TransferRequestBuilder WithFromParticipantId(Guid id)
    {
        RuleFor(x => x.FromParticipantId, faker => id);
        return this;
    }

    public TransferRequestBuilder WithToParticipantId(Guid id)
    {
        RuleFor(x => x.ToParticipantId, faker => id);
        return this;
    }

    public TransferRequestBuilder WithAmount(decimal amount)
    {
        RuleFor(x => x.Amount, faker => amount);
        return this;
    }
}
