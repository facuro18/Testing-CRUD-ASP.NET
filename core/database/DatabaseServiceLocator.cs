using Microsoft.EntityFrameworkCore;
using Testing_CRUD.core.database.interfaces;

namespace Testing_CRUD.core.database;

public class DatabaseServiceLocator
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseServiceLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    //Option 1: Directly get the context

    public TestingCrudMySqlDbContext GetTestingCrudContext()
    {
        return _serviceProvider.GetRequiredService<TestingCrudMySqlDbContext>();
    }

    public UserServicePostgresDbContext GetUserServiceContext()
    {
        return _serviceProvider.GetRequiredService<UserServicePostgresDbContext>();
    }

    //Option 2 : Method to get context by database name
    public IDatabaseContext? GetContextByDatabaseName(string databaseName)
    {
        var contexts = _serviceProvider.GetServices<IDatabaseContext>();
        return contexts.FirstOrDefault(c =>
            c.DatabaseName.Equals(databaseName, StringComparison.OrdinalIgnoreCase)
        );
    }

    // Option 3: Generic method to get any database context
    public T GetContext<T>()
        where T : DbContext
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}
