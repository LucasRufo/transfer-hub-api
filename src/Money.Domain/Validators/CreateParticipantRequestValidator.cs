using FluentValidation;
using Money.Domain.Requests;

namespace Money.Domain.Validators;

public class CreateParticipantRequestValidator : AbstractValidator<CreateParticipantRequest>
{
    public CreateParticipantRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.CPF)
            .NotEmpty()
            .IsValidCPF().WithMessage("The CPF is invalid.");
    }
}
