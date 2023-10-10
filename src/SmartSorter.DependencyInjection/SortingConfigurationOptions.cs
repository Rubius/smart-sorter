using System;
using System.Collections.Generic;
using System.Reflection;

namespace SmartSorter.DependencyInjection;

public class SortingConfigurationOptions
{
    internal List<Assembly> ConfigurationAssemblies { get; } = new();
    internal List<Type> SourceTypes { get; } = new();

    public SortingConfigurationOptions AddAssembly(Assembly assembly)
    {
        ConfigurationAssemblies.Add(assembly);

        return this;
    }

    public SortingConfigurationOptions ConfigureByProperties(Type sourceType)
    {
        SourceTypes.Add(sourceType);

        return this;
    }
}