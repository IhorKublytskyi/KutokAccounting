using FluentValidation;
using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.Stores.Models;

public class PaginationValidator : AbstractValidator<Pagination>
{
	public PaginationValidator()
	{
		RuleFor(p => p.Page)
			.GreaterThan(0)
			.LessThanOrEqualTo(100);

		RuleFor(p => p.PageSize)
			.GreaterThan(0)
			.LessThanOrEqualTo(10);
	}
}