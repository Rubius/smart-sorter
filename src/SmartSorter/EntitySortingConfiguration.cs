using System;
using System.Collections.Generic;

namespace SmartSorter;

public class EntitySortingConfiguration : IEntitySortingConfiguration
{
    private readonly Dictionary<string, RuleConfiguration> _sortingExpressions;

    public EntitySortingConfiguration(IDictionary<string, RuleConfiguration> sortingExpressions)
    {
        _sortingExpressions = new Dictionary<string, RuleConfiguration>(sortingExpressions);
    }

    RuleConfiguration IEntitySortingConfiguration.GetRuleConfiguration(string sortingRule)
    {
        return _sortingExpressions.TryGetValue(sortingRule, out var configuration)
            ? configuration
            : throw new Exception($"Expression for rule : {sortingRule} not found");
    }
}