using Microsoft.EntityFrameworkCore;
using PostgresDemo.Library.Models;

namespace PostgresDemo.Database.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
}