using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Infrastructure.Persistance;

namespace ProductCatalog.Infrastructure.Tests;

internal class SqliteInMemoryRepositoryTest
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions _contextOptions;
    public SqliteInMemoryRepositoryTest()
    {        
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<EasyCommerceDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new EasyCommerceDbContext(_contextOptions);     
    }

    public EasyCommerceDbContext CreateContext() => new EasyCommerceDbContext(_contextOptions);

    public void Dispose() => _connection.Dispose();


}
