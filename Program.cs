using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Testing_CRUD.core.database;
using Testing_CRUD.core.database.interfaces;
using Testing_CRUD.src.modules.product.repositories;
using Testing_CRUD.src.modules.product.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register Services

builder.Services.AddScoped<IProductService, ProductService>();

// Register Repositories

builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register database service locator
builder.Services.AddScoped<DatabaseServiceLocator>();

// Configure databases

string? testingCrudConnectionString = builder.Configuration.GetConnectionString("TestingCrudMySql");

// Register the contexts as a service

// builder.Services.AddScoped<IDatabaseContext>(provider =>
// provider.GetRequiredService<UserServicePostgresDbContext>()
// );

builder.Services.AddScoped<IDatabaseContext>(provider =>
    provider.GetRequiredService<TestingCrudMySqlDbContext>()
);

builder.Services.AddDbContext<TestingCrudMySqlDbContext>(options =>
    options.UseMySql(
        testingCrudConnectionString,
        ServerVersion.AutoDetect(testingCrudConnectionString)
    )
);

// Example: Configure additional databases

// var userServiceConnectionString = builder.Configuration.GetConnectionString("UserServicePostgres");
// builder.Services.AddDbContext<UserServicePostgresDbContext>(options =>
//     options.UseNpgsql(userServiceConnectionString)
// );

var app = builder.Build();

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
