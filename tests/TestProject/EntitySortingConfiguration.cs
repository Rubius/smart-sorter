using SmartSorter;

namespace TestProject;

public class EntitySortingConfiguration : ISortingConfiguration<Entity>
{
    public void Configure(ISortingConfigurationBuilder<Entity> builder)
    {
        builder
            .RuleFor(SortRuleConstants.Id, x => x.Id)
            .RuleFor(SortRuleConstants.Weight, x => x.Weight)
            .RuleFor(SortRuleConstants.Height, x => x.Height)
            .RuleFor(SortRuleConstants.Age, x => x.Age)
            .RuleFor(SortRuleConstants.Name, x => x.Name);
    }
}