namespace SmartSorter;

public interface IConfigurationProvider
{
    IEntitySortingConfiguration GetConfiguration<TSource>();
}