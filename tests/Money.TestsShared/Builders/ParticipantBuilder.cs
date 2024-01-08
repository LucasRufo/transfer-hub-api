using Bogus;
using Money.Domain.Entities;

namespace Money.TestsShared.Builders;

public class ParticipantBuilder : Faker<Participant>
{
    public ParticipantBuilder()
    {
        RuleFor(x => x.Id, faker => faker.Random.Guid());
        RuleFor(x => x.Name, faker => faker.Person.FullName);
        RuleFor(x => x.CPF, faker => faker.Random.ReplaceNumbers("###########"));
        RuleFor(x => x.CreatedAt, faker => faker.Date.Recent().ToUniversalTime());
        RuleFor(x => x.UpdatedAt, faker => faker.Date.Recent().ToUniversalTime());
    }
}
