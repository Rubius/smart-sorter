using System.Linq;
using SmartSorter.QueryableExtensions.Internal;

namespace SmartSorter.QueryableExtensions;

public static class QueryableExtensions
{
    public static IQueryable<TSource> ApplySorting<TSource>(
        this IQueryable<TSource> source,
        IConfigurationProvider configurationProvider,
        SortingRules sortingRules)
    {
        return new QueryableSorter(configurationProvider, sortingRules).Apply(source);
    }
}