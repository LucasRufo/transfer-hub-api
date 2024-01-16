using FluentValidation.TestHelper;
using Money.Domain.Validators;

namespace Money.UnitTests.Domain.Validators;

public class TransferRequestValidatorTests
{
    private readonly TransferRequestValidator _validator = new();

    [Test]
    public void ShouldNotBeValidWhenFromParticipantIdIsEmpty()
    {
        var transferRequest = new TransferRequestBuilder()
            .WithFromParticipantId(Guid.Empty)
            .Generate();

        var validate = _validator.TestValidate(transferRequest);

        validate.ShouldHaveValidationErrorFor(x => x.FromParticipantId)
            .WithErrorMessage($"'From Participant Id' must not be empty.");
    }

    [Test]
    public void ShouldNotBeValidWhenToParticipantIdIsEmpty()
    {
        var transferRequest = new TransferRequestBuilder()
            .WithToParticipantId(Guid.Empty)
            .Generate();

        var validate = _validator.TestValidate(transferRequest);

        validate.ShouldHaveValidationErrorFor(x => x.ToParticipantId)
            .WithErrorMessage($"'To Participant Id' must not be empty.");
    }

    [TestCase(0)]
    [TestCase(-10)]
    public void ShouldNotBeValidWhenAmountIsZeroOrLessThanZero(decimal amount)
    {
        var transferRequest = new TransferRequestBuilder()
            .WithAmount(amount)
            .Generate();

        var validate = _validator.TestValidate(transferRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorMessage($"'Amount' must be greater than '0'.");
    }

    [Test]
    public void ShouldNotBeValidWhenAmountHasInvalidPrecision()
    {
        var amount = 55.999M;

        var transferRequest = new TransferRequestBuilder()
            .WithAmount(amount)
            .Generate();

        var validate = _validator.TestValidate(transferRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorMessage($"'Amount' must not be more than 8 digits in total, with allowance for 2 decimals. 2 digits and 3 decimals were found.");
    }
}
