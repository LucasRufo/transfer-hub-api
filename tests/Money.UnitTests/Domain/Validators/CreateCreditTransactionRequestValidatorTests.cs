using FluentValidation.TestHelper;
using Money.Domain.Validators;

namespace Money.UnitTests.Domain.Validators;

public class CreateCreditTransactionRequestValidatorTests : BaseTests
{
    private readonly CreateCreditTransactionRequestValidator _validator = new();

    [Test]
    public void ShouldNotBeValidWhenParticipantIdIsEmpty()
    {
        var createCreditTransactionRequest = new CreateCreditTransactionRequestBuilder()
            .WithParticipantId(Guid.Empty)
            .Generate();

        var validate = _validator.TestValidate(createCreditTransactionRequest);

        validate.ShouldHaveValidationErrorFor(x => x.ParticipantId)
            .WithErrorMessage($"'Participant Id' must not be empty.");
    }

    [TestCase(0)]
    [TestCase(-10)]
    public void ShouldNotBeValidWhenAmountIsZeroOrLessThanZero(decimal amount)
    {
        var createCreditTransactionRequest = new CreateCreditTransactionRequestBuilder()
            .WithAmount(amount)
            .Generate();

        var validate = _validator.TestValidate(createCreditTransactionRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorMessage($"'Amount' must be greater than '0'.");
    }
}
