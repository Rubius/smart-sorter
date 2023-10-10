using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartSorter.DependencyInjection;

internal class ConfigurationTypeFromAssemblyGatherer
{
    private readonly Type _baseConfigurationType = typeof(ISortingConfiguration<>);

    public IEnumerable<Type> GetConfigurationTypes(Assembly assembly)
    {
        return assembly
            .ExportedTypes
            .Where(t => t
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == _baseConfigurationType));
    }
}