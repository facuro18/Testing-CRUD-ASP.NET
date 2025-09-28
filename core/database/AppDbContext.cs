using Microsoft.EntityFrameworkCore;
using Testing_CRUD.src.models;

namespace Testing_CRUD.core.database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; }
}
