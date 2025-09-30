using Microsoft.EntityFrameworkCore;

namespace Testing_CRUD.core.database.interfaces;

public interface IDatabaseContext
{
    string DatabaseName { get; }
    string Provider { get; }
    DbContext GetContext();
}
