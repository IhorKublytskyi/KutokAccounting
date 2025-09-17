using FluentValidation;
using KutokAccounting.Services.Vendors.Models;

namespace KutokAccounting.Services.Vendors.Validators;

public sealed class VendorQueryParametersValidator : AbstractValidator<QueryParameters>
{
    public VendorQueryParametersValidator()
    {
        RuleFor(p => p.Name).Length(1, 100);

        RuleFor(p => p.Pagination.Page)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(p => p.Pagination.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(10);
    }
}