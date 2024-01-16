namespace Money.UnitTests.Domain.Entities;

public class ParticipantTests : BaseTests
{
    [Test]
    public void ShouldIncreaseBalance()
    {
        var participant = new ParticipantBuilder()
            .WithBalance(0M)
            .Generate();

        var amount = 50M;

        participant.IncreaseBalance(amount);

        participant.Balance.Should().Be(amount);
    }

    [Test]
    public void TryDecreaseBalanceShouldReturnFalseWhenInsufficientBalance()
    {
        var balance = 10M;

        var participant = new ParticipantBuilder()
            .WithBalance(balance)
            .Generate();

        var amount = 50M;

        var result = participant.TryDecreaseBalance(amount);

        result.Should().BeFalse();
        participant.Balance.Should().Be(balance);
    }

    [Test]
    public void TryDecreaseBalanceShouldReturnTrueWhenEnoughBalance()
    {
        var balance = 10M;

        var participant = new ParticipantBuilder()
            .WithBalance(balance)
            .Generate();

        var amount = 5M;

        var result = participant.TryDecreaseBalance(amount);

        result.Should().BeTrue();
        participant.Balance.Should().Be(amount);
    }
}
