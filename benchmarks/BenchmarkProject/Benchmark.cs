using System.Reflection;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartSorter;
using SmartSorter.DependencyInjection;
using SmartSorter.QueryableExtensions;

namespace BenchmarkProject;

[MemoryDiagnoser]
public class Benchmark : IDisposable, IAsyncDisposable
{
    private readonly BenchmarkContext _testContext;
    private readonly IConfigurationProvider _configurationProvider;

    private readonly SortingRules _sortingRules = new(new Dictionary<string, SortDirection>
    {
        ["Name"] = SortDirection.Descending,
        ["Age"] = SortDirection.Ascending,
        ["Id"] = SortDirection.Descending
    });

    public Benchmark()
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? string.Empty;

        var serviceProvider = new ServiceCollection()
            .AddSmartSorter(Assembly.GetExecutingAssembly())
            .BuildServiceProvider();

        _testContext = new BenchmarkContext(connectionString);
        _testContext.Database.EnsureCreated();

        _configurationProvider = serviceProvider.GetRequiredService<IConfigurationProvider>();
    }

    [Benchmark]
    public string SmartSorter()
    {
        return _testContext
            .Entities
            .ApplySorting(_configurationProvider, _sortingRules)
            .ToQueryString();
    }

    [Benchmark(Baseline = true)]
    public string DefaultOrderBy()
    {
        return _testContext
            .Entities
            .OrderByDescending(x => x.Name)
            .ThenBy(x => x.Age)
            .ThenByDescending(x => x.Id)
            .ToQueryString();
    }

    public void Dispose()
    {
        _testContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _testContext.DisposeAsync();
    }
}