using System;
using System.Linq;

namespace SmartSorter.QueryableExtensions.Internal;

internal class QueryableSorter
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly SortingRules _sortingRules;

    public QueryableSorter(IConfigurationProvider configurationProvider, SortingRules sortingRules)
    {
        _configurationProvider = configurationProvider;
        _sortingRules = sortingRules;
    }

    public IQueryable<TSource> Apply<TSource>(IQueryable<TSource> source)
    {
        var sourceConfiguration = _configurationProvider.GetConfiguration<TSource>();
        var result = source;

        foreach (var (sortingRule, sortDirection) in _sortingRules)
        {
            var ruleConfiguration = sourceConfiguration.GetRuleConfiguration(sortingRule);

            var property = (ISortRule<TSource>)Activator.CreateInstance(
                typeof(SortRule<,>).MakeGenericType(typeof(TSource), ruleConfiguration.KeyType),
                ruleConfiguration.Expression,
                sortDirection)!;

            result = property.Apply(result);
        }

        return result;
    }
}