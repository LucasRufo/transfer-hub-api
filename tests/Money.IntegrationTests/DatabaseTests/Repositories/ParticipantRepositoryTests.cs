using Microsoft.Extensions.DependencyInjection;
using Money.Domain.Repositories;

namespace Money.IntegrationTests.DatabaseTests.Repositories;

public class ParticipantRepositoryTests : BaseIntegrationTests
{
    private IParticipantRepository _participantRepository;

    [SetUp]
    public void SetUp() => _participantRepository = ServiceProvider.GetRequiredService<IParticipantRepository>();

    [Test]
    public async Task ShouldSaveParticipant()
    {
        var participant = new ParticipantBuilder().Generate();

        await _participantRepository.Save(participant);

        var participantFromDatabase = ContextForAsserts.Participant.First();

        participant.Should().BeEquivalentTo(participantFromDatabase);
    }

    [Test]
    public async Task ShouldGetParticipantByCPF()
    {
        var participantFromDb = new ParticipantBuilder().GenerateInDatabase(Context);

        var participant = await _participantRepository.GetByCPF(participantFromDb.CPF);

        participantFromDb.Should().BeEquivalentTo(participant);
    }
}
