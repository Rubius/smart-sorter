using System;
using System.Linq.Expressions;

namespace SmartSorter;

public interface ISortingConfigurationBuilder
{
    IEntitySortingConfiguration Build();
}

public interface ISortingConfigurationBuilder<TSource> : ISortingConfigurationBuilder
{
    ISortingConfigurationBuilder<TSource> RuleFor<TValue>(
        string sortingRule,
        Expression<Func<TSource, TValue>> bindExpression);
}