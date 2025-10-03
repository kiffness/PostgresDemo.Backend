using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using PostgresDemo.Api.Services;
using PostgresDemo.Database.Data;
using PostgresDemo.Database.Services;
using PostgresDemo.Library.Interfaces;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ITodoService, EfTodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.UseHttpsRedirection();


app.Run();