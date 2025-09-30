using Microsoft.EntityFrameworkCore;
using Testing_CRUD.core.database.interfaces;
using Testing_CRUD.src.modules.product.models;

namespace Testing_CRUD.core.database;

public class TestingCrudMySqlDbContext : DbContext, IDatabaseContext
{
    public TestingCrudMySqlDbContext(DbContextOptions<TestingCrudMySqlDbContext> options)
        : base(options) { }

    public string DatabaseName => "testing_crud";
    public string Provider => "MySQL";

    public DbContext GetContext() => this;

    public DbSet<ProductModel> Products { get; set; }
}
