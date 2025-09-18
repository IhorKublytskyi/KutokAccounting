using KutokAccounting.DataProvider.Models;

namespace KutokAccounting.Services.Stores.Abstractions;

public interface ISearchQueryBuilder
{
	IQueryable<Store> MatchedStores();
}