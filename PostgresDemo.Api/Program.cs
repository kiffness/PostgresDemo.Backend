using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using PostgresDemo.Api.Services;
using PostgresDemo.Database.Data;
using PostgresDemo.Database.Services;
using PostgresDemo.Library.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Compute path to the solution root (adjust depth as needed)
var solutionRoot = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..");

// Build the database path — store it under your main solution folder
var dbPath = Path.GetFullPath(Path.Combine(solutionRoot, "PostgresDemo", "todo.db"));

// Use Sqlite connection string
var connectionString = $"Data Source={dbPath}";

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<ITodoService, EfTodoService>();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.UseHttpsRedirection();


app.Run();