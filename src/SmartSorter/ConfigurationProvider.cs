using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmartSorter;

public class ConfigurationProvider : IConfigurationProvider
{
    private readonly ReadOnlyDictionary<Type, IEntitySortingConfiguration> _sortingConfigurations;

    public ConfigurationProvider(IDictionary<Type, IEntitySortingConfiguration> sortingConfigurations)
    {
        _sortingConfigurations = new ReadOnlyDictionary<Type, IEntitySortingConfiguration>(sortingConfigurations);
    }

    public IEntitySortingConfiguration GetConfiguration<TSource>()
    {
        return _sortingConfigurations.TryGetValue(typeof(TSource), out var entitySortingConfiguration)
            ? entitySortingConfiguration
            : throw new Exception();
    }
}