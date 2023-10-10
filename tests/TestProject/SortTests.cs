using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using SmartSorter;
using Xunit;
using SmartSorter.QueryableExtensions;

namespace TestProject;

public class SortTests : IDisposable, IAsyncDisposable
{
    private readonly TestContext _testContext;
    private readonly Faker<Entity> _faker;

    public SortTests()
    {
        var now = DateTime.Now;

        _faker = new Faker<Entity>()
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.Age, f => now.Year - f.Person.DateOfBirth.Year)
            .RuleFor(x => x.Height, f => f.Random.Int(165, 195))
            .RuleFor(x => x.Weight, f => f.Random.Int(65, 105));

        _testContext = new TestContext(Environment.GetEnvironmentVariable("DB_CONNECTION") ?? string.Empty);
        _testContext.Database.EnsureCreated();
    }

    [Fact]
    public void OrderByAge_Should_OrderByAge()
    {
        // arrange
        ISortingConfigurationBuilder<Entity> builder = new SortingConfigurationBuilder<Entity>();
        new EntitySortingConfiguration().Configure(builder);

        var provider = new ConfigurationProvider(new Dictionary<Type, IEntitySortingConfiguration>
        {
            { typeof(Entity), builder.Build() }
        });

        var rules = new SortingRules(new Dictionary<string, SortDirection>
        {
            [SortRuleConstants.Age] = SortDirection.Ascending
        });

        using var transaction = _testContext.Database.BeginTransaction();

        _testContext.Entities.AddRange(_faker
            .GenerateForever()
            .Take(Random.Shared.Next(50, 100)));

        _testContext.SaveChanges();

        // act
        var sortedEntities = _testContext
            .Entities
            .ApplySorting(provider, rules)
            .ToList();

        // assert
        sortedEntities.Should().BeInAscendingOrder(x => x.Age);
    }

    [Fact]
    public void OrderByWeightAndThenByHeight_Should_OrderByWeightAndThenByHeight()
    {
        // arrange
        ISortingConfigurationBuilder<Entity> builder = new SortingConfigurationBuilder<Entity>();
        new EntitySortingConfiguration().Configure(builder);

        var provider = new ConfigurationProvider(new Dictionary<Type, IEntitySortingConfiguration>
        {
            { typeof(Entity), builder.Build() }
        });

        var rules = new SortingRules(new Dictionary<string, SortDirection>
        {
            [SortRuleConstants.Weight] = SortDirection.Descending,
            [SortRuleConstants.Height] = SortDirection.Ascending
        });

        using var transaction = _testContext.Database.BeginTransaction();

        _testContext.Entities.AddRange(_faker
            .GenerateForever()
            .Take(Random.Shared.Next(50, 100)));

        _testContext.SaveChanges();

        // act
        var sortedEntities = _testContext
            .Entities
            .ApplySorting(provider, rules)
            .ToList();

        // assert
        sortedEntities
            .Should()
            .BeInDescendingOrder(x => x.Weight)
            .And
            .ThenBeInAscendingOrder(x => x.Height);
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