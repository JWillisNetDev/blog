using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace blog.Services.Tests;

public class TestDbContextFactory : IDbContextFactory<BlogDbContext>, IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<BlogDbContext> _options;

    public TestDbContextFactory()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseSqlite(_connection)
            .Options;
    }
    
    public BlogDbContext CreateDbContext()
    {
        BlogDbContext db = new(_options);
        db.Database.EnsureCreated();
        return db;
    }
    
    public void Dispose()
    {
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}