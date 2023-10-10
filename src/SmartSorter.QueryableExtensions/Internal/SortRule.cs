using System;
using System.Linq;
using System.Linq.Expressions;

namespace SmartSorter.QueryableExtensions.Internal;

internal class SortRule<TSource, TValue> : ISortRule<TSource>
{
    private readonly Expression<Func<TSource, TValue>> _sortExpression;
    private readonly SortDirection _sortDirection;

    public SortRule(Expression<Func<TSource, TValue>> sortExpression, SortDirection sortDirection)
    {
        _sortExpression = sortExpression;
        _sortDirection = sortDirection;
    }

    public IQueryable<TSource> Apply(IQueryable<TSource> source)
    {
        var visitor = new OrderingMethodFinder();

        visitor.Visit(source.Expression);

        return _sortDirection switch
        {
            SortDirection.Ascending when visitor.OrderingMethodFound => ((IOrderedQueryable<TSource>)source)
                .ThenBy(_sortExpression),
            SortDirection.Ascending when visitor.OrderingMethodFound is false => source
                .OrderBy(_sortExpression),
            SortDirection.Descending when visitor.OrderingMethodFound => ((IOrderedQueryable<TSource>)source)
                .ThenByDescending(_sortExpression),
            SortDirection.Descending when visitor.OrderingMethodFound is false => source
                .OrderByDescending(_sortExpression),
            _ => source
        };
    }
}