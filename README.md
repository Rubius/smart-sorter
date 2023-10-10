# SmartSorter

## How to use

* Create entity
```csharp
public class Entity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
}
```

* Create sorting configuration class. Inherit it from the ISortingConfiguration\<T\> interface

```csharp
public class EntitySortingConfiguration : ISortingConfiguration<Entity>
{
    public void Configure(ISortingConfigurationBuilder<Entity> builder)
    {
        builder
            .RuleFor("Id", x => x.Id)
            .RuleFor("Name", x => x.Name)
            .RuleFor("Age", x => x.Age)
            .RuleFor("Height", x => x.Height)
            .RuleFor("Weight", x => x.Weight);
    }
}
```

* Call the extension method `AddSmartSorter(this IServiceCollection serviceCollection, Action<SortingConfigurationOptions> optionsBuilder)`
 and pass the assembly for scanning configurations

```csharp
var serviceProvider = new ServiceCollection()
    .AddSmartSorter(options => options.AddAssembly(Assembly.GetExecutingAssembly()))
    .BuildServiceProvider();
```

* Get the `IConfigurationProvider` interface as dependency

```csharp
configurationProvider = serviceProvider.GetRequiredService<IConfigurationProvider>()
```

* Use the extension method `ApplySorting<TSource>(this IQueryable<TSource> source, IConfigurationProvider configurationProvider, SortingRules sortingRules)`
to apply sorting rules to your query

```csharp
var sortingRules = new(new Dictionary<string, SortDirection>
    {
        ["Name"] = SortDirection.Descending,
        ["Age"] = SortDirection.Ascending,
        ["Id"] = SortDirection.Descending
    })

var sortedEntities = await _testContext
    .Entities
    .ApplySorting(configurationProvider, sortingRules)
    .ToListAsync(cancellationToken)
```

## Benchmarks

```csharp
[MemoryDiagnoser]
public class Benchmark : IDisposable, IAsyncDisposable
{
    ...
    private readonly SortingRules _sortingRules = new(new Dictionary<string, SortDirection>
    {
        ["Name"] = SortDirection.Descending,
        ["Age"] = SortDirection.Ascending,
        ["Id"] = SortDirection.Descending
    });

    ...

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

    ...
}
```

<details><summary>Environment</summary>
<br />
<pre>
BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 10 (10.0.19045.3516/22H2/2022Update)
Intel Core i5-8500 CPU 3.00GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET SDK 7.0.304
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT AVX2
</pre>
</details>


| Method          |     Mean |    Error |   StdDev | Ratio |   Gen0 | Allocated | Alloc Ratio |
|:----------------|---------:|---------:|---------:|------:|-------:|----------:|------------:|
| SmartSorter     | 26.45 us | 0.137 us | 0.114 us |  0.93 | 1.5564 |   7.17 KB |        0.91 |
| DefaultOrderBy  | 28.61 us | 0.213 us | 0.200 us |  1.00 | 1.6785 |   7.84 KB |        1.00 |