using Microsoft.EntityFrameworkCore;
using Q1_WebAPI.Data;

namespace Q1_WebAPI.Tests;

/// <summary>
/// Shared test helpers for creating an in-memory DbContext (no SQL Server required).
/// </summary>
internal static class TestDb
{
    public static AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
