using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Testing_CRUD.core.database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var MySQLConnectionString = builder.Configuration.GetConnectionString("AppDbConnectionStringMySql");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(MySQLConnectionString, ServerVersion.AutoDetect(MySQLConnectionString))
);

var app = builder.Build();

// Initialize database
InitDatabase.StartDatabase();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // app.UseSwaggerUI(options =>
    // {
    //     options.SwaggerEndpoint("/openapi/v1.json", "v1");
    // });

    app.MapScalarApiReference("/api/docs");
}
app.UseAuthorization();

app.MapControllers();

app.Run();
