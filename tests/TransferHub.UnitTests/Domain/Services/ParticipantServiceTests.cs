using TransferHub.Domain.Entities;
using TransferHub.Domain.Providers.Datetime;
using TransferHub.Domain.Repositories;
using TransferHub.Domain.Responses;
using TransferHub.Domain.Services;
using TransferHub.TestsShared.Builders.Domain.Responses;
using TransferHub.UnitTests.Configuration;

namespace TransferHub.UnitTests.Domain.Services;

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
    public async Task CreateShouldCreateParticipant()
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
    public async Task CreateShouldReturnErrorWhenCPFAlreadyExists()
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

    [Test]
    public async Task StatementShouldGetStatement()
    {
        var participant = new ParticipantBuilder().Generate();
        var page = 1;
        var pageSize = 20;

        var createdAtFake = Faker.Date.Past();

        var statementTransaction = new StatementTransactionResponseBuilder().Generate(3);

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetById(participant.Id))
            .Returns(participant);

        A.CallTo(() => AutoFake
            .Resolve<ITransactionRepository>().GetStatementTransactions(participant.Id, page, pageSize))
            .Returns(statementTransaction);

        A.CallTo(() => AutoFake
            .Resolve<IDateTimeProvider>().UtcNow)
            .Returns(createdAtFake);

        var statementResult = await AutoFake.Resolve<ParticipantService>()
            .Statement(participant.Id, page, pageSize);

        var expectedStatement = new StatementResponse(page, pageSize, participant.Id, participant.Name, participant.Balance, statementTransaction);

        statementResult.IsError.Should().BeFalse();
        statementResult.Value.Should().BeEquivalentTo(expectedStatement);
    }

    [Test]
    public async Task StatementShouldReturnErrorWhenParticipantIsNotFound()
    {
        var participant = new ParticipantBuilder().Generate();
        var page = 1;
        var pageSize = 20;

        var createdAtFake = Faker.Date.Past();

        var statementTransaction = new StatementTransactionResponseBuilder().Generate(3);

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetById(participant.Id))
            .Returns<Participant?>(null);

        var statementResult = await AutoFake.Resolve<ParticipantService>()
            .Statement(participant.Id, page, pageSize);

        var error = Error.Failure("ParticipantNotFound", $"The participant with Id {participant.Id} was not found.");

        statementResult.IsError.Should().BeTrue();
        statementResult.FirstError.Should().BeEquivalentTo(error);
    }
}
