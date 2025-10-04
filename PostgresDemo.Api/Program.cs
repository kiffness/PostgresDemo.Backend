using Microsoft.EntityFrameworkCore;
using PostgresDemo.Api.Services;
using PostgresDemo.Database.Data;
using PostgresDemo.Database.Services;
using PostgresDemo.Library.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Determine SQLite path from environment variable or fallback
// Set environment variable: SQLITE_DB_PATH
var dbPath = Environment.GetEnvironmentVariable("SQLITE_DB_PATH")!;

if (string.IsNullOrEmpty(dbPath))
{
    if (builder.Environment.IsDevelopment())
    {
        // Local fallback path
        var solutionRoot = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..");
        dbPath = Path.GetFullPath(Path.Combine(solutionRoot, "PostgresDemo", "todo.db"));
    }
    else
    {
        // Azure App Service fallback path
        if (OperatingSystem.IsWindows())
        {
            var home = Environment.GetEnvironmentVariable("HOME")!;
            dbPath = Path.Combine(home, "site", "wwwroot", "todo.db");
        }
        else
        {
            dbPath = Path.Combine("/home/site/wwwroot", "todo.db");
        }
    }
}

var connectionString = $"Data Source={dbPath}";

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => 
        policy.WithOrigins(
            "https://todoapi-hacrbqewfeana9dv.ukwest-01.azurewebsites.net",
            "http://localhost:5173"
        ).AllowAnyHeader().AllowAnyMethod()
    );
});

// Add OpenAPI / Swagger
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Register DbContext with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Register Todo service
builder.Services.AddScoped<ITodoService, EfTodoService>();

var app = builder.Build();

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Middleware
app.UseCors();
app.UseHttpsRedirection();

// Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
