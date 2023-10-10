using System;
using System.Linq.Expressions;

namespace SmartSorter;

public class RuleConfiguration
{
    public LambdaExpression Expression { get; }
    public Type KeyType { get; }

    public RuleConfiguration(LambdaExpression expression, Type keyType)
    {
        Expression = expression;
        KeyType = keyType;
    }
}