using System;
using System.Linq;
using System.Linq.Expressions;

namespace SmartSorter.QueryableExtensions.Internal;

internal class OrderingMethodFinder : ExpressionVisitor
{
    /// <summary>
    /// Является ли метод найденным
    /// </summary>
    public bool OrderingMethodFound { get; private set; }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        var name = node.Method.Name;

        if (node.Method.DeclaringType == typeof(Queryable) && (
                name.StartsWith("OrderBy", StringComparison.Ordinal) ||
                name.StartsWith("ThenBy", StringComparison.Ordinal)))
        {
            OrderingMethodFound = true;
        }

        return base.VisitMethodCall(node);
    }
}