using System.Data;
using FluentValidation;

namespace KutokAccounting.Services.Stores.Models;

public class PageDtoValidator : AbstractValidator<Page>
{
	public PageDtoValidator()
	{
		RuleFor(p=> p.PageSize)
			.NotEmpty()
			.GreaterThan(0);
		RuleFor(p => p.PageNumber)
			.NotEmpty()
			.GreaterThan(0);
	}
}