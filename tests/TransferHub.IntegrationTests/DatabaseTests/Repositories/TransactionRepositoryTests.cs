using Microsoft.Extensions.DependencyInjection;
using TransferHub.Domain.Repositories;
using TransferHub.Domain.Responses;
using TransferHub.TestsShared.Builders.Domain.Responses;

namespace TransferHub.IntegrationTests.DatabaseTests.Repositories;

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

    [Test]
    public async Task ShouldGetStatementTransactions()
    {
        int page = 1;
        int pageSize = 20;

        var participant = new ParticipantBuilder().GenerateInDatabase(Context);

        var transactions = new TransactionBuilder()
            .WithParticipantId(participant.Id)
            .GenerateInDatabase(Context, 5);

        var expectedStatementTransactions = new StatementTransactionResponse().Convert(transactions);

        var statementTransactions = await _transactionRepository.GetStatementTransactions(participant.Id, page, pageSize);

        statementTransactions.Should().BeEquivalentTo(expectedStatementTransactions);
    }
}
