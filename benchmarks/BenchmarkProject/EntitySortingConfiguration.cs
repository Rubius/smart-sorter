using SmartSorter;

namespace BenchmarkProject;

public class EntitySortingConfiguration : ISortingConfiguration<Entity>
{
    public void Configure(ISortingConfigurationBuilder<Entity> builder)
    {
        builder
            .RuleFor("Id", x => x.Id)
            .RuleFor("Weight", x => x.Weight)
            .RuleFor("Height", x => x.Height)
            .RuleFor("Age", x => x.Age)
            .RuleFor("Name", x => x.Name);
    }
}