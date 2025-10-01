namespace KutokAccounting.Services.Stores.Models;

public record struct DateTimeRange(
	DateTime StartOfRange,
	DateTime EndOfRange);