using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SmartSorter.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private static readonly Type BaseConfigurationType = typeof(ISortingConfiguration<>);

    private static MethodInfo? _configureMethod;

    private static MethodInfo ConfigureMethod(Type sourceType) =>
        (_configureMethod ??= new Action<ISortingConfiguration<object>, ISortingConfigurationBuilder<object>>(Configure)
            .GetMethodInfo()
            .GetGenericMethodDefinition())
        .MakeGenericMethod(sourceType);

    public static IServiceCollection AddSmartSorter(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var configurations = new Dictionary<Type, IEntitySortingConfiguration>();

        foreach (var configurationType in new ConfigurationTypeFromAssemblyGatherer().GetConfigurationTypes(assembly))
        {
            var configurationInterface = configurationType
                .GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == BaseConfigurationType);

            var sourceType = configurationInterface.GetGenericArguments()[0];

            var builder = (ISortingConfigurationBuilder)Activator.CreateInstance(typeof(SortingConfigurationBuilder<>)
                .MakeGenericType(sourceType))!;

            var configuration = Activator.CreateInstance(configurationType);

            ConfigureMethod(sourceType).Invoke(null, new[] { configuration, builder });

            configurations.Add(sourceType, builder.Build());
        }

        serviceCollection.AddSingleton<IConfigurationProvider>(new ConfigurationProvider(configurations));

        return serviceCollection;
    }

    private static void Configure<TSource>(
        ISortingConfiguration<TSource> configuration,
        ISortingConfigurationBuilder<TSource> builder)
    {
        configuration.Configure(builder);
    }
}