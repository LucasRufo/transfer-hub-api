using FluentValidation;
using Money.API.Requests;

namespace Money.Domain.Validators;

public class CreateParticipantRequestValidator : AbstractValidator<CreateParticipantRequest>
{
    public CreateParticipantRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.CPF)
            .NotEmpty()
            .IsValidCPF().WithMessage("The CPF is invalid.");
    }
}
