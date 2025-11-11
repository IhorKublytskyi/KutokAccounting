using FluentValidation;
using KutokAccounting.Services.Transactions.Models;

namespace KutokAccounting.Services.Transactions.Validators;

public sealed class TransactionDtoValidator : AbstractValidator<TransactionDto>
{
	public TransactionDtoValidator()
	{
		RuleFor(t => t.Name)
			.NotNull()
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(t => t.Description)
			.MaximumLength(1024);

		RuleFor(t => t.Value)
			.GreaterThanOrEqualTo(0);
	}
}