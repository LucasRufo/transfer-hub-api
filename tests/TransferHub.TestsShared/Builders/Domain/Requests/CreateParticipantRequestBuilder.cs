using Bogus;
using Bogus.Extensions.Brazil;
using TransferHub.Domain.Requests;

namespace TransferHub.TestsShared.Builders.Domain.Requests;

public class CreateParticipantRequestBuilder : Faker<CreateParticipantRequest>
{
    public CreateParticipantRequestBuilder()
    {
        RuleFor(x => x.Name, faker => faker.Name.FullName());
        RuleFor(x => x.CPF, faker => faker.Person.Cpf(false));
    }

    public CreateParticipantRequestBuilder WithName(string name)
    {
        RuleFor(x => x.Name, faker => name);
        return this;
    }

    public CreateParticipantRequestBuilder WithCPF(string cpf)
    {
        RuleFor(x => x.CPF, faker => cpf);
        return this;
    }
}

