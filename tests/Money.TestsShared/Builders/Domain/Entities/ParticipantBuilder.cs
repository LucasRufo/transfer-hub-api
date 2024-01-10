using Bogus;
using Money.Domain.Entities;

namespace Money.TestsShared.Builders.Domain.Entities;

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

    public ParticipantBuilder WithId(Guid id)
    {
        RuleFor(x => x.Id, faker => id);
        return this;
    }

    public ParticipantBuilder WithName(string name)
    {
        RuleFor(x => x.Name, faker => name);
        return this;
    }

    public ParticipantBuilder WithCPF(string cpf)
    {
        RuleFor(x => x.CPF, faker => cpf);
        return this;
    }

    public ParticipantBuilder WithCreatedAt(DateTime datetime)
    {
        RuleFor(x => x.CreatedAt, faker => datetime);
        return this;
    }

    public ParticipantBuilder WithUpdateAt(DateTime? datetime)
    {
        RuleFor(x => x.UpdatedAt, faker => datetime);
        return this;
    }
}
