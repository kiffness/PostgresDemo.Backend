using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PostgresDemo.Database.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Compute path to solution root
        var solutionRoot = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..");
        var envPath = Path.Combine(solutionRoot, ".env");

        DotNetEnv.Env.Load(envPath);

        var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION")
                               ?? "Host=localhost;Database=postgres_demo;Username=postgres;Password=fallback";

        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}