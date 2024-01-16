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

    [Test]
    public async Task ShouldSaveMultipleTransactions()
    {
        var participant = new ParticipantBuilder().GenerateInDatabase(Context);

        var transactions = new TransactionBuilder()
            .WithParticipantId(participant.Id)
            .Generate(3);

        await _transactionRepository.Save(transactions);

        var transactionsFromDatabase = ContextForAsserts.Transaction.ToList();

        transactions.Should().BeEquivalentTo(transactionsFromDatabase);
    }
}
