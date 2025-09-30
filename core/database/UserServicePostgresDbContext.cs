using Microsoft.EntityFrameworkCore;
using Testing_CRUD.core.database.interfaces;

namespace Testing_CRUD.core.database;

// Example: User Service PostgreSQL Database Context
public class UserServicePostgresDbContext : DbContext, IDatabaseContext
{
    public UserServicePostgresDbContext(DbContextOptions<UserServicePostgresDbContext> options)
        : base(options) { }

    public string DatabaseName => "user_service";
    public string Provider => "PostgreSQL";

    public DbContext GetContext() => this;

    // Example DbSets for User Service
    // public DbSet<UserModel> Users { get; set; }
    // public DbSet<RoleModel> Roles { get; set; }
    // public DbSet<PermissionModel> Permissions { get; set; }
}
