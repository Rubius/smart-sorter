using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SmartSorter;

public class SortingConfigurationBuilder<TSource> : ISortingConfigurationBuilder<TSource>
{
    private readonly Dictionary<string, RuleConfiguration> _sortingExpressions = new();

    public ISortingConfigurationBuilder<TSource> RuleFor<TValue>(
        string sortingRule,
        Expression<Func<TSource, TValue>> bindExpression)
    {
        if (_sortingExpressions.ContainsKey(sortingRule))
        {
            return this;
        }

        _sortingExpressions.Add(sortingRule, new RuleConfiguration(bindExpression, typeof(TValue)));

        return this;
    }

    IEntitySortingConfiguration ISortingConfigurationBuilder.Build()
    {
        return new EntitySortingConfiguration(_sortingExpressions);
    }
}