using FluentValidation;
using KutokAccounting.Services.TransactionTypes.Models;

namespace KutokAccounting.Services.TransactionTypes.Validators;

public class TransactionTypeDtoValidator : AbstractValidator<TransactionTypeDto>
{
    public TransactionTypeDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);
    }
}