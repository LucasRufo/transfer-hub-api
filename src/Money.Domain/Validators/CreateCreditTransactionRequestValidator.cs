﻿using FluentValidation;
using Money.Domain.Requests;

namespace Money.Domain.Validators;

public class CreateCreditTransactionRequestValidator : AbstractValidator<CreateCreditTransactionRequest>
{
    public CreateCreditTransactionRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.ParticipantId).NotEmpty();

        RuleFor(x => x.Amount).GreaterThan(0);
    }
}