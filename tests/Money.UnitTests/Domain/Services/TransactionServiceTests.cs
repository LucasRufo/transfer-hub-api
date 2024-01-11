using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using Money.Domain.Entities;
using Money.Domain.Providers.Datetime;
using Money.Domain.Repositories;
using Money.Domain.Requests;
using Money.Domain.Services;
using Money.UnitTests.Configuration;

namespace Money.UnitTests.Domain.Services;

public class TransactionServiceTests : BaseTests
{
    [SetUp]
    public void SetUp()
    {
        AutoFake.WithInMemoryContext();
        AutoFake.Provide(A.Fake<IParticipantRepository>());
        AutoFake.Provide(A.Fake<ITransactionRepository>());
        AutoFake.Provide(A.Fake<IDateTimeProvider>());
    }

    [Test]
    public async Task ShouldCreateTransaction()
    {
        var participant = new ParticipantBuilder().Generate();
        var createCreditTransactionRequest = new CreateCreditTransactionRequestBuilder()
            .WithParticipantId(participant.Id)
            .Generate();

        var createdAtFake = Faker.Date.Past();

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetById(createCreditTransactionRequest.ParticipantId))
            .Returns(participant);

        A.CallTo(() => AutoFake
            .Resolve<IDateTimeProvider>().UtcNow)
            .Returns(createdAtFake);

        var transactionResult = await AutoFake.Resolve<TransactionService>()
            .Credit(createCreditTransactionRequest);

        var expectedTransaction = new TransactionBuilder()
            .WithId(transactionResult.Value.Id)
            .WithAmount(createCreditTransactionRequest.Amount)
            .WithParticipantId(createCreditTransactionRequest.ParticipantId)
            .WithType(TransactionType.Credit)
            .WithCreatedAt(createdAtFake)
            .Generate();

        transactionResult.IsError.Should().BeFalse();
        transactionResult.Value.Should().BeEquivalentTo(expectedTransaction);
    }

    [Test]
    public async Task ShouldReturnErrorWhenParticipantIsNotFound()
    {
        var createCreditTransactionRequest = new CreateCreditTransactionRequestBuilder()
            .Generate();

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetById(createCreditTransactionRequest.ParticipantId))
            .Returns<Participant?>(null);

        var participantResult = await AutoFake.Resolve<TransactionService>()
            .Credit(createCreditTransactionRequest);

        var error = Error.Failure("ParticipantNotFound", $"The participant with Id {createCreditTransactionRequest.ParticipantId} was not found.");

        participantResult.IsError.Should().BeTrue();
        participantResult.FirstError.Should().BeEquivalentTo(error);
    }
}
