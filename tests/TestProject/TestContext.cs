using Microsoft.EntityFrameworkCore;

namespace TestProject;

internal class TestContext : DbContext
{
    private readonly string _connectionString;

    public DbSet<Entity> Entities => Set<Entity>();

    public TestContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}