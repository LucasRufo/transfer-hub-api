using FluentValidation.TestHelper;
using Money.Domain.Validators;

namespace Money.UnitTests.Domain.Validators;

public class CreateParticipantRequestValidatorTests : BaseTests
{
    private readonly CreateParticipantRequestValidator _validator = new();

    [Test]
    public void ShouldNotBeValidWhenNameIsEmpty()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithName(string.Empty)
            .Generate();

        var validate = _validator.TestValidate(createParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage($"'Name' must not be empty.");
    }

    [Test]
    public void ShouldNotBeValidWhenNameExceedMaximumLength()
    {
        var invalidStringLength = 201;
        var fakeName = Faker.Random.String(invalidStringLength);
        
        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithName(fakeName)
            .Generate();

        var validate = _validator.TestValidate(createParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage($"The length of 'Name' must be 200 characters or fewer. You entered {invalidStringLength} characters.");
    }

    [Test]
    public void ShouldNotBeValidWhenCPFIsEmpty()
    {
        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithCPF(string.Empty)
            .Generate();

        var validate = _validator.TestValidate(createParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.CPF)
            .WithErrorMessage($"'CPF' must not be empty.");
    }

    [Test]
    public void ShouldNotBeValidWhenCPFIsInvalid()
    {
        var invalidCPF = "00011122233";

        var createParticipantRequest = new CreateParticipantRequestBuilder()
            .WithCPF(invalidCPF)
            .Generate();

        var validate = _validator.TestValidate(createParticipantRequest);

        validate.ShouldHaveValidationErrorFor(x => x.CPF)
            .WithErrorMessage($"The CPF is invalid.");
    }
}
