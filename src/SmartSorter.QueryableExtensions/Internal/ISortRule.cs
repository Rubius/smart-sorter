using System.Linq;

namespace SmartSorter.QueryableExtensions.Internal;

internal interface ISortRule<TSource>
{
    IQueryable<TSource> Apply(IQueryable<TSource> source);
}