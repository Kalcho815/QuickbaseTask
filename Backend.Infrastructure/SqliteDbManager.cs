using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace Backend;

public class SqliteDbManager : IDbManager
{
    private readonly ILogger<SqliteDbManager> _logger;

    public SqliteDbManager(ILogger<SqliteDbManager> logger)
    {
        _logger = logger;
    }

    public DbConnection? GetConnection(string connectionString)
    {
        try
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open(); 
            return connection;
        }
        catch(SqliteException ex)
        {
            _logger.LogError(ex, "Failed to open SQLite connection with connection string: {ConnectionString}", connectionString);
            return null;
        }
    }
}
