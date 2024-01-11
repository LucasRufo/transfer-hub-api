using Microsoft.Extensions.DependencyInjection;
using Money.Domain.Repositories;

namespace Money.IntegrationTests.DatabaseTests.Repositories;

public class TransactionRepositoryTests : BaseIntegrationTests
{
    private ITransactionRepository _transactionRepository;

    [SetUp]
    public void SetUp() => _transactionRepository = ServiceProvider.GetRequiredService<ITransactionRepository>();

    [Test]
    public async Task ShouldSaveTransaction()
    {
        var participant = new ParticipantBuilder().GenerateInDatabase(Context);

        var transaction = new TransactionBuilder()
            .WithParticipantId(participant.Id)
            .Generate();

        await _transactionRepository.Save(transaction);

        var transactionFromDatabase = ContextForAsserts.Transaction.First();

        transaction.Should().BeEquivalentTo(transactionFromDatabase);
    }
}
