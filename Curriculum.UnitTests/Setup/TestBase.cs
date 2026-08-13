using Curriculum.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Curriculum.UnitTests.Setup;

public class TestBase : IDisposable
{
    protected readonly CurriculumContext Context;
    private readonly SqliteConnection _connection;

    public TestBase()
    {
        _connection = new("DataSource=:memory:");
        _connection.Open();
        
        var options = new DbContextOptionsBuilder<CurriculumContext>()
            .UseSqlite(_connection)
            .Options;
        
        Context = new(options);
        Context.Database.EnsureCreated();
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Context.Database.EnsureDeleted();
        Context.Dispose();
        _connection.Dispose();
    }
    
}