using Bogus;
using Money.Domain.Requests;

namespace Money.TestsShared.Builders.Domain.Requests;

public class CreateCreditTransactionRequestBuilder : Faker<CreateCreditTransactionRequest>
{
    public CreateCreditTransactionRequestBuilder()
    {
        RuleFor(x => x.ParticipantId, faker => faker.Random.Guid());
        RuleFor(x => x.Amount, faker => faker.Random.Decimal());
    }

    public CreateCreditTransactionRequestBuilder WithParticipantId(Guid id)
    {
        RuleFor(x => x.ParticipantId, faker => id);
        return this;
    }

    public CreateCreditTransactionRequestBuilder WithAmount(decimal amount)
    {
        RuleFor(x => x.Amount, faker => amount);
        return this;
    }
}
