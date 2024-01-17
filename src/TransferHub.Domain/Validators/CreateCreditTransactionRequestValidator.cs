using FluentValidation;
using TransferHub.Domain.Requests;

namespace TransferHub.Domain.Validators;

public class CreateCreditTransactionRequestValidator : AbstractValidator<CreateCreditTransactionRequest>
{
    public CreateCreditTransactionRequestValidator()
    {
        RuleFor(x => x.ParticipantId).NotEmpty();

        RuleFor(x => x.Amount).GreaterThan(0).PrecisionScale(8, 2, false);
    }
}
