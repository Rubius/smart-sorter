namespace SmartSorter;

public interface IEntitySortingConfiguration
{
    RuleConfiguration GetRuleConfiguration(string sortingRule);
}