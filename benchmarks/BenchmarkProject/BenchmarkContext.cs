using Microsoft.EntityFrameworkCore;

namespace BenchmarkProject;

public class BenchmarkContext : DbContext
{
    private readonly string _connectionString;
    
    public DbSet<Entity> Entities => Set<Entity>();

    public BenchmarkContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}