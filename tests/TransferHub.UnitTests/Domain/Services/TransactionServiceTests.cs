using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using TransferHub.Domain.Entities;
using TransferHub.Domain.Providers.Datetime;
using TransferHub.Domain.Repositories;
using TransferHub.Domain.Requests;
using TransferHub.Domain.Services;
using TransferHub.UnitTests.Configuration;

namespace TransferHub.UnitTests.Domain.Services;

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
    public async Task CreditShouldCreateTransaction()
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
            .WithParticipant(participant)
            .Generate();

        transactionResult.IsError.Should().BeFalse();
        transactionResult.Value.Should().BeEquivalentTo(expectedTransaction);
    }

    [Test]
    public async Task CreditShouldReturnErrorWhenParticipantIsNotFound()
    {
        var createCreditTransactionRequest = new CreateCreditTransactionRequestBuilder()
            .Generate();

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetById(createCreditTransactionRequest.ParticipantId))
            .Returns<Participant?>(null);

        var transactionResult = await AutoFake.Resolve<TransactionService>()
            .Credit(createCreditTransactionRequest);

        var error = Error.Failure("ParticipantNotFound", $"The participant with Id {createCreditTransactionRequest.ParticipantId} was not found.");

        transactionResult.IsError.Should().BeTrue();
        transactionResult.FirstError.Should().BeEquivalentTo(error);
    }

    [Test]
    public async Task TransferShouldCreateTransactionsAndTransfer()
    {
        var fromParticipant = new ParticipantBuilder()
            .WithBalance(10)
            .Generate();

        var toParticipant = new ParticipantBuilder().Generate();

        var transferRequest = new TransferRequestBuilder()
            .WithFromParticipantId(fromParticipant.Id)
            .WithToParticipantId(toParticipant.Id)
            .WithAmount(10)
            .Generate();

        var createdAtFake = Faker.Date.Past();

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetById(transferRequest.FromParticipantId))
            .Returns(fromParticipant);

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetById(transferRequest.ToParticipantId))
            .Returns(toParticipant);

        A.CallTo(() => AutoFake
            .Resolve<IDateTimeProvider>().UtcNow)
            .Returns(createdAtFake);

        var transferResult = await AutoFake.Resolve<TransactionService>()
            .Transfer(transferRequest);

        var expectedTransfer = new TransferBuilder()
            .WithId(transferResult.Value.Id)
            .WithFromParticipantId(transferRequest.FromParticipantId)
            .WithToParticipantId(transferRequest.ToParticipantId)
            .WithCreatedAt(createdAtFake)
            .Generate();

        transferResult.IsError.Should().BeFalse();
        transferResult.Value.Should().BeEquivalentTo(expectedTransfer);
    }

    [Test]
    public async Task TransferShouldReturnErrorWhenFromParticipantIsNotFound()
    {
        var transferRequest = new TransferRequestBuilder()
            .Generate();

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetById(transferRequest.FromParticipantId))
            .Returns<Participant?>(null);

        var transferResult = await AutoFake.Resolve<TransactionService>()
            .Transfer(transferRequest);

        var error = Error.Failure("ParticipantNotFound", $"The participant with Id {transferRequest.FromParticipantId} was not found.");

        transferResult.IsError.Should().BeTrue();
        transferResult.FirstError.Should().BeEquivalentTo(error);
    }

    [Test]
    public async Task TransferShouldReturnErrorWhenToParticipantIsNotFound()
    {
        var fromParticipant = new ParticipantBuilder().Generate();

        var transferRequest = new TransferRequestBuilder()
            .WithFromParticipantId(fromParticipant.Id)
            .Generate();

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetById(transferRequest.FromParticipantId))
            .Returns(fromParticipant);

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetById(transferRequest.ToParticipantId))
            .Returns<Participant?>(null);

        var transferResult = await AutoFake.Resolve<TransactionService>()
            .Transfer(transferRequest);

        var error = Error.Failure("ParticipantNotFound", $"The participant with Id {transferRequest.ToParticipantId} was not found.");

        transferResult.IsError.Should().BeTrue();
        transferResult.FirstError.Should().BeEquivalentTo(error);
    }

    [Test]
    public async Task TransferShouldReturnErrorWhenFromParticipantHasInsufficientBalance()
    {
        var fromParticipant = new ParticipantBuilder()
            .WithBalance(10)
            .Generate();

        var toParticipant = new ParticipantBuilder().Generate();

        var transferRequest = new TransferRequestBuilder()
            .WithFromParticipantId(fromParticipant.Id)
            .WithToParticipantId(toParticipant.Id)
            .WithAmount(50)
            .Generate();

        var createdAtFake = Faker.Date.Past();

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetById(transferRequest.FromParticipantId))
            .Returns(fromParticipant);

        A.CallTo(() => AutoFake
            .Resolve<IParticipantRepository>().GetById(transferRequest.ToParticipantId))
            .Returns(toParticipant);

        var transferResult = await AutoFake.Resolve<TransactionService>()
            .Transfer(transferRequest);

        var error = Error.Failure("InsufficientBalance", $"The participant doesn't have enough balance for the transfer.");

        transferResult.IsError.Should().BeTrue();
        transferResult.FirstError.Should().BeEquivalentTo(error);
    }
}
