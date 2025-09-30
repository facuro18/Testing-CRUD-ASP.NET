# Database Architecture

This project implements a multi-database architecture that allows you to connect to multiple databases for different microservices.

## Architecture Overview

### 1. Database Contexts

Each database has its own context that implements `IDatabaseContext`:

- `TestingCrudMySqlDbContext` - MySQL database for the Testing_CRUD service
- `UserServicePostgresDbContext` - PostgreSQL database for User service (example)

### 2. Database Service Locator

The `DatabaseServiceLocator` provides easy access to different database contexts:

```csharp
// Inject the service locator
public class SomeService
{
    private readonly DatabaseServiceLocator _dbLocator;

    public SomeService(DatabaseServiceLocator dbLocator)
    {
        _dbLocator = dbLocator;
    }

    public async Task DoSomething()
    {
        // Get specific context
        var testingCrudContext = _dbLocator.GetTestingCrudContext();
        var userServiceContext = _dbLocator.GetUserServiceContext();

        // Or get by database name
        var context = _dbLocator.GetContextByDatabaseName("testing_crud");

        // Or get generic context
        var context2 = _dbLocator.GetContext<TestingCrudMySqlDbContext>();
    }
}
```

## Adding New Databases

### 1. Create the Database Context

```csharp
public class NewServiceSqlServerDbContext : DbContext, IDatabaseContext
{
    public NewServiceSqlServerDbContext(DbContextOptions<NewServiceSqlServerDbContext> options)
        : base(options) { }

    public string DatabaseName => "new_service";
    public string Provider => "SQL Server";

    public DbContext GetContext() => this;

    public DbSet<SomeModel> SomeModels { get; set; }
}
```

### 2. Add Connection String

Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "NewServiceSqlServer": "Server=localhost;Database=new_service;Trusted_Connection=true;"
  }
}
```

### 3. Register in Program.cs

```csharp
// Add the new database context
var newServiceConnectionString = builder.Configuration.GetConnectionString("NewServiceSqlServer");
builder.Services.AddDbContext<NewServiceSqlServerDbContext>(options =>
    options.UseSqlServer(newServiceConnectionString)
);

// Register as IDatabaseContext
builder.Services.AddScoped<IDatabaseContext>(provider =>
    provider.GetRequiredService<NewServiceSqlServerDbContext>());
```

### 4. Add to DatabaseServiceLocator

```csharp
public NewServiceSqlServerDbContext GetNewServiceContext()
{
    return _serviceProvider.GetRequiredService<NewServiceSqlServerDbContext>();
}
```

## Benefits

1. **Clear Separation**: Each database context is clearly identified by service and database type
2. **Scalability**: Easy to add new databases for different microservices
3. **Type Safety**: Strongly typed access to different contexts
4. **Flexibility**: Can mix different database providers (MySQL, PostgreSQL, SQL Server, etc.)
5. **Maintainability**: Centralized database configuration and management

## Usage Examples

### In Controllers

```csharp
[ApiController]
public class ProductController : ControllerBase
{
    private readonly DatabaseServiceLocator _dbLocator;

    public ProductController(DatabaseServiceLocator dbLocator)
    {
        _dbLocator = dbLocator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var context = _dbLocator.GetTestingCrudContext();
        var products = await context.Products.ToListAsync();
        return Ok(products);
    }
}
```

### In Services

```csharp
public class ProductService
{
    private readonly DatabaseServiceLocator _dbLocator;

    public ProductService(DatabaseServiceLocator dbLocator)
    {
        _dbLocator = dbLocator;
    }

    public async Task<Product> CreateProductAsync(CreateProductDto dto)
    {
        var context = _dbLocator.GetTestingCrudContext();
        // Implementation...
    }
}
```
