using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PostgresDemo.Database.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Compute path to the solution root (adjust depth as needed)
        var solutionRoot = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..");

        // Build the database path — store it under your main solution folder
        var dbPath = Path.GetFullPath(Path.Combine(solutionRoot, "PostgresDemo", "todo.db"));
        
        var dbDir = Path.GetDirectoryName(dbPath);
        if (!Directory.Exists(dbDir))
        {
            Directory.CreateDirectory(dbDir!);
        }

        // Use Sqlite connection string
        var connectionString = $"Data Source={dbPath}";

        optionsBuilder.UseSqlite(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}