using Money.Domain.Entities;
using Money.Domain.Providers.Datetime;
using Money.Domain.Repositories;
using Money.Domain.Services;
using Money.UnitTests.Configuration;

namespace Money.UnitTests.Domain.Services;

public class ParticipantServiceTests : BaseTests
{
    [SetUp]
    public void SetUp()
    {
        AutoFake.WithInMemoryContext();
        AutoFake.Provide(A.Fake<IParticipantRepository>());
        AutoFake.Provide(A.Fake<IDateTimeProvider>());
    }

    [Test]
    public async Task ShouldCreateParticipant()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder().Generate();
        var createdAtFake = Faker.Date.Past();

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetByCPF(createParticipantRequest.CPF))
            .Returns<Participant?>(null);

        A.CallTo(() => AutoFake
            .Resolve<IDateTimeProvider>().UtcNow)
            .Returns(createdAtFake);

        var participantResult = await AutoFake.Resolve<ParticipantService>()
            .Create(createParticipantRequest);

        var expectedParticipant = new ParticipantBuilder()
            .WithId(participantResult.Value.Id)
            .WithName(createParticipantRequest.Name)
            .WithCPF(createParticipantRequest.CPF)
            .WithBalance(0M)
            .WithCreatedAt(createdAtFake)
            .WithUpdateAt(null)
            .Generate();

        participantResult.IsError.Should().BeFalse();
        participantResult.Value.Should().BeEquivalentTo(expectedParticipant);
    }

    [Test]
    public async Task ShouldReturnErrorWhenCPFAlreadyExists()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder().Generate();

        var existingParticipant = new ParticipantBuilder().Generate();

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetByCPF(createParticipantRequest.CPF))
            .Returns(existingParticipant);

        var participantResult = await AutoFake.Resolve<ParticipantService>()
            .Create(createParticipantRequest);

        var error = Error.Failure("ParticipantAlreadyExists", "A participant with this CPF already exists.");

        participantResult.IsError.Should().BeTrue();
        participantResult.FirstError.Should().BeEquivalentTo(error);
    }
}
