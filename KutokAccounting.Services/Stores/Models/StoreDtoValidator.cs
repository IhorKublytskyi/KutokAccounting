using FluentValidation;
using KutokAccounting.Services.Stores.Dtos;

namespace KutokAccounting.Services.Stores.Models;

public class StoreDtoValidator : AbstractValidator<StoreDto>
{
	public StoreDtoValidator()
	{
		RuleFor(s => s.Name).NotEmpty().MaximumLength(100);
		RuleFor(s => s.SetupDate).NotEmpty();
		RuleFor(s => s.IsOpened).NotEmpty();
		RuleFor(s => s.Address).MaximumLength(100);
	}
}