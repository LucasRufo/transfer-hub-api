using FluentValidation;
using TransferHub.Domain.Requests;

namespace TransferHub.Domain.Validators;

public class TransferRequestValidator : AbstractValidator<TransferRequest>
{
    public TransferRequestValidator()
    {
        RuleFor(x => x.FromParticipantId).NotEmpty();

        RuleFor(x => x.ToParticipantId).NotEmpty();

        RuleFor(x => x.Amount).GreaterThan(0).PrecisionScale(8, 2, false);
    }
}

