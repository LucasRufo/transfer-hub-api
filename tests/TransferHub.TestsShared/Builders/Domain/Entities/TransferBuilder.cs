using Bogus;
using TransferHub.Domain.Entities;
using TransferHub.TestsShared.Builders.Domain.Requests;

namespace TransferHub.TestsShared.Builders.Domain.Entities;

public class TransferBuilder : Faker<Transfer>
{
    public TransferBuilder()
    {
        RuleFor(x => x.Id, faker => faker.Random.Guid());
        RuleFor(x => x.FromParticipantId, faker => faker.Random.Guid());
        RuleFor(x => x.ToParticipantId, faker => faker.Random.Guid());
        RuleFor(x => x.CreatedAt, faker => faker.Date.Recent().ToUniversalTime());
    }

    public TransferBuilder WithId(Guid id)
    {
        RuleFor(x => x.Id, faker => id);
        return this;
    }

    public TransferBuilder WithFromParticipantId(Guid id)
    {
        RuleFor(x => x.FromParticipantId, faker => id);
        return this;
    }

    public TransferBuilder WithToParticipantId(Guid id)
    {
        RuleFor(x => x.ToParticipantId, faker => id);
        return this;
    }

    public TransferBuilder WithCreatedAt(DateTime datetime)
    {
        RuleFor(x => x.CreatedAt, faker => datetime);
        return this;
    }
}
