using FluentValidation;
using KutokAccounting.Services.TransactionTypes.Models;

namespace KutokAccounting.Services.TransactionTypes.Validators;

public sealed class TransactionTypeQueryParametersValidator : AbstractValidator<TransactionTypeQueryParameters>
{
    public TransactionTypeQueryParametersValidator()
    {
        RuleFor(p => p.Filters.Name).Length(1, 100);

        RuleFor(p => p.Pagination.Page)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(p => p.Pagination.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(10);
    }
}