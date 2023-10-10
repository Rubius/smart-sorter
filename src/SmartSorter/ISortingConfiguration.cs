namespace SmartSorter;

public interface ISortingConfiguration<TSource>
{
    void Configure(ISortingConfigurationBuilder<TSource> builder);
}