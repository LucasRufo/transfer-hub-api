using FluentValidation.TestHelper;
using TransferHub.Domain.Validators;

namespace TransferHub.UnitTests.Domain.Validators;

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

    [Test]
    public void ShouldNotBeValidWhenAmountHasInvalidPrecision()
    {
        var amount = 55.999M;

        var createCreditTransactionRequest = new CreateCreditTransactionRequestBuilder()
            .WithAmount(amount)
            .Generate();

        var validate = _validator.TestValidate(createCreditTransactionRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorMessage($"'Amount' must not be more than 8 digits in total, with allowance for 2 decimals. 2 digits and 3 decimals were found.");
    }
}
