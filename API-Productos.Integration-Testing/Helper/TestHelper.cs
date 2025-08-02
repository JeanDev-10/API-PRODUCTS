using System;
using API_Productos.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace API_Productos.Integration_Testing.Helper;

public class TestHelper
{
    public static AppDbContext createInMemoryDbContext()
    {
        var connection = new SqliteConnection($"DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>()
                 .UseSqlite(connection)
                 .Options;
        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
